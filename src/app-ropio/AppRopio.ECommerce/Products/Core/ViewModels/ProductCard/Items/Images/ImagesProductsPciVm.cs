using System.Collections.Generic;
using AppRopio.Models.Base.Responses;
using MvvmCross.Core.ViewModels;
using System.Linq;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Images
{
    public class ImagesProductsPciVm : MvxViewModel, IImagesProductsPciVm
    {
        public List<string> ImagesUrls { get; }

        public ImagesProductsPciVm(List<Image> images)
        {
            ImagesUrls = images.Select(x => x.LargeUrl).ToList();
        }
    }
}
