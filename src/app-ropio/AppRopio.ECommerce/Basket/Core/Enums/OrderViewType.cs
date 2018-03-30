using System;
namespace AppRopio.ECommerce.Basket.Core.Enums
{
    public enum OrderViewType
    {
        /// <summary>
        /// Вся информация для заказа на одном экране
        /// </summary>
        Full = 0,

        /// <summary>
        /// [NOT SUPPORTED] Разделение на отдельные экраны (пользователь, тип доставки, тип оплаты)
        /// </summary>
        [Obsolete]
        Partial = 1
    }
}
