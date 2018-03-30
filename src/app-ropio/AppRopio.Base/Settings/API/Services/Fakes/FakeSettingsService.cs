using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Models.Settings.Responses;

namespace AppRopio.Base.Settings.API.Services.Fakes
{
    public class FakeSettingsService : ISettingsService
    {
        private List<RegionGroup> _regions = new List<RegionGroup>
        {
            new RegionGroup()
            {
                Regions = new List<Region>() {
                                new Region
                                {
                                    Id = "0",
                                    Title = "Все города"
                                },
                                new Region
                                {
                                    Id = "2",
                                    Title = "Москва"
                                },
                                new Region
                                {
                                    Id = "1",
                                    Title = "Санкт-Петербург"
                                },
                }
            },

            new RegionGroup()
            {
                Title = "А",
                Regions = new List<Region>() {
                                new Region
                                {
                                    Id = "4",
                                    Title = "Алтайский край"
                                },
                                new Region
                                {
                                    Id = "5",
                                    Title = "Амурская область"
                                }
                }
            },

            new RegionGroup()
            {
                Title = "Б",
                Regions = new List<Region>() {
                                new Region
                                {
                                    Id = "6",
                                    Title = "Белгородская область"
                                }
                }
            },

            new RegionGroup()
            {
                Title = "В",
                Regions = new List<Region>() {
                                new Region
                                {
                                    Id = "7",
                                    Title = "Вологодская область"
                                }
                }
            },

            new RegionGroup()
            {
                Title = "И",
                Regions = new List<Region>() {
                                new Region
                                {
                                    Id = "7",
                                    Title = "Иркутская область"
                                }
                }
            },

            new RegionGroup()
            {
                Title = "Л",
                Regions = new List<Region>() {
                                new Region
                                {
                                    Id = "8",
                                    Title = "Ленинградская область"
                                }
                }
            },

            new RegionGroup()
            {
                Title = "М",
                Regions = new List<Region>() {
                                new Region
                                {
                                    Id = "9",
                                    Title = "Московская область"
                                }
                }
            },

            new RegionGroup()
            {
                Title = "Н",
                Regions = new List<Region>() {
                                new Region
                                {
                                    Id = "10",
                                    Title = "Новгородская область"
                                }
                }
            },

            new RegionGroup()
            {
                Title = "О",
                Regions = new List<Region>() {
                                new Region
                                {
                                    Id = "11",
                                    Title = "Оренбуржская область"
                                }
                }
            }
        };

        public async Task<List<RegionGroup>> GetRegions()
        {
            await Task.Delay(500);

            return _regions;
        }

        public async Task<Region> GetRegion(string id)
        {
            await Task.Delay(500);

            return _regions.SelectMany(c => c.Regions).FirstOrDefault(c => c.Id == id);
        }

        public async Task<List<RegionGroup>> SearchRegions(string query)
        {
            await Task.Delay(500);

            return _regions
                .Where(x => x.Regions.Any(y => y.Title.StartsWith(query)))
                .Select(x => new RegionGroup { Title = x.Title, Regions = x.Regions.Where(y => y.Title.StartsWith(query)).ToList() })
                .ToList();
        }

        public async Task<Region> GetCurrentRegion()
        {
            await Task.Delay(500);

            return _regions.First().Regions.Last();
        }
    }
}