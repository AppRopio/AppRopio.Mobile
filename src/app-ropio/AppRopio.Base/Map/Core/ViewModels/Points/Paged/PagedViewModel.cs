using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using AppRopio.Base.Core.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace AppRopio.Base.Map.Core.ViewModels.Points.Paged
{
    public abstract class PagedViewModel : BaseViewModel, IPagedViewModel
    {
        #region Commands

        private ICommand _titleSelectedCommand;
        public ICommand TitleSelectedCommand
        {
            get
            {
                return _titleSelectedCommand ?? (_titleSelectedCommand = new MvxCommand<PageTitleViewModel>(OnTitleSelected));
            }
        }

        private ICommand _pageChangedCommand;
        public ICommand PageChangedCommand
        {
            get
            {
                return _pageChangedCommand ?? (_pageChangedCommand = new MvxCommand<int>(OnPageChanged));
            }
        }

        #endregion

        #region Properties

        private int _currentPage;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                RaisePropertyChanged(() => CurrentPage);
            }
        }

        private ObservableCollection<IPageTitleViewModel> _titleViewModels;
        public ObservableCollection<IPageTitleViewModel> TitleViewModels
        {
            get
            {
                return _titleViewModels;
            }
            set
            {
                _titleViewModels = value;
                RaisePropertyChanged(() => TitleViewModels);
            }
        }

        private ObservableCollection<IPageViewModel> _pages;
        public ObservableCollection<IPageViewModel> Pages
        {
            get
            {
                return _pages;
            }
            set
            {
                _pages = value;
                RaisePropertyChanged(() => Pages);

                if (value != null && value.Count > 0)
                {
                    value.First().TitleViewModel.IsSelected = true;
                    TitleViewModels = new ObservableCollection<IPageTitleViewModel>(value.Select(x => x.TitleViewModel));
                }
            }
        }

        #endregion

        #region Constructor

        public PagedViewModel()
        {
            TitleViewModels = new ObservableCollection<IPageTitleViewModel>();
            Pages = new ObservableCollection<IPageViewModel>();
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected abstract void InitPages();

        protected virtual void OnTitleSelected(PageTitleViewModel titleVM)
        {
            CurrentPage = TitleViewModels.IndexOf(titleVM);

            TitleViewModels.ForEach(x => x.IsSelected = false);
            titleVM.IsSelected = true;
        }

        protected void OnPageChanged(int index)
        {
            CurrentPage = index;

            TitleViewModels.ForEach(x => x.IsSelected = false);
            TitleViewModels[index].IsSelected = true;
        }

        //public override void Prepare(IMvxBundle parameters)
        //{
        //    InitPages();
        //}

        #endregion

        #region Public

        public override Task Initialize()
        {
            return Task.Run(() => InitPages());
        }

        #endregion
    }
}
