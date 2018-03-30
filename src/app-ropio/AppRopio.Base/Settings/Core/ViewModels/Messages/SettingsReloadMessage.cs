using System;
using AppRopio.Base.Settings.Core.Models;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Settings.Core.ViewModels.Messages
{
	public class SettingsReloadMessage : MvxMessage
	{
        public SettingsElementType ElementType { get; set; }

        public string Id { get; set; }

        public string ValueTitle { get; set; }

        public SettingsReloadMessage(object sender, SettingsElementType elementType, string id, string valueTitle)
			: base(sender)
		{
            ElementType = elementType;
            Id = id;
            ValueTitle = valueTitle;
		}
	}
}