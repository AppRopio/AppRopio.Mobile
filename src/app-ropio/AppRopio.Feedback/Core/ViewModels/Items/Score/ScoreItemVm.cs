namespace AppRopio.Feedback.Core.ViewModels.Items.Score
{
    public class ScoreItemVm : ReviewParameterItemVm, IScoreItemVm
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