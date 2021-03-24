using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Basket.Responses.Enums;
using AppRopio.Models.Basket.Responses.Order;
using MvvmCross;

namespace AppRopio.ECommerce.Basket.API.Services.Fake
{
    public class DeliveryFakeService : IDeliveryService
    {
        public bool IsRussianCulture => Mvx.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        private List<Delivery> _deliveries;

        private List<DeliveryPoint> _deliveryPoints;

        private List<OrderField> _addressFields;

        private Dictionary<string, List<DeliveryDay>> _deliveryDays;

        public DeliveryFakeService()
        {
            _deliveries = new List<Delivery>
            {
                new Delivery
                {
                    Id = "0",
                    Name = IsRussianCulture ? "Самовывоз" : "Pickup",
                    Type = DeliveryType.DeliveryPoint,
                    DeliveryTimeIsNeeded = true,
                    RequiredDataEntry = true,
                    Price = null
                },
                new Delivery
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Курьером" : "By courier",
                    Type = DeliveryType.Address,
                    Price = 100500,
                    DeliveryTimeIsNeeded = true,
                    RequiredDataEntry = true
                },
                new Delivery
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Почтой России" : "By post",
                    Type = DeliveryType.Address,
                    Price = 0,
                    DeliveryTimeIsNeeded = true
                }
            };
            _deliveryPoints = new List<DeliveryPoint>
            {
                new DeliveryPoint
                {
                    Id = "0",
                    Name = IsRussianCulture ? "Тест 1" : "Test 1",
                    Address = IsRussianCulture ? "Тест 11" : "Test 11",
                    WorkTime = "24/7",
                    Phone = "+7(111)111-11-11",
                    Distance = IsRussianCulture ? "4 км" : "4 km",
                    AdditionalInfo = IsRussianCulture ? "Тест 111" : "Test 111",
                    Coordinates = new Coordinates
                    {
                        Latitude = 59.907490,
                        Longitude = 30.324494
                    }
                },
                new DeliveryPoint
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Тест 2" : "Test 2",
                    Address = IsRussianCulture ? "Тест 22" : "Test 22",
                    WorkTime = "24/7",
                    Distance = IsRussianCulture ? "7 км" : "7 km",
                    Coordinates = new Coordinates
                    {
                        Latitude = 59.959063,
                        Longitude = 30.327412
                    }
                },
                new DeliveryPoint
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Тест 3" : "Test 3",
                    Address = IsRussianCulture ? "Тест 33" : "Test 33",
                    WorkTime = "24/7",
                    AdditionalInfo = IsRussianCulture ? "Тест 333" : "Test 333",
                    Phone = "+7(111)111-11-11"
                },
                new DeliveryPoint
                {
                    Id = "3",
                    Name = IsRussianCulture ? "Тест 4" : "Test 4",
                    Address = IsRussianCulture ? "Тест 44" : "Test 44",
                    Distance = IsRussianCulture ? "42 км" : "42 km",
                    Phone = "+7(111)111-11-11",
                    Coordinates = new Coordinates
                    {
                        Latitude = 59.959051,
                        Longitude = 30.327412
                    }
                }
            };
            _addressFields = new List<OrderField>
            {
                new OrderField
                {
                    Id = "0",
                    Name = IsRussianCulture ? "Город" : "City",
                    Type = OrderFieldType.City,
                    Editable = true,
                    IsRequired = true,
                    HasAutocomplete = true,
                    AutocompleteStartIndex = 2
                },
                new OrderField
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Улица" : "Street",
                    Type = OrderFieldType.Text,
                    Editable = true,
                    IsRequired = true,
                    HasAutocomplete = true,
                    AutocompleteStartIndex = 2,
                    DependentFieldsIds = new List<string> { "0" }
                },
                new OrderField
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Дом" : "House",
                    Type = OrderFieldType.Text,
                    Editable = true,
                    IsRequired = true
                },
                new OrderField
                {
                    Id = "3",
                    Name = IsRussianCulture ? "Квартира" : "Apartment",
                    Type = OrderFieldType.Text,
                    Editable = true,
                    IsRequired = true
                },
                new OrderField
                {
                    Id = "4",
                    Name = IsRussianCulture ? "Комментарий" : "Comment",
                    Type = OrderFieldType.Text,
                    Editable = true,
                    IsRequired = false,
                    IsOptional = true
                }
            };
            _deliveryDays = new Dictionary<string, List<DeliveryDay>>
            {
                { "0", new List<DeliveryDay>
                    {
                        new DeliveryDay
                        {
                            Id = "0",
                            Name = IsRussianCulture ? "1 августа" : "1 August",
                            Times = new List<DeliveryTime>
                            {
                                new DeliveryTime { Id = "02", Name = "10:00 - 12:00" },
                                new DeliveryTime { Id = "03", Name = "12:00 - 14:00" },
                                new DeliveryTime { Id = "04", Name = "18:00 - 20:00" },
                                new DeliveryTime { Id = "05", Name = "20:00 - 22:00" }
                            }
                        },
                        new DeliveryDay
                        {
                            Id = "1",
                            Name = IsRussianCulture ? "2 августа" : "2 August",
                            Times = new List<DeliveryTime>
                            {
                                new DeliveryTime { Id = "10", Name = "10:00 - 12:00" },
                                new DeliveryTime { Id = "11", Name = "12:00 - 14:00" },
                                new DeliveryTime { Id = "12", Name = "16:00 - 18:00" }
                            }
                        },
                        new DeliveryDay
                        {
                            Id = "2",
                            Name = IsRussianCulture ? "3 августа" : "3 August",
                            Times = new List<DeliveryTime>
                            {
                                new DeliveryTime { Id = "20", Name = "16:00 - 18:00" },
                                new DeliveryTime { Id = "21", Name = "18:00 - 20:00" }
                            }
                        }
                    }
                },
                { "1", new List<DeliveryDay>
                    {
                        new DeliveryDay
                        {
                            Id = "0",
                            Name = IsRussianCulture ? "1 августа" : "1 August",
                            Times = new List<DeliveryTime>
                            {
                                new DeliveryTime { Id = "00", Name = "10:00 - 12:00" }
                            }
                        },
                        new DeliveryDay
                        {
                            Id = "1",
                            Name = IsRussianCulture ? "2 августа" : "2 August",
                            Times = new List<DeliveryTime>
                            {
                                new DeliveryTime { Id = "10", Name = "16:00 - 18:00" },
                                new DeliveryTime { Id = "11", Name = "18:00 - 20:00" }
                            }
                        }
                    }
                },
                { "2", new List<DeliveryDay>
                    {
                        new DeliveryDay
                        {
                            Id = "0",
                            Name = IsRussianCulture ? "1 августа" : "1 August",
                            Times = new List<DeliveryTime>
                            {
                                new DeliveryTime { Id = "00", Name = "06:00 - 08:00" },
                                new DeliveryTime { Id = "01", Name = "08:00 - 10:00" },
                                new DeliveryTime { Id = "02", Name = "10:00 - 12:00" },
                                new DeliveryTime { Id = "03", Name = "12:00 - 14:00" },
                                new DeliveryTime { Id = "04", Name = "18:00 - 20:00" },
                                new DeliveryTime { Id = "05", Name = "20:00 - 22:00" },
                                new DeliveryTime { Id = "06", Name = "22:00 - 00:00" },
                                new DeliveryTime { Id = "07", Name = "00:00 - 02:00" }
                            }
                        },
                        new DeliveryDay
                        {
                            Id = "1",
                            Name = IsRussianCulture ? "2 августа" : "2 August",
                            Times = new List<DeliveryTime>
                            {
                                new DeliveryTime { Id = "10", Name = "10:00 - 12:00" },
                                new DeliveryTime { Id = "11", Name = "12:00 - 14:00" },
                                new DeliveryTime { Id = "12", Name = "16:00 - 18:00" }
                            }
                        },
                        new DeliveryDay
                        {
                            Id = "2",
                            Name = IsRussianCulture ? "3 августа" : "3 August",
                            Times = new List<DeliveryTime>
                            {
                                new DeliveryTime { Id = "20", Name = "16:00 - 18:00" },
                                new DeliveryTime { Id = "21", Name = "18:00 - 20:00" }
                            }
                        }
                    }
                }
            };
        }

        public async Task<List<Delivery>> GetDeliveries()
        {
            await Task.Delay(700);
            return _deliveries;
        }

        public async Task<List<DeliveryPoint>> GetDeliveryPoints(string deliveryId, string searchText)
        {
            await Task.Delay(700);
            return string.IsNullOrEmpty(searchText) ? _deliveryPoints : _deliveryPoints.Where(x => x.Name.Contains(searchText) || x.Address.Contains(searchText)).ToList();
        }

        public async Task<List<OrderField>> GetDeliveryAddressFields(string deliveryId, Coordinates coordinates = null)
        {
            await Task.Delay(700);
            return _addressFields;
        }

        public async Task ConfirmDeliveryPoint(string deliveryId, string deliveryPointId)
        {
            await Task.Delay(700);

            foreach (var delivery in _deliveries)
                delivery.PreviouslySelected = false;

            var selectedDelivery = _deliveries.FirstOrDefault(x => x.Id == deliveryId);
            if (selectedDelivery != null)
                selectedDelivery.PreviouslySelected = false;
        }

        public async Task<FieldsValidation> ConfirmDeliveryAddress(string deliveryId, Dictionary<string, string> addressFieldsIdValues)
        {
            await Task.Delay(700);
            var result = new FieldsValidation
            {
                NotValidFields = new List<Field>(),
                Error = ""
            };

            foreach (var field in addressFieldsIdValues)
            {
                var dbField = _addressFields.FirstOrDefault(x => x.Id.Equals(field.Key));
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

        public async Task<List<DeliveryDay>> GetDeliveryTime(string deliveryId)
        {
            await Task.Delay(700);
            return _deliveryDays[deliveryId];
        }

        public async Task<decimal?> GetDeliveryPrice(string deliveryId)
        {
            await Task.Delay(700);
            return _deliveries.FirstOrDefault(x => x.Id.Equals(deliveryId))?.Price;
        }

        public async Task ConfirmDeliveryTime(string deliveryTimeId)
        {
            await Task.Delay(700);
        }

        public async Task ConfirmDelivery(string id)
        {
            await Task.Delay(700);
        }
    }
}
