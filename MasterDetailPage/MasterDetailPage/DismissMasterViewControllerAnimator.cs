/*
 * https://www.thorntech.com/2016/03/ios-tutorial-make-interactive-slide-menu-swift/
 */
using System;
using CoreGraphics;
using UIKit;

namespace MasterDetailPage.MasterDetailPage
{
    internal class DismissMasterViewControllerAnimator : UIViewControllerAnimatedTransitioning
    {
        public DismissMasterViewControllerAnimator()
        {
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
            var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            var containerView = transitionContext.ContainerView;

            if (fromViewController == null || toViewController == null || containerView == null)
            {
                return;
            }

            var snapshot = containerView.ViewWithTag(MenuHelper.SnapshotNumber);

            UIView.Animate(
                TransitionDuration(transitionContext),
                () =>
                {
                    if (snapshot == null)
                    {
                        return;
                    }

                    var frame = new CGRect(CGPoint.Empty, UIScreen.MainScreen.Bounds.Size);
                    snapshot.Superview.Frame = frame;
                },
                () =>
                {
                    var transitionComplete = !transitionContext.TransitionWasCancelled;

                    if (transitionComplete && snapshot != null)
                    {
                        containerView.InsertSubviewAbove(toViewController.View, fromViewController.View);
                        snapshot.Superview.RemoveFromSuperview();
                        snapshot.RemoveFromSuperview();    
                    }
                    else if(transitionComplete)
                    {
                        containerView.InsertSubviewAbove(toViewController.View, fromViewController.View);
                    }

                    transitionContext.CompleteTransition(transitionComplete);
                });
        }

        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return MenuHelper.MenuAnimationDuration;
        }

        internal DismissMasterViewControllerAnimator(IntPtr handle) : base(handle)
        {
        }
    }
}
