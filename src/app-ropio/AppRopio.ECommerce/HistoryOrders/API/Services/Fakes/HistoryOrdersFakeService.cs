using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.HistoryOrders.Responses;
using AppRopio.Models.Payments.Responses;

namespace AppRopio.ECommerce.HistoryOrders.API.Services.Fakes
{
    public class HistoryOrdersFakeService : IHistoryOrdersService
    {
        public async Task<List<HistoryOrder>> GetOrders(int count, int offset = 0)
        {
            await Task.Delay(1500);

            return new List<HistoryOrder>()
            {
                new HistoryOrder()
                {
                    Id = "1",
                    OrderNumber = "№12345",
                    ItemsCount = 5,
                    PaymentStatus = PaymentStatus.Paid,
                    OrderStatus = "Оплачен 22.06 в 14:53",
                    TotalPrice = 14383,
                    ImageUrl = ""
                },

				new HistoryOrder()
				{
					Id = "2",
					OrderNumber = "№12346",
					ItemsCount = 2,
                    PaymentStatus = PaymentStatus.NotPaid,
					OrderStatus = "Оплачен 21.06 в 10:40",
					TotalPrice = 5828,
					ImageUrl = "http://wlooks.ru/images/article/orig/2016/07/zhenskaya-sinyaya-rubashka-23.jpg"
				},

				new HistoryOrder()
				{
					Id = "3",
					OrderNumber = "№12347",
					ItemsCount = 2,
                    PaymentStatus = PaymentStatus.Unknown,
					OrderStatus = "Оплачен 20.06 в 05:40",
					TotalPrice = 2334,
                    ImageUrl = "http://wlooks.ru/images/article/orig/2016/07/zhenskaya-sinyaya-rubashka-23.jpg",
				}
            };
        }

		public async Task<HistoryOrderDetails> GetOrderDetails(string orderId)
		{
            await Task.Delay(1500);

            var details = new Dictionary<string, HistoryOrderDetails>
            {
				["1"] = new HistoryOrderDetails()
				{
					Id = "1",
					OrderStatus = new List<string>
                    {
                        "Заказ сделан 22.04 в 14:55",
                        "Передан в обработку 23.04 в 14:54",
                        "Оплачен 30.04 в 14:54"
                    },
                    DeliveryPoint = new Models.Basket.Responses.Order.DeliveryPoint()
                    {
                        Id = "1",
                        Name = "Магазин «Может быть какой-нибудь такой»",
                        Address = "Санкт-Петербург, ул. Белградская, д. 45, лит. А",
                        Coordinates = new Coordinates()
                        {
                            Latitude = 35,
                            Longitude = 65
                        }
                    },
                    Payment = new Models.Basket.Responses.Order.Payment()
                    {
                        Id = "1",
                        Name = "Банковской картой при получении",
                        Type = PaymentType.CreditCard
                    },
                    Delivery = new Models.Basket.Responses.Order.Delivery()
                    {
                        Id = "1",
                        Name = "Доставка",
                        Price = 0
                    }
				},
				["2"] = new HistoryOrderDetails()
				{
					Id = "2",
					OrderStatus = new List<string>
					{
						"Заказ сделан 22.04 в 14:55",
						"Передан в обработку 23.04 в 14:54",
						"Оплачен 30.04 в 14:54"
					},
					DeliveryPoint = new Models.Basket.Responses.Order.DeliveryPoint()
					{
						Id = "1",
						Name = "Магазин «Может быть какой-нибудь такой»",
						Address = "Санкт-Петербург, ул. Белградская, д. 45, лит. А",
						Coordinates = new Coordinates()
						{
							Latitude = 35,
							Longitude = 65
						}
					},
					Payment = new Models.Basket.Responses.Order.Payment()
					{
						Id = "1",
						Name = "Наличными",
                        Type = PaymentType.Cash
					},
					Delivery = new Models.Basket.Responses.Order.Delivery()
					{
						Id = "1",
						Name = "Самовывоз",
						Price = 400
					}
				},
				["3"] = new HistoryOrderDetails()
				{
					Id = "3",
					OrderStatus = new List<string>
					{
						"Заказ сделан 22.04 в 14:55",
						"Передан в обработку 23.04 в 14:54",
						"Оплачен 30.04 в 14:54"
					},
					DeliveryPoint = new Models.Basket.Responses.Order.DeliveryPoint()
					{
						Id = "1",
						Name = "Магазин «Может быть какой-нибудь такой»",
						Address = "Санкт-Петербург, ул. Белградская, д. 45, лит. А",
						Coordinates = new Coordinates()
						{
							Latitude = 35,
							Longitude = 65
						}
					},
					Payment = new Models.Basket.Responses.Order.Payment()
					{
						Id = "1",
						Name = "Картой",
						Type = PaymentType.CreditCard
					},
					Delivery = new Models.Basket.Responses.Order.Delivery()
					{
						Id = "1",
						Name = "Доставка",
						Price = 0
					}
				}
            };

            if (details.ContainsKey(orderId))
                return details[orderId];

            return null;
		}

        public async Task<List<HistoryOrderProduct>> GetOrderProducts(string orderId)
		{
            await Task.Delay(500);

            return new List<HistoryOrderProduct>
            {
                new HistoryOrderProduct
                {
                    Id = "1",
                    Title = "Natura Siberica Пенка «Увлажняющая»...",
                    TotalPrice = 200100,
                    Amount = 1,
                    IsAvailable = true,
                    ImageUrl = ""
                },
				new HistoryOrderProduct
				{
					Id = "2",
					Title = "Тёмно-синий пиджак суперузкого кроя...",
                    Amount = 12,
                    IsAvailable = false,
                    Badge = "Нет в наличии",
					ImageUrl = "http://wlooks.ru/images/article/orig/2016/07/zhenskaya-sinyaya-rubashka-23.jpg"
				},
				new HistoryOrderProduct
				{
					Id = "3",
					Title = "Natura Siberica Пенка «Увлажняющая»... Артикул: NA345212367",
					TotalPrice = 200100,
					Amount = 1,
                    IsAvailable = true,
					ImageUrl = "http://wlooks.ru/images/article/orig/2016/07/zhenskaya-sinyaya-rubashka-23.jpg"
				},
				new HistoryOrderProduct
				{
					Id = "4",
					Title = "Natura Siberica Пенка «Увлажняющая»... Артикул: NA345212367",
					TotalPrice = 200100,
					Amount = 12,
                    IsAvailable = true,
					ImageUrl = "http://wlooks.ru/images/article/orig/2016/07/zhenskaya-sinyaya-rubashka-23.jpg"
				},
			};
		}

        public async Task<HistoryOrderRepeatResponse> RepeatOrder(string orderId)
        {
            await Task.Delay(500);

            return new HistoryOrderRepeatResponse()
            {
                Message = "Все товары добавлены в корзину"
            };
        }
	}
}