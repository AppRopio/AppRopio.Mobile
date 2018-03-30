using System;
using MvvmCross.Plugins.Messenger;
using AppRopio.Models.Filters.Responses;
using System.Collections.Generic;
namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Messages
{
    public class FiltersSelectionChangedMessage : MvxMessage
    {
        public List<ApplyedFilterValue> ApplyedFilterValues { get; private set; }

        public string FilterId { get; private set; }

        public FiltersSelectionChangedMessage(object sender, string filterId, List<ApplyedFilterValue> applyedFilterValues)
            : base(sender)
        {
            FilterId = filterId;
            ApplyedFilterValues = applyedFilterValues;
        }
    }
}
