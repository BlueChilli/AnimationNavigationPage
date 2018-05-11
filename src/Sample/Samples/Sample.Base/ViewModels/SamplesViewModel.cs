using System.Collections.Generic;
using System.Linq;
using FormsControls;

namespace Sample
{
    public class SamplesViewModel
    {
        private List<SampleGroup> _samples;

        public List<SampleGroup> SamplesGroupedByCategory => _samples ?? (_samples = GetSamples());

        private List<SampleGroup> GetSamples ()
        {
            var sampleList = new List<SampleGroup>
            {
                new SampleGroup("SIMPLE TYPES")
                {
                    new SingleSample("Empty Animation", '\xF0C9', typeof(EmptyAnimationsPage), new EmptyPageAnimation()),
                    new SingleSample("Fade Animations", '\xF0CA', typeof(SamplesPage), new FadePageAnimation()),
                    new SingleSample("Slide Animations",'\xF03A', typeof(SamplesPage), new SlidePageAnimation()),
                    new SingleSample("Push Animations", '\xF0C9', typeof(SamplesPage), new PushPageAnimation())
                },
                new SampleGroup("EXTENDED TYPES")
                {
                    new SingleSample("Landing Animations", '\xF0CA', typeof(SamplesPage), new LandingPageAnimation(), true),
                    new SingleSample("Roll Animations", '\xF03A', typeof(SamplesPage), new RollPageAnimation(), true),
                    new SingleSample("Rotate Animations", '\xF022', typeof(SamplesPage), new RotatePageAnimation(), true)
                }
            };
            if (!App.Limitations)
            {
                sampleList.Last ().Insert (0, new SingleSample ("Flip Animations", '\xF022', typeof (SamplesPage), new FlipPageAnimation ()));
            }
            return sampleList;
        }
    }
}