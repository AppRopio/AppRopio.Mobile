using System;
namespace AppRopio.ECommerce.Products.Core.Models
{
    public enum CategoriesType
    {
        /// <summary>
        /// Категории отключены, пользователь попадает сразу в каталог
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// Категории включены, соблюдается последовательность переходов: категории, каталог (для категории), карточка товара
        /// </summary>
        StepByStep = 1,

        /// <summary>
        /// Каталог отображается внутри категории, переход не требуется (пэйджинг)
        /// </summary>
        Cataloged = 2,

        Mixed = 3
    }
}

