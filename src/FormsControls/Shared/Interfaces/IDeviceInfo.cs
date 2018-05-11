using System;
using Xamarin.Forms;

namespace FormsControls
{
    public interface IDeviceInfo
    {
        double ScreenHeight { get; }
        double ScreenWidth { get; }
    }
}