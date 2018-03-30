﻿using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Loyalty.Abstractions
{
    public interface ILoyaltyViewModel : IMvxViewModel
    {
        void RegisterUpdateMessage(MvxMessage message);
    }
}
