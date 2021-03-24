using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.Models.Products.Responses;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Transition
{
    public class TransitionPciVm : ProductDetailsItemVM, ITransitionPciVm
    {
        protected Type CustomVmType { get; set; }

        protected string ProductId { get; }

        protected string ProductGroupId { get; }

        public string Value { get; }

        protected IProductsNavigationVmService NavigationVmService => Mvx.Resolve<IProductsNavigationVmService>();

        protected IProductConfigService ConfigService => Mvx.Resolve<IProductConfigService>();

        public TransitionPciVm(string groupId, string productId, ProductParameter parameter)
            : base(parameter)
        {
            ProductGroupId = groupId;
            ProductId = productId;

            Value = parameter.DataType == ProductDataType.Custom ? parameter.Content : string.Empty;
        }

        private Task InitCustomTypeIfExist()
        {
            return Task.Run(() =>
            {
                var existCustomType = ConfigService.Config.ProductDetails?.CustomTypes?.FirstOrDefault(x => x.Name.ToLowerInvariant() == CustomType.ToLowerInvariant());

                if (existCustomType != null)
                {
                    var assembly = Assembly.Load(new AssemblyName(existCustomType.Assembly.AssemblyName));

                    CustomVmType = assembly.GetType(existCustomType.Assembly.TypeName);
                }
            });
        }

        public override void ClearSelectedValue()
        {

        }

        public void OnSelected()
        {
            if (CustomType.IsNullOrEmtpy())
            {
                switch (DataType)
                {
                    case ProductDataType.Text:
                        NavigationVmService.NavigateToTextContent(new BaseTextContentBundle(Name, Content, NavigationType.Push));
                        break;
                    case ProductDataType.Html:
                        NavigationVmService.NavigateToWebContent(new BaseWebContentBundle(NavigationType.Push, Name, Content));
                        break;
                }
            }
            else if (CustomVmType != null)
            {
                NavigationVmService.NavigateToCustomType(
                    CustomVmType,
                    new BaseBundle(NavigationType.Push, new Dictionary<string, string> 
                    { 
                        [nameof(ProductId)] = ProductId,
                        [nameof(ProductGroupId)] = ProductGroupId 
                    })
                );
            }
        }

        public override Task Initialize()
        {
            if (!CustomType.IsNullOrEmtpy())
                return Task.Run(InitCustomTypeIfExist);

            return Task.FromResult<object>(null);
        }
    }
}
