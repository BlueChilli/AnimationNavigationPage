using System;

namespace FormsControls
{
    public class EmptyPageAnimation : PageAnimation
    {
        public EmptyPageAnimation()
        {
            Type = AnimationType.Empty;
            Duration = 0;
        }
    }
}