using System;
using MvvmCross.Plugin.Messenger;
using System.Collections.Generic;

namespace AppRopio.Base.Core.Messsages.DataBase
{
    public class DataBaseUpdatedMessage<T> : MvxMessage
        where T : class
    {
        public List<T> Items { get; private set; }

        public DataBaseUpdatedMessage(object sender, List<T> items)
            : base(sender)
        {
            Items = items;
        }
    }
}

