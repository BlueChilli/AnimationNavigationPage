using System;
using FormsControls;
using Xamarin.Forms;

namespace Sample
{
    public class SingleSample
    {
        public SingleSample(string name, char icon, Type pageType, IPageAnimation animation, bool showBadge = false)
        {
            Name = name;
            Icon = icon;
            Animation = animation;
            SamplePageType = pageType;
            ShowBadge = showBadge;
        }

        public bool ShowBadge { get; }

        public string Name { get; }

        public char Icon { get; }

        public Type SamplePageType { get; }

        public IPageAnimation Animation { get; }

        public Page CreateAnimationPage()
        {
            var page = Activator.CreateInstance(SamplePageType) as Page;
            page.BindingContext = new AnimationPageViewModel(page.Navigation, Animation, Name);
            return page;
        }
    }
}