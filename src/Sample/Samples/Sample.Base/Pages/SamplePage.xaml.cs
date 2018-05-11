using System;
using FormsControls.Base;

namespace Sample
{
    public partial class SamplePage : AnimationPage
    {
        public SamplePage()
        {
            InitializeComponent();
        }

        private void OnNextButtonClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new SamplePage { Title = PageAnimation.Subtype.ToString(), PageAnimation = PageAnimation });
        }

        private void OnBackButtonClicked(object sender, EventArgs args)
        {
            Navigation.PopAsync();
        }

        private void OnPopToPootButtonClicked(object sender, EventArgs args)
        {
            Navigation.PopToRootAsync();
        }

        protected override void OnAnimationStarted (bool isPopAnimation)
        {
            base.OnAnimationStarted (isPopAnimation);
        }

        protected override void OnAnimationFinished (bool isPopAnimation)
        {
            base.OnAnimationFinished (isPopAnimation);
        }
    }
}