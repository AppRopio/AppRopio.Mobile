using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;
using System;

namespace AppRopio.Base.iOS.ViewSources
{
    public class ScrolledEventsTableViewSource : MvxStandardTableViewSource
    {
        public event EventHandler OnDecelerationStarted;
        public event EventHandler OnDecelerationEnded;

        public event EventHandler OnDraggingStarted;
        public event EventHandler<DraggingEventArgs> OnDraggingEnded;

        public event EventHandler OnScrolled;

        public ScrolledEventsTableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        public ScrolledEventsTableViewSource(UITableView tableView, NSString cellIdentifier)
            : base(tableView, cellIdentifier)
        {
        }

        public override void DraggingStarted(UIScrollView scrollView)
        {
            OnDecelerationStarted?.Invoke(scrollView, EventArgs.Empty);
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            OnDecelerationEnded?.Invoke(scrollView, EventArgs.Empty);
        }

        public override void DecelerationStarted(UIScrollView scrollView)
        {
            OnDraggingStarted?.Invoke(scrollView, EventArgs.Empty);
        }

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            OnDraggingEnded?.Invoke(scrollView, new DraggingEventArgs(willDecelerate));
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            OnScrolled?.Invoke(scrollView, EventArgs.Empty);
        }
    }
}
