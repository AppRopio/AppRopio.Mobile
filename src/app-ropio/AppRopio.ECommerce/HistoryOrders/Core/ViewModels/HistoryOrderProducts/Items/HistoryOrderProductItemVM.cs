using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrderProducts.Items
{
    public class HistoryOrderProductItemVM : MvxViewModel, IHistoryOrderProductItemVM
    {
        public HistoryOrderProduct Product { get; set; }

		public string ProductId
        {
            get { return Product.ProductId; }
        }

		public string GroupId
		{
            get { return Product.GroupId; }
		}

		public string Title
		{
            get { return Product.Title; }
		}

		public int Amount
		{
            get { return Product.Amount; }
		}

		public decimal TotalPrice
		{
            get { return Product.TotalPrice; }
		}

		public bool IsAvailable
		{
            get { return Product.IsAvailable; }
		}

		public string Badge
		{
            get { return Product.Badge; }
		}

		public string ImageUrl
		{
            get { return Product.ImageUrl; }
		}

        public HistoryOrderProductItemVM(HistoryOrderProduct product)
        {
            Product = product;
        }
    }
}