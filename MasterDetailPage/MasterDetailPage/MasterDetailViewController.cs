using System;
using CoreGraphics;
using UIKit;

namespace MasterDetailPage.MasterDetailPage
{
    public class MasterDetailViewController : UIViewController
    {
        private Interactor _interactor = new Interactor();
        private bool _disposed;
        private UIViewController _masterViewController;
        private UIViewController _detailViewController;

        public UIViewController MasterViewController
        {
            get
            {
                return _masterViewController;
            }
            set
            {
                if (_masterViewController == value)
                {
                    return;
                }

                if (_masterViewController != null)
                {
                    _masterViewController.TransitioningDelegate = null;
                }

                _masterViewController = value;

                if (_masterViewController != null)
                {                    
                    _masterViewController.TransitioningDelegate =
                        new MasterViewControllerTransitioningDelegate(
                            _interactor,
                            value.View,
                            CloseMasterViewController);
                }
            }
        }

        public UIViewController DetailViewController
        {
            get
            {
                return _detailViewController;
            }

            set
            {
                if (_detailViewController == value)
                {
                    return;
                }               

                if (value == null)
                {
                    RemoveDetailViewController();
                }

                _detailViewController = value;

                if (_detailViewController != null)
                {
                    AttachDetaiViewController();
                }
            }
        }
        
        public MasterDetailViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var screenEdgePanGestureRecognizer = new UIScreenEdgePanGestureRecognizer()
            {
                Edges = UIRectEdge.Left
            };
            screenEdgePanGestureRecognizer.AddTarget(() => SlideToOpenMasterViewController(screenEdgePanGestureRecognizer));

            View.AddGestureRecognizer(screenEdgePanGestureRecognizer);

            if (DetailViewController == null)
            {
                return;
            }

            AttachDetaiViewController();            
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            CloseMasterViewController();
            base.ViewWillTransitionToSize(toSize, coordinator);
        }

        public void OpenMasterViewController()
        {
            if (MasterViewController == null)
            {
                return;
            }

            PresentModalViewController(MasterViewController, true);
        }

        public void CloseMasterViewController()
        {
            if (PresentedViewController != MasterViewController)
            {
                return;
            }

            DismissViewController(true, null);
        }

        private void AttachDetaiViewController()
        {
            if (DetailViewController.ParentViewController == this)
            {
                return;
            }

            DetailViewController.WillMoveToParentViewController(this);
            View.AddSubview(DetailViewController.View);
            AddChildViewController(DetailViewController);
            DetailViewController.DidMoveToParentViewController(this);
            AttachLeftBarButtonItem();            
        }

        private void AttachLeftBarButtonItem()
        {
            if (!(DetailViewController is UINavigationController navigationController))
            {
                return;
            }

            var masterViewControllerBarButtonItem = new UIBarButtonItem
            {
                Title = "Menu",
                Style = UIBarButtonItemStyle.Plain
            };

            masterViewControllerBarButtonItem.Clicked += MenuBarButtonItem_Clicked;
            navigationController.TopViewController.NavigationItem.LeftBarButtonItem = masterViewControllerBarButtonItem;
        }

        private void RemoveDetailViewController()
        {
            DetailViewController.WillMoveToParentViewController(null);
            DetailViewController.View.RemoveFromSuperview();
            DetailViewController.RemoveFromParentViewController();
            DetailViewController.DidMoveToParentViewController(null);
        }

        private void MenuBarButtonItem_Clicked(object sender, EventArgs e)
        {
            OpenMasterViewController();
        }

        private void SlideToOpenMasterViewController(UIPanGestureRecognizer panGestureRecognizer)
        {
            var translation = panGestureRecognizer.TranslationInView(View);
            var progress = MenuHelper.CalculateProgress(translation, View.Bounds, Direction.Rigth);

            MenuHelper.MapGestureStateToInteractor(
                panGestureRecognizer.State,
                progress,
                _interactor,
                OpenMasterViewController);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _masterViewController = null;
            _detailViewController = null;
            _interactor = null;
        }

        internal MasterDetailViewController(IntPtr handle) : base(handle)
        {
        }        
    }
}
