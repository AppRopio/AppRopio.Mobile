using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Feedback.Core.Models.Bundle;
using AppRopio.Feedback.Core.ViewModels.ReviewDetails.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Feedback.Core.ViewModels.ReviewDetails
{
    public class ReviewDetailsViewModel : BaseViewModel, IReviewDetailsViewModel
    {
        #region Fields

        #endregion

        #region Commands

        private IMvxCommand _productDetailsCommand;
        public IMvxCommand ProductDetailsCommand
        {
            get
            {
                return _productDetailsCommand ?? (_productDetailsCommand = new MvxCommand(OnProductDetailsRequested));
            }
        }

        private IMvxCommand _editReviewCommand;
        public IMvxCommand EditReviewCommand
        {
            get
            {
                return _editReviewCommand ?? (_editReviewCommand = new MvxCommand(OnEditReview));
            }
        }

        private IMvxCommand _deleteReviewCommand;
        public IMvxCommand DeleteReviewCommand
        {
            get
            {
                return _deleteReviewCommand ?? (_deleteReviewCommand = new MvxCommand(OnDeleteReview));
            }
        }

        #endregion

        #region Properties

        protected string ReviewId { get; set; }

        private bool _canEdit;
        public bool CanEdit
        {
            get { return _canEdit; }
            set { SetProperty(ref _canEdit, value); }
        }

        private AppRopio.Models.Feedback.Responses.ReviewDetails _reviewDetails;
        protected AppRopio.Models.Feedback.Responses.ReviewDetails ReviewDetails
        {
            get { return _reviewDetails; }
            set
            {
                if (SetProperty(ref _reviewDetails, value))
                {
                    RaisePropertyChanged(nameof(ProductImageUrl));
                    RaisePropertyChanged(nameof(ProductTitle));
                    RaisePropertyChanged(nameof(Date));
                    RaisePropertyChanged(nameof(Text));
                }
            }
        }

        public string ProductImageUrl
        {
            get { return ReviewDetails.ProductImageUrl; }
        }

        public string ProductTitle
        {
            get { return ReviewDetails.ProductTitle; }
        }

        public DateTime Date
        {
            get { return ReviewDetails.Date; }
        }

        public string Text
        {
            get { return ReviewDetails.Text; }
        }

        public IScoreViewModel ScoreViewModel { get; protected set; }

        #endregion

        #region Services

        protected IReviewDetailsVmService VmService { get { return Mvx.Resolve<IReviewDetailsVmService>(); } }

        #endregion

        #region Protected

        protected virtual async Task LoadContent()
        {
            Loading = true;

            var reviewDetails = await VmService.LoadReviewDetails(ReviewId);

            InvokeOnMainThread(() =>
            {
                ReviewDetails = reviewDetails;
                ScoreViewModel.Score = reviewDetails.Rating;            
            });

            CanEdit = reviewDetails.Editable;

            Loading = false;
        }

        protected virtual void OnProductDetailsRequested()
        {
            VmService.NavigateToProduct(ReviewDetails.ProductId, ReviewDetails.ProductGroupId);
        }

        protected virtual void OnEditReview()
        {
            VmService.NavigateToReviewApplication(ReviewDetails.Id, ReviewDetails.ProductGroupId, ReviewDetails.ProductId);
        }

        protected virtual async void OnDeleteReview()
        {
            Loading = true;

            if (await VmService.DeleteReview(ReviewDetails.Id))
            {
                UserDialogs.Alert("Ваш отзыв удален");

                Close(this);
            }
            else
            {
                UserDialogs.Error("Не удалось удалить отзыв");
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

            ScoreViewModel = new ScoreViewModel();

            ScoreViewModel.Prepare(new BaseBundle(NavigationType.InsideScreen));
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