using System;
using System.Threading.Tasks;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Feedback.Core.Models.Bundle;
using AppRopio.Feedback.Core.ViewModels.Reviews.Items;
using AppRopio.Feedback.Core.ViewModels.Reviews.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Feedback.Core.ViewModels.Reviews
{
    public class ReviewsViewModel : LoadMoreViewModel, IReviewsViewModel
    {
        #region Fields

        protected int LOADING_REVIEWS_COUNT = 10;

        #endregion

        #region Commands

        private IMvxCommand _selectionChangedCommand;
        public IMvxCommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IReviewItemVm>(OnReviewSelected));
            }
        }

        private IMvxCommand _reviewCommand;
        public IMvxCommand ReviewCommand
        {
            get
            {
                return _reviewCommand ?? (_reviewCommand = new MvxCommand(OnReviewCommandExecuted));
            }
        }

        #endregion

        #region Properties

        protected string ProductId { get; set; }

        protected string ProductGroupId { get; set; }

        private MvxObservableCollection<IReviewItemVm> _reviews;
        public MvxObservableCollection<IReviewItemVm> Reviews
        {
            get { return _reviews; }
            set { SetProperty(ref _reviews, value); }
        }

        private bool _canPostReview;
        public bool CanPostReview
        {
            get { return _canPostReview; }
            set { SetProperty(ref _canPostReview, value); }
        }


        #endregion

        #region Services

        protected IReviewsVmService VmService { get { return Mvx.Resolve<IReviewsVmService>(); } }

        #endregion

        #region Protected

        protected virtual async Task LoadContent()
        {
            Loading = true;

            var reviews = await VmService.LoadReviews(ProductGroupId, ProductId, LOADING_REVIEWS_COUNT);
            InvokeOnMainThread(() => Reviews = reviews);

            Loading = false;

            CanPostReview = Mvx.CanResolve<ISessionService>() ? Mvx.Resolve<ISessionService>().Alive : false;
#if DEBUG
            CanPostReview = true;
#endif
        }

        protected override async Task LoadMoreContent()
        {
            await Task.Run(async () =>
            {
                var dataSource = await VmService.LoadReviews(ProductGroupId, ProductId, LOADING_REVIEWS_COUNT, Reviews.Count);

                CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_REVIEWS_COUNT;
                LoadMoreCommand.RaiseCanExecuteChanged();

                InvokeOnMainThread(() => Reviews.AddRange(dataSource));
            });
        }

        protected virtual void OnReviewSelected(IReviewItemVm review)
        {
            VmService.HandleItemSelection(review);
        }

        protected virtual void OnReviewCommandExecuted()
        {
            VmService.NavigateToReviewApplication(ProductGroupId, ProductId);
        }

        #endregion

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var navigationBundle = parameters.ReadAs<ProductReviewsBundle>();
            this.InitFromBundle(navigationBundle);
        }

        protected virtual void InitFromBundle(ProductReviewsBundle parameters)
        {
            VmNavigationType = parameters.NavigationType == NavigationType.None ?
                                                            NavigationType.ClearAndPush :
                                                            parameters.NavigationType;

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