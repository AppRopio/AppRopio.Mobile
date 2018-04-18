using System;
using System.Globalization;
using Android.Support.V7.Widget;
using Android.Widget;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Listeners;
using AppRopio.Base.Droid.Views;
using AppRopio.Base.Filters.Core.ViewModels.Filters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax.Date;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax.Number;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Android.App;
using AppRopio.Base.Filters.Core;

namespace AppRopio.Base.Filters.Droid.Views.Filters
{
    public class FiltersFragment : CommonFragment<IFiltersViewModel>
    {
        protected const int CLEAR_ID = 1;

        public FiltersFragment()
            : base(Resource.Layout.app_filters_filters)
        {
            HasOptionsMenu = true;
            Title = LocalizationService.GetLocalizableString(FiltersConstants.RESX_NAME, "Title");
        }

        protected virtual void SetupRecyclerView(MvxRecyclerView recyclerView)
        {
            SetupAdapter(recyclerView);
        }

        protected virtual void SetupAdapter(MvxRecyclerView recyclerView)
        {
            recyclerView.Adapter = new ARFlatGroupAdapter(null, SetupTemplateSelector(), BindingContext)
            {
                TuneSectionItemOnBind = TuneItemOnBind
            };
        }

        protected virtual FiltersTemplateSelector SetupTemplateSelector()
        {
            return new FiltersTemplateSelector();
        }

        protected virtual void TuneItemOnBind(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            if (viewHolder.ItemViewType == Resource.Layout.app_filters_filters_numberMinMax)
            {
                var fromInput = viewHolder.ItemView.FindViewById<EditText>(Resource.Id.app_filters_filters_minMax_content_input_from);
                var toInput = viewHolder.ItemView.FindViewById<EditText>(Resource.Id.app_filters_filters_minMax_content_input_to);

                var mvxBindingContextOwner = (viewHolder as IMvxBindingContextOwner);

                BindMinMaxInputs<float>(mvxBindingContextOwner, fromInput, toInput, StringExtentionsMethods.StringPrice);

                var itemVm = mvxBindingContextOwner.BindingContext.DataContext as INumberMinMaxFiVm;

                fromInput.SetOnKeyListener(new AROnEnterKeyListener(Activity, () => itemVm.MinValueChangedCommand.Execute(null)));
                toInput.SetOnKeyListener(new AROnEnterKeyListener(Activity, () => itemVm.MaxValueChangedCommand.Execute(null)));
            }
            else if (viewHolder.ItemViewType == Resource.Layout.app_filters_filters_dateMinMax)
            {
                var fromInput = viewHolder.ItemView.FindViewById<EditText>(Resource.Id.app_filters_filters_minMax_content_input_from);
                var toInput = viewHolder.ItemView.FindViewById<EditText>(Resource.Id.app_filters_filters_minMax_content_input_to);

                fromInput.Hint = CultureInfo.CurrentCulture == new CultureInfo("ru-RU") ? "дд.мм.гггг" : $"MM{CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator}dd{CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator}yyyy";
                toInput.Hint = CultureInfo.CurrentCulture == new CultureInfo("ru-RU") ? "дд.мм.гггг" : $"MM{CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator}dd{CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator}yyyy";

                var mvxBindingContextOwner = (viewHolder as IMvxBindingContextOwner);

                BindMinMaxInputs<DateTime>(mvxBindingContextOwner, fromInput, toInput, StringExtentionsMethods.StringDate);

                var itemVm = mvxBindingContextOwner.BindingContext.DataContext as IDateMinMaxFiVm;

                SetupDatePicker(fromInput, date => itemVm.MinValueChangedCommand.Execute(date), itemVm.AbsoluteMin, itemVm.AbsoluteMax, () => itemVm.Min);
                SetupDatePicker(toInput, date => itemVm.MaxValueChangedCommand.Execute(date), itemVm.AbsoluteMin, itemVm.AbsoluteMax, () => itemVm.Max);
            }
        }

        protected virtual void BindMinMaxInputs<T>(IMvxBindingContextOwner mvxBindingContextOwner, EditText fromInput, EditText toInput, Func<object, string> stringFormat)
            where T : IComparable, IComparable<T>, IEquatable<T>
        {
            var set = mvxBindingContextOwner.CreateBindingSet<IMvxBindingContextOwner, IBaseMinMaxFiVm<T>>();

            set.Bind(fromInput).To(vm => vm.Min).WithConversion("StringFormat", new StringFormatParameter { StringFormat = stringFormat });

            set.Bind(toInput).To(vm => vm.Max).WithConversion("StringFormat", new StringFormatParameter { StringFormat = stringFormat });

            set.Apply();
        }

        protected virtual void SetupDatePicker(EditText editText, Action<DateTime> onDateSelected, DateTime minDate, DateTime maxDate, Func<DateTime> currentDateFunc)
        {
            editText.Focusable = false;
            editText.FocusableInTouchMode = false; // user touches widget on phone with touch screen
            editText.Clickable = false; // user navigates with wheel and selects widget

            editText.SetOnClickListener(new AROnClickListener(() =>
            {
                var currentDate = currentDateFunc.Invoke();

                var datePickerDialog = new DatePickerDialog(Context, (sender, e) =>
                {
                    onDateSelected?.Invoke(e.Date);
                }, currentDate.Year, currentDate.Month, currentDate.Day);

                var javaMinDt = new DateTime(1970, 1, 1);

                datePickerDialog.DatePicker.DateTime = currentDate;

                if (minDate.CompareTo(javaMinDt) > 0)
                    datePickerDialog.DatePicker.MinDate = (long)(minDate - javaMinDt).TotalMilliseconds;

                if (maxDate.CompareTo(javaMinDt) > 0)
                    datePickerDialog.DatePicker.MaxDate = (long)(maxDate - javaMinDt).TotalMilliseconds;

                datePickerDialog.Show();
            }));
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_filters_filters_recyclerView);
            SetupRecyclerView(recyclerView);
        }

        public override void OnCreateOptionsMenu(Android.Views.IMenu menu, Android.Views.MenuInflater inflater)
        {
            var menuItem = menu.Add(0, CLEAR_ID, 0, new Java.Lang.String(LocalizationService.GetLocalizableString(FiltersConstants.RESX_NAME, "Filters_Clear")));
            menuItem.SetShowAsAction(Android.Views.ShowAsAction.Always);
            menuItem.SetActionView(Resource.Layout.app_filters_filters_clearButton);
            menuItem.ActionView.Click += (sender, e) =>
            {
                ViewModel?.ClearCommand.Execute(null);
            };
        }
    }
}
