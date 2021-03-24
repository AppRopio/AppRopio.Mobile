using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Filters.Responses;
using MvvmCross;

namespace AppRopio.Base.Filters.API.Services.Fakes
{
    public class FiltersFakeService : IFiltersService
    {
        public bool IsRussianCulture => Mvx.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        public async Task<List<Filter>> LoadFilters(string categoryId)
        {
            await Task.Delay(500);

            return new List<Filter>
            {
                new Filter
                {
                    Id = "1",
                    WidgetType = FilterWidgetType.HorizontalCollection,
                    DataType = FilterDataType.Color,
                    Name = IsRussianCulture ? "Цвет" : "Color",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "#F86C50", ValueName = "Оранжевый" },
                        new FilterValue { Id = "2", Value = "#95BA6D", ValueName = "Зеленый" },
                        new FilterValue { Id = "3", Value = "#7DCDBB", ValueName = "Бирюзовый" },
                        new FilterValue { Id = "4", Value = "#86AADB", ValueName = "Ультрамарин" },
                        new FilterValue { Id = "5", Value = "#B5B5BF", ValueName = "Серый" },
                        new FilterValue { Id = "6", Value = "#F6A623", ValueName = "Желтый" },
                        new FilterValue { Id = "7", Value = "#5F94E1", ValueName = "Синий" },
                        new FilterValue { Id = "8", Value = "#20C987", ValueName = "Изумрудный" },
                        new FilterValue { Id = "9", Value = "#FC224B", ValueName = "Красный" }
                    }
                },
                new Filter
                {
                    Id = "11",
                    WidgetType = FilterWidgetType.HorizontalCollection,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Бренд" : "Brand",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "oodji", ValueName = "oodji" },
                        new FilterValue { Id = "2", Value = "Zara", ValueName = "Zara" },
                        new FilterValue { Id = "3", Value = "Adidas", ValueName = "Adidas" },
                        new FilterValue { Id = "4", Value = "Mango", ValueName = "Mango" },
                        new FilterValue { Id = "5", Value = "Nike", ValueName = "Nike" },
                        new FilterValue { Id = "6", Value = "GAP", ValueName = "GAP" },
                        new FilterValue { Id = "7", Value = "new balance", ValueName = "new balance" },
                        new FilterValue { Id = "8", Value = "Under Armour", ValueName = "Under Armour" }
                    }
                },
                new Filter
                {
                    Id = "2",
                    WidgetType = FilterWidgetType.MinMax,
                    DataType = FilterDataType.Number,
                    Name = IsRussianCulture ? "Цена" : "Price",
                    MinValue = "399.0",
                    MaxValue = "10000.0"
                },
                new Filter
                {
                    Id = "4",
                    WidgetType = FilterWidgetType.MinMax,
                    DataType = FilterDataType.Date,
                    Name = IsRussianCulture ? "Дата доставки" : "Delivery date",
                    MinValue = "2017-03-27",
                    MaxValue = "2017-04-02"
                },
                new Filter
                {
                    Id = "3",
                    WidgetType = FilterWidgetType.VerticalCollection,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Размер" : "Size",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "42", ValueName = "XXS" },
                        new FilterValue { Id = "2", Value = "44", ValueName = "XS" },
                        new FilterValue { Id = "3", Value = "46", ValueName = "S" },
                        new FilterValue { Id = "4", Value = "48", ValueName = "M" },
                        new FilterValue { Id = "5", Value = "50", ValueName = "L" },
                        new FilterValue { Id = "6", Value = "52", ValueName = "XL" },
                        new FilterValue { Id = "7", Value = "54", ValueName = "XXL" }
                    }
                },
                new Filter
                {
                    Id = "5",
                    WidgetType = FilterWidgetType.Picker,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Сортировка по" : "Sort by",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "1", ValueName = IsRussianCulture ? "Определенный" : "Sort type 1" },
                        new FilterValue { Id = "2", Value = "2", ValueName = IsRussianCulture ? "Какой-то цвет" : "Sort type 2" },
                        new FilterValue { Id = "3", Value = "3", ValueName = IsRussianCulture ? "Один цвет" : "Sort type 3" },
                        new FilterValue { Id = "4", Value = "4", ValueName = IsRussianCulture ? "Выбранный цвет" : "Sort type 4"},
                        new FilterValue { Id = "5", Value = "5", ValueName = IsRussianCulture ? "Цвет другой" : "Sort type 5"},
                        new FilterValue { Id = "6", Value = "6", ValueName = IsRussianCulture ? "Третий цвет" : "Sort type 6"},
                        new FilterValue { Id = "7", Value = "7", ValueName = IsRussianCulture ? "Еще один цвет" : "Sort type 7"}
                    }
                },

                new Filter
                {
                    Id = "66",
                    WidgetType = FilterWidgetType.MultiSelection,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Бренд" : "Brand",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "Adidas", ValueName = "Adidas" },
                        new FilterValue { Id = "2", Value = "GAP", ValueName = "GAP" },
                        new FilterValue { Id = "3", Value = "Mango", ValueName = "Mango" },
                        new FilterValue { Id = "4", Value = "new balance", ValueName = "new balance" },
                        new FilterValue { Id = "5", Value = "Nike", ValueName = "Nike" },
                        new FilterValue { Id = "6", Value = "oodji", ValueName = "oodji" },
                        new FilterValue { Id = "7", Value = "Under Armour", ValueName = "Under Armour" },
                        new FilterValue { Id = "8", Value = "Zara", ValueName = "Zara" },
                    }
                },
                new Filter
                {
                    Id = "6",
                    WidgetType = FilterWidgetType.OneSelection,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Наличие в магазинах" : "Availability in stores",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "1", ValueName = IsRussianCulture ? "Да" : "Yes" },
                        new FilterValue { Id = "2", Value = "0", ValueName = IsRussianCulture ? "Нет" : "No" },
                        new FilterValue { Id = "3", Value = "2", ValueName = IsRussianCulture ? "Не важно" : "Does not matter" }
                    }
                },
                new Filter
                {
                    Id = "7",
                    WidgetType = FilterWidgetType.Switch,
                    DataType = FilterDataType.Boolean,
                    Name = IsRussianCulture ? "Быстрая доставка" : "Fast delivery"
                },
                new Filter
                {
                    Id = "8",
                    WidgetType = FilterWidgetType.HorizontalCollection,
                    DataType = FilterDataType.Color,
                    Name = IsRussianCulture ? "Цвет" : "Color",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "#F86C50", ValueName = "Оранжевый" },
                        new FilterValue { Id = "2", Value = "#95BA6D", ValueName = "Зеленый" },
                        new FilterValue { Id = "3", Value = "#7DCDBB", ValueName = "Бирюзовый" },
                        new FilterValue { Id = "4", Value = "#86AADB", ValueName = "Ультрамарин" },
                        new FilterValue { Id = "5", Value = "#B5B5BF", ValueName = "Серый" },
                        new FilterValue { Id = "6", Value = "#F6A623", ValueName = "Желтый" },
                        new FilterValue { Id = "7", Value = "#5F94E1", ValueName = "Синий" },
                        new FilterValue { Id = "8", Value = "#20C987", ValueName = "Изумрудный" },
                        new FilterValue { Id = "9", Value = "#FC224B", ValueName = "Красный" }
                    }
                },
                new Filter
                {
                    Id = "9",
                    WidgetType = FilterWidgetType.HorizontalCollection,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Бренд" : "Brand",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "oodji", ValueName = "oodji" },
                        new FilterValue { Id = "2", Value = "Zara", ValueName = "Zara" },
                        new FilterValue { Id = "3", Value = "Adidas", ValueName = "Adidas" },
                        new FilterValue { Id = "4", Value = "Mango", ValueName = "Mango" },
                        new FilterValue { Id = "5", Value = "Nike", ValueName = "Nike" },
                        new FilterValue { Id = "6", Value = "GAP", ValueName = "GAP" },
                        new FilterValue { Id = "7", Value = "new balance", ValueName = "new balance" },
                        new FilterValue { Id = "8", Value = "Under Armour", ValueName = "Under Armour" }
                    }
                },
                new Filter
                {
                    Id = "10",
                    WidgetType = FilterWidgetType.MinMax,
                    DataType = FilterDataType.Number,
                    Name = IsRussianCulture ? "Цена" : "Price",
                    MinValue = "399.0",
                    MaxValue = "10000.0"
                },
                new Filter
                {
                    Id = "12",
                    WidgetType = FilterWidgetType.MinMax,
                    DataType = FilterDataType.Date,
                    Name = IsRussianCulture ? "Дата доставки" : "Delivery date",
                    MinValue = "2017-03-27",
                    MaxValue = "2017-04-02"
                },
                new Filter
                {
                    Id = "13",
                    WidgetType = FilterWidgetType.VerticalCollection,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Размер" : "Size",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "42", ValueName = "XXS" },
                        new FilterValue { Id = "2", Value = "44", ValueName = "XS" },
                        new FilterValue { Id = "3", Value = "46", ValueName = "S" },
                        new FilterValue { Id = "4", Value = "48", ValueName = "M" },
                        new FilterValue { Id = "5", Value = "50", ValueName = "L" },
                        new FilterValue { Id = "6", Value = "52", ValueName = "XL" },
                        new FilterValue { Id = "7", Value = "54", ValueName = "XXL" }
                    }
                },
                new Filter
                {
                    Id = "14",
                    WidgetType = FilterWidgetType.Picker,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Сортировка по" : "Sort by",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "1", ValueName = IsRussianCulture ? "Определенный" : "Sort type 1" },
                        new FilterValue { Id = "2", Value = "2", ValueName = IsRussianCulture ? "Какой-то цвет" : "Sort type 2" },
                        new FilterValue { Id = "3", Value = "3", ValueName = IsRussianCulture ? "Один цвет" : "Sort type 3" },
                        new FilterValue { Id = "4", Value = "4", ValueName = IsRussianCulture ? "Выбранный цвет" : "Sort type 4"},
                        new FilterValue { Id = "5", Value = "5", ValueName = IsRussianCulture ? "Цвет другой" : "Sort type 5"},
                        new FilterValue { Id = "6", Value = "6", ValueName = IsRussianCulture ? "Третий цвет" : "Sort type 6"},
                        new FilterValue { Id = "7", Value = "7", ValueName = IsRussianCulture ? "Еще один цвет" : "Sort type 7"}
                    }
                },

                new Filter
                {
                    Id = "15",
                    WidgetType = FilterWidgetType.MultiSelection,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Бренд" : "Brand",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "Adidas", ValueName = "Adidas" },
                        new FilterValue { Id = "2", Value = "GAP", ValueName = "GAP" },
                        new FilterValue { Id = "3", Value = "Mango", ValueName = "Mango" },
                        new FilterValue { Id = "4", Value = "new balance", ValueName = "new balance" },
                        new FilterValue { Id = "5", Value = "Nike", ValueName = "Nike" },
                        new FilterValue { Id = "6", Value = "oodji", ValueName = "oodji" },
                        new FilterValue { Id = "7", Value = "Under Armour", ValueName = "Under Armour" },
                        new FilterValue { Id = "8", Value = "Zara", ValueName = "Zara" },
                    }
                },
                new Filter
                {
                    Id = "16",
                    WidgetType = FilterWidgetType.OneSelection,
                    DataType = FilterDataType.Text,
                    Name = IsRussianCulture ? "Наличие в магазинах" : "Availability in stores",
                    Values = new List<FilterValue>
                    {
                        new FilterValue { Id = "1", Value = "1", ValueName = IsRussianCulture ? "Да" : "Yes" },
                        new FilterValue { Id = "2", Value = "0", ValueName = IsRussianCulture ? "Нет" : "No" },
                        new FilterValue { Id = "3", Value = "2", ValueName = IsRussianCulture ? "Не важно" : "Does not matter" }
                    }
                },
                new Filter
                {
                    Id = "17",
                    WidgetType = FilterWidgetType.Switch,
                    DataType = FilterDataType.Boolean,
                    Name = IsRussianCulture ? "Быстрая доставка" : "Fast delivery"
                }
            };
        }

        public async Task<List<SortType>> LoadSortTypes(string categoryId)
        {
            await Task.Delay(500);

            return new List<SortType>
            {
                new SortType { Id = "1", Name = IsRussianCulture ? "По новизне" : "Sort type 1" },
                new SortType { Id = "2", Name = IsRussianCulture ? "Цена по убыванию" : "Sort type 2" },
                new SortType { Id = "3", Name = IsRussianCulture ? "Цена по возрастанию" : "Sort type 3" },
                new SortType { Id = "4", Name = IsRussianCulture ? "По популярности" : "Sort type 4" },
                new SortType { Id = "11", Name = IsRussianCulture ? "По новизне" : "Sort type 5" },
                new SortType { Id = "22", Name = IsRussianCulture ? "Цена по убыванию" : "Sort type 6" },
                new SortType { Id = "33", Name = IsRussianCulture ? "Цена по возрастанию" : "Sort type 7" },
                new SortType { Id = "44", Name = IsRussianCulture ? "По популярности" : "Sort type 8" }
            };
        }
    }
}
