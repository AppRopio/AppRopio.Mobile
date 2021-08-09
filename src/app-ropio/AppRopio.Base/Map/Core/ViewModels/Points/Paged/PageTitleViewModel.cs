using MvvmCross.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points.Paged
{
    public class PageTitleViewModel : MvxViewModel, IPageTitleViewModel
    {
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(Title, (obj as PageTitleViewModel)?.Title);
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}
