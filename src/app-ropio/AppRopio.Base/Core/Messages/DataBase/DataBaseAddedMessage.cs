using System;
using System.Collections.Generic;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Core.Messsages.DataBase
{
    public class DataBaseAddedMessage<T> : MvxMessage
        where T : class
    {
        public List<T> Items { get; private set; }

        public DataBaseAddedMessage(object sender, List<T> items)
            : base(sender)
        {
            Items = items;
        }
    }
}

