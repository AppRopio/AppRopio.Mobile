using System;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Core.Messages.Module
{
    public class ModulesInteractionMessage<T> : MvxMessage
    {
        public T Value { get; set; }

        public Type Type { get; set; }

        public ModulesInteractionMessage(object sender)
            : base(sender)
        {
        }

        public ModulesInteractionMessage(object sender, T value)
            : base(sender)
        {
            Value = value;
        }
    }
}
