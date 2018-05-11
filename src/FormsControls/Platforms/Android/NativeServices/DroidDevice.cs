using Android.Content;
using Android.Util;
using FormsControls;
using FormsControls.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidDevice))]
namespace FormsControls.Droid
{
    public class DroidDevice : IDeviceInfo
    {
        private static Context Context => Main.Context;

        public static DisplayMetrics Metrics => Context.Resources.DisplayMetrics;

        private static float ActionBarHeight
        {
            get
            {
                var styledAttributes = Context.Theme.ObtainStyledAttributes( new[] { Android.Resource.Attribute.ActionBarSize });
                var actionbarHeight = styledAttributes.GetDimension(0, 0);
                styledAttributes.Recycle();
                return actionbarHeight;
            }
        }

        public double ScreenHeight => Metrics.HeightPixels - ActionBarHeight;

        public double ScreenWidth => Metrics.WidthPixels;
    }
}