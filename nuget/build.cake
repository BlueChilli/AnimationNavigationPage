using System.Threading;
using System.Text.RegularExpressions;

//////////////////////////////////////////////////////////////////////
// ADDINS
//////////////////////////////////////////////////////////////////////

#addin "Cake.FileHelpers"
#addin nuget:?package=Cake.Incubator&version=2.0.0
#addin "Cake.Watch"
#addin nuget:?package=Newtonsoft.Json
#tool nuget:?package=vswhere

//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

#tool "GitReleaseManager"
#tool "GitVersion.CommandLine"
#tool "GitLink"
using Cake.Common.Build.TeamCity;
using Cake.Core.IO;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

if (string.IsNullOrWhiteSpace(target))
{
    target = "Default";
}


//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Should MSBuild & GitLink treat any errors as warnings?
var treatWarningsAsErrors = false;
Func<string, int> GetEnvironmentInteger = name => {
	
	var data = EnvironmentVariable(name);
	int d = 0;
	if(!String.IsNullOrEmpty(data) && int.TryParse(data, out d)) 
	{
		return d;
	} 
	
	return 0;

};

// Load json Configuartion
var configFilePath = "./config.json";
JObject config;

if(!FileExists(configFilePath)) {
	
	throw new Exception(string.Format("config.json can not be found at {0}", configFilePath));
}

var configFile = File(configFilePath);

using(var stream = new StreamReader(System.IO.File.OpenRead(configFile.Path.FullPath))) {
	var json = stream.ReadToEnd();
	config = JObject.Parse(json);
};

if(config == null) {
	throw new Exception(string.Format("config.json can not be found at {0}", configFilePath));
}


// Build configuration
var productName = config.Value<string>("productName");
var project = config.Value<string>("projectName");
var local = BuildSystem.IsLocalBuild;
var isTeamCity = BuildSystem.TeamCity.IsRunningOnTeamCity;
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();
var teamCity = BuildSystem.TeamCity;
var branch = EnvironmentVariable("Git_Branch");
var isPullRequest = !String.IsNullOrEmpty(branch) && branch.ToLower().Contains("refs/pull");
var projectName =  EnvironmentVariable("TEAMCITY_PROJECT_NAME"); //  teamCity.Environment.Project.Name;
var isRepository = StringComparer.OrdinalIgnoreCase.Equals(productName, projectName);
var isTagged = !String.IsNullOrEmpty(branch) && branch.ToLower().Contains("refs/tags");
var buildConfName = EnvironmentVariable("TEAMCITY_BUILDCONF_NAME"); //teamCity.Environment.Build.BuildConfName
var buildNumber = GetEnvironmentInteger("BUILD_NUMBER");
var isReleaseBranch = StringComparer.OrdinalIgnoreCase.Equals("master", buildConfName)|| StringComparer.OrdinalIgnoreCase.Equals("release", buildConfName);
var shouldAddLicenseHeader = false;
if(!string.IsNullOrEmpty(EnvironmentVariable("ShouldAddLicenseHeader"))) {
	shouldAddLicenseHeader = bool.Parse(EnvironmentVariable("ShouldAddLicenseHeader"));
}

// Version
string majorMinorPatch;
string semVersion;
string informationalVersion ;
string nugetVersion;
string buildVersion;

Action SetGitVersionData = () => {

	if(!isPullRequest) {
		var gitVersion = GitVersion();
		majorMinorPatch = gitVersion.MajorMinorPatch;
		semVersion = gitVersion.SemVer;
		informationalVersion = gitVersion.InformationalVersion;
		nugetVersion = gitVersion.NuGetVersion;
		buildVersion = gitVersion.FullBuildMetaData;
	}
	else {
		majorMinorPatch = "1.0.0";
		semVersion = "0";
		informationalVersion ="1.0.0";
		nugetVersion = "1.0.0";
		buildVersion = "alpha";
	}
};

SetGitVersionData();

// Artifacts
var artifactDirectory = "./artifacts/";
var packageWhitelist = config.Value<JArray>("packageWhiteList").Values<string>();

var buildSolution = config.Value<string>("solutionFile");
var configuration = "Release";
// Macros
Func<string> GetMSBuildLoggerArguments = () => {
    return BuildSystem.TeamCity.IsRunningOnTeamCity ? EnvironmentVariable("MsBuildLogger"): null;
};

Action Abort = () => { throw new Exception("A non-recoverable fatal error occurred."); };
Action<string> TestFailuresAbort = testResult => { throw new Exception(testResult); };
Action NonMacOSAbort = () => { throw new Exception("Running on platforms other macOS is not supported."); };

Action<DirectoryPathCollection> PrintDirectories = (directories) => 
{
	foreach(var directory in directories)
	{
		Information("{0}\n", directory);
	}
};

Action<string, string, Exception> WriteErrorLog = (message, identity, ex) => 
{
	if(isTeamCity) 
	{
		teamCity.BuildProblem(message, identity);
		teamCity.WriteStatus(String.Format("{0}", identity), "ERROR", ex.ToString());
		throw ex;
	}
	else {
		throw new Exception(String.Format("task {0} - {1}", identity, message), ex);
	}
};


Func<FilePath> GetMsBuildPath = () => {

	FilePath msBuildPath = null;

	if(isRunningOnWindows) {
		msBuildPath =  VSWhereLatest().CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");
	}

	return msBuildPath;
};

Func<string, IDisposable> BuildBlock = message => {

	if(BuildSystem.TeamCity.IsRunningOnTeamCity) 
	{
		return BuildSystem.TeamCity.BuildBlock(message);
	}
	
	return null;
	
};

Func<string, IDisposable> Block = message => {

	if(BuildSystem.TeamCity.IsRunningOnTeamCity) 
	{
		BuildSystem.TeamCity.Block(message);
	}

	return null;
};


Action<string> build = (solution) =>
{
    Information("Building {0}", solution);
	using(BuildBlock("Build")) 
	{			
		MSBuild(solution, settings => {
				settings
				.SetConfiguration(configuration);

			if(isRunningOnWindows) {
				settings.ToolPath = GetMsBuildPath();
			}

				settings
    			.WithProperty("NoWarn", "1591") // ignore missing XML doc warnings
				.WithProperty("TreatWarningsAsErrors", treatWarningsAsErrors.ToString())
				.SetNodeReuse(false);

				var msBuildLogger = GetMSBuildLoggerArguments();
			
				if(!string.IsNullOrEmpty(msBuildLogger)) 
				{
					Information("Using custom MSBuild logger: {0}", msBuildLogger);
					settings.ArgumentCustomization = arguments =>
					arguments.Append(string.Format("/logger:{0}", msBuildLogger));
				}
			});
    };		

};

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup((context) =>
{
    Information("Building version {0} of {1}. (isTagged: {2})", informationalVersion, project, isTagged);

		if (isTeamCity)
		{
			Information(
					@"Environment:
					 PullRequest: {0}
					 Build Configuration Name: {1}
					 TeamCity Project Name: {2}
					 Branch: {3}",
					 isPullRequest,
					 buildConfName,
					 projectName,
					 branch
					);
        }
        else
        {
             Information("Not running on TeamCity");
        }		

		DeleteFiles("../src/**/*.tmp");
		DeleteFiles("../src/**/*.tmp.*");

		CleanDirectories(GetDirectories("../src/**/obj"));
		CleanDirectories(GetDirectories("../src/**/bin"));		
		CleanDirectory(Directory(artifactDirectory));		
});

// Teardown((context) =>
// {
//     // Executed AFTER the last task.
// });

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Restore")
	 .Does (() =>
{
	NuGetRestore(buildSolution);
})
.OnError(exception => {
	WriteErrorLog("Build failed", "Build", exception);
});


Task("Build")
	.IsDependentOn("Restore")
    .Does (() =>
{
    build(buildSolution);
})
.OnError(exception => {
	WriteErrorLog("Build failed", "Build", exception);
});

Task("Pack")
	.IsDependentOn("Build")
    .Does (() =>
{
    using(BuildBlock("Pack")) {

		foreach(var package in packageWhitelist)
		{
			// only push the package which was created during this build run.
			var packagePath = "./" + string.Concat(package, ".nuspec");

			// Push the package.
			NuGetPack(packagePath, new NuGetPackSettings() {
				BasePath = "./",
				OutputDirectory  = artifactDirectory

			});
		}
	}
})
.OnError(exception => {
	WriteErrorLog("Build failed", "Build", exception);
});

Task("PublishPackages")
    .IsDependentOn("Pack")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .Does (() =>
{
	using(BuildBlock("Package"))
	{
		string apiKey;
		string source;

		if (isReleaseBranch && !isTagged)
		{
			// Resolve the API key.
			apiKey = EnvironmentVariable("MYGET_APIKEY");
			if (string.IsNullOrEmpty(apiKey))
			{
				throw new Exception("The MYGET_APIKEY environment variable is not defined.");
			}

			source = EnvironmentVariable("MYGET_SOURCE");
			if (string.IsNullOrEmpty(source))
			{
				throw new Exception("The MYGET_SOURCE environment variable is not defined.");
			}
		}
		else 
		{
			// Resolve the API key.
			apiKey = EnvironmentVariable("NUGET_APIKEY");
			if (string.IsNullOrEmpty(apiKey))
			{
				throw new Exception("The NUGET_APIKEY environment variable is not defined.");
			}

			source = EnvironmentVariable("NUGET_SOURCE");
			if (string.IsNullOrEmpty(source))
			{
				throw new Exception("The NUGET_SOURCE environment variable is not defined.");
			}
		}



		// only push whitelisted packages.
		foreach(var package in packageWhitelist)
		{
			// only push the package which was created during this build run.
			var packagePath = artifactDirectory + File(string.Concat(package, ".", nugetVersion, ".nupkg"));

			// Push the package.
			NuGetPush(packagePath, new NuGetPushSettings {
				Source = source,
				ApiKey = apiKey
			});
		}

	};

  
})
.OnError(exception => {
	WriteErrorLog("publishing packages failed", "PublishPackages", exception);
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("PublishPackages")
    .Does (() =>
{

});

// Used to test Setup / Teardown
Task("None")
	.Does(() => {

	});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);