using System;
using AppRopio.Base.Core.Services.DataBase;
using AppRopio.ECommerce.Products.Core.Models.DataBase;
namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Services.DataBase
{
    public class HistorySearchDbService : DataBaseService<HistorySearchDbo, string, string>, IHistorySearchDbService
    {
        protected override string ConvertFromDBO(HistorySearchDbo dbo)
        {
            return dbo.SearchText;
        }

        protected override HistorySearchDbo ConvertToDBO(HistorySearchDbo dbo, string model)
        {
            return new HistorySearchDbo
            {
                SearchText = model
            };
        }

        protected override string GetId(HistorySearchDbo dbo)
        {
            return dbo.SearchText;
        }

        protected override string GetId(string model)
        {
            return model;
        }
    }
}
