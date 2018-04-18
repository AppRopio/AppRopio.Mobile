using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Settings.Responses;
using MvvmCross.Platform;

namespace AppRopio.Base.Settings.API.Services.Fakes
{
    public class FakeSettingsService : ISettingsService
    {
        public bool IsRussianCulture => Mvx.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        private List<RegionGroup> _regions;

        public FakeSettingsService()
        {
            _regions = new List<RegionGroup>
            {
                new RegionGroup()
                {
                    Regions = new List<Region>() {
                                    new Region
                                    {
                                        Id = "0",
                            Title = IsRussianCulture ? "Все города" : "All cities"
                                    },
                                    new Region
                                    {
                                        Id = "2",
                            Title = IsRussianCulture ? "Москва" : "Moscow"
                                    },
                                    new Region
                                    {
                                        Id = "1",
                            Title = IsRussianCulture ? "Санкт-Петербург" : "Saint-Petersburg"
                                    },
                    }
                },

                new RegionGroup()
                {
                    Title = IsRussianCulture ? "А" : "A",
                    Regions = new List<Region>() {
                                    new Region
                                    {
                                        Id = "4",
                            Title = IsRussianCulture ? "Алтайский край" : "Altai Territory"
                                    },
                                    new Region
                                    {
                                        Id = "5",
                            Title = IsRussianCulture ? "Амурская область" : "Amur Region"
                                    }
                    }
                },

                new RegionGroup()
                {
                    Title = IsRussianCulture ? "Б" : "B",
                    Regions = new List<Region>() {
                                    new Region
                                    {
                                        Id = "6",
                            Title = IsRussianCulture ? "Белгородская область" : "Belgorod region"
                                    }
                    }
                },

                new RegionGroup()
                {
                    Title = IsRussianCulture ? "В" : "V",
                    Regions = new List<Region>() {
                                    new Region
                                    {
                                        Id = "7",
                            Title = IsRussianCulture ? "Вологодская область" : "Vologda Region"
                                    }
                    }
                },

                new RegionGroup()
                {
                    Title = IsRussianCulture ? "И" : "I",
                    Regions = new List<Region>() {
                                    new Region
                                    {
                                        Id = "7",
                            Title = IsRussianCulture ? "Иркутская область" : "Irkutsk Region"
                                    }
                    }
                },

                new RegionGroup()
                {
                    Title = IsRussianCulture ? "Л" : "L",
                    Regions = new List<Region>() {
                                    new Region
                                    {
                                        Id = "8",
                            Title = IsRussianCulture ? "Ленинградская область" : "Leningrad Region"
                                    }
                    }
                },

                new RegionGroup()
                {
                    Title = IsRussianCulture ? "М" : "M",
                    Regions = new List<Region>() {
                                    new Region
                                    {
                                        Id = "9",
                            Title = IsRussianCulture ? "Московская область" : "Moscow Region"
                                    }
                    }
                },

                new RegionGroup()
                {
                    Title = IsRussianCulture ? "Н" : "N",
                    Regions = new List<Region>() {
                                    new Region
                                    {
                                        Id = "10",
                            Title = IsRussianCulture ? "Новгородская область" : "Novgorod Region"
                                    }
                    }
                },

                new RegionGroup()
                {
                    Title = IsRussianCulture ? "О" : "O",
                    Regions = new List<Region>() {
                                    new Region
                                    {
                                        Id = "11",
                            Title = IsRussianCulture ? "Оренбуржская область" : "Orenburg Region"
                                    }
                    }
                }
            };
        }

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