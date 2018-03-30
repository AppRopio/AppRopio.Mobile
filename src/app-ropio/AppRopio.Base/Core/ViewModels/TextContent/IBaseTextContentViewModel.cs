using System;
namespace AppRopio.Base.Core.ViewModels.TextContent
{
    public interface IBaseTextContentViewModel : IBaseViewModel
    {
        string Title { get; }

        string Text { get; }
    }
}
