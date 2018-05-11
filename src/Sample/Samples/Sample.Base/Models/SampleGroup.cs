using System.Collections.Generic;

namespace Sample
{
    public class SampleGroup : List<SingleSample>
    {
        public string Name { get; }

        public SampleGroup(string name)
        {
            Name = name;
        }
    }
}