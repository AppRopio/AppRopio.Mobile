using System;
using Realms;

namespace AppRopio.ECommerce.Products.Core.Models.DataBase
{
    public class HistorySearchDbo : RealmObject
    {
        [PrimaryKey]
        public string SearchText { get; set; }
    }
}
