using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Images
{
    public interface IImagesProductsPciVm : IProductBasicItemVM
    {
        List<string> ImagesUrls { get; }
    }
}
