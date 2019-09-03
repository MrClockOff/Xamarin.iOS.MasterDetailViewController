/*
 * https://www.thorntech.com/2016/03/ios-tutorial-make-interactive-slide-menu-swift/
 */
using System;
using CoreGraphics;
using UIKit;

namespace MasterDetailPage.MasterDetailPage
{
    internal enum Direction
    {
        Left,
        Rigth
    }

    internal struct MenuHelper
    {
        private static readonly float MenuPortraitWidth = 0.8f;
        private static readonly float MenuLandscapeWidth = 0.4f;
        public static readonly double MenuAnimationDuration = 0.3;
        public static readonly float PercentTreshold = 0.3f;
        public static readonly int SnapshotNumber = 12345;

        public static float CalculateProgress(CGPoint translationInView, CGRect viewBounds, Direction direction)
        {
            float pointOnAxis = (float)translationInView.X;
            float axisLenght = (float)viewBounds.Width;
            float movementOnAxis = pointOnAxis / axisLenght;
            float positiveMovementOnAxis;
            float positiveMovementOnAxisPercent;

            switch (direction)
            {
                case Direction.Rigth:
                    positiveMovementOnAxis = Math.Max(movementOnAxis, 0.0f);
                    positiveMovementOnAxisPercent = Math.Min(positiveMovementOnAxis, 1.0f);
                    break;
                case Direction.Left:
                    positiveMovementOnAxis = Math.Min(movementOnAxis, 0.0f);
                    positiveMovementOnAxisPercent = -Math.Max(positiveMovementOnAxis, -1.0f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Direction));
            }

            return positiveMovementOnAxisPercent;
        }

        public static void MapGestureStateToInteractor(UIGestureRecognizerState state, float progress, Interactor interactor, Action triggerSegue)
        {
            if (interactor == null)
            {
                return;
            }

            switch (state)
            {
                case UIGestureRecognizerState.Began:
                    interactor.HasStarted = true;
                    triggerSegue?.Invoke();
                    break;
                case UIGestureRecognizerState.Changed:
                    interactor.ShouldFinish = progress > PercentTreshold;
                    interactor.UpdateInteractiveTransition(progress);
                    break;
                case UIGestureRecognizerState.Cancelled:
                    interactor.HasStarted = false;
                    interactor.CancelInteractiveTransition();
                    break;
                case UIGestureRecognizerState.Ended:
                    interactor.HasStarted = false;
                    if (interactor.ShouldFinish)
                    {
                        interactor.FinishInteractiveTransition();
                    }
                    else
                    {
                        interactor.CancelInteractiveTransition();
                    }
                    break;
                default:
                    break;
            }
        }

        public static float MenuWidth
        {
            get
            {
                if (UIDevice.CurrentDevice.Orientation.IsPortrait())
                {
                    return MenuPortraitWidth;
                }

                return MenuLandscapeWidth;
            }
        }
    }
}
