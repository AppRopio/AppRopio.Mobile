using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Filters.Responses;
using AppRopio.Models.Products.Responses;
using MvvmCross;

namespace AppRopio.ECommerce.Products.API.Services.Fakes
{
    public class ProductFakeService : IProductService
    {
        public bool IsRussianCulture => Mvx.IoCProvider.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        List<Product> _products;

        public ProductFakeService()
        {
            _products = _products = new List<Product>
            {
                new Product
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Угловой диван Камелот" : "Corner sofa Camelot",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg" },
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-canapes-sandwich-of-rye-bread-on-a-wooden-base-529082098.jpg" } ,
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-shooters-in-the-traditional-shot-glass-with-the-smoke-on-a-wooden-base-533348815.jpg" },
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg" },
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-canapes-sandwich-of-rye-bread-on-a-wooden-base-529082098.jpg" } ,
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-shooters-in-the-traditional-shot-glass-with-the-smoke-on-a-wooden-base-533348815.jpg" }
                    },
                    Price = 1999000,
                    OldPrice = 2100200,
                    MaxPrice = 3000000,
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
                    UnitName = IsRussianCulture ? "шт" : "PC",
                    UnitNameOld = IsRussianCulture ? "шт" : "PC",
                    UnitStep = 1.0f
                },
                new Product
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Тар-тар из телятины с теплыми лисичками" : "Veal tartar with warm chanterelles",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg" },
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-succulent-thick-juicy-portions-of-grilled-fillet-steak-served-with-tomatoes-and-roast-vegetables-on-138421859.jpg" } ,
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-gourmet-tasty-steak-burgers-with-ham-slices-on-a-wooden-tray-with-potato-wedges-and-dipping-sauce-316591013.jpg" }
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
                    UnitName = IsRussianCulture ? "кг" : "kg",
                    UnitNameOld = IsRussianCulture ? "кг" : "kg",
                    UnitStep = 1.0f,
                    ExtraPrice = new Price
                    {
                        Value = 3000300
                    }
                },
                new Product
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Угловой диван Камелот" : "Corner sofa Camelot",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg" },
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-succulent-thick-juicy-portions-of-grilled-fillet-steak-served-with-tomatoes-and-roast-vegetables-on-138421859.jpg" }
                    },
                    Price = 2000100,
                    MaxPrice = 3000000,
                    OldPrice = null,
                    UnitName = IsRussianCulture ? "мл" : "ml",
                    UnitNameOld = IsRussianCulture ? "мл" : "ml",
                    UnitStep = 1.0f
                },
                new Product
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Угловой диван Бруклин" : "Corner sofa Brooklyn",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg" }
                    },
                    Price = 2000100,
                    OldPrice = 2000200,
                    UnitName = IsRussianCulture ? "шт" : "PC",
                    UnitNameOld = IsRussianCulture ? "шт" : "PC",
                    UnitStep = 1.0f
                },
                new Product
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Угловой диван Камелот" : "Corner sofa Camelot",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "nophoto" },
                        new Image { LargeUrl = "nophoto" } ,
                        new Image { LargeUrl = "nophoto" }
                    },
                    Price = 2000100,
                    OldPrice = null,
                    UnitName = IsRussianCulture ? "кг" : "kg",
                    UnitNameOld = IsRussianCulture ? "кг" : "kg",
                    UnitStep = 1.0f
                },
                new Product
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Угловой диван Бруклин" : "Corner sofa Brooklyn",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg" },
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-succulent-thick-juicy-portions-of-grilled-fillet-steak-served-with-tomatoes-and-roast-vegetables-on-138421859.jpg" } ,
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-gourmet-tasty-steak-burgers-with-ham-slices-on-a-wooden-tray-with-potato-wedges-and-dipping-sauce-316591013.jpg" }
                    },
                    Price = 2000100,
                    OldPrice = 2000200,
                    MaxPrice = 3000000,
                    UnitName = IsRussianCulture ? "мл" : "ml",
                    UnitNameOld = IsRussianCulture ? "мл" : "ml",
                    UnitStep = 1.0f
                },
                new Product
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Угловой диван Камелот" : "Corner sofa Camelot",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg" },
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-succulent-thick-juicy-portions-of-grilled-fillet-steak-served-with-tomatoes-and-roast-vegetables-on-138421859.jpg" } ,
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-gourmet-tasty-steak-burgers-with-ham-slices-on-a-wooden-tray-with-potato-wedges-and-dipping-sauce-316591013.jpg" }
                    },
                    Price = 2000100,
                    OldPrice = null,
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge
                        {
                            Name = "new",
                            Color = "#39C3FF"
                        }
                    },
                    UnitName = IsRussianCulture ? "шт" : "PC",
                    UnitNameOld = IsRussianCulture ? "шт" : "PC",
                    UnitStep = 1.0f
                },
                new Product
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Угловой диван Бруклин" : "Corner sofa Brooklyn",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-mixed-salad-with-bacon-and-tomato-sauce-on-a-wooden-background-top-view-614268470.jpg" },
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-healthy-rice-pasta-with-vegetables-broccoli-cherry-tomato-and-cucumber-on-a-wooden-base-541171141.jpg" } ,
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-panorama-of-ham-pizza-with-capsicum-mushrooms-olives-and-basil-leaves-on-wooden-board-on-old-338081030.jpg" }
                    },
                    Price = 2000100,
                    OldPrice = 2000200,
                    MaxPrice = 3000000,
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge
                        {
                            Name = "sale",
                            Color = "#FC224B"
                        }
                    },
                    UnitName = IsRussianCulture ? "кг" : "kg",
                    UnitNameOld = IsRussianCulture ? "кг" : "kg",
                    UnitStep = 1.0f
                },
                new Product
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Угловой диван Камелот" : "Corner sofa Camelot",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg" },
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-succulent-thick-juicy-portions-of-grilled-fillet-steak-served-with-tomatoes-and-roast-vegetables-on-138421859.jpg" } ,
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-gourmet-tasty-steak-burgers-with-ham-slices-on-a-wooden-tray-with-potato-wedges-and-dipping-sauce-316591013.jpg" }
                    },
                    Price = 2000100,
                    MaxPrice = 3000000,
                    OldPrice = null,
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge
                        {
                            Name = "new",
                            Color = "#39C3FF"
                        }
                    },
                    UnitName = IsRussianCulture ? "мл" : "ml",
                    UnitNameOld = IsRussianCulture ? "мл" : "ml",
                    UnitStep = 1.0f
                },
                new Product
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Угловой диван Бруклин" : "Corner sofa Brooklyn",
                    ImageUrls = new List<Image>
                    {
                        new Image { SmallUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg", LargeUrl = "https://image.shutterstock.com/z/stock-photo-mixed-salad-with-bacon-and-tomato-sauce-on-a-wooden-background-top-view-614268470.jpg" },
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-healthy-rice-pasta-with-vegetables-broccoli-cherry-tomato-and-cucumber-on-a-wooden-base-541171141.jpg" } ,
                        new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-panorama-of-ham-pizza-with-capsicum-mushrooms-olives-and-basil-leaves-on-wooden-board-on-old-338081030.jpg" }
                    },
                    Price = 2000100,
                    OldPrice = 2000200,
                    MaxPrice = 3000000,
                    Badges = new List<ProductBadge>
                    {
                        new ProductBadge
                        {
                            Name = "sale",
                            Color = "#FC224B"
                        }
                    },
                    UnitName = IsRussianCulture ? "шт" : "PC",
                    UnitNameOld = IsRussianCulture ? "шт" : "PC",
                    UnitStep = 1.0f
                }
            };
        }

        public async Task<ProductShare> GetProductForShare(string groupId, string productId)
        {
            await Task.Delay(500);
            return new ProductShare
            {
                Title = IsRussianCulture ? "Угловой диван Бруклин" : "Corner sofa Brooklyn",
                Text = IsRussianCulture ? "Здесь продают угловой диван Бруклин по выгодной цене!" : "Special price only for you!",
                Url = "https://hoff.ru/catalog/gostinaya/divany/uglovye_kozhanye/divan_bruklin_id14210/"
            };
        }

        public async Task<ProductDetails> LoadProductDetails(string groupId, string productId)
        {
            await Task.Delay(500);

            if (groupId == _products[1].GroupId && productId == _products[1].Id)
            {
                return new ProductDetails
                {
                    Parameters = new List<ProductParameter>
                    {
                        new ProductParameter
                        {
                            Id = "1",
                            WidgetType = ProductWidgetType.MultilineText,
                            DataType = ProductDataType.Text,
                            Name = IsRussianCulture ? "Состав" : "Composition",
                            Content = IsRussianCulture ? "Грудка индейки горячего копчения, телятина запеченая с дымком, сырокопченая говядина, сыровяленая баранина" : "Hot smoked turkey breast, veal baked with haze, raw smoked beef, raw lamb"
                        },
                        new ProductParameter
                        {
                            Id = "2",
                            WidgetType = ProductWidgetType.HorizontalCollection,
                            DataType = ProductDataType.Products,
                            Name = IsRussianCulture ? "Недавно смотрели" : "Recently viewed"
                        },
                        new ProductParameter
                        {
                            Id = "3",
                            WidgetType = ProductWidgetType.HorizontalCollection,
                            DataType = ProductDataType.Products,
                            Name = IsRussianCulture ? "Мы рекомендуем" : "Recommended"
                        }
                    }
                };
            }

            return new ProductDetails
            {
                Parameters = new List<ProductParameter>
                {
                    new ProductParameter
                    {
                        Id = "1",
                        WidgetType = ProductWidgetType.HorizontalCollection,
                        DataType = ProductDataType.Color,
                        Name = IsRussianCulture ? "Цвет" : "Color",
                        Values = new List<ProductParameterValue>
                        {
                            new ProductParameterValue { Id = "1", Value = "#F86C50", ValueName = "Оранжевый" },
                            new ProductParameterValue { Id = "2", Value = "#95BA6D", ValueName = "Зеленый" },
                            new ProductParameterValue { Id = "3", Value = "#7DCDBB", ValueName = "Бирюзовый" },
                            new ProductParameterValue { Id = "4", Value = "#86AADB", ValueName = "Ультрамарин" },
                            new ProductParameterValue { Id = "5", Value = "#B5B5BF", ValueName = "Серый" },
                            new ProductParameterValue { Id = "6", Value = "#F6A623", ValueName = "Желтый" },
                            new ProductParameterValue { Id = "7", Value = "#5F94E1", ValueName = "Синий" },
                            new ProductParameterValue { Id = "8", Value = "#20C987", ValueName = "Изумрудный" },
                            new ProductParameterValue { Id = "9", Value = "#FC224B", ValueName = "Красный" }
                        }
                    },
                    new ProductParameter
                    {
                        Id = "2",
                        WidgetType = ProductWidgetType.HorizontalCollection,
                        DataType = ProductDataType.Text,
                        Name = IsRussianCulture ? "Бренд" : "Brand",
                        Values = new List<ProductParameterValue>
                        {
                            new ProductParameterValue { Id = "1", Value = "oodji", ValueName = "oodji" },
                            new ProductParameterValue { Id = "2", Value = "Zara", ValueName = "Zara" },
                            new ProductParameterValue { Id = "3", Value = "Adidas", ValueName = "Adidas" },
                            new ProductParameterValue { Id = "4", Value = "Mango", ValueName = "Mango" },
                            new ProductParameterValue { Id = "5", Value = "Nike", ValueName = "Nike" },
                            new ProductParameterValue { Id = "6", Value = "GAP", ValueName = "GAP" },
                            new ProductParameterValue { Id = "7", Value = "new balance", ValueName = "new balance" },
                            new ProductParameterValue { Id = "8", Value = "Under Armour", ValueName = "Under Armour" }
                        }
                    },
                    new ProductParameter
                    {
                        Id = "3",
                        WidgetType = ProductWidgetType.MinMax,
                        DataType = ProductDataType.Number,
                        Name = IsRussianCulture ? "Цена" : "Price",
                        MinValue = "399.0",
                        MaxValue = "10000.0"
                    },
                    new ProductParameter
                    {
                        Id = "4",
                        WidgetType = ProductWidgetType.MinMax,
                        DataType = ProductDataType.Date,
                        Name = IsRussianCulture ? "Дата доставки" : "Delivery date",
                        MinValue = "2017-03-27",
                        MaxValue = "2017-04-02"
                    },
                    new ProductParameter
                    {
                        Id = "5",
                        WidgetType = ProductWidgetType.VerticalCollection,
                        DataType = ProductDataType.Text,
                        Name = IsRussianCulture ? "Размер" : "Size",
                        Values = new List<ProductParameterValue>
                        {
                            new ProductParameterValue { Id = "1", Value = "42", ValueName = "XXS" },
                            new ProductParameterValue { Id = "2", Value = "44", ValueName = "XS" },
                            new ProductParameterValue { Id = "3", Value = "46", ValueName = "S" },
                            new ProductParameterValue { Id = "4", Value = "48", ValueName = "M" },
                            new ProductParameterValue { Id = "5", Value = "50", ValueName = "L" },
                            new ProductParameterValue { Id = "6", Value = "52", ValueName = "XL" },
                            new ProductParameterValue { Id = "7", Value = "54", ValueName = "XXL" }
                        }
                    },
                    new ProductParameter()
                    {
                        Id = "13",
                        WidgetType = ProductWidgetType.Transition,
                        DataType = ProductDataType.Custom,
                        CustomType = "reviews",
                        Name = IsRussianCulture ? "Отзывы" : "Reviews",
                        Content = "4.5"
                    },
                    new ProductParameter
                    {
                        Id = "6",
                        WidgetType = ProductWidgetType.Picker,
                        DataType = ProductDataType.Text,
                        Name = IsRussianCulture ? "Сортировка по" : "Sort by",
                        Values = new List<ProductParameterValue>
                        {
                            new ProductParameterValue { Id = "1", Value = "1", ValueName = IsRussianCulture ? "Определенный" : "Type 1" },
                            new ProductParameterValue { Id = "2", Value = "2", ValueName = IsRussianCulture ? "Какой-то цвет" : "Type 2" },
                            new ProductParameterValue { Id = "3", Value = "3", ValueName = IsRussianCulture ? "Один цвет" : "Type 3" },
                            new ProductParameterValue { Id = "4", Value = "4", ValueName = IsRussianCulture ? "Выбранный цвет" : "Type 4" },
                            new ProductParameterValue { Id = "5", Value = "5", ValueName = IsRussianCulture ? "Цвет другой" : "Type 5" },
                            new ProductParameterValue { Id = "6", Value = "6", ValueName = IsRussianCulture ? "Третий цвет" : "Type 6" },
                            new ProductParameterValue { Id = "7", Value = "7", ValueName = IsRussianCulture ? "Еще один цвет" : "Type 7" }
                        }
                    },

                    new ProductParameter
                    {
                        Id = "7",
                        WidgetType = ProductWidgetType.MultiSelection,
                        DataType = ProductDataType.Text,
                        Name = IsRussianCulture ? "Бренд" : "Brand",
                        Values = new List<ProductParameterValue>
                        {
                            new ProductParameterValue { Id = "1", Value = "Adidas", ValueName = "Adidas" },
                            new ProductParameterValue { Id = "2", Value = "GAP", ValueName = "GAP" },
                            new ProductParameterValue { Id = "3", Value = "Mango", ValueName = "Mango" },
                            new ProductParameterValue { Id = "4", Value = "new balance", ValueName = "new balance" },
                            new ProductParameterValue { Id = "5", Value = "Nike", ValueName = "Nike" },
                            new ProductParameterValue { Id = "6", Value = "oodji", ValueName = "oodji" },
                            new ProductParameterValue { Id = "7", Value = "Under Armour", ValueName = "Under Armour" },
                            new ProductParameterValue { Id = "8", Value = "Zara", ValueName = "Zara" },
                        }
                    },
                    new ProductParameter
                    {
                        Id = "8",
                        WidgetType = ProductWidgetType.OneSelection,
                        DataType = ProductDataType.Text,
                        Name = IsRussianCulture ? "Наличие в магазинах" : "Availability in shops",
                        Values = new List<ProductParameterValue>
                        {
                            new ProductParameterValue { Id = "1", Value = "1", ValueName = IsRussianCulture ? "Да" : "Yes" },
                            new ProductParameterValue { Id = "2", Value = "0", ValueName = IsRussianCulture ? "Нет" : "No" },
                            new ProductParameterValue { Id = "3", Value = "2", ValueName = IsRussianCulture ? "Не важно" : "It does not matter" }
                        }
                    },
                    new ProductParameter
                    {
                        Id = "9",
                        WidgetType = ProductWidgetType.Switch,
                        DataType = ProductDataType.Boolean,
                        Name = IsRussianCulture ? "Быстрая доставка" : "Fast delivery"
                    },
                    new ProductParameter
                    {
                        Id = "10",
                        WidgetType = ProductWidgetType.Transition,
                        DataType = ProductDataType.Text,
                        Name = IsRussianCulture ? "Описание товара" : "Description",
                        Content = IsRussianCulture ?
                            "Наполовину полон или наполовину пуст? Известная шутка со стаканом измеряет оптимизм, ну а бутылка от Monbento им наполняет, даря заряд позитива и хорошего настроения. Прозрачная половинка плюс цветная крышка из приятного прорезиненного пластика – получается идеальная бутылка небольшого размера, но отличной вместимости. \nМногоразовая бутылка вместимостью 330 мл пригодится в спортзале, на прогулке, дома, на даче – в общем, везде! Забудьте про одноразовые пластиковые ёмкости – они не красивые, да и засоряют окружающую среду. А такая красота в руках точно привлечет взгляды окружающих. \nГерметично закрывается. Изготовлена из безопасного пищевого пластика (BPA free). \nМожно мыть в посудомоечной машине."
                            :
                            "Half full or half empty? A well-known joke with a glass measures optimism, but the bottle from Monbento fills them, giving a charge of positive and good mood. Transparent half plus a colored lid made of pleasant rubberized plastic - it turns out an ideal bottle of small size, but of excellent capacity. \nA multi-purpose bottle with a capacity of 330 ml is useful in the gym, on a walk, at home, at the dacha - in general, everywhere! Forget about disposable plastic containers - they are not beautiful, and clog the environment. And such beauty in the hands will definitely attract the views of others. \nMeasily closes. It is made of safe food plastic (BPA free). \nYou can wash in the dishwasher."
                    },
                    new ProductParameter
                    {
                        Id = "11",
                        WidgetType = ProductWidgetType.Transition,
                        DataType = ProductDataType.Html,
                        Name = IsRussianCulture ? "Характеристики товара" : "Product features",
                        Content = @"<html><body><h1>Замечательный товар</h1>
<p>
<li>
  характеристика 1
</li>
<li>
  характеристика 2
</li>
<li>
  характеристика 3
</li>
</p>
<p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Nam cursus. Morbi ut mi. Nullam enim leo, egestas id, condimentum at, laoreet mattis, massa. Sed eleifend nonummy diam. Praesent mauris ante, elementum et, bibendum at, posuere sit amet, nibh. Duis tincidunt lectus quis dui viverra vestibulum. Suspendisse vulputate aliquam dui. Nulla elementum dui ut augue. Aliquam vehicula mi at mauris. Maecenas placerat, nisl at consequat rhoncus, sem nunc gravida justo, quis eleifend arcu velit quis lacus. Morbi magna magna, tincidunt a, mattis non, imperdiet vitae, tellus. Sed odio est, auctor ac, sollicitudin in, consequat vitae, orci. Fusce id felis. Vivamus sollicitudin metus eget eros. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. In posuere felis nec tortor.</p></body></html>"
                    },
                    new ProductParameter()
                    {
                        Id = "12",
                        WidgetType = ProductWidgetType.MultilineText,
                        DataType = ProductDataType.Text,
                        Name = IsRussianCulture ? "Описание товара" : "Description",
                        Content = IsRussianCulture ?
                            "Наполовину полон или наполовину пуст? Известная шутка со стаканом измеряет оптимизм, ну а бутылка от Monbento им наполняет, даря заряд позитива и хорошего настроения. Прозрачная половинка плюс цветная крышка из приятного прорезиненного пластика – получается идеальная бутылка небольшого размера, но отличной вместимости. \nМногоразовая бутылка вместимостью 330 мл пригодится в спортзале, на прогулке, дома, на даче – в общем, везде! Забудьте про одноразовые пластиковые ёмкости – они не красивые, да и засоряют окружающую среду. А такая красота в руках точно привлечет взгляды окружающих. \nГерметично закрывается. Изготовлена из безопасного пищевого пластика (BPA free). \nМожно мыть в посудомоечной машине."
                            :
                            "Half full or half empty? A well-known joke with a glass measures optimism, but the bottle from Monbento fills them, giving a charge of positive and good mood. Transparent half plus a colored lid made of pleasant rubberized plastic - it turns out an ideal bottle of small size, but of excellent capacity. \nA multi-purpose bottle with a capacity of 330 ml is useful in the gym, on a walk, at home, at the dacha - in general, everywhere! Forget about disposable plastic containers - they are not beautiful, and clog the environment. And such beauty in the hands will definitely attract the views of others. \nMeasily closes. It is made of safe food plastic (BPA free). \nYou can wash in the dishwasher."
                    },
                    new ProductParameter()
                    {
                        Id = "14",
                        WidgetType = ProductWidgetType.HorizontalCollection,
                        DataType = ProductDataType.ShopsAvailability_Indicator,
                        Name = IsRussianCulture ? "Индикация наличия в магазинах" : "Availability indication in stores"
                    },
                    new ProductParameter()
                    {
                        Id = "15",
                        WidgetType = ProductWidgetType.HorizontalCollection,
                        DataType = ProductDataType.ShopsAvailability_Count,
                        Name = IsRussianCulture ? "Остатки в магазинах" : "Remains in stores"
                    },
                    new ProductParameter
                    {
                        Id = "16",
                        WidgetType = ProductWidgetType.HorizontalCollection,
                        DataType = ProductDataType.Products,
                        Name = IsRussianCulture ? "Аксессуары" : "Accessories",
                    }
                }
            };
        }

        public async Task<List<Product>> LoadProductsCompilationForParameter(string groupId, string productId, string parameterId)
        {
            await Task.Delay(500);

            return _products;
        }

        public async Task<Product> LoadProductShortInfo(string groupId, string productId)
        {
            await Task.Delay(500);

            return _products.FirstOrDefault(x => x.GroupId == groupId && x.Id == productId) ?? _products.First();
        }

        public async Task<List<Product>> LoadProductsInCategory(string categoryId, int offset = 0, int count = 10, string searchText = null, List<ApplyedFilter> filters = null, SortType sort = null)
        {
            await Task.Delay(500);

            if (offset > 20)
                return new List<Product>();

            return string.IsNullOrEmpty(searchText) ?
                         _products.Select(x => { x.Name = x.Name + categoryId; return x; }).ToList()
                             :
                         _products.Where(x => searchText.Contains(" ") ? searchText.ToLowerInvariant().Split(' ').Any(y => x.Name.ToLowerInvariant().Contains(y)) : x.Name.ToLowerInvariant().Contains(searchText.ToLowerInvariant())).ToList();
        }

        public async Task<List<Shop>> LoadShopsCompilationForParameter(string groupId, string productId, string parameterId)
        {
            await Task.Delay(500);

            if (parameterId == "15")
                return new List<Shop>
            {
                new Shop { Id = "1", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true, Count = 1 },
                new Shop { Id = "2", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture  ?"Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true, Count = 2 },
                new Shop { Id = "3", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true, Count = 5 },
                new Shop { Id = "4", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = false, Count = 0 },
                new Shop { Id = "5", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true, Count = 25 },
                new Shop { Id = "6", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true, Count = 250 },
                new Shop { Id = "7", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = false, Count = 0 },
                new Shop { Id = "8", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = false, Count = 0 },
                new Shop { Id = "9", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true, Count = 1 },
            };
            else
                return new List<Shop>
            {
                new Shop { Id = "1", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true },
                new Shop { Id = "2", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true },
                new Shop { Id = "3", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true  },
                new Shop { Id = "4", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = false },
                new Shop { Id = "5", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true  },
                new Shop { Id = "6", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true },
                new Shop { Id = "7", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = false },
                new Shop { Id = "8", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = false },
                new Shop { Id = "9", Name = IsRussianCulture ? "Магазин «Может быть какой-нибудь такой»" : "Shop name", Address = IsRussianCulture ? "Санкт-Петербург, ул. Белградская, д. 45, лит. А" : "Shop address", IsProductAvailable = true },
            };
        }

        public async Task MarkProduct(string groupId, string productId, bool isMarked)
        {
            await Task.Delay(500);
        }

        public async Task<Product> ReloadProductByParameter(string groupId, string productId, IEnumerable<ApplyedProductParameter> applyedParameters)
        {
            await Task.Delay(500);

            var firstProduct = _products.First();

            firstProduct.GroupId = "111";
            firstProduct.Id = "222";
            firstProduct.Name = IsRussianCulture ? "Замечательный пиджак" : "Wonderful jacket";
            firstProduct.Price = 10000;
            firstProduct.OldPrice = 12000;
            firstProduct.State = new ProductState { Name = IsRussianCulture ? "Нет в наличии" : "Out of stock", Type = ProductStateType.NotAvailable };
            firstProduct.ImageUrls = new List<Image>
            {
                new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-tan-skin-asian-black-hair-muscle-good-looking-man-in-black-vest-military-green-pant-jean-grey-573163942.jpg" },
                new Image { LargeUrl = "https://image.shutterstock.com/z/stock-photo-tan-skin-asian-black-hair-muscle-good-looking-man-in-black-vest-military-green-pant-jean-grey-573163936.jpg" }
            };

            return firstProduct;
        }
    }
}
