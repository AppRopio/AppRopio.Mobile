using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Feedback.Core.ViewModels.Items.Score;
using AppRopio.Feedback.iOS.Models;
using AppRopio.Feedback.iOS.Services;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.Feedback.iOS.Views.ReviewPost.Cell.Score
{
    public partial class ScoreCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ScoreCell");
        public static readonly UINib Nib;

		protected FeedbackThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

		static ScoreCell()
        {
            Nib = UINib.FromName("ScoreCell", NSBundle.MainBundle);
        }

        protected ScoreCell(IntPtr handle) : base(handle)
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

            var vm = ((IScoreItemVm)this.DataContext);
			for (int i = 0; i < ScoreView.Subviews.Length; i++)
			{
				var subview = ScoreView.Subviews[i];
				subview.ChangeFrame(x: x);

				x = (float)subview.Frame.Right + space;

                if (i + 1 == vm.Value)
                {
                    BackgroundHighlightWidthConstraint.Constant = subview.Frame.Right;
                }
			}
		}

		#region InitializationControls

		protected virtual void InitializeControls()
		{
			BackgroundHighlightView.Layer.CornerRadius = BackgroundHighlightView.Frame.Height / 2;

			SetupTitle(TitleLabel);
            SetupBackgroundView(BackgroundView);
            SetupScoreView(ScoreView);
		}

		protected virtual void SetupTitle(UILabel titleLabel)
		{
			titleLabel.SetupStyle(ThemeConfig.ReviewPost.Title);
		}

		protected virtual void SetupBackgroundView(UIView backgroundView)
		{
            backgroundView.BackgroundColor = ThemeConfig.ReviewPost.Score.BackgroundColor.ToUIColor();
            backgroundView.Layer.CornerRadius = backgroundView.Frame.Height / 2;
		}

        protected virtual void SetupScoreView(UIView scoreView)
        {
			CreateItems();
        }

        protected virtual void CreateItems()
		{
            var vm = ((IScoreItemVm)this.DataContext);
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
            for (int i = 0; i < ScoreView.Subviews.Length; i++)
            {
                var subview = ScoreView.Subviews[i];
                if (subview.Tag <= view.Tag)
					subview.BackgroundColor = ThemeConfig.ReviewPost.Score.ForegroundColor.ToUIColor();
                else
                    subview.BackgroundColor = ThemeConfig.ReviewPost.Score.BackgroundColor.ToUIColor();
            }

            BackgroundHighlightView.BackgroundColor = ThemeConfig.ReviewPost.Score.ForegroundColor.ToUIColor();
            BackgroundHighlightWidthConstraint.Constant = view.Frame.Right;
            //BackgroundHighlightView.ChangeFrame(x: BackgroundView.Frame.Left, y: BackgroundView.Frame.Top, w: view.Frame.Right, h: BackgroundView.Frame.Height);

			((IScoreItemVm)DataContext).Value = (int)view.Tag;
		}

		#endregion

		#region BindingControls

		protected virtual void BindControls()
		{
			var bindingSet = this.CreateBindingSet<ScoreCell, IScoreItemVm>();

			BindTitle(TitleLabel, bindingSet);

			bindingSet.Apply();
		}

		protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<ScoreCell, IScoreItemVm> set)
		{
			set.Bind(title).To(vm => vm.Title);
		}

		#endregion
	}
}
