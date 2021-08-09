﻿using System;
using System.Linq;
using AppRopio.Base.Core.Messages.Module;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Navigation.Menu.Core.ViewModels.Items
{
    public class MenuItemVM : MvxViewModel, IMenuItemVM
    {
        #region Fields

        private MvxSubscriptionToken _subscribtionToken;

        #endregion

        #region Properties

        public bool HasIcon => !Icon.IsNullOrEmpty();

        public string Icon { get; protected set; }

        public string Type { get; protected set; }

        public string Title { get; protected set; }
  
        public bool HideBadgeOnNull { get; }

        private bool _badgeEnabled;
        public bool BadgeEnabled
        {
            get
            {
                return _badgeEnabled;
            }
            set
            {
                _badgeEnabled = value;
                RaisePropertyChanged(() => BadgeEnabled);
            }
        }

        private int _badgeCount;
        public int BadgeCount
        {
            get
            {
                return _badgeCount;
            }
            set
            {
                _badgeCount = value;
                RaisePropertyChanged(() => BadgeCount);
            }
        }

        #endregion

        #region Constructor

        public MenuItemVM(string navigationModelType)
        {
            Type = navigationModelType;
        }

        public MenuItemVM(string icon, string title, string navigationModelType)
            : this(navigationModelType)
        {
            Icon = icon;
            Title = title;
        }

        public MenuItemVM(string icon, string title, string navigationModelType, bool badgeEnabled, bool hideBageOnNull)
            : this(icon, title, navigationModelType)
        {
            HideBadgeOnNull = hideBageOnNull;

            if (HideBadgeOnNull)
                BadgeEnabled = badgeEnabled && BadgeCount > 0;
            else
                BadgeEnabled = badgeEnabled;
            
            if (badgeEnabled)
                _subscribtionToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<ModulesInteractionMessage<int>>(ChangeBadgeWhenInteractionMsgRecieve);
        }

        #endregion

        #region Protected

        protected virtual void ChangeBadgeWhenInteractionMsgRecieve(ModulesInteractionMessage<int> msg)
        {
            if (msg.Sender.GetType().GetInterfaces().Any(x => x.FullName.Equals(Type)) || 
                (msg.Type != null && msg.Type.FullName.Equals(Type)))
            {
                BadgeCount = msg.Value;

                if (HideBadgeOnNull)
                    BadgeEnabled = BadgeCount > 0;
            }
        }

        #endregion
    }
}