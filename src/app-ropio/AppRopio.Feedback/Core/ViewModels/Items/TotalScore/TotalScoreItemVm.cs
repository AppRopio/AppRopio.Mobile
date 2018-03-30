using System;
using AppRopio.Base.Core.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.Items.TotalScore
{
    public class TotalScoreItemVm : ReviewParameterItemVm, ITotalScoreItemVm
    {
		public int MaxValue { get; set; }

        private int _value;

        public int Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
    }
}
