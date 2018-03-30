using System;
using AppRopio.Base.Core.Services.DataBase;
using AppRopio.ECommerce.Products.Core.Models.DataBase;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Services.DataBase
{
    public interface IHistorySearchDbService : IDataBaseService<string, string>
    {
    }
}
