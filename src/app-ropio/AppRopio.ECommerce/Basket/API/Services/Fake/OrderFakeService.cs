using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Basket.Responses.Enums;
using AppRopio.Models.Basket.Responses.Order;
using AppRopio.Models.Payments.Responses;
using MvvmCross;

namespace AppRopio.ECommerce.Basket.API.Services.Fake
{
    public class OrderFakeService : IOrderService
    {
        public bool IsRussianCulture => Mvx.IoCProvider.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        private List<OrderFieldsGroup> _userFieldsGroups;

        private List<Payment> _payments;

        public OrderFakeService()
        {
            _userFieldsGroups = new List<OrderFieldsGroup>
        {
            new OrderFieldsGroup
            {
                Id = "0",
                    Name = IsRussianCulture ? "Личные данные" : "Personal data",
                Items = new List<OrderField>
                {
                    new OrderField
                    {
                        Id = "00",
                            Name = IsRussianCulture ? "Имя" : "Name",
                        Type = OrderFieldType.Text,
                        Editable = true,
                        IsRequired = true
                    },
                    new OrderField
                    {
                        Id = "01",
                            Name = IsRussianCulture ? "Фамилия" : "Surname",
                        Type = OrderFieldType.Text,
                        Editable = true,
                        IsRequired = true
                    },
                    new OrderField
                    {
                        Id = "02",
                            Name = "Email",
                        Type = OrderFieldType.Email,
                        Editable = true,
                        IsRequired = true
                    },
                    new OrderField
                    {
                        Id = "03",
                            Name = IsRussianCulture ? "Телефон" : "Phone",
                        Type = OrderFieldType.Phone,
                        Editable = true,
                        IsRequired = false
                    }
                }
            },
            new OrderFieldsGroup
            {
                Id = "1",
                    Name = IsRussianCulture ? "Данные получателя" : "Recipient Information",
                Items = new List<OrderField>
                {
                    new OrderField
                    {
                        Id = "10",
                            Name = IsRussianCulture ? "Имя" : "Name",
                        Type = OrderFieldType.Text,
                        Editable = true,
                        IsRequired = false
                    },
                    new OrderField
                    {
                        Id = "11",
                            Name = IsRussianCulture ? "Фамилия" : "Surname",
                        Type = OrderFieldType.Text,
                        Editable = true,
                        IsRequired = false
                    },
                    new OrderField
                    {
                        Id = "12",
                        Name = "Email",
                        Type = OrderFieldType.Email,
                        Editable = true,
                        IsRequired = false
                    },
                    new OrderField
                    {
                        Id = "13",
                            Name = IsRussianCulture ? "Телефон" : "Phone",
                        Type = OrderFieldType.Phone,
                        Editable = true,
                        IsRequired = false
                    },
                    new OrderField
                    {
                        Id = "14",
                            Name = IsRussianCulture ? "Открытка" : "Postcard",
                        Type = OrderFieldType.Text,
                        Editable = true,
                        IsRequired = false,
                        IsOptional = true
                    }
                }
            },
            new OrderFieldsGroup
            {
                Id = "2",
                Name = null,
                Items = new List<OrderField>
                {
                    new OrderField
                    {
                        Id = "21",
                            Name = IsRussianCulture ? "Количество персон" : "Number of persons",
                        Type = OrderFieldType.Counter,
                        Editable = true,
                        IsRequired = true,
                        Values = new List<string>
                        {
                                "1", "2", "3", "4", "5",  IsRussianCulture ? "более 5" : "more 5"
                        }
                    }
                }
            },
            new OrderFieldsGroup
            {
                Id = "3",
                Name = null,
                Items = new List<OrderField>
                {
                    new OrderField
                    {
                        Id = "31",
                            Name = IsRussianCulture ? "Комментарий к заказу" : "Comment",
                        Type = OrderFieldType.Text,
                        Editable = true,
                        IsRequired = false
                    }
                }
            }
        };

            _payments = new List<Payment>
            {
                new Payment
                {
                    Id = "0",
                    Name = IsRussianCulture ? "Наличными" : "Cash",
                    Type = PaymentType.Cash
                },
                new Payment
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Банковской картой при получении" : "Credit card",
                    Type = PaymentType.CreditCard,
                    InAppPurchase = true
                }
                #if DEBUG
                ,new Payment
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Банковской картой в приложении" : "Credit card in app",
                    Type = PaymentType.CreditCard,
                    InAppPurchase = true
                },
                new Payment
                {
                    Id = "3",
                    Name = "Apple Pay",
                    Type = PaymentType.Native,
                    InAppPurchase = true
                }
                #endif
            };
        }

        public async Task<List<OrderFieldsGroup>> GetUserFieldsGroups()
        {
            await Task.Delay(700);
            return _userFieldsGroups;
        }

        public async Task<FieldsValidation> ConfirmUser(Dictionary<string, string> userFieldsIdValues)
        {
            await Task.Delay(700);
            var result = new FieldsValidation
            {
                NotValidFields = new List<Field>(),
                Error = ""
            };

            foreach (var field in userFieldsIdValues)
            {
                var dbField = _userFieldsGroups.SelectMany(x => x.Items).FirstOrDefault(x => x.Id.Equals(field.Key));
                if (dbField.IsRequired && string.IsNullOrWhiteSpace(field.Value))
                {
                    result.NotValidFields.Add(new Field { Id = field.Key });

                    if (string.IsNullOrWhiteSpace(result.Error))
                        result.Error = IsRussianCulture ? "Не заполнены следующие обязательные поля:" : "The following required fields are missing:";
                    result.Error += $" {dbField.Name.ToLower()},";
                }
            }
            result.Error.TrimEnd(new[] { ',' });

            return result;
        }

        public async Task<PaymentNecessary> GetPaymentNecessary(string deliveryId)
        {
            await Task.Delay(700);
            return new PaymentNecessary { IsNecessary = true };
        }

        public async Task<List<Payment>> GetPayments(string deliveryId)
        {
            await Task.Delay(700);
            return _payments;
        }

        public Task ConfirmPayment(string paymentId)
        {
            return Task.Delay(700);
        }

        public async Task<Order> CreateOrder()
        {
            await Task.Delay(700);
            return new Order { Id = $"order_12345" };
        }

        public async Task ConfirmOrder(string orderId, bool isPaid)
        {
            await Task.Delay(700);
        }

        public async Task<ConfirmedOrderInfo> ConfirmedOrderInfo(string orderId)
        {
            await Task.Delay(700);

            return new ConfirmedOrderInfo
            {
                Price = 1,
                Quantity = 1,
                Currency = "RUB"
            };
        }

        public async Task<List<OrderFieldAutocompleteValue>> GetAutocompleteValues(string fieldId, string value, Dictionary<string, string> dependentFieldsValues)
        {
            await Task.Delay(700);
            var t = new List<OrderFieldAutocompleteValue>();
            for (var i = 0; i < 20; i++)
                t.Add(new OrderFieldAutocompleteValue { Value = $"{value}_{i}" });
            return t;
        }
    }
}
