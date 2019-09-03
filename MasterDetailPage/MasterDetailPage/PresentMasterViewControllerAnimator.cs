/*
 * https://www.thorntech.com/2016/03/ios-tutorial-make-interactive-slide-menu-swift/
 */
using System;
using CoreGraphics;
using UIKit;

namespace MasterDetailPage.MasterDetailPage
{
    internal class PresentMasterViewControllerAnimator : UIViewControllerAnimatedTransitioning
    {
        private bool _disposed;
        /// <summary>
        /// Snapshot container view is required to handle tap outside and pan gestures.
        /// It will be added into container view when master view controller will
        /// be shown and will be removed after dismissing master view controller.
        /// </summary>
        private UIView _snapshotContainerView;

        public PresentMasterViewControllerAnimator(Action<UITapGestureRecognizer> tapToCloseAction, Action<UIPanGestureRecognizer> slideToCloseAction)
        {
            _snapshotContainerView = new UIView(CGRect.Empty)
            {
                BackgroundColor = UIColor.Clear,
                ClipsToBounds = false
            };

            var tapGestureRecognizer = new UITapGestureRecognizer(tapToCloseAction);
            var panGestureRecognizer = new UIPanGestureRecognizer(slideToCloseAction);

            _snapshotContainerView.AddGestureRecognizer(tapGestureRecognizer);
            _snapshotContainerView.AddGestureRecognizer(panGestureRecognizer);
        }

        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return MenuHelper.MenuAnimationDuration;
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

            containerView.InsertSubviewBelow(toViewController.View, fromViewController.View);

            var snapshot = fromViewController.View.SnapshotView(false);
            snapshot.Tag = MenuHelper.SnapshotNumber;
            snapshot.UserInteractionEnabled = false;
            snapshot.Layer.ShadowOpacity = 0.7f;
            _snapshotContainerView.Frame = snapshot.Frame;
            // Workaround for build up collection of snapshots inside snapshot container view
            foreach(var subView in _snapshotContainerView.Subviews)
            {
                subView.RemoveFromSuperview();
            }
            _snapshotContainerView.AddSubview(snapshot);
            containerView.InsertSubviewAbove(_snapshotContainerView, toViewController.View);
            fromViewController.View.Hidden = true;

            UIView.Animate(
                TransitionDuration(transitionContext),
                () =>
                {
                    var center = _snapshotContainerView.Center;

                    center.X += UIScreen.MainScreen.Bounds.Width * MenuHelper.MenuWidth;
                    _snapshotContainerView.Center = center;
                },
                () =>
                {
                    fromViewController.View.Hidden = false;
                    transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled);
                });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _snapshotContainerView = null;
        }

        internal PresentMasterViewControllerAnimator(IntPtr handle) : base(handle)
        {
        }
    }
}
