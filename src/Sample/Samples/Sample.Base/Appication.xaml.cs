using System;
using Xamarin.Forms;

namespace Sample
{
    public partial class App : Application
    {
        public static bool Limitations { get; set; }
        public App()
        {
            InitializeComponent();
            MainPage = new RootPage();
        }
    }
}

