using System;
using FormsControls;
using Xamarin.Forms;

namespace Sample
{
    public partial class NoAnimationPage : ContentPage, IAnimationPage
    {
        public NoAnimationPage()
        {
            InitializeComponent();
        }

        public IPageAnimation PageAnimation { get; } = new EmptyPageAnimation();

        public void OnAnimationFinished(bool isPopAnimation)
        {
            throw new NotImplementedException();
        }

        public void OnAnimationStarted(bool isPopAnimation)
        {
            throw new NotImplementedException();
        }

        private void OnButtonClicked(object sender, EventArgs args)
        {
            Navigation.PopAsync();
        }
    }
}