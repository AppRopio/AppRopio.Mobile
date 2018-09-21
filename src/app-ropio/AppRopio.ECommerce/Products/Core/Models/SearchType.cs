using System;
namespace AppRopio.ECommerce.Products.Core.Models
{
    public enum SearchType
    {
        /// <summary>
        /// Поиск отключен
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// Поиск включён и отображается как меню в тулбаре
        /// </summary>
        Screen = 1,

        /// <summary>
        /// Поиск включён и отображается как панель под тулбаром
        /// </summary>
        Bar = 2,

        /// <summary>
        /// Поиск включён и отображается как панель под тулбаром только для первой страницы, для остальных - в меню
        /// </summary>
        BarOnFirstScreen = 3,
    }
}
