using System;
using FormsControls;
using Xamarin.Forms;

namespace Sample
{
    public static class Utils
    {
        public static void SetAnimationDurationProperty(AnimationDuration value)
        {
            Application.Current.Properties[Constants.AnimationDurationKey] = (int)value;
        }

        public static AnimationDuration GetAnimationDurationProperty()
        {
            if (Application.Current.Properties.ContainsKey(Constants.AnimationDurationKey))
            {
                return (AnimationDuration)Convert.ToInt32(Application.Current.Properties[Constants.AnimationDurationKey]); 
            }
            SetAnimationDurationProperty(Constants.DefaultDuration);
            return Constants.DefaultDuration;
        }
    }
}

