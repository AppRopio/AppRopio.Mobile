using AppRopio.Base.Core.Plugins;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Payments.ApplePay
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "ApplePay";

        public override void Load()
        {
            base.Load();
		}
	}
}
