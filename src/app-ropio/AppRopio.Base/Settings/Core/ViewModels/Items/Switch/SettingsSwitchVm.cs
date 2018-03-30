using AppRopio.Base.Settings.Core.Models;

namespace AppRopio.Base.Settings.Core.ViewModels.Items.Switch
{
    public class SettingsSwitchVm : SettingsItemVm
    {
        private bool _enabled;

        public bool Enabled
        {
            get { return _enabled; }
            set { SetProperty(ref _enabled, value); }
        }

        public SettingsSwitchVm(string title, SettingsElementType elementType) : base(title, elementType)
		{
            
		}
	}
}