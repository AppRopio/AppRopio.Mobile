using System;
using AppRopio.Base.Core.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.Items.Text
{
    public class TextItemVm : ReviewParameterItemVm, ITextItemVm
    {
        public int MinLength { get; set; }

		public int MaxLength { get; set; }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }
}