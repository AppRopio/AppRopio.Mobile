using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Feedback.API.Services;
using AppRopio.Feedback.Core.ViewModels.Items;
using AppRopio.Feedback.Core.ViewModels.Items.Score;
using AppRopio.Feedback.Core.ViewModels.Items.Text;
using AppRopio.Feedback.Core.ViewModels.Items.TotalScore;
using AppRopio.Models.Feedback.Responses;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Feedback.Core.ViewModels.ReviewPost.Services
{
    public class ReviewPostVmService : BaseVmService, IReviewPostVmService
    {
        #region Services

        protected IReviewsService ApiService { get { return Mvx.Resolve<IReviewsService>(); } }

        #endregion

        public async Task<MvxObservableCollection<IReviewParameterItemVm>> LoadReviewApplication(string reviewId, string productGroupId, string productId)
        {
            MvxObservableCollection<IReviewParameterItemVm> dataSource = null;

            try
            {
                var reviewParameters = await ApiService.GetReviewApplication(reviewId, productGroupId, productId);
                dataSource = new MvxObservableCollection<IReviewParameterItemVm>(reviewParameters.Select(i =>
                {
                    var minValue = i.MinValue;
                    var maxValue = i.MaxValue;

                    switch (i.WidgetType)
                    {
                        case ReviewWidgetType.TotalScore:
                            var totalScore = 0;
                            int.TryParse(i.Value, out totalScore);
                            return (IReviewParameterItemVm)(new TotalScoreItemVm() { Id = i.Id, Title = i.Title, MaxValue = maxValue, Value = totalScore, WidgetType = ReviewWidgetType.TotalScore });

                        case ReviewWidgetType.Score:
                            var score = 0;
                            int.TryParse(i.Value, out score);
                            return (IReviewParameterItemVm)(new ScoreItemVm() { Id = i.Id, Title = i.Title, MaxValue = maxValue, Value = score, WidgetType = ReviewWidgetType.Score });

                        case ReviewWidgetType.Text:
                            return (IReviewParameterItemVm)(new TextItemVm() { Id = i.Id, Title = i.Title, Text = i.Value, MinLength = minValue, MaxLength = maxValue, WidgetType = ReviewWidgetType.Text });

                        default:
                            throw new Exception("Unknown widget type");
                    }
                }).ToList());
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return dataSource;
        }

        public async Task<bool> PostReview(string reviewId, string productGroupId, string productId, List<IReviewParameterItemVm> reviewItems)
        {
            try
            {
                var parameters = reviewItems.Select(i =>
                {
                    var reviewParameter = new ReviewParameter();
                    reviewParameter.Id = i.Id;
                    reviewParameter.WidgetType = i.WidgetType;
                    reviewParameter.Title = i.Title;

                    switch (i.WidgetType)
                    {
                        case ReviewWidgetType.Text:
                            var text = (ITextItemVm)i;
                            reviewParameter.Value = text.Text;
                            break;

                        case ReviewWidgetType.Score:
                            var score = (IScoreItemVm)i;
                            reviewParameter.Value = score.Value.ToString();
                            break;

                        case ReviewWidgetType.TotalScore:
                            var totalScore = (ITotalScoreItemVm)i;
                            reviewParameter.Value = totalScore.Value.ToString();
                            break;
                    }

                    return reviewParameter;
                }).ToList();

                await ApiService.PostReview(reviewId, productGroupId, productId, parameters);

                return true;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
    }
}