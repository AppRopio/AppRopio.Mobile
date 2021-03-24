using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.Contacts.Core.ViewModels.Contacts.Services;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Contacts.Responses;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Base.Contacts.Core.ViewModels.Contacts
{
    public class ContactsViewModel : BaseViewModel, IContactsViewModel
    {
		#region Fields

		#endregion

		#region Commands


		private IMvxCommand _selectionChangedCommand;
		public IMvxCommand SelectionChangedCommand
		{
			get
			{
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<ListResponseItem>(OnContactSelected));
			}
		}

        #endregion

        #region Properties

        private List<ListResponseItem> _contacts;

        /// <summary>
        /// Контакты
        /// </summary>
        public List<ListResponseItem> Contacts
        {
            get
            {
                return _contacts;
            }
            private set
            {
                SetProperty(ref _contacts, value);
            }
        }

        #endregion

        #region Services

        protected IContactsVmService VmService { get { return Mvx.Resolve<IContactsVmService>(); } }

        #endregion

        #region Constructor

        public ContactsViewModel()
        {
        }

        #endregion

        #region Private

        //загрузка контактов
        private async Task LoadContent()
        {
            Loading = true;

            var dataSource = await VmService.LoadContacts();

            InvokeOnMainThread(() => Contacts = dataSource);

            Loading = false;
        }

        private void OnContactSelected(ListResponseItem contact)
		{
            VmService.HandleContactSelection(contact);
		}

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var navigationBundle = parameters.ReadAs<BaseBundle>();
            this.InitFromBundle(navigationBundle);
        }

        protected virtual void InitFromBundle(BaseBundle parameters)
        {
            VmNavigationType = parameters.NavigationType == NavigationType.None ?
                                                            NavigationType.ClearAndPush : 
                                                            parameters.NavigationType;
        }

        #endregion

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        #endregion
    }
}
