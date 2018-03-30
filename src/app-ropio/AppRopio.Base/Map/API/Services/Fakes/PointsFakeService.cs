using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Map.Responses;
using System.Linq;

namespace AppRopio.Base.Map.API.Services.Fakes
{
    public class PointsFakeService : IPointsService
    {
        private List<Point> _points = new List<Point>
        {
            new Point
            {
                Id = "0",
                Name = "Магазин 1",
                Address = "Адрес 11",
                WorkTime = "пн -пт: 10.00 - 21.00\nсб - вс: 10.00 - 20.00",
                Phone = "+7(111)111-11-11",
                Distance = "4 км",
                AdditionalInfo = "Очень длинное описание, например о сроке доставки" +
                    "или еще что-нибудь полезное о данном магазине. "
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
        };

        public PointsFakeService()
        {
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
