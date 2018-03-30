using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;

namespace AppRopio.Payments.iOS.Models
{
    public class PaymentsThemeConfig
    {
        [JsonProperty("cardPayment")]
        public CardPayment CardPayment { get; private set; }

        public PaymentsThemeConfig ()
        {
            CardPayment = new CardPayment();
        }
    }

    public class CardPayment
    {
        [JsonProperty("cardNumberTextField")]
        public TextField CardNumberTextField { get; private set; }

        [JsonProperty("cardHolderTextField")]
        public TextField CardHolderTextField { get; private set; }

        [JsonProperty("expirationDateTextField")]
        public TextField ExpirationDateTextField { get; private set; }

        [JsonProperty("cvvTextField")]
        public TextField CvvTextField { get; private set; }

        [JsonProperty("payButton")]
        public Button PayButton { get; private set; }

        [JsonProperty("accessoryNextButton")]
        public Button AccessoryNextButton { get; private set; }

        public CardPayment()
        {
            CardNumberTextField = (TextField)Theme.ControlPalette.TextField.Clone();
            CardHolderTextField = (TextField)Theme.ControlPalette.TextField.Clone();
            ExpirationDateTextField = (TextField)Theme.ControlPalette.TextField.Clone();
            CvvTextField = (TextField)Theme.ControlPalette.TextField.Clone();
            PayButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            AccessoryNextButton = (Button)Theme.ControlPalette.Button.Accessory.Clone();
        }
    }
}
