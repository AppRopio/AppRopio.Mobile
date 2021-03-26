using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Core.ViewModels.WebContent
{
    public abstract class BaseWebContentViewModel : BaseViewModel, IBaseWebContentViewModel
    {
        #region Commands

        private ICommand _loadFinishedCommand;
        public ICommand LoadFinishedCommand => _loadFinishedCommand ?? (_loadFinishedCommand = new MvxCommand<string>(OnLoadFinishedExecute, CanLoadFinishedExecute));

        #endregion

        #region Properties

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, nameof(Title));
        }

        private string _url;
        public string Url
        {
            get => _url;
            protected set => SetProperty(ref _url, value, nameof(Url));
        }

        private string _htmlContent;
        public string HtmlContent
        {
            get => _htmlContent;
            protected set => SetProperty(ref _htmlContent, value, nameof(HtmlContent));
        }

        #endregion

        #region Protected

        protected abstract void OnLoadFinishedExecute(string url);

        protected abstract bool CanLoadFinishedExecute(string url);

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var webContentBundle = parameters.ReadAs<BaseWebContentBundle>();
            this.InitFromBundle(webContentBundle);
        }

        protected virtual void InitFromBundle(BaseWebContentBundle parameters)
        {
            VmNavigationType = parameters.NavigationType;

            Title = parameters.Title;
            Url = parameters.Url;
            HtmlContent = parameters.HtmlContent;
        }

        #endregion

        #endregion
    }
}
