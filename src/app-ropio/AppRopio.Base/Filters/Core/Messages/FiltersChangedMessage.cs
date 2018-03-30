using System;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.Core.Messages
{
    public class FiltersChangedMessage : MvxMessage
    {
        public string CategoryId { get; private set; }

        public List<ApplyedFilter> ApplyedFilters { get; private set; }

        public FiltersChangedMessage(object sender, string categoryId, List<ApplyedFilter> applyedFilters)
            : base(sender)
        {
            ApplyedFilters = applyedFilters;
            CategoryId = categoryId;
        }
    }
}
