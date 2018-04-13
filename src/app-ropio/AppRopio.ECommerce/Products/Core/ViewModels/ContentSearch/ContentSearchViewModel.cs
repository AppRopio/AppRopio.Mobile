using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Autocomplete;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Hint;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.History;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch
{
    public class ContentSearchViewModel : SearchViewModel, IContentSearchViewModel
    {
        #region Commands

        #region History

        private ICommand _historySelectionCommand;
        public ICommand HistorySelectionCommand
        {
            get
            {
                return _historySelectionCommand ?? (_historySelectionCommand = new MvxCommand<IHistorySearchItemVM>(OnHistorySelectionExecute));
            }
        }

        private ICommand _clearHistoryCommand;
        public ICommand ClearHistoryCommand
        {
            get
            {
                return _clearHistoryCommand ?? (_clearHistoryCommand = new MvxCommand(OnClearHistoryExecute));
            }
        }

        #endregion

        #region Hints

        private ICommand _hintSelectionCommand;
        public ICommand HintSelectionCommand
        {
            get
            {
                return _hintSelectionCommand ?? (_hintSelectionCommand = new MvxCommand<IHintItemVM>(OnHintSelectionExecute));
            }
        }

        private ICommand _autocompleteSelectionCommand;
        public ICommand AutocompleteSelectionCommand
        {
            get
            {
                return _autocompleteSelectionCommand ?? (_autocompleteSelectionCommand = new MvxCommand<IAutocompleteItemVM>(OnAutocompleteSelectionExecute));
            }
        }

        #endregion

        #endregion

        #region Properties

        #region Content

        private IContentSearchInternalViewModel _contentVm;
        public IContentSearchInternalViewModel ContentVm
        {
            get
            {
                return _contentVm;
            }
            set
            {
                _contentVm = value;
                RaisePropertyChanged(() => ContentVm);
            }
        }

        public bool ContentVisible
        {
            get
            {
                return !HistoryVisible && !HintsVisible;
            }
        }

        #endregion

        #region History

        private bool _historyVisible;
        public bool HistoryVisible
        {
            get
            {
                return _historyVisible;
            }
            set
            {
                SetProperty(ref _historyVisible, value);
            }
        }

        private ObservableCollection<IHistorySearchItemVM> _historyItems;
        public ObservableCollection<IHistorySearchItemVM> HistoryItems
        {
            get
            {
                return _historyItems;
            }
            set
            {
                SetProperty(ref _historyItems, value);
            }
        }

        public string ClearHistoryTitle => LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "ContentSearch_ClearHistory");

        #endregion

        #region Hints

        protected CancellationTokenSource DelayCTS { get; set; }

        protected CancellationTokenSource HintsCTS { get; set; }

        private bool _hintsVisible;
        public bool HintsVisible
        {
            get
            {
                return _hintsVisible;
            }
            set
            {
                _hintsVisible = value;
                RaisePropertyChanged(() => HintsVisible);
            }
        }

        private ObservableCollection<IHintItemVM> _hintsItems;
        public ObservableCollection<IHintItemVM> HintsItems
        {
            get
            {
                return _hintsItems;
            }
            set
            {
                _hintsItems = value;
                RaisePropertyChanged(() => HintsItems);
            }
        }

        private ObservableCollection<IAutocompleteItemVM> _autocomleteItems;
        public ObservableCollection<IAutocompleteItemVM> AutocomleteItems
        {
            get
            {
                return _autocomleteItems;
            }
            set
            {
                _autocomleteItems = value;
                RaisePropertyChanged(() => AutocomleteItems);
            }
        }

        #endregion

        #endregion

        #region Services

        protected IContentSearchVmService VmService { get { return Mvx.Resolve<IContentSearchVmService>(); } }

        #endregion

        #region Constructor

        public ContentSearchViewModel()
        {
            HistoryVisible = true;

            HintsItems = new ObservableCollection<IHintItemVM>();
            HistoryItems = new ObservableCollection<IHistorySearchItemVM>();
            AutocomleteItems = new ObservableCollection<IAutocompleteItemVM>();
        }

        #endregion

        #region Private

        private async Task LoadContent()
        {
            var history = await VmService.LoadSearchHistory();

            InvokeOnMainThread(() => HistoryItems = history);
        }

        private void StartSearch(IContentSearchItemVM item)
        {
            if (DelayCTS != null && !DelayCTS.IsCancellationRequested)
            {
                DelayCTS.Cancel();
                DelayCTS = new CancellationTokenSource();
            }

            if (HintsCTS != null && !HintsCTS.IsCancellationRequested)
                HintsCTS.Cancel();

            SetProperty(ref _searchText, item.SearchText, "SearchText");

            ContentVm.SearchText = SearchText;

            SearchCommand.Execute(null);

            InvokeOnMainThread(() =>
            {
                HistoryVisible = false;
                HintsVisible = false;

                RaisePropertyChanged(() => ContentVisible);
            });
        }

        private async Task LoadHintsContent()
        {
            HintsCTS = new CancellationTokenSource();

            var searchText = SearchText;

            var hintsTask = VmService.LoadHints(searchText, HintsCTS.Token);
            var autocompleteTask = VmService.LoadAutocompletes(searchText, HintsCTS.Token);

            await Task.WhenAll(hintsTask);

            if (HintsCTS != null && !HintsCTS.IsCancellationRequested)
            {
                InvokeOnMainThread(() =>
                {
                    HintsItems = hintsTask.Result;
                    AutocomleteItems = autocompleteTask.Result;

                    HistoryVisible = false;
                    HintsVisible = true;

                    RaisePropertyChanged(() => ContentVisible);
                });
            }
        }

        #endregion

        #region Protected

        #region History

        protected virtual void OnHistorySelectionExecute(IHistorySearchItemVM item)
        {
            StartSearch(item);
        }

        protected virtual void OnClearHistoryExecute()
        {
            if (!HistoryItems.IsNullOrEmpty())
            {
                HistoryItems = new ObservableCollection<IHistorySearchItemVM>();

                Task.Run(VmService.ClearHistory);
            }
        }

        #endregion

        #region Hints

        protected virtual void OnHintSelectionExecute(IHintItemVM item)
        {
            StartSearch(item);
        }

        protected virtual void OnAutocompleteSelectionExecute(IAutocompleteItemVM item)
        {
            var searchText = SearchText.Trim();
            SearchText = item.ResultSearchText;
        }

        #endregion

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            VmNavigationType = NavigationType.Push;

            ContentVm = (IContentSearchInternalViewModel)Activator.CreateInstance(LookupService.Resolve<IContentSearchInternalViewModel>());
            ContentVm.Prepare(new BaseBundle(NavigationType.InsideScreen));
        }

        #endregion

        #region SearchViewModel implementation

        protected override void OnSearchTextChanged(string searchText)
        {
            ContentVm.SearchText = searchText;

            if (DelayCTS != null && !DelayCTS.IsCancellationRequested)
            {
                DelayCTS.Cancel();
                DelayCTS = new CancellationTokenSource();
            }

            if (HintsCTS != null && !HintsCTS.IsCancellationRequested)
            {
                HintsCTS.Cancel();
                HintsCTS = null;
            }

            if (ContentVm.SearchText.IsNullOrEmtpy())
            {
                InvokeOnMainThread(() =>
                {
                    HintsItems = new ObservableCollection<IHintItemVM>();
                    AutocomleteItems = new ObservableCollection<IAutocompleteItemVM>();

                    HintsVisible = false;
                    HistoryVisible = true;

                    RaisePropertyChanged(() => ContentVisible);
                });

                return;
            }

            try
            {
                DelayCTS = new CancellationTokenSource();
                Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(300, DelayCTS.Token);
                        await LoadHintsContent();
                    }
                    catch (OperationCanceledException)
                    {

                    }
                }, DelayCTS.Token);
            }
            catch (OperationCanceledException)
            {

            }
        }

        protected override void SearchCommandExecute()
        {
            if (DelayCTS != null && !DelayCTS.IsCancellationRequested)
            {
                DelayCTS.Cancel();
                DelayCTS = new CancellationTokenSource();
            }

            if (HintsCTS != null && !HintsCTS.IsCancellationRequested)
                HintsCTS.Cancel();

            Task.Run(async () =>
            {
                await VmService.SaveSearchRequestInHistory(SearchText);
                await LoadContent();
            });

            ContentVm.SearchCommand.Execute(null);

            InvokeOnMainThread(() =>
            {
                HintsItems = new ObservableCollection<IHintItemVM>();
                AutocomleteItems = new ObservableCollection<IAutocompleteItemVM>();

                HintsVisible = false;
                HistoryVisible = false;

                RaisePropertyChanged(() => ContentVisible);
            });
        }

        protected override void CancelSearchExecute()
        {
            SearchText = string.Empty;

            InvokeOnMainThread(() =>
            {
                HintsItems = new ObservableCollection<IHintItemVM>();
                AutocomleteItems = new ObservableCollection<IAutocompleteItemVM>();

                HintsVisible = false;
                HistoryVisible = true;
            });

            ContentVm.CancelSearchCommand.Execute(null);
        }

        #endregion

        #endregion

        #region Public

        public override Task Initialize()
        {
            ContentVm.Initialize();

            return LoadContent();
        }

        #endregion
    }
}
