using System;
using AppRopio.Models.Base.Responses;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items
{
    public interface IHasCoordinates
    {
        Coordinates Coordinates { get; }
    }
}
