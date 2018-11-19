using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.ECommerce.Products.Core.Models;
using Newtonsoft.Json;

namespace AppRopio.ECommerce.Products.iOS.Models
{
    public class ProductsThemeConfig
    {
        [JsonProperty("searchBar")]
        public SearchBar SearchBar { get; private set; }

        [JsonProperty("bannersCell")]
        public BannersCell BannersCell { get; private set; }

        [JsonProperty("categories")]
        public Categories Categories { get; private set; }

        [JsonProperty("navBarSearch")]
        public ProductsNavBarSearch NavBarSearch { get; private set; }

        [JsonProperty("products")]
        public Products Products { get; private set; }

        [JsonProperty("productDetails")]
        public ProductDetails ProductDetails { get; private set; }

        public ContentSearch ContentSearch { get; private set; }

        public ProductsThemeConfig()
        {
            SearchBar = (SearchBar)Theme.ControlPalette.SearchBar.Clone();
            Categories = new Categories();
            Products = new Products();
            NavBarSearch = new ProductsNavBarSearch();
            ContentSearch = new ContentSearch();
            ProductDetails = new ProductDetails();
            BannersCell = new BannersCell();
        }
    }

    public class BannersCell : View
    {
        [JsonProperty("image")]
        public Image Image { get; private set; }

        public BannersCell()
        {
            Image = new Image();
        }
    }

    public class ProductsNavBarSearch
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; private set; }

        [JsonProperty("image")]
        public Image Image { get; private set; }

        public ProductsNavBarSearch()
        {
            Enabled = false;
            Image = new Image();
        }
    }

    public class Categories
    {
        [JsonProperty("categoryCell")]
        public CategoryCell CategoryCell { get; private set; }

        [JsonProperty("collectionType")]
        public CollectionType CollectionType { get; private set; }

        [JsonProperty("tabCell")]
        public TabCell TabCell { get; private set; }

        [JsonProperty("tabLayer")]
        public Layer TabLayer { get; private set; }

        [JsonProperty("collectionColumns")]
        public int CollectionColumns { get; private set; }

        public Categories()
        {
            CategoryCell = new CategoryCell();
            TabCell = new TabCell();
            TabLayer = new Layer { Shadow = new Shadow { X = 0, Y = -2, Blur = 4, Color = new Color { Hex = "C0C7CF" }, Opacity = (float)0.25 } };
            CollectionType = CollectionType.List;
            CollectionColumns = 2;
        }
    }

    public class TabCell
    {
        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("size")]
        public Size Size { get; set; }

        public TabCell()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                HighlightedTextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            Size = new Size { Width = 90, Height = 44 };
        }
    }

    public class CategoryCell : View
    {
        [JsonProperty("backgroundImage")]
        public Image BackgroundImage { get; private set; }

        [JsonProperty("icon")]
        public Image Icon { get; private set; }
        
        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("separatorVisibility")]
        public bool separatorVisibility { get; private set; }

        [JsonProperty("gradientVisibility")]
        public bool gradientVisibility { get; private set; }

        public CategoryCell()
        {
            BackgroundImage = new Image();
            Icon = new Image();
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                HighlightedTextColor = (Color)Theme.ColorPalette.Accent.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(16)
            };
            Size = new Size { Width = 90, Height = 52 };
            Layer = new Layer();
            separatorVisibility = false;
            gradientVisibility = false;
        }
    }

    public class Products
    {
        [JsonProperty("sortButton")]
        public Button SortButton { get; private set; }

        [JsonProperty("filtersButton")]
        public Button FiltersButton { get; private set; }

        [JsonProperty("productCell")]
        public ProductCell ProductCell { get; private set; }

        [JsonProperty("emptyView")]
        public ProductsEmptyView EmptyView { get; private set; }

        [JsonProperty("collectionType")]
        public CollectionType CollectionType { get; set; }

        public Products()
        {
            SortButton = new Button()
            {
                Layer = new Layer { CornerRadius = 4, MaskToBounds = true },
                Font = Theme.FontsPalette.BoldOfSize(12),
                States = new States
                {
                    Normal = new State
                    {
                        Content = (Color)Theme.ColorPalette.TextBase.Clone(),
                        Background = (Color)Theme.ColorPalette.Separator.Clone()
                    },
                    Highlighted = new State
                    {
                        Content = (Color)Theme.ColorPalette.TextAccent.Clone(),
                        Background = (Color)Theme.ColorPalette.Accent.Clone()
                    }
                }
            };

            FiltersButton = (Button)SortButton.Clone();

            ProductCell = new ProductCell();

            EmptyView = new ProductsEmptyView();
        }
    }

    public class ProductsEmptyView : View
    {
        [JsonProperty("image")]
        public Image Image { get; private set; }

        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("text")]
        public Label Text { get; private set; }

        [JsonProperty("catalogButton")]
        public Button CatalogButton { get; private set; }

        public ProductsEmptyView()
        {
            Background = (Color)Theme.ColorPalette.Background.Clone();
            Image = new Image();
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16),
                TextAlignment = TextAlignment.Center
            };
            Text = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(16),
                TextAlignment = TextAlignment.Center
            };
            CatalogButton = (Button)Theme.ControlPalette.Button.Base.Clone();
        }
    }

    public class ProductCell : View
    {
        [JsonProperty("image")]
        public Image Image { get; private set; }

        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("price")]
        public Label Price { get; private set; }

        [JsonProperty("oldPrice")]
        public Label OldPrice { get; private set; }

        [JsonProperty("badge")]
        public Label Badge { get; private set; }

        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("markButton")]
        public Button MarkButton { get; private set; }

        [JsonProperty("addToCartHeight")]
        public float AddToCartHeight { get; set; }

        public ProductCell()
        {
            Image = new Image();
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            Price = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };
            OldPrice = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };

            Badge = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };

            MarkButton = (Button)Theme.ControlPalette.Button.Icon.Clone();

            Layer = null;

            Size = new Size();

            AddToCartHeight = 32.0f;
        }
    }

    public class ContentSearch
    {
        [JsonProperty("searchBar")]
        public SearchBar SearchBar { get; private set; }

        [JsonProperty("clearHistoryButton")]
        public Button ClearHistoryButton { get; private set; }

        [JsonProperty("searchCell")]
        public ContentSearchCell SearchCell { get; private set; }

        public ContentAutocompleteCell AutocompeleteCell { get; private set; }

        public ContentSearch()
        {
            SearchBar = (SearchBar)Theme.ControlPalette.SearchBar.Clone();
            ClearHistoryButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            SearchCell = new ContentSearchCell();
            AutocompeleteCell = new ContentAutocompleteCell();
        }
    }

    public class ContentSearchCell : View
    {
        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("historyIcon")]
        public Image HistoryIcon { get; private set; }

        [JsonProperty("linkIcon")]
        public Image LinkIcon { get; private set; }

        public ContentSearchCell()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
        }
    }

    public class ContentAutocompleteCell : View
    {
        [JsonProperty("title")]
        public Label Title { get; private set; }

        public ContentAutocompleteCell()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            Background = (Color)Theme.ColorPalette.DisabledControl.Clone();
            Layer = new Layer
            {
                CornerRadius = 15
            };
        }
    }
}
