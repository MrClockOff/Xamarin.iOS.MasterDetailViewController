/*
 * https://www.thorntech.com/2016/03/ios-tutorial-make-interactive-slide-menu-swift/
 */
using System;
using UIKit;

namespace MasterDetailPage.MasterDetailPage
{
    internal class Interactor : UIPercentDrivenInteractiveTransition
    {
        public bool HasStarted { get; set; }
        public bool ShouldFinish { get; set; }

        internal Interactor(IntPtr handle) : base(handle)
        {
        }

        public Interactor()
        {
        }
    }
}
