using System;
using System.Collections.Specialized;
using System.Windows.Input;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace AppRopio.Test.iOS
{
    // This class is never actually executed, but when Xamarin linking is enabled it does how to ensure types and properties
    // are preserved in the deployed app
    [Foundation.Preserve(AllMembers = true)]
    public class LinkerPleaseInclude
    {
        public void Include(ConsoleColor color)
        {
            Console.Write("");
            Console.WriteLine("");
            color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        public void Include(MvxTaskBasedBindingContext c)
        {
            c.Dispose();
            var c2 = new MvxTaskBasedBindingContext();
            c2.ClearAllBindings();
            c2.Dispose();
        }

        public void Include(UIButton uiButton)
        {
            uiButton.TouchUpInside += (s, e) =>
                uiButton.SetTitle(uiButton.Title(UIControlState.Normal), UIControlState.Normal);
            uiButton.Selected = uiButton.Selected;
        }

        public void Include(UIBarButtonItem barButton)
        {
            barButton.Clicked += (s, e) =>
                barButton.Title = barButton.Title + "";
            barButton.WeakSubscribe(nameof(barButton.Clicked), null);
        }

        public void Include(UITextField textField)
        {
            textField.Text = textField.Text + "";
            textField.EditingChanged += (sender, args) => { textField.Text = ""; };
            textField.AttributedText = new NSAttributedString(textField.AttributedText.ToString() + "");
            textField.WeakSubscribe(nameof(textField.EditingChanged), null);
        }

        public void Include(UITextView textView)
        {
            textView.Text = textView.Text + "";
            textView.Changed += (sender, args) => { textView.Text = ""; };
            textView.LayoutManager.TextStorage.DidProcessEditing += (sender, e) => { };
            textView.LayoutManager.TextStorage.WeakSubscribe<NSTextStorage, NSTextStorageEventArgs>(nameof(textView.LayoutManager.TextStorage.DidProcessEditing), null);
        }

        public void Include(NSLayoutManager layoutManager)
        {
            layoutManager.TextStorage.DidProcessEditing += (sender, e) => { };
            layoutManager.TextStorage.WeakSubscribe<NSTextStorage, NSTextStorageEventArgs>(nameof(layoutManager.TextStorage.DidProcessEditing), null);
        }

        public void Include(NSTextStorage textStorage)
        {
            textStorage.DidProcessEditing += (sender, e) => { };
            textStorage.WeakSubscribe<NSTextStorage, NSTextStorageEventArgs>(nameof(textStorage.DidProcessEditing), null);
        }

        public void Include(UILabel label)
        {
            label.Text = label.Text + "";
            label.AttributedText = new NSAttributedString(label.AttributedText.ToString() + "");
        }

        public void Include(UIImageView imageView)
        {
            imageView.Image = new UIImage(imageView.Image.CGImage);
        }

        public void Include(UIDatePicker datePicker)
        {
            datePicker.Date = datePicker.Date.AddSeconds(1);
            datePicker.ValueChanged += (sender, args) => { datePicker.Date = NSDate.DistantFuture; };
            datePicker.WeakSubscribe<UIDatePicker, EventArgs>(nameof(datePicker.ValueChanged), null);
        }

        public void Include(UISlider slider)
        {
            slider.Value = slider.Value + 1;
            slider.ValueChanged += (sender, args) => { slider.Value = 1; };
        }

        public void Include(UIProgressView progress)
        {
            progress.Progress = progress.Progress + 1;
        }

        public void Include(UISwitch sw)
        {
            sw.On = !sw.On;
            sw.ValueChanged += (sender, args) => { sw.On = false; };
            sw.WeakSubscribe(nameof(sw.ValueChanged), null);
        }

        public void Include(MvxViewController vc)
        {
            vc.Title = vc.Title + "";
        }

        public void Include(UIStepper s)
        {
            s.Value = s.Value + 1;
            s.ValueChanged += (sender, args) => { s.Value = 0; };
        }

        public void Include(UIPageControl pageControl)
        {
            pageControl.Pages = pageControl.Pages + 1;
            pageControl.ValueChanged += (sender, args) => { pageControl.Pages = 0; };
            pageControl.WeakSubscribe(nameof(pageControl.ValueChanged), null);
        }

        public void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged += (s, e) => { var test = $"{e.Action}{e.NewItems}{e.NewStartingIndex}{e.OldItems}{e.OldStartingIndex}"; };
        }

        public void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) => { if (command.CanExecute(null)) command.Execute(null); };
        }

        public void Include(MvvmCross.Platform.IoC.MvxPropertyInjector injector)
        {
            injector = new MvvmCross.Platform.IoC.MvxPropertyInjector();
        }

        public void Include(System.ComponentModel.INotifyPropertyChanged changed)
        {
            changed.PropertyChanged += (sender, e) => { var test = e.PropertyName; };
        }

        public void Include(MvxWeakEventSubscription<object, EventArgs> subscription)
        {
            typeof(object).GetEvent("");
            subscription = new MvxWeakEventSubscription<object, EventArgs>(null, "", null);
            subscription.Dispose();
        }

        public void Include(MvxNotifyPropertyChangedEventSubscription subsctiption)
        {
            subsctiption = new MvxNotifyPropertyChangedEventSubscription(null, null);
            subsctiption.Dispose();
        }

        public void Include(MvxCanExecuteChangedEventSubscription subsctiption)
        {
            subsctiption = new MvxCanExecuteChangedEventSubscription(null, null);
            subsctiption.Dispose();
        }

        public void Include(MvxGeneralEventSubscription subsctiption)
        {
            subsctiption = new MvxGeneralEventSubscription(null, null, null);
            subsctiption.Dispose();
        }

        public void Include(UISearchBar searchBar)
        {
            searchBar.TextChanged += (sender, e) => { };
            searchBar.WeakSubscribe<UISearchBar, UISearchBarTextChangedEventArgs>(nameof(searchBar.TextChanged), null);
        }
    }
}

