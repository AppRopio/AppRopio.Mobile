using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Models.Map.Responses;

namespace AppRopio.Base.Map.Core.Models.Bundle
{
    public class PointBundle : BaseBundle
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

        public PointBundle()
        {
        }

        public PointBundle(Point model, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string> {
            { nameof(Id), model.Id },
            { nameof(Name), model.Name },
            { nameof(Address), model.Address },
            { nameof(WorkTime), model.WorkTime },
            { nameof(Distance), model.Distance },
            { nameof(Phone), model.Phone },
            { nameof(Info), model.AdditionalInfo },
            { nameof(Longitude), model.Coordinates?.Longitude.ToString() ?? "-1" },
            { nameof(Latitude), model.Coordinates?.Latitude.ToString() ?? "-1" }
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
