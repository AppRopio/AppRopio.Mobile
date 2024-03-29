﻿using AppRopio.Models.Products.Responses;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Products.Core.Messages
{
    public class ProductCardMarkedMessage : MvxMessage
    {
        public Product Model { get; set; }

        public bool Marked { get; set; }

        public ProductCardMarkedMessage(object sender, Product model, bool isMarked)
            : base(sender)
        {
            Model = model;
            Marked = isMarked;
        }
    }
}
