using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;

namespace AppRopio.ECommerce.Products.iOS.Models
{
    public class ProductDetails
    {
        [JsonProperty("shareButton")]
        public Button ShareButton { get; private set; }

        [JsonProperty("markButton")]
        public Button MarkButton { get; private set; }

        [JsonProperty("closeButton")]
        public Button CloseButton { get; private set; }

        [JsonProperty("image")]
        public Image Image { get; private set; }

        [JsonProperty("status")]
        public Label Status { get; private set; }

        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("price")]
        public Label Price { get; private set; }

        [JsonProperty("oldPrice")]
        public Label OldPrice { get; private set; }

        [JsonProperty("extraPrice")]
        public Label ExtraPrice { get; private set; }

        [JsonProperty("textContent")]
        public TextView TextContent { get; private set; }

        [JsonProperty("detailsCell")]
        public ProductDetailsCell DetailsCell { get; private set; }

        public ProductDetailsSelection Selection { get; private set; }

        public ProductDetails()
        {
            Image = new Image();
            
            ShareButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
            MarkButton = (Button)Theme.ControlPalette.Button.Icon.Clone();

            CloseButton = (Button)Theme.ControlPalette.Button.Icon.Clone();

            Status = new Label
            {
				TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
				Font = Theme.FontsPalette.BoldOfSize(14)
            };
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
            ExtraPrice = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };

            TextContent = new TextView
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(14)
            };

            DetailsCell = new ProductDetailsCell();

            Selection = new ProductDetailsSelection();
        }
    }

    public class ProductDetailsBaseCell : View
    {
        [JsonProperty("title")]
        public Label Title { get; set; }
    }

    public class ProductDetailsCell : View
    {
        [JsonProperty("title")]
        public Label Title { get; set; }

        [JsonProperty("collection")]
        public ProductDetailsCollection Collection { get; private set; }

        [JsonProperty("minMax")]
        public ProductDetailsMinMax MinMax { get; private set; }

        [JsonProperty("picker")]
        public ProductDetailsPicker Picker { get; private set; }

        [JsonProperty("oneSelection")]
        public ProductDetailsOneSelection OneSelection { get; private set; }

        [JsonProperty("multiSelection")]
        public ProductDetailsMultiSelection MultiSelection { get; private set; }

        [JsonProperty("switch")]
        public ProductDetailsSwitch Switch { get; private set; }

        [JsonProperty("multiLineText")]
        public ProductDetailsMultiLineText MultiLineText { get; private set; }

        [JsonProperty("productsCompilation")]
        public ProductDetailsCompilation ProductsCompilation { get; private set; }

        [JsonProperty("shopsCompilation")]
        public ProductDetailsShopsCompilation ShopsCompilation { get; private set; }

        [JsonProperty("transition")]
        public ProductDetailsTransition Transition { get; private set; }

        public ProductDetailsCell()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };

            Collection = new ProductDetailsCollection();
            MinMax = new ProductDetailsMinMax();
            Picker = new ProductDetailsPicker();
            OneSelection = new ProductDetailsOneSelection();
            MultiSelection = new ProductDetailsMultiSelection();
            Switch = new ProductDetailsSwitch();
            MultiLineText = new ProductDetailsMultiLineText();
            ProductsCompilation = new ProductDetailsCompilation();
            ShopsCompilation = new ProductDetailsShopsCompilation();
            Transition = new ProductDetailsTransition();
        }
    }

    public class ProductDetailsTransition : ProductDetailsBaseCell
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        public ProductDetailsTransition()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14),
                TextAlignment = TextAlignment.Right
            };
        }
    }

    public class ProductDetailsCollection : ProductDetailsBaseCell
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        public ProductDetailsCollection()
        {
            Value = new Label
            {
				TextColor = (Color)Theme.ColorPalette.TextAccentDisabled.Clone(),
				HighlightedTextColor = (Color)Theme.ColorPalette.TextAccentHighlighted.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };
        }
    }

    public class ProductDetailsMinMax : ProductDetailsBaseCell
    {
        [JsonProperty("textField")]
        public TextField TextField { get; private set; }

        [JsonProperty("fromToLabel")]
        public Label Label { get; private set; }

        [JsonProperty("dateInputMode")]
        public DateInputMode DateInputMode { get; private set; }

        [JsonProperty("doneButton")]
        public Button DoneButton { get; private set; }

        public ProductDetailsMinMax()
        {
            TextField = (TextField)Theme.ControlPalette.TextField.Clone();
            TextField.Font = Theme.FontsPalette.BoldOfSize(14);

            Label = new Label { TextColor = (Color)Theme.ColorPalette.TextGray.Clone(), Font = Theme.FontsPalette.BoldOfSize(14) };

            DoneButton = (Button)Theme.ControlPalette.Button.Text.Clone();

            if (DoneButton.States != null && DoneButton.States.Normal != null)
                DoneButton.States.Normal.Content = (Color)Theme.ColorPalette.TextBase.Clone();
            else
                DoneButton.TextColor = (Color)Theme.ColorPalette.TextBase.Clone();
        }
    }

    public enum DateInputMode
    {
        Picker = 0,
        Manual = 1
    }

    public class ProductDetailsPicker : ProductDetailsBaseCell
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        [JsonProperty("pickerCell")]
        public ProductDetailsPickerCell PickerCell { get; private set; }

        public ProductDetailsPicker()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                HighlightedTextColor = (Color)Theme.ColorPalette.Accent.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14),
                TextAlignment = TextAlignment.Right
            };

            PickerCell = new ProductDetailsPickerCell();
        }
    }

    public class ProductDetailsPickerCell
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        [JsonProperty("size")]
        public Size Size { get; private set; }

        public ProductDetailsPickerCell()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16),
                TextAlignment = TextAlignment.Center
            };
            Size = new Size { Height = 30 };
        }
    }

    public class ProductDetailsOneSelection : ProductDetailsBaseCell
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        public ProductDetailsOneSelection()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
        }
    }

    public class ProductDetailsMultiSelection : ProductDetailsBaseCell
    {
        [JsonProperty("multiSelectionCell")]
        public ProductDetailsMultiSelectionCell MultiSelectionCell { get; private set; }

        public ProductDetailsMultiSelection()
        {
            MultiSelectionCell = new ProductDetailsMultiSelectionCell();
        }
    }

    public class ProductDetailsMultiSelectionCell
    {
        [JsonProperty("image")]
        public Image Image { get; private set; }
        
        [JsonProperty("value")]
        public Label Value { get; private set; }

        public ProductDetailsMultiSelectionCell()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextAccentDisabled.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };
        }
    }

    public class ProductDetailsSwitch : ProductDetailsBaseCell
    {
        public Switch Switch { get; private set; }

        public ProductDetailsSwitch()
        {
            Switch = (Switch)Theme.ControlPalette.Switch.Clone();
        }
    }

    public class ProductDetailsMultiLineText : ProductDetailsBaseCell
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        public ProductDetailsMultiLineText()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(14)
            };
        }
    }

    public class ProductDetailsCompilation : ProductDetailsBaseCell
    {
        public ProductDetailsCompilation()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
        }
    }

    public class ProductDetailsShopsCompilation : ProductDetailsBaseCell
    {
        public ProductDetailsShopsCompilationCell Cell { get; private set; }

        public ProductDetailsShopsCompilation()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            Cell = new ProductDetailsShopsCompilationCell();
        }
    }

    public class ProductDetailsShopsCompilationCell
    {
        public Label Name { get; private set; }

        public Label Address { get; private set; }

        public Label Count { get; private set; } 

        public Layer Layer { get; private set; }

        public ProductDetailsShopsCompilationCell()
        {
            Name = new Label { TextColor = (Color)Theme.ColorPalette.TextBase.Clone(), Font = Theme.FontsPalette.SemiboldOfSize(14) };
            Address = new Label { TextColor = (Color)Theme.ColorPalette.TextBase.Clone(), Font = Theme.FontsPalette.RegularOfSize(13) };
            Count = new Label { TextColor = (Color)Theme.ColorPalette.TextBase.Clone(), Font = Theme.FontsPalette.BoldOfSize(14) };

            Layer = new Layer { CornerRadius = 0, MaskToBounds = true, Border = new Border { Width = 1, Color = (Color)Theme.ColorPalette.Separator.Clone() } };
        }
    }

    public class ProductDetailsSelection
    {
        [JsonProperty("searchBar")]
        public SearchBar SearchBar { get; private set; }

        [JsonProperty("selectionCell")]
        public ProductDetailsSelectionCell SelectionCell { get; private set; }

        [JsonProperty("clearButton")]
        public Button ClearButton { get; private set; }

        [JsonProperty("applyButton")]
        public Button ApplyButton { get; private set; }

        public ProductDetailsSelection()
        {
            SearchBar = (SearchBar)Theme.ControlPalette.SearchBar.Clone();

            ClearButton = (Button)Theme.ControlPalette.Button.Text.Clone();
            ApplyButton = (Button)Theme.ControlPalette.Button.Base.Clone();

            SelectionCell = new ProductDetailsSelectionCell();
        }
    }

    public class ProductDetailsSelectionCell : View
    {
        [JsonProperty("title")]
        public Label Value { get; private set; }

        [JsonProperty("size")]
        public Size Size { get; private set; }

        public ProductDetailsSelectionCell()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            Size = new Size { Height = 50 };
        }
    }
}
