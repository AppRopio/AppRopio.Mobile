using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Feedback.Core.ViewModels.Items.TotalScore;
using AppRopio.Feedback.iOS.Models;
using AppRopio.Feedback.iOS.Services;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.Feedback.iOS.Views.ReviewPost.Cell.TotalScore
{
    public partial class TotalScoreCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("TotalScoreCell");
        public static readonly UINib Nib;

        private UIView _activeView;

        protected FeedbackThemeConfig ThemeConfig { get { return Mvx.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

        static TotalScoreCell()
        {
            Nib = UINib.FromName("TotalScoreCell", NSBundle.MainBundle);
        }

        protected TotalScoreCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

            var itemsCount = ScoreView.Subviews.Length;
            //по непонятной причине ScoreView никак не хочет получать здесь правильную ширину, поэтому считаем сами
            var space = (float)(this.Frame.Width - ScoreView.Frame.Left * 2 - 40 * itemsCount) / (itemsCount - 1);

			float x = 0;

            for (int i = 0; i < ScoreView.Subviews.Length; i++)
            {
                var subview = ScoreView.Subviews[i];
                subview.ChangeFrame(x: x);

				x = (float)subview.Frame.Right + space;
            }
		}

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupTitle(TitleLabel);
            CreateItems();
        }

        protected virtual void SetupTitle(UILabel titleLabel)
        {
            titleLabel.SetupStyle(ThemeConfig.ReviewPost.Title);
        }

        protected virtual void CreateItems()
        {
            var vm = ((ITotalScoreItemVm)this.DataContext);
            for (var i = 1; i <= vm.MaxValue; i++)
            {
                var scoreItemView = new UIView(frame: new CGRect(0, 0, ScoreView.Frame.Height, ScoreView.Frame.Height));
                scoreItemView.Tag = i;
                scoreItemView.BackgroundColor = ThemeConfig.ReviewPost.Score.BackgroundColor.ToUIColor();
                scoreItemView.Layer.CornerRadius = scoreItemView.Frame.Height / 2;
                scoreItemView.ActionOnTap(() => 
                {
                    SelectView(scoreItemView);
                });
                ScoreView.AddSubview(scoreItemView);

                var scoreItemLabel = new AppRopio.Base.iOS.Controls.ARLabel(frame: scoreItemView.Bounds);
                scoreItemLabel.Text = i.ToString();
                scoreItemLabel.TextAlignment = UITextAlignment.Center;
                scoreItemLabel.SetupStyle(ThemeConfig.ReviewPost.ScoreValue);
                scoreItemView.AddSubview(scoreItemLabel);

                if (i == vm.Value)
                    SelectView(scoreItemView);
            }
        }

        protected virtual void SelectView(UIView view)
        {
            if (_activeView != null)
            {
                _activeView.BackgroundColor = ThemeConfig.ReviewPost.Score.BackgroundColor.ToUIColor();
            }

            view.BackgroundColor = ThemeConfig.ReviewPost.Score.ForegroundColor.ToUIColor();
            _activeView = view;
            ((ITotalScoreItemVm)DataContext).Value = (int)view.Tag;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var bindingSet = this.CreateBindingSet<TotalScoreCell, ITotalScoreItemVm>();

            BindTitle(TitleLabel, bindingSet);

            bindingSet.Apply();
        }

		protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<TotalScoreCell, ITotalScoreItemVm> set)
		{
            set.Bind(title).To(vm => vm.Title);
		}

        #endregion
    }
}
