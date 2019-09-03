using System;
using UIKit;

namespace MasterDetailPage.MasterDetailPage
{
    public class MenuViewController : UIViewController
    {
        public MenuViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.Green;
        }

        internal MenuViewController(IntPtr handle) : base(handle)
        {
        }
    }
}
