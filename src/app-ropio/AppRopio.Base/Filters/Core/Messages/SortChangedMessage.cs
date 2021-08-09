using System;
using MvvmCross.Plugin.Messenger;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.Core.Messages
{
    public class SortChangedMessage : MvxMessage
    {
        public SortType Sort { get; private set; }

        public string CategoryId { get; private set; }

        public SortChangedMessage(object sender, string categoryId, SortType sort)
            : base(sender)
        {
            CategoryId = categoryId;

            Sort = sort;
        }
    }
}
