using System;
using System.ComponentModel;
using System.Windows.Input;
using FormsControls.Base;
using Xamarin.Forms;

namespace Sample
{
    public class AnimationPageViewModel : INotifyPropertyChanged
    {
        private readonly INavigation _navigation;
        private readonly IPageAnimation _animation;
        private AnimationDuration _duration;
        private bool _bounceEffect;
        private Slider _durationSlider;

        public AnimationPageViewModel(INavigation navigation, IPageAnimation animation, string title)
        {
            _navigation = navigation;
            _animation = animation;
            PageTitle = title;
            ButtonClickedCommand = new Command<AnimationSubtype>(OnButtonClicked);
            PageAppearingCommand = new Command<Slider>(OnPageAppearing);
            DurationChangedCommand = new Command<ValueChangedEventArgs> (OnDurationChangedCommand);
            BounceEffectChangedCommand = new Command<Switch> (OnBounceEffectChangedCommand);
            Duration = Utils.GetAnimationDurationProperty();
        }

        private void OnDurationChangedCommand (ValueChangedEventArgs e)
        {
            var newVal = (int)Math.Round(e.NewValue);
            _durationSlider.Value = newVal;
            Duration = (AnimationDuration)newVal;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ShowDefaultButton => _animation.Type == AnimationType.Fade || _animation.Type == AnimationType.Landing || _animation.Type == AnimationType.Rotate;

        public bool ShowBounceSwitcher => _animation.Type != AnimationType.Flip;

        public string PageTitle { get; }

        public ICommand ButtonClickedCommand { get; }

        public ICommand PageAppearingCommand { get; }

        public ICommand DurationChangedCommand { get; }

        public ICommand BounceEffectChangedCommand { get; }

        public AnimationDuration Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    Utils.SetAnimationDurationProperty (value);
                    OnPropertyChanged (nameof (Duration));
                }
            }
        }
       
        private void OnPageAppearing(Slider slider)
        {
            _durationSlider = slider;
            Duration = Utils.GetAnimationDurationProperty();
            _durationSlider.Value = (int)Duration;
        }

        private void OnBounceEffectChangedCommand (Switch switcher)
        {
            _bounceEffect = switcher.IsToggled;
        }

        private void OnButtonClicked(AnimationSubtype animationSubType)
        {
            _animation.Duration = Duration;
            _animation.BounceEffect = _bounceEffect;
            _animation.Subtype = animationSubType;
            _navigation.PushAsync(new SamplePage { Title = animationSubType.ToString(), PageAnimation = _animation });
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}