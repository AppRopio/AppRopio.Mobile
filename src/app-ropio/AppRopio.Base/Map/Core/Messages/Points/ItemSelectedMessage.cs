using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Map.Core.Messages.Points
{
    public class ItemSelectedMessage : MvxMessage
    {
        public string Id { get; set; }

        public ItemSelectedMessage(object sender)
            : base (sender)
        {
        }
    }
}
