using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.Feedback.Core.Models.Bundle
{
    public class ProductReviewsBundle : BaseBundle
    {
		public string ProductId { get; private set; }

        public string ProductGroupId { get; private set; }

		public ProductReviewsBundle()
		{

		}

        public ProductReviewsBundle(string productGroupId, string productId, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string>
            {
                [nameof(ProductId)] = productId,
                [nameof(ProductGroupId)] = productGroupId
            })
        {

		}
    }
}