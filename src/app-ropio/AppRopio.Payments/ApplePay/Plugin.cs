using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Payments.ApplePay
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();
		}
	}
}
