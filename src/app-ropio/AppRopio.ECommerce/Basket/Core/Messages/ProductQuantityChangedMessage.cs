﻿using System;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Basket.Core.Messages
{
    public class ProductQuantityChangedMessage : MvxMessage
    {
        public ProductQuantityChangedMessage(object sender)
            : base (sender)
        {
        }
    }
}
