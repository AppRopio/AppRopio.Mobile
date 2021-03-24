﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Marked.Responses;
using AppRopio.Models.Products.Responses;
using MvvmCross;

namespace AppRopio.ECommerce.Marked.API.Services.Fakes
{
    public class FakeMarkedSerivce : IMarkedService
    {
        public bool IsRussianCulture => Mvx.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        public async Task<List<MarkedProduct>> GetMarkedProducts(int count, int offset = 0)
        {
            await Task.Delay(500);

            return new List<MarkedProduct>
            {
                new MarkedProduct()
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Рубашка «Камелот»" : "Product name",
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge() { Name = "sale", Color = "#45D1FF" },
                        new ProductBadge() { Name = "sale", Color = "#FC224B" }
                    },
                    OldPrice = 1000100,
                    Price = 2000200,
                    MaxPrice = 3000000,
                    ImageUrls = new List<Image>
                    {
                        new Image()
                        {
                            LargeUrl = "https://static-eu.insales.ru/images/products/1/4065/109039585/DRESSCOD.ORG-Rubashka-belaya-Kamelot-2.jpg",
                            SmallUrl = "https://static-eu.insales.ru/images/products/1/4065/109039585/DRESSCOD.ORG-Rubashka-belaya-Kamelot-2.jpg"
                        }
                    },
                    IsMarked = true
                },

                new MarkedProduct()
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Рубашка «Камелот»" : "Product name",
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge() { Name = "sale", Color = "#45D1FF" }
                    },
                    Price = 2000100,
                    ImageUrls = new List<Image>
                    {
                        new Image()
                        {
                            LargeUrl = "https://lanita.ru/images/offer/11227/0/950220/950220.jpg",
                            SmallUrl = "https://lanita.ru/images/offer/11227/0/950220/950220.jpg"
                        }
                    },
					IsMarked = true
                },

                new MarkedProduct()
				{
					Id = "3",
                    Name = IsRussianCulture ? "Рубашка «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#45D1FF" },
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
                    MaxPrice = 3000000,
                    ImageUrls = new List<Image>
                    {
                        new Image()
                        {
                            LargeUrl = "https://img2.wbstatic.net/c246x328/new/1440000/1445266-1.jpg",
                            SmallUrl = "https://img2.wbstatic.net/c246x328/new/1440000/1445266-1.jpg"
                        }
                    },
					IsMarked = true
				},

				new MarkedProduct()
				{
					Id = "4",
                    Name = IsRussianCulture ? "Платье «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
                    MaxPrice = 3000000,
                    ImageUrls = new List<Image>
					{
						new Image()
						{
							LargeUrl = "https://img2.wbstatic.net/big/new/950000/950226-3.jpg",
							SmallUrl = "https://img2.wbstatic.net/big/new/950000/950226-3.jpg"
						}
					},
					IsMarked = true
				},

				new MarkedProduct()
				{
					Id = "5",
                    Name = IsRussianCulture ? "Рубашка «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#45D1FF" },
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
					ImageUrls = new List<Image>
					{
						new Image()
						{
                            LargeUrl = "https://static-eu.insales.ru/images/products/1/4065/109039585/DRESSCOD.ORG-Rubashka-belaya-Kamelot-2.jpg",
                            SmallUrl = "https://static-eu.insales.ru/images/products/1/4065/109039585/DRESSCOD.ORG-Rubashka-belaya-Kamelot-2.jpg"
						}
					},
					IsMarked = true
				},

				new MarkedProduct()
				{
					Id = "6",
                    Name = IsRussianCulture ? "Платье «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
					ImageUrls = new List<Image>
					{
						new Image()
						{
                            LargeUrl = "https://lanita.ru/images/offer/11227/0/950220/950220.jpg",
                            SmallUrl = "https://lanita.ru/images/offer/11227/0/950220/950220.jpg"
						}
					},
					IsMarked = true
				},
				new MarkedProduct()
				{
					Id = "7",
                    Name = IsRussianCulture ? "Рубашка «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#45D1FF" },
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
					ImageUrls = new List<Image>
					{
						new Image()
						{
							LargeUrl = "https://img2.wbstatic.net/c246x328/new/1440000/1445266-1.jpg",
							SmallUrl = "https://img2.wbstatic.net/c246x328/new/1440000/1445266-1.jpg"
						}
					},
					IsMarked = true
				},

				new MarkedProduct()
				{
					Id = "8",
                    Name = IsRussianCulture ? "Платье «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
                    MaxPrice = 3000000,
                    ImageUrls = new List<Image>
					{
						new Image()
						{
							LargeUrl = "https://img2.wbstatic.net/big/new/950000/950226-3.jpg",
							SmallUrl = "https://img2.wbstatic.net/big/new/950000/950226-3.jpg"
						}
					},
					IsMarked = true
				},
				new MarkedProduct()
				{
					Id = "9",
                    Name = IsRussianCulture ? "Рубашка «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#45D1FF" },
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
                    MaxPrice = 3000000,
                    ImageUrls = new List<Image>
					{
						new Image()
						{
                            LargeUrl = "https://static-eu.insales.ru/images/products/1/4065/109039585/DRESSCOD.ORG-Rubashka-belaya-Kamelot-2.jpg",
                            SmallUrl = "https://static-eu.insales.ru/images/products/1/4065/109039585/DRESSCOD.ORG-Rubashka-belaya-Kamelot-2.jpg"
						}
					},
					IsMarked = true
				},

				new MarkedProduct()
				{
					Id = "10",
                    Name = IsRussianCulture ? "Платье «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
					ImageUrls = new List<Image>
					{
						new Image()
						{
                            LargeUrl = "https://lanita.ru/images/offer/11227/0/950220/950220.jpg",
                            SmallUrl = "https://lanita.ru/images/offer/11227/0/950220/950220.jpg"
						}
					},
					IsMarked = true
				},
				new MarkedProduct()
				{
					Id = "11",
                    Name = IsRussianCulture ? "Рубашка «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#45D1FF" },
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
                    MaxPrice = 3000000,
                    ImageUrls = new List<Image>
					{
						new Image()
						{
							LargeUrl = "https://img2.wbstatic.net/c246x328/new/1440000/1445266-1.jpg",
							SmallUrl = "https://img2.wbstatic.net/c246x328/new/1440000/1445266-1.jpg"
						}
					},
					IsMarked = true
				},

				new MarkedProduct()
				{
					Id = "12",
                    Name = IsRussianCulture ? "Платье «Камелот»" : "Product name",
					Badges = new List<ProductBadge>
					{
						new ProductBadge() { Name = "sale", Color = "#FC224B" }
					},
					OldPrice = 2000100,
					Price = 2000100,
                    MaxPrice = 3000000,
                    ImageUrls = new List<Image>
					{
						new Image()
						{
							LargeUrl = "https://img2.wbstatic.net/big/new/950000/950226-3.jpg",
							SmallUrl = "https://img2.wbstatic.net/big/new/950000/950226-3.jpg"
						}
					},
					IsMarked = true
				},
            }.Skip(offset).Take(count).ToList();
        }

        private int _quantity = -1;
        public async Task<int> GetQuantity()
        {
            await Task.Delay(500);

            _quantity++;

            return _quantity;
        }
    }
}