using System;
using System.Threading.Tasks;
using AppRopio.Models.Settings.Responses;

namespace AppRopio.Base.Settings.Core.Services
{
    public interface IRegionService
    {
        Task CheckRegion();

        void ChangeSelectedRegion(string regionId, string regionTitle);
    }
}
