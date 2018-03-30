using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Marked.Responses;

namespace AppRopio.ECommerce.Marked.API.Services.Implementation
{
    public class MarkedService : BaseService, IMarkedService
    {
        protected string MARKED_URL = "marked";
        protected string MARKED_PRODUCTS_QUANTITY = "marked/quantity";

		public async Task<List<MarkedProduct>> GetMarkedProducts(int count, int offset = 0)
		{
            return await Get<List<MarkedProduct>>($"{MARKED_URL}?offset={offset}&count={count}");
		}

        public async Task<int> GetQuantity()
        {
            return await Get(MARKED_PRODUCTS_QUANTITY, (string arg) => Convert.ToInt32(arg, NumberFormatInfo.InvariantInfo));
        }
    }
}