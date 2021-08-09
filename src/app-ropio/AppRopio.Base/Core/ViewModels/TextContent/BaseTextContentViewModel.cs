using System;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;

namespace AppRopio.Base.Core.ViewModels.TextContent
{
    public abstract class BaseTextContentViewModel : BaseViewModel, IBaseTextContentViewModel
    {
        #region Properties

        private string _text;
        public string Text
        {
            get => _text;
            protected set => SetProperty(ref _text, value, nameof(Text));
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, nameof(Title));
        }

        #endregion

        #region Protected

        #region Init

        public override void Prepare(MvvmCross.ViewModels.IMvxBundle parameters)
        {
            var textContentBundle = parameters.ReadAs<BaseTextContentBundle>();
            this.InitFromBundle(textContentBundle);
        }

        protected virtual void InitFromBundle(BaseTextContentBundle parameters)
        {
            VmNavigationType = parameters.NavigationType;
            
            Title = parameters.Title;
            Text = parameters.Text;
        }

        #endregion

        #endregion
    }
}
