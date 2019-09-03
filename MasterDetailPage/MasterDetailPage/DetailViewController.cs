using System;
using CoreGraphics;
using UIKit;

namespace MasterDetailPage.MasterDetailPage
{
    public class DetailViewController : UIViewController
    {
        public DetailViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.Red;

            var button = new UIButton(new CGRect(100, 100, 50, 50));
            button.SetTitle("Next", UIControlState.Normal);
            button.TouchUpInside += Button_TouchUpInside;

            View.AddSubview(button);
        }

        private void Button_TouchUpInside(object sender, EventArgs e)
        {
            ShowViewController(new NextViewController(UIColor.Yellow), null);
        }

        internal DetailViewController(IntPtr handle) : base(handle)
        {
        }

        private class NextViewController : UIViewController
        {
            private UIColor _color;
            private bool _disposed;

            public NextViewController(UIColor color)
            {
                _color = color;
            }

            public override void ViewDidLoad()
            {
                base.ViewDidLoad();
                View.BackgroundColor = _color;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (_disposed)
                {
                    return;
                }

                _disposed = true;
                _color = null;
            }

            internal NextViewController(IntPtr handle) : base(handle)
            {
            }
        }
    }
}
