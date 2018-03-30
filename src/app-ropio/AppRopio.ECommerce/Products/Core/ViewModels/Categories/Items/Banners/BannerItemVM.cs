using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items.Banners
{
    public class BannerItemVM : MvxViewModel, IBannerItemVM
    {
        internal Banner Model { get; private set; }

        public string Deeplink { get; private set; }

        private string _imageUrl;
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
                RaisePropertyChanged(() => ImageUrl);
            }
        }

        public BannerItemVM(Banner model)
        {
            ImageUrl = model.ImageUrl;
            Deeplink = model.Deeplink;
        }
    }
}
