using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Map.Core.Messages.Points;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using AppRopio.Base.Map.Core.ViewModels.Points.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Map.Core.Services;

namespace AppRopio.Base.Map.Core.ViewModels.Points
{
    public abstract class PointsCollectionVM : RefreshViewModel, IPointsCollectionVM
    {
        #region Fields

        private MvxSubscriptionToken _itemSelectedToken;

        #endregion

        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IPointItemVM>(OnSelectionChangedExecute));
            }
        }

        #endregion

        #region Properties

        private MvxObservableCollection<IPointItemVM> _items;
        public MvxObservableCollection<IPointItemVM> Items
        {
            get
            {
                return _items;
            }
            set
            {
                if (Equals(_items, value))
                    return;

                _items = value;
                RaisePropertyChanged(() => Items);
            }
        }

        private IPointItemVM _selectedItem;
        public IPointItemVM SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
        }

        #endregion

        #region Services

        private IPointsVmService _vmService;
        public IPointsVmService VmService => _vmService ?? (_vmService = Mvx.Resolve<IPointsVmService>());

        private IMapNavigationVmService _navigationVmService;
        public IMapNavigationVmService NavigationVmService => _navigationVmService ?? (_navigationVmService = Mvx.Resolve<IMapNavigationVmService>());

        #endregion

        #region Constructor

        protected PointsCollectionVM()
        {
            Items = new MvxObservableCollection<IPointItemVM>();
        }

        #endregion

        #region Private

        private void UnsubscribeTokens()
        {
            if (_itemSelectedToken != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ItemSelectedMessage>(_itemSelectedToken);
                _itemSelectedToken = null;
            }
        }

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var baseBundle = parameters.ReadAs<BaseBundle>();
            this.InitFromBundle(baseBundle);
        }

        protected virtual void InitFromBundle(BaseBundle bundle)
        {
            VmNavigationType = bundle.NavigationType;
        }

        #endregion

        protected virtual void OnItemSelected(IPointItemVM item)
        {
            if (Items.IsNullOrEmpty())
                return;
            
            Items.ForEach(x => x.IsSelected = false);

            SelectedItem = item;
            if (item != null)
                item.IsSelected = true;
        }

        protected void OnSelectionChangedExecute(IPointItemVM item)
        {
            OnItemSelected(item);

            Messenger.Publish(new ItemSelectedMessage(this) { Id = item?.Id });
        }

        protected void OnItemSelectedMessage(ItemSelectedMessage message)
        {
            if (message.Sender.Equals(this))
                return;

            if (message.Id == null)
                OnItemSelected(null);

            var item = Items?.FirstOrDefault(x => x.Id.Equals(message.Id));
            if (item == null)
                return;
            
            OnItemSelected(item);
        }

        protected abstract Task LoadContent();

        #endregion

        #region Public

        public override Task Initialize()
        {
            _itemSelectedToken = Mvx.Resolve<IMvxMessenger>().Subscribe<ItemSelectedMessage>(OnItemSelectedMessage);

            return LoadContent();
        }

        #endregion
    }
}
