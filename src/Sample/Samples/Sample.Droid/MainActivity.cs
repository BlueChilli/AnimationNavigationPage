using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views.Animations;
using System;
using Xamarin.Forms.Platform.Android;

namespace Sample.Droid
{
    [Activity( Theme = "@style/MyThemeAppCompat", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this, bundle);
            FormsControls.Droid.Main.Init(this);
            App.Limitations = this is FormsAppCompatActivity;
            LoadApplication(new App());

        }
    }
}