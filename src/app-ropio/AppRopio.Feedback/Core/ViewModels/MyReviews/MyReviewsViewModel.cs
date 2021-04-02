using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Feedback.Core.ViewModels.MyReviews.Services;
using AppRopio.Feedback.Core.ViewModels.Reviews.Items;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.MyReviews
{
    public class MyReviewsViewModel : LoadMoreViewModel, IMyReviewsViewModel
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

        #endregion

        #region Properties

        private MvxObservableCollection<IReviewItemVm> _reviews;
        public MvxObservableCollection<IReviewItemVm> Reviews
        {
            get { return _reviews; }
            set { SetProperty(ref _reviews, value); }
        }

        #endregion

        #region Services

        protected IMyReviewsVmService VmService { get { return Mvx.IoCProvider.Resolve<IMyReviewsVmService>(); } }

        #endregion

        #region Protected

        protected virtual async Task LoadContent()
        {
            Loading = true;

            var dataSource = await VmService.LoadReviews(LOADING_REVIEWS_COUNT);
            InvokeOnMainThread(() => Reviews = dataSource);

            CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_REVIEWS_COUNT;
            LoadMoreCommand.RaiseCanExecuteChanged();

            Loading = false;
        }

        protected override async Task LoadMoreContent()
        {
            await Task.Run(async () =>
            {
                var dataSource = await VmService.LoadReviews(LOADING_REVIEWS_COUNT, Reviews.Count);

                CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_REVIEWS_COUNT;
                LoadMoreCommand.RaiseCanExecuteChanged();

                InvokeOnMainThread(() => Reviews.AddRange(dataSource));
            });
        }

        protected virtual void OnReviewSelected(IReviewItemVm review)
        {
            VmService.HandleItemSelection(review);
        }

        #endregion

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var navigationBundle = parameters.ReadAs<BaseBundle>();
            this.InitFromBundle(navigationBundle);
        }

        protected virtual void InitFromBundle(BaseBundle parameters)
        {
            VmNavigationType = parameters.NavigationType == NavigationType.None ?
                                                            NavigationType.ClearAndPush :
                                                            parameters.NavigationType;
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