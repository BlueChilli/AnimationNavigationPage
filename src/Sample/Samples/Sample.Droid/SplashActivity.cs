

using Android.App;
using Android.Content;
using Android.OS;

namespace Sample.Droid
{
    [Activity(Label = "AnimationNavPage", Icon = "@drawable/logo", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]          
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(new Intent(this, typeof(MainActivity)));
            Finish();
        }
    }
}

