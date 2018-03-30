namespace AppRopio.Base.Settings.Core.ViewModels.Regions.Items
{
    public interface IRegionItemVm
    {
        string Id { get; }

        string Title { get; }

        bool Selected { get; set; }
    }
}