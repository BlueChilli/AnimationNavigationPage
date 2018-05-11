using System;
using FormsControls;

namespace FormsControls.Droid
{
    public static class SlowAnimationsId
    {
        public static AnimationsId GetAnimationsId(IPageAnimation animation, bool isPush)
        {
            if (animation.Type == AnimationType.Push)
            {
                return GetPushAnimationsId(animation, isPush);
            }
            if (animation.Type == AnimationType.Slide)
            {
                return GetSlideAnimationsId(animation, isPush);
            }
            if (animation.Type == AnimationType.Fade)
            {
                return GetFadeAnimationsId(animation, isPush);
            }
            if (animation.Type == AnimationType.Landing)
            {
                return GetLandingAnimationsId(animation, isPush);
            }
            if (animation.Type == AnimationType.Roll)
            {
                return GetRollAnimationsId(animation, isPush);
            }
            if (animation.Type == AnimationType.Rotate)
            {
                return GetRotateAnimationsId(animation, isPush);
            }
            return AnimationsId.Empty;
        }

        private static AnimationsId GetPushAnimationsId(IPageAnimation animation, bool isPush)
        {
            switch (animation.Subtype)
            {
                case AnimationSubtype.FromRight:
                    if (animation.BounceEffect && isPush)
                        return AnimationsId.Create(Resource.Animation.enter_from_right_short_bounce, Resource.Animation.exit_to_left_short_bounce);
                    return isPush ? AnimationsId.Create(Resource.Animation.enter_from_right_short, Resource.Animation.exit_to_left_short) :
                                    AnimationsId.Create(Resource.Animation.enter_from_left_short, Resource.Animation.exit_to_right_short);
                case AnimationSubtype.FromTop:
                    if (animation.BounceEffect && isPush)
                        return AnimationsId.Create(Resource.Animation.enter_from_top_short_bounce, Resource.Animation.exit_to_bottom_short_bounce);
                    return isPush ? AnimationsId.Create(Resource.Animation.enter_from_top_short, Resource.Animation.exit_to_bottom_short) :
                                    AnimationsId.Create(Resource.Animation.enter_from_bottom_short, Resource.Animation.exit_to_top_short);
                case AnimationSubtype.FromBottom:
                    if (animation.BounceEffect && isPush)
                        return AnimationsId.Create(Resource.Animation.enter_from_bottom_short_bounce, Resource.Animation.exit_to_top_short_bounce);
                    return isPush ? AnimationsId.Create(Resource.Animation.enter_from_bottom_short, Resource.Animation.exit_to_top_short) :
                                    AnimationsId.Create(Resource.Animation.enter_from_top_short, Resource.Animation.exit_to_bottom_short);
                default:
                    if (animation.BounceEffect && isPush)
                        return AnimationsId.Create(Resource.Animation.enter_from_left_short_bounce, Resource.Animation.exit_to_right_short_bounce);
                    return isPush ? AnimationsId.Create(Resource.Animation.enter_from_left_short, Resource.Animation.exit_to_right_short) :
                                    AnimationsId.Create(Resource.Animation.enter_from_right_short, Resource.Animation.exit_to_left_short);
            }
        }

        private static AnimationsId GetSlideAnimationsId(IPageAnimation animation, bool isPush)
        {
            switch (animation.Subtype)
            {
                case AnimationSubtype.FromRight:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_from_right_short_bounce : Resource.Animation.enter_from_right_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_to_right_short);
                case AnimationSubtype.FromTop:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_from_top_short_bounce : Resource.Animation.enter_from_top_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_to_top_short);
                case AnimationSubtype.FromBottom:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_from_bottom_short_bounce : Resource.Animation.enter_from_bottom_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_to_bottom_short);
                default:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_from_left_short_bounce : Resource.Animation.enter_from_left_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_to_left_short);
            }
        }

        private static AnimationsId GetFadeAnimationsId(IPageAnimation animation, bool isPush)
        {
            switch (animation.Subtype)
            {
                case AnimationSubtype.FromLeft:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_scale_from_left_short_bounce : Resource.Animation.enter_scale_from_left_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_scale_from_left_short);
                case AnimationSubtype.FromRight:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_scale_from_right_short_bounce : Resource.Animation.enter_scale_from_right_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_scale_from_right_short);
                case AnimationSubtype.FromTop:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_scale_from_top_short_bounce : Resource.Animation.enter_scale_from_top_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_scale_from_top_short);
                case AnimationSubtype.FromBottom:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_scale_from_bottom_short_bounce : Resource.Animation.enter_scale_from_bottom_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_scale_from_bottom_short);
                default:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_scale_short_bounce : Resource.Animation.enter_scale_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_scale_short);
            }
        }

        private static AnimationsId GetLandingAnimationsId(IPageAnimation animation, bool isPush)
        {
            switch (animation.Subtype)
            {
                case AnimationSubtype.FromLeft:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_landing_from_left_short_bounce : Resource.Animation.enter_landing_from_left_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_landing_from_left_short);
                case AnimationSubtype.FromRight:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_landing_from_right_short_bounce : Resource.Animation.enter_landing_from_right_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_landing_from_right_short);
                case AnimationSubtype.FromTop:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_landing_from_top_short_bounce : Resource.Animation.enter_landing_from_top_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_landing_from_top_short);
                case AnimationSubtype.FromBottom:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_landing_from_bottom_short_bounce : Resource.Animation.enter_landing_from_bottom_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_landing_from_bottom_short);
                default:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_landing_short_bounce : Resource.Animation.enter_landing_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_landing_short);
            }
        }

        private static AnimationsId GetRollAnimationsId(IPageAnimation animation, bool isPush)
        {
            switch (animation.Subtype)
            {
                case AnimationSubtype.FromRight:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_roll_from_right_short_bounce : Resource.Animation.enter_roll_from_right_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_roll_to_right_short);
                case AnimationSubtype.FromTop:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_roll_from_top_short_bounce : Resource.Animation.enter_roll_from_top_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_roll_to_top_short);
                case AnimationSubtype.FromBottom:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_roll_from_bottom_short_bounce : Resource.Animation.enter_roll_from_bottom_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_roll_to_bottom_short);
                default:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_roll_from_left_short_bounce : Resource.Animation.enter_roll_from_left_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_roll_to_left_short);
            }
        }

        private static AnimationsId GetRotateAnimationsId(IPageAnimation animation, bool isPush)
        {
            switch (animation.Subtype)
            {
                case AnimationSubtype.FromRight:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_rotate_from_right_short_bounce : Resource.Animation.enter_rotate_from_right_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_rotate_to_right_short);
                case AnimationSubtype.FromTop:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_rotate_from_top_short_bounce : Resource.Animation.enter_rotate_from_top_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_rotate_to_top_short);
                case AnimationSubtype.FromBottom:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_rotate_from_bottom_short_bounce : Resource.Animation.enter_rotate_from_bottom_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_rotate_to_bottom_short);
                case AnimationSubtype.FromLeft:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_rotate_from_left_short_bounce : Resource.Animation.enter_rotate_from_left_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_rotate_to_left_short);
                default:
                    return isPush ? AnimationsId.Create(animation.BounceEffect ? Resource.Animation.enter_rotate_short_bounce : Resource.Animation.enter_rotate_short,  Resource.Animation.empty_animation_short) :
                                    AnimationsId.Create( Resource.Animation.empty_animation_short, Resource.Animation.exit_rotate_short);
            }
        }
    }
}