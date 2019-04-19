namespace AppRopio.ECommerce.Products.Core.Models
{
    public enum PriceType
    {
        /// <summary>
        /// Regular price: $200
        /// </summary>
        Default = 0,

        /// <summary>
        /// Starting price: from $200
        /// </summary>
        From = 1,

        /// <summary>
        /// Ending price: to $400
        /// </summary>
        To = 2,

        /// <summary>
        /// Price range: from $200 to $400
        /// </summary>
        FromTo = 3
    }
}
