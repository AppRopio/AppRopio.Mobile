using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.Feedback.Core.Models.Bundle
{
	public class ReviewBundle : BaseBundle
	{
		public string ReviewId { get; private set; }

        public string ProductId { get; private set; }

        public string ProductGroupId { get; private set; }

		public ReviewBundle()
		{

		}

		public ReviewBundle(string reviewId, NavigationType navigationType)
            	: base(navigationType, new Dictionary<string, string>
            	{
            		[nameof(ReviewId)] = reviewId
            	})
		{

		}

        public ReviewBundle(string reviewId, string productGroupId, string productId, NavigationType navigationType)
			: base(navigationType, new Dictionary<string, string>
			{
				[nameof(ReviewId)] = reviewId,
                [nameof(ProductId)] = productId,
            [nameof(ProductGroupId)] = productGroupId
			})
		{

		}
	}
}
