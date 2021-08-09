using System;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using MvvmCross;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.Views;

namespace AppRopio.Base.Droid.AttributeHelpers
{
	public class ARLinearLayoutAttributeExtensions
    {
        private static string ReadItemTemplateSelectorClassName(Context context, IAttributeSet attrs)
        {
            TypedArray typedArray = null;

            string className = string.Empty;
            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, GroupId);
                int numberOfStyles = typedArray.IndexCount;

                for (int i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == ItemTemplateSelector)
                        className = typedArray.GetString(attributeId);
                }
            }
            finally
            {
                typedArray?.Recycle();
            }

            return className;
        }

        public static IMvxTemplateSelector BuildItemTemplateSelector(Context context, IAttributeSet attrs)
        {
            var templateSelectorClassName = ReadItemTemplateSelectorClassName(context, attrs);

            if (string.IsNullOrEmpty(templateSelectorClassName))
                return null;

            var type = Type.GetType(templateSelectorClassName);
            if (type == null)
            {
                var message = $"Sorry but type with class name: {templateSelectorClassName} does not exist." +
                             $"Make sure you have provided full Type name: namespace + class name, AssemblyName." +
                              $"Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
                Mvx.IoCProvider.Resolve<IMvxLog>().Error(message);
                throw new InvalidOperationException(message);
            }

            if (!typeof(IMvxTemplateSelector).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(IMvxTemplateSelector)} interface.";
                Mvx.IoCProvider.Resolve<IMvxLog>().Error(message);
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                string message = $"Sorry can not instatiate {nameof(IMvxTemplateSelector)} as provided type: {type} is abstract/interface.";
                Mvx.IoCProvider.Resolve<IMvxLog>().Error(message);
                throw new InvalidOperationException(message);
            }

            return Activator.CreateInstance(type) as IMvxTemplateSelector;
        }

        public static int ReadTemplateId(Context context, IAttributeSet attrs)
        {
            return MvxAttributeHelpers.ReadAttributeValue(context, attrs, Resource.Styleable.ARLinearLayout, Resource.Styleable.ARLinearLayout_MvxItemTemplate);
        }

        private static int[] GroupId { get; set; } = Resource.Styleable.ARLinearLayout;
        private static int ItemTemplateSelector { get; set; } = Resource.Styleable.ARLinearLayout_MvxTemplateSelector;
        private static int ItemTemplateId { get; set; } = Resource.Styleable.ARLinearLayout_MvxItemTemplate;
    }
}
