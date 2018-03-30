using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Basket.Responses.Enums;
using AppRopio.Models.Basket.Responses.Order;

namespace AppRopio.ECommerce.Basket.API.Services.Fake
{
    public class DeliveryFakeService : IDeliveryService
    {
        private List<Delivery> _deliveries = new List<Delivery>
        {
            new Delivery
            {
                Id = "0",
                Name = "Самовывоз",
                Type = DeliveryType.DeliveryPoint,
                DeliveryTimeIsNeeded = true,
                RequiredDataEntry = true,
                Price = null
            },
            new Delivery
            {
                Id = "1",
                Name = "Курьером",
                Type = DeliveryType.Address,
                Price = 100500,
                DeliveryTimeIsNeeded = true,
                RequiredDataEntry = true
            },
            new Delivery
            {
                Id = "2",
                Name = "Почтой России",
                Type = DeliveryType.Address,
                Price = 0,
                DeliveryTimeIsNeeded = true
            }
        };

        private List<DeliveryPoint> _deliveryPoints = new List<DeliveryPoint>
        {
            new DeliveryPoint
            {
                Id = "0",
                Name = "Тест 1",
                Address = "Тест 11",
                WorkTime = "24/7",
                Phone = "+7(111)111-11-11",
                Distance = "4 км",
                AdditionalInfo = "Тест 111",
                Coordinates = new Coordinates
                {
                    Latitude = 59.907490,
                    Longitude = 30.324494
                }
            },
            new DeliveryPoint
            {
                Id = "1",
                Name = "Тест 2",
                Address = "Тест 22",
                WorkTime = "24/7",
                Distance = "7 км",
                Coordinates = new Coordinates
                {
                    Latitude = 59.959063,
                    Longitude = 30.327412
                }
            },
            new DeliveryPoint
            {
                Id = "2",
                Name = "Тест 3",
                Address = "Тест 33",
                WorkTime = "24/7",
                AdditionalInfo = "Тест 333",
                Phone = "+7(111)111-11-11"
            },
            new DeliveryPoint
            {
                Id = "3",
                Name = "Тест 4",
                Address = "Тест 44",
                Distance = "42 км",
                Phone = "+7(111)111-11-11",
                Coordinates = new Coordinates
                {
                    Latitude = 59.959051,
                    Longitude = 30.327412
                }
            }
        };

        private List<OrderField> _addressFields = new List<OrderField>
        {
            new OrderField
            {
                Id = "0",
                Name = "Город",
                Type = OrderFieldType.City,
                Editable = true,
                IsRequired = true,
                HasAutocomplete = true,
                AutocompleteStartIndex = 2
            },
            new OrderField
            {
                Id = "1",
                Name = "Улица",
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
                Name = "Дом",
                Type = OrderFieldType.Text,
                Editable = true,
                IsRequired = true
            },
            new OrderField
            {
                Id = "3",
                Name = "Квартира",
                Type = OrderFieldType.Text,
                Editable = true,
                IsRequired = true
            },
            new OrderField
            {
                Id = "4",
                Name = "Комментарий",
                Type = OrderFieldType.Text,
                Editable = true,
                IsRequired = false,
                IsOptional = true
            }
        };

        private Dictionary<string, List<DeliveryDay>> _deliveryDays = new Dictionary<string, List<DeliveryDay>>
        {
            { "0", new List<DeliveryDay>
                {
                    new DeliveryDay
                    {
                        Id = "0",
                        Name = "1 августа",
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
                        Name = "2 августа",
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
                        Name = "3 августа",
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
                        Name = "1 августа",
                        Times = new List<DeliveryTime>
                        {
                            new DeliveryTime { Id = "00", Name = "10:00 - 12:00" }
                        }
                    },
                    new DeliveryDay
                    {
                        Id = "1",
                        Name = "2 августа",
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
                        Name = "1 августа",
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
                        Name = "2 августа",
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
                        Name = "3 августа",
                        Times = new List<DeliveryTime>
                        {
                            new DeliveryTime { Id = "20", Name = "16:00 - 18:00" },
                            new DeliveryTime { Id = "21", Name = "18:00 - 20:00" }
                        }
                    }
                }
            }
        };

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
                        result.Error = "Не заполнены следующие обязательные поля:";
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
