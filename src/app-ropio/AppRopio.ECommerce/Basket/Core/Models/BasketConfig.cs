using System;
using AppRopio.Base.Core.Models.Config;
using AppRopio.ECommerce.Basket.Core.Enums;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace AppRopio.ECommerce.Basket.Core.Models
{
    public class BasketConfig
    {
        /// <summary>
        /// Конфигурация блока лояльности
        /// </summary>
        public AssemblyElement Loyalty { get; set; }

		/// <summary>
		/// Конфигурация блока платежей внутри приложения
		/// </summary>
        public Dictionary<BasketInAppPaymentType, AssemblyElement> InAppPayments { get; set; }

        /// <summary>
        /// Тип экранов оформления заказа
        /// </summary>
        public OrderViewType OrderViewType { get; set; }

        /// <summary>
        /// Возможность отправки открытки вместе с доставкой
        /// </summary>
        [Obsolete("Рефакторинг", true)]
        public bool CanDeliveryWithPostcard { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BasketInAppPaymentType
    {
        /// <summary>
        /// Модуль обрабатывает все платежи или является единственным
        /// </summary>
        [EnumMember(Value = "all")]
        All = 0,

        /// <summary>
        /// Банковская карта
        /// </summary>
        [EnumMember(Value = "creditCard")]
        CreditCard = 1,

        /// <summary>
        /// Через электронные деньги
        /// </summary>
        [EnumMember(Value = "eWallet")]
        EWallet = 2,

        /// <summary>
        /// Через нативные средства оплаты (Apple Pay, Google Pay)
        /// </summary>
        [EnumMember(Value = "native")]
        Native = 3
    }
}
