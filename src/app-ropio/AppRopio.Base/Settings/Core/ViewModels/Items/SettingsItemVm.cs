using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Settings.Core.Models;

namespace AppRopio.Base.Settings.Core.ViewModels.Items
{
    public abstract class SettingsItemVm : BaseViewModel, ISettingsItemVm
    {
		public string Title { get; protected set; }

        public SettingsElementType ElementType { get; protected set; }

        public Action<ISettingsItemVm> OnValueChanged { get; set; }

        public SettingsItemVm(string title, SettingsElementType elementType)
        {
            Title = title;
            ElementType = elementType;
        }

        protected override bool SetProperty<T>(ref T storage, T value, string propertyName = null)
        {
            var result = base.SetProperty(ref storage, value, propertyName);
            if (result)
            {
                OnValueChanged?.Invoke(this);
            }

            return result;
        }
    }
}