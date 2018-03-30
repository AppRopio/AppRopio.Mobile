using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;

namespace AppRopio.Base.Filters.iOS.Models
{
    public class FiltersThemeConfig
    {
        [JsonProperty("filters")]
        public Filters Filters { get; private set; }

        [JsonProperty("sortTypes")]
        public SortTypes SortTypes { get; private set; }

        public FiltersThemeConfig()
        {
            Filters = new Filters();

            SortTypes = new SortTypes();
        }
    }

    public class SortTypes
    {
        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("sortCell")]
        public SortCell SortCell { get; private set; }

        [JsonProperty("cancelButton")]
        public Button CancelButton { get; private set; }

        public SortTypes()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            SortCell = new SortCell();
            CancelButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
        }
    }

    public class SortCell : View
    {
        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("size")]
        public Size Size { get; private set; }

        public SortCell()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16)
            };

            Size = new Size { Height = 60 };
        }
    }

    public class Filters
    {
        [JsonProperty("clearButton")]
        public Button ClearButton { get; private set; }

        [JsonProperty("applyButton")]
        public Button ApplyButton { get; private set; }

        [JsonProperty("filtersCell")]
        public FiltersCell FiltersCell { get; private set; }

        [JsonProperty("selection")]
        public FiltersSelection Selection { get; private set; }

        public Filters()
        {
            ApplyButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            ClearButton = (Button)Theme.ControlPalette.Button.Text.Clone();
            FiltersCell = new FiltersCell();
        }
    }

    public class FiltersCell : View
    {
        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("collection")]
        public FiltersCollection Collection { get; private set; }

        [JsonProperty("minMax")]
        public FiltersMinMax MinMax { get; private set; }

        [JsonProperty("picker")]
        public FiltersPicker Picker { get; private set; }

        [JsonProperty("oneSelection")]
        public FiltersOneSelection OneSelection { get; private set; }

        [JsonProperty("multiSelection")]
        public FiltersMultiSelection MultiSelection { get; private set; }

        [JsonProperty("switch")]
        public FiltersSwitch Switch { get; private set; }

        public FiltersCell()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            Collection = new FiltersCollection();
            MinMax = new FiltersMinMax();
            Picker = new FiltersPicker();
            OneSelection = new FiltersOneSelection();
            MultiSelection = new FiltersMultiSelection();
            Switch = new FiltersSwitch();
        }
    }

    public class FiltersCollection
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        public FiltersCollection()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextAccentDisabled.Clone(),
                HighlightedTextColor = (Color)Theme.ColorPalette.TextAccentHighlighted.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };
        }
    }

    public class FiltersMinMax
    {
        [JsonProperty("textField")]
        public TextField TextField { get; private set; }

        [JsonProperty("fromToLabel")]
        public Label Label { get; private set; }

        [JsonProperty("dateInputMode")]
        public DateInputMode DateInputMode { get; private set; }

        [JsonProperty("doneButton")]
        public Button DoneButton { get; private set; }

        public FiltersMinMax()
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

    public class FiltersPicker
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        [JsonProperty("pickerCell")]
        public FiltersPickerCell PickerCell { get; private set; }

        public FiltersPicker()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                HighlightedTextColor = (Color)Theme.ColorPalette.Accent.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14),
                TextAlignment = TextAlignment.Right
            };

            PickerCell = new FiltersPickerCell();
        }
    }

    public class FiltersPickerCell
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        [JsonProperty("size")]
        public Size Size { get; private set; }

        public FiltersPickerCell()
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

    public class FiltersOneSelection
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        public FiltersOneSelection()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
        }
    }

    public class FiltersMultiSelection
    {
        [JsonProperty("multiSelectionCell")]
        public FiltersMultiSelectionCell MultiSelectionCell { get; private set; }

        public FiltersMultiSelection()
        {
            MultiSelectionCell = new FiltersMultiSelectionCell();
        }
    }

    public class FiltersMultiSelectionCell
    {
        [JsonProperty("value")]
        public Label Value { get; private set; }

        public Image Image { get; private set; }

        public FiltersMultiSelectionCell()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextAccentDisabled.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };

            Image = new Image();
        }
    }

    public class FiltersSwitch
    {
        public Switch Switch { get; private set; }

        public FiltersSwitch()
        {
            Switch = (Switch)Theme.ControlPalette.Switch.Clone();
        }
    }

    public class FiltersSelection
    {
        [JsonProperty("searchBar")]
        public SearchBar SearchBar { get; private set; }

        [JsonProperty("selectionCell")]
        public FiltersSelectionCell SelectionCell { get; private set; }

        public FiltersSelection()
        {
            SearchBar = (SearchBar)Theme.ControlPalette.SearchBar.Clone();

            SelectionCell = new FiltersSelectionCell();
        }
    }

    public class FiltersSelectionCell : View
    {
        [JsonProperty("title")]
        public Label Value { get; private set; }

        [JsonProperty("size")]
        public Size Size { get; private set; }

        public FiltersSelectionCell()
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
