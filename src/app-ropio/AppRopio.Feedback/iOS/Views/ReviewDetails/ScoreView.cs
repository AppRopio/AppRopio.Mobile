using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Feedback.Core.ViewModels.ReviewDetails;
using AppRopio.Feedback.iOS.Models;
using AppRopio.Feedback.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross;
using ObjCRuntime;
using UIKit;

namespace AppRopio.Feedback.iOS.Views.ReviewDetails
{
    public partial class ScoreView : MvxView, IMvxIosView<IScoreViewModel>
    {
		protected FeedbackThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

		IMvxViewModel IMvxView.ViewModel
		{
			get
			{
				return ViewModel;
			}
			set
			{
				ViewModel = (IScoreViewModel)value;
			}
		}

		private MvxViewModelRequest _request;
		public MvxViewModelRequest Request
		{
			get
			{
				return _request;
			}
			set
			{
				_request = value;
				ViewModel = (IScoreViewModel)((value as MvxViewModelInstanceRequest).ViewModelInstance);
				DataContext = ViewModel;
                InitializeControls();
			}
		}

		public IScoreViewModel ViewModel { get; set; }

		public ScoreView()
        {
            InitializeView();
            this.DelayBind(BindControls);
        }

        protected ScoreView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        #region InitializationControls

        private void InitializeView()
        {
			var arr = NSBundle.MainBundle.LoadNib(nameof(ScoreView), null, null);
			var viewFromNib = Runtime.GetNSObject<ScoreView>(arr.ValueAt(0));

			viewFromNib.Frame = Bounds;
			viewFromNib.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			AddSubview(viewFromNib);

			BackgroundView = viewFromNib.BackgroundView;
			OverlayView = viewFromNib.OverlayView;
			ScoreLabel = viewFromNib.ScoreLabel;
		}

        private void InitializeControls()
        {
            BackgroundView.BackgroundColor = ThemeConfig.ReviewDetails.Score.BackgroundColor.ToUIColor();
            OverlayView.BackgroundColor = ThemeConfig.ReviewDetails.Score.ForegroundColor.ToUIColor();

			SetupOverlay(OverlayView);
            SetupLabel(ScoreLabel);
        }

        #endregion

        protected virtual void SetupOverlay(UIView overlayView)
        {
            UpdateOverlayFrame(overlayView);
        }

        protected virtual void SetupLabel(UILabel label)
		{
            label.SetupStyle(ThemeConfig.ReviewDetails.Score.Label);
		}


		#region BindingControls

		private void BindControls()
        {
			var set = this.CreateBindingSet<ScoreView, IScoreViewModel>();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            BindScore(ScoreLabel, set);

            set.Bind().For("Visibility").To(vm => vm.Score).WithConversion("Visibility");

			set.Apply();
        }

		#endregion

        protected void BindScore(UILabel score, MvxFluentBindingDescriptionSet<ScoreView, IScoreViewModel> set)
		{
            set.Bind(score).To(vm => vm.Score.Value);
		}

        protected virtual void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Score))
            {
                UpdateOverlayFrame(OverlayView);
            }
        }

        protected virtual void UpdateOverlayFrame(UIView overlayView)
        {
            if (ViewModel.Score != null)
                overlayView.ChangeFrame(w: ViewModel.Score.Value * (BackgroundView.Frame.Width / ViewModel.Score.Max));
        }
    }
}
