using System;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using AppRopio.Models.Basket.Responses.Order;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using System.Globalization;

namespace AppRopio.ECommerce.Basket.Core.Models.Bundle
{
    public class DeliveryPointBundle : BaseBundle
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string WorkTime { get; set; }

        public string Distance { get; set; }

        public string Phone { get; set; }

        public string Info { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public DeliveryPointBundle()
        {
        }

        public DeliveryPointBundle(DeliveryPoint model, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string> {
            { nameof(Id), model.Id },
            { nameof(Name), model.Name },
            { nameof(Address), model.Address },
            { nameof(WorkTime), model.WorkTime },
            { nameof(Distance), model.Distance },
            { nameof(Phone), model.Phone },
            { nameof(Info), model.AdditionalInfo },
            { nameof(Longitude), model.Coordinates?.Longitude.ToString(NumberFormatInfo.InvariantInfo) ?? "-1" },
            { nameof(Latitude), model.Coordinates?.Latitude.ToString(NumberFormatInfo.InvariantInfo) ?? "-1" }
        })
        {
            Id = model.Id;
            Name = model.Name;
            Address = model.Address;
            WorkTime = model.WorkTime;
            Distance = model.Distance;
            Phone = model.Phone;
            Info = model.AdditionalInfo;
            Longitude = model.Coordinates?.Longitude ?? -1;
            Latitude = model.Coordinates?.Latitude ?? -1;
        }
    }
}
