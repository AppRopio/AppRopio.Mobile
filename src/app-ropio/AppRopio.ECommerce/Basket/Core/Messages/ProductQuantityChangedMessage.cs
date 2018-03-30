﻿using System;
using MvvmCross.Plugins.Messenger;

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
