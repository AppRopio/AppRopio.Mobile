﻿using System;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.HistoryOrders.Core.Models.Bundle;

namespace AppRopio.ECommerce.HistoryOrders.Core.Services
{
    public interface IHistoryOrdersNavigationVmService : IBaseVmNavigationService
    {
        void NavigateToOrder(HistoryOrderBundle order);

        void NavigateToOrderProducts(HistoryOrderBundle order);

        void NavigateToBasket(BaseBundle bundle);
    }
}