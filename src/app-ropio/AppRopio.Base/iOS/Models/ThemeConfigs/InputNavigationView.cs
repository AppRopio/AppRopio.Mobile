using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class InputNavigationView : View
    {
        [JsonProperty("previous")]
        public Button Previous { get; set; }

        [JsonProperty("next")]
        public Button Next { get; set; }

        [JsonProperty("continue")]
        public Button Continue { get; set; }
        
        public override object Clone()
        {
            return new InputNavigationView
            {
                Background = (Color)this.Background?.Clone(),
                Layer = (Layer)this.Layer?.Clone(),
                Previous = (Button)this.Previous?.Clone(),
                Next = (Button)this.Next?.Clone(),
                Continue = (Button)this.Continue?.Clone()
            };
        }
    }
}