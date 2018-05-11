using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Basket.Responses.Basket;
using AppRopio.Models.Products.Responses;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Basket.API.Services.Fake
{
    public class BasketFakeService : IBasketService
    {
        public bool IsRussianCulture => Mvx.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        private BasketModel _basket;

        public BasketFakeService()
        {
            _basket = new BasketModel
            {
                Items = new List<BasketItem>
            {

                new BasketItem
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Угловой диван Камелот" : "Corner sofa Camelot",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg" },
                    },
                    Price = 1999000,
                    OldPrice = 2100200,
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge
                        {
                            Name = "new",
                            Color = "#39C3FF"
                        },
                        new ProductBadge
                        {
                            Name = "sale",
                            Color = "#FC224B"
                        }
                    },
                        State = new ProductState { Name = IsRussianCulture ? "В наличии" : "In stock", Type = ProductStateType.InStock },
                    Quantity = 2,
                    UnitName = IsRussianCulture ? "шт" : "PC",
                    UnitStep = 1.0f
                },
                new BasketItem
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Угловой диван Бруклин" : "Corner sofa Brooklyn",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg" },
                    },
                    Price = 2000100,
                    OldPrice = 2000200,
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge
                        {
                            Name = "sale",
                            Color = "#FC224B"
                        }
                    },
                    Quantity = 1,
                    UnitName = IsRussianCulture ? "шт" : "PC",
                    UnitStep = 1.0f
                },
                new BasketItem
                {
                    Id = "3",
                        Name = IsRussianCulture ? "Угловой диван Камелот" : "Corner sofa Camelot",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg" },
                    },
                    Price = 1999000,
                    OldPrice = 2100200,
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge
                        {
                            Name = "new",
                            Color = "#39C3FF"
                        },
                        new ProductBadge
                        {
                            Name = "sale",
                            Color = "#FC224B"
                        }
                    },
                        State = new ProductState { Name = IsRussianCulture ? "В наличии" : "In stock", Type = ProductStateType.InStock },
                    Quantity = 2,
                    UnitName = IsRussianCulture ? "шт" : "PC",
                    UnitStep = 1.0f
                },
                new BasketItem
                {
                    Id = "4",
                        Name = IsRussianCulture ? "Угловой диван Бруклин" : "Corner sofa Brooklyn",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg" },
                    },
                    Price = 2000100,
                    OldPrice = 2000200,
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge
                        {
                            Name = "sale",
                            Color = "#FC224B"
                        }
                    },
                    Quantity = 1,
                    UnitName = IsRussianCulture ? "шт" : "PC",
                    UnitStep = 1.0f
                }
            },
                Amount = 6000300,
                Discount = 100,
                VersionId = "42"
            };
        }

        public async Task<bool> IsNeedToLoad(string versionId, CancellationToken cancellationToken)
        {
            await Task.Delay(700, cancellationToken);
            return !_basket.VersionId.Equals(versionId);
        }

        public async Task<BasketModel> GetBasket(CancellationToken cancellationToken)
        {
            await Task.Delay(700, cancellationToken);
            return _basket;
        }

        public async Task DeleteAllItems()
        {
            await Task.Delay(700);
            _basket.Items.Clear();
            _basket.Amount = 0;
            _basket.Discount = 0;
        }

        public async Task DeleteItem(string id)
        {
            await Task.Delay(700);
            var p = _basket.Items.FirstOrDefault(x => x.Id.Equals(id));
            if (p != null)
            {
                _basket.Items.Remove(p);
                _basket.Amount -= p.Price;
                _basket.Discount -= (p.Price - p.OldPrice ?? p.Price);
            }
        }

        public async Task<int> GetQuantity()
        {
            await Task.Delay(700);

            return _basket.Items.Sum(x => (int)x.Quantity);
        }

        public async Task AddProductToBasket(string groupId, string productId)
        {
            await Task.Delay(500);
            _basket.Items.Add(
                new BasketItem
                {
                    Id = productId,
                    Name = IsRussianCulture ? "Угловой диван Бруклин" : "Corner sofa Brooklyn",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg" },
                    },
                    Price = 2000100,
                    OldPrice = 2000200,
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge
                        {
                            Name = "sale",
                            Color = "#FC224B"
                        }
                    },
                    Quantity = 1,
                    UnitName = IsRussianCulture ? "шт" : "PC",
                    UnitStep = 1.0f
                }
            );
        }

        public async Task<ProductQuantity> BasketProductQuantity(string productId)
        {
            await Task.Delay(500);

            return productId == "1" ? new ProductQuantity { Quantity = 10 } : null;
        }

        public async Task<ProductQuantity> ChangeQuantity(string id, float quantity, CancellationToken token)
        {
            await Task.Delay(500, token);

            if (quantity > 10)
                return new ProductQuantity { Quantity = 10, Error = IsRussianCulture ? "Больше 10 шт. не положено!" : "A maximum of ten pieces" };

            var p = _basket.Items.FirstOrDefault(x => x.Id.Equals(id));
            if (p != null)
            {
                _basket.Amount -= p.Price * (decimal)p.Quantity;

                _basket.Amount += p.Price * (decimal)quantity;
            }

            return new ProductQuantity { Quantity = quantity };
        }

        public Task DeleteBasketProduct(string productId)
        {
            var p = _basket.Items.FirstOrDefault(x => x.Id.Equals(productId));
            if (p != null)
            {
                _basket.Amount -= p.Price * (decimal)p.Quantity;
                _basket.Items.Remove(p);
            }

            return Task.Delay(500);
        }

        public async Task<BasketValidity> CheckBasketValidity(string versionId, CancellationToken cancellationToken)
        {
            await Task.Delay(500);
            return new BasketValidity { IsValid = true /*, NotValidMessage = "Чтобы оформить заказ, выберите товаров ещё на 1000 руб."*/ };
        }

        public async Task<BasketAmount> GetBasketSummaryAmount(CancellationToken token)
        {
            await Task.Delay(500);
            return new BasketAmount { Amount = _basket.Amount };
        }
    }
}
