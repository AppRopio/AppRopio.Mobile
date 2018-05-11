using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Map.Responses;
using System.Linq;
using MvvmCross.Platform;
using AppRopio.Base.API.Services;

namespace AppRopio.Base.Map.API.Services.Fakes
{
    public class PointsFakeService : IPointsService
    {
        public bool IsRussianCulture => Mvx.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        private List<Point> _points;

        public PointsFakeService()
        {
            _points = IsRussianCulture ?
                new List<Point>
            {
                new Point
                {
                    Id = "0",
                    Name = IsRussianCulture ? "Магазин 1" : "Shop 1",
                    Address = IsRussianCulture ? "Адрес 11" : "Address 1",
                    WorkTime = IsRussianCulture ? "пн -пт: 10.00 - 21.00\nсб - вс: 10.00 - 20.00" : "24/7",
                    Phone = "+7(111)111-11-11",
                    Distance = IsRussianCulture ? "4 км" : "4 km",
                    AdditionalInfo = IsRussianCulture ? "Очень длинное описание, например о сроке доставки или еще что-нибудь полезное о данном магазине." : "A very long description, for example about the delivery time or something else useful about this store."
                },
                new Point
                {
                    Id = "1",
                    Name = "Магазин 2\nс названием, длиннее обычного",
                    Address = "Адрес 22",
                    WorkTime = "пн -пт: 10.00 - 21.00\nсб - вс: 10.00 - 20.00",
                    Distance = "7 км"
                },
                new Point
                {
                    Id = "2",
                    Name = "Магазин 3",
                    Address = "Адрес 33\nОчень длинный адрес",
                    WorkTime = "пн -пт: 10.00 - 21.00\nсб - вс: 10.00 - 20.00",
                    AdditionalInfo = "Очень длинное описание, например о сроке доставки" +
                        "или еще что-нибудь полезное о данном магазине. ",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "3",
                    Name = "Магазин 4",
                    Address = "Адрес 44",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "4",
                    Name = "Магазин 5",
                    Address = "Адрес 55",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "5",
                    Name = "Магазин 6",
                    Address = "Адрес 66",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "6",
                    Name = "Магазин 7",
                    Address = "Адрес 77",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "7",
                    Name = "Магазин 8",
                    Address = "Адрес 88",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "8",
                    Name = "Магазин 9",
                    Address = "Адрес 99",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "9",
                    Name = "Магазин 10",
                    Address = "Адрес 1010",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "10",
                    Name = "Магазин 11",
                    Address = "Адрес 1111",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "11",
                    Name = "Магазин 12",
                    Address = "Адрес 1212",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "12",
                    Name = "Магазин 13",
                    Address = "Адрес 1313",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                },
                new Point
                {
                    Id = "13",
                    Name = "Магазин 14",
                    Address = "Адрес 1414",
                    Distance = "42 км",
                    Phone = "+7(111)111-11-11"
                }
            }
                :
            new List<Point>
            {
                new Point
                {
                    Id = "0",
                    Name = IsRussianCulture? "Shop 1": "Shop 1",
                    Address = IsRussianCulture? "Address 11": "Address 1",
                    WorkTime = IsRussianCulture? "Mon-Fri: 10.00 - 21.00 \nSat - Sun: 10.00 - 20.00": "24/7",
                    Phone = "+7 (111) 111-11-11",
                    Distance = IsRussianCulture? "4 km": "4 km",
                    AdditionalInfo = IsRussianCulture? "A very long description, for example about the delivery time or something else useful about this store." : "A very long description, for example about the delivery time or something else useful about this store."
                },
                new Point
                {
                    Id = "1",
                    Name = "Store 2 \n with a name longer than usual",
                    Address = "Address 22",
                    WorkTime = "Mon-Fri: 10.00 - 21.00 \nSat - Sun: 10.00 - 20.00",
                    Distance = "7 km"
                },
                new Point
                {
                    Id = "2",
                    Name = "Shop 3",
                    Address = "Address 33 \nVery Long Address",
                    WorkTime = "Mon-Fri: 10.00 - 21.00 \nSat - Sun: 10.00 - 20.00",
                    AdditionalInfo = "A very long description, such as the delivery time" +
                    "or something else useful about this store.",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "3",
                    Name = "Shop 4",
                    Address = "Address 44",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "4",
                    Name = "Store 5",
                    Address = "Address 55",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                Id = "5",
                Name = "Shop 6",
                Address = "Address 66",
                Distance = "42 km",
                Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "6",
                    Name = "Shop 7",
                    Address = "Address 77",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "7",
                    Name = "Shop 8",
                    Address = "Address 88",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "8",
                    Name = "Shop 9",
                    Address = "Address 99",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "9",
                    Name = "Shop 10",
                    Address = "Address 1010",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "10",
                    Name = "Store 11",
                    Address = "Address 1111",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "11",
                    Name = "Store 12",
                    Address = "Address 1212",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "12",
                    Name = "Store 13",
                    Address = "Address 1313",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                },
                new Point
                {
                    Id = "13",
                    Name = "Shop 14",
                    Address = "Address 1414",
                    Distance = "42 km",
                    Phone = "+7 (111) 111-11-11"
                }
            };

            var r = new Random();
            foreach (var point in _points)
            {
                point.Coordinates = new Coordinates
                {
                    Latitude = 59.9 + r.NextDouble() / 10,
                    Longitude = 30.3 + r.NextDouble() / 10
                };
            }
        }

        public async Task<List<Point>> GetPoints(Coordinates position, string searchText, int offset = 0, int count = 10)
        {
            await Task.Delay(700);
            return string.IsNullOrEmpty(searchText) ?
                         (offset < 20 ? _points : new List<Point>())
                             :
                         _points.Where(x => x.Name.Contains(searchText) || x.Address.Contains(searchText)).ToList();
        }
    }
}
