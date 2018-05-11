using System;
using FormsControls;
using Xamarin.Forms;

namespace Sample
{
    public partial class EmptyAnimationsPage : ContentPage, IAnimationPage
    {
        private IPageAnimation _pageAnimation;

        public EmptyAnimationsPage()
        {
            InitializeComponent();
        }

        public IPageAnimation PageAnimation =>  _pageAnimation ?? (_pageAnimation = new EmptyPageAnimation());

        public void OnAnimationFinished (bool isPopAnimation)
        {
        }

        public void OnAnimationStarted (bool isPopAnimation)
        {
        }
    }
}