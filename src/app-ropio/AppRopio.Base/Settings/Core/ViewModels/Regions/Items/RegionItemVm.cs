using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Settings.Responses;

namespace AppRopio.Base.Settings.Core.ViewModels.Regions.Items
{
    public class RegionItemVm : BaseViewModel, IRegionItemVm
    {
		private string _id;

		public string Id
		{
			get { return _id; }
            protected set { _id = value; }
		}

        private string _title;

        public string Title
        {
            get { return _title; }
            protected set { _title = value; }
        }

        protected bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public RegionItemVm(string id, string title, bool selected = false)
        {
            Id = id;
            Title = title;
            Selected = selected;
        }
    }
}