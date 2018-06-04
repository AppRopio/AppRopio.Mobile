using System;
namespace AppRopio.Payments.CloudPayments.Core.Models
{
    public class CloudPaymentsConfig
    {
        /// <summary>
        /// Берется из личного кабинета
        /// </summary>
        public string StoreId { get; set; }

		/// <summary>
		/// Берется из личного кабинета
		/// </summary>
		public string ApiSecret { get; set; }

        /// <summary>
        /// Url для завершения оплаты через 3DS
        /// </summary>
        public string ThreeDSRedirectUrl { get; set; }

        public MessageSchemeType MessageScheme { get; set; }
    }
}