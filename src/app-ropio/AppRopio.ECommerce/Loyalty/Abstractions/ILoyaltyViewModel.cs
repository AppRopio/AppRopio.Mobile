﻿using MvvmCross.ViewModels;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Loyalty.Abstractions
{
    public interface ILoyaltyViewModel : IMvxViewModel
    {
        void RegisterUpdateMessage(MvxMessage message);
    }
}
