/*
 * https://www.thorntech.com/2016/03/ios-tutorial-make-interactive-slide-menu-swift/
 */
using System;
using UIKit;

namespace MasterDetailPage.MasterDetailPage
{
    internal class MasterViewControllerTransitioningDelegate : UIViewControllerTransitioningDelegate
    {
        private PresentMasterViewControllerAnimator _presentAnimator;
        private DismissMasterViewControllerAnimator _dismissAnimator;
        private Interactor _interactor;
        private bool _disposed;
        private Action _closeMasterAction;
        private UIView _masterRootView;

        public MasterViewControllerTransitioningDelegate(Interactor interactor, UIView masterRootView, Action closeMasterAction)
        {
            _presentAnimator = new PresentMasterViewControllerAnimator(TapToCloseActionHandler, SlideToCloseActionHandler);
            _dismissAnimator = new DismissMasterViewControllerAnimator();
            _interactor = interactor;
            _closeMasterAction = closeMasterAction;
            _masterRootView = masterRootView;
        }

        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
        {
            return _presentAnimator;
        }

        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
        {
            return _dismissAnimator;
        }

        public override IUIViewControllerInteractiveTransitioning GetInteractionControllerForDismissal(IUIViewControllerAnimatedTransitioning animator)
        {
            return _interactor.HasStarted ? _interactor : null;
        }

        public override IUIViewControllerInteractiveTransitioning GetInteractionControllerForPresentation(IUIViewControllerAnimatedTransitioning animator)
        {
            return _interactor.HasStarted ? _interactor : null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _presentAnimator = null;
            _dismissAnimator = null;
            _interactor = null;
            _masterRootView = null;
            _closeMasterAction = null;
        }

        private void TapToCloseActionHandler(UITapGestureRecognizer tapGestureRecognizer)
        {
            _closeMasterAction?.Invoke();
        }

        private void SlideToCloseActionHandler(UIPanGestureRecognizer panGestureRecognizer)
        {
            var translation = panGestureRecognizer.TranslationInView(_masterRootView);
            var progress = MenuHelper.CalculateProgress(translation, _masterRootView.Bounds, Direction.Left);

            MenuHelper.MapGestureStateToInteractor(
                panGestureRecognizer.State,
                progress,
                _interactor,
                _closeMasterAction);
        }

        internal MasterViewControllerTransitioningDelegate(IntPtr handle) : base(handle)
        {
        }
    }
}
