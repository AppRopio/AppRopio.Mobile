using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.PresentationHints;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Feedback.Core.Models.Bundle;
using AppRopio.Feedback.Core.ViewModels.Items;
using AppRopio.Feedback.Core.ViewModels.ReviewPost.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.ReviewPost
{
    public class ReviewPostViewModel : BaseViewModel, IReviewPostViewModel
    {
        #region Fields

        #endregion

        #region Commands

        private IMvxCommand _postCommand;
        public IMvxCommand PostCommand
        {
            get
            {
                return _postCommand ?? (_postCommand = new MvxAsyncCommand(OnPostReview));
            }
        }

        #endregion

        #region Properties

        protected string ReviewId { get; set; }

        protected string ProductId { get; set; }

        protected string ProductGroupId { get; set; }

        private MvxObservableCollection<IReviewParameterItemVm> _reviewItems;
        public MvxObservableCollection<IReviewParameterItemVm> ReviewItems
        {
            get { return _reviewItems; }
            set
            {
                if (SetProperty(ref _reviewItems, value))
                    RaisePropertyChanged(nameof(CanPostReview));
            }
        }

        public bool CanPostReview
        {
            get
            {
                return ReviewItems != null && ReviewItems.Count > 0;
            }
        }

        #endregion

        #region Services

        protected IReviewPostVmService VmService { get { return Mvx.IoCProvider.Resolve<IReviewPostVmService>(); } }

        #endregion

        #region Protected

        protected virtual async Task LoadContent()
        {
            Loading = true;

            var reviewItems = await VmService.LoadReviewApplication(ReviewId, ProductGroupId, ProductId);
            InvokeOnMainThread(() => ReviewItems = reviewItems);

            Loading = false;
        }

        protected virtual async Task OnPostReview()
        {
            Loading = true;

            if (await VmService.PostReview(ReviewId, ProductGroupId, ProductId, ReviewItems.OfType<IReviewParameterItemVm>().ToList()))
            {
                UserDialogs.Alert(LocalizationService.GetLocalizableString(FeedbackConstants.RESX_NAME, "ReviewPost_Sent"));

                await NavigationVmService.ChangePresentation(new NavigateToDefaultViewModelHint());
            }
            else
            {
                UserDialogs.Alert(LocalizationService.GetLocalizableString(FeedbackConstants.RESX_NAME, "ReviewPost_FailedToSend"));
            }

            Loading = false;
        }

        #endregion

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var navigationBundle = parameters.ReadAs<ReviewBundle>();
            this.InitFromBundle(navigationBundle);
        }

        protected virtual void InitFromBundle(ReviewBundle parameters)
        {
            VmNavigationType = parameters.NavigationType == NavigationType.None ?
                                                            NavigationType.ClearAndPush :
                                                            parameters.NavigationType;

            ReviewId = parameters.ReviewId;
            ProductId = parameters.ProductId;
            ProductGroupId = parameters.ProductGroupId;
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        #endregion
    }
}