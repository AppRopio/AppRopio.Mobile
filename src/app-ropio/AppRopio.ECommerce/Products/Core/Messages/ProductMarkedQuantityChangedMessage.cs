﻿using System;
using MvvmCross.Plugin.Messenger;
namespace AppRopio.ECommerce.Products.Core.Messages
{
    public class ProductMarkedQuantityChangedMessage : MvxMessage
    {
        public ProductMarkedQuantityChangedMessage(object sender)
            : base (sender)
        {
        }
    }
}
