using System;
using System.Linq;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using AppRopio.Base.Droid.Listeners;
using AppRopio.Base.Droid.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace AppRopio.Base.Droid.Adapters
{
    public class ARSimpleFragmentPagerAdapter : FragmentStatePagerAdapter
    {
        #region Fields

        private readonly IMvxAndroidBindingContext _bindingContext;

        private readonly Context _context;

        protected FragmentManager _fragmentManager;

        private int _itemTemplateId;

        #endregion

        #region Delegates

        public delegate Fragment FragmentCreatorDelegate(int position, IMvxViewModel vm);

        public delegate int TemplateSelectorDelegate(int position, IMvxViewModel vm);

        public delegate void TuneFragmentDelegate(MvxFragment createdFragment);

        #endregion

        #region Properties

        public FragmentCreatorDelegate FragmentCreator { get; set; }

        public TemplateSelectorDelegate TemplateSelector { get; set; }

        public TuneFragmentDelegate TuneFragment { get; set; }

        public Func<int, string> GetTitleFunc { get; set; }

        private int _count;
        public override int Count => _count;

        protected Context Context
        {
            get { return _context; }
        }

        protected IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
        }

        public int ItemTemplateId
        {
            get { return _itemTemplateId; }
            set
            {
                if (_itemTemplateId == value)
                    return;
                
                _itemTemplateId = value;

                if (_itemTemplateId > 0)
                    NotifyDataSetChanged();
            }
        }

        protected ICommand _itemClick;
        public ICommand ItemClick
        {
            get { return _itemClick; }
            set
            {
                if (ReferenceEquals(_itemClick, value))
                    return;

                _itemClick = value;
            }
        }

        #endregion

        #region Constructor

        public ARSimpleFragmentPagerAdapter(Context context, IMvxAndroidBindingContext bindingContext, FragmentManager fm, int fragmentCount) : base(fm)
        {
            _context = context;
            _bindingContext = bindingContext;
            _count = fragmentCount;
            _fragmentManager = fm;
            if (_bindingContext == null)
                throw new MvxException(
                    "MvxBindableListView can only be used within a Context which supports IMvxBindingActivity");
            Initialize();
        }

        protected ARSimpleFragmentPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        #endregion

        #region Private

        private void MvxFragment_ResumeCalled(object sender, EventArgs e)
        {
            var mvxFragment = (sender as MvxFragment);
            mvxFragment.CreateViewCalled -= MvxFragment_ResumeCalled;
            OnFragmentResume(mvxFragment);
        }

        private void MvxFragment_StartCalled(object sender, EventArgs e)
        {
            var mvxFragment = (sender as MvxFragment);
            mvxFragment.StartCalled -= MvxFragment_StartCalled;
            OnFragmentViewCreated(mvxFragment);
        }

        #endregion

        #region Protected

        protected void Initialize()
        {
            if (_fragmentManager.Fragments?.Any() ?? false)
            {
                foreach (var item in _fragmentManager.Fragments)
                {
                    var target = item as MvxFragment;
                    if (target != null && target.DataContext == null)
                        target.DataContext = _bindingContext.DataContext;
                }
            }
        }

        protected virtual object GetItemDataContext(int position)
        {
            return BindingContext.DataContext;
        }

        protected virtual void DestroyMvxFragment(MvxFragment fragment)
        {
            fragment.StartCalled -= MvxFragment_StartCalled;
            fragment.ResumeCalled -= MvxFragment_ResumeCalled;
            if (fragment.View != null && fragment.View.HasOnClickListeners)
                fragment.View.SetOnClickListener(null);
        }

        protected virtual void OnFragmentViewCreated(MvxFragment mvxFragment)
        {
            SetOnClickListenerToFragment(mvxFragment);
            TuneFragment?.Invoke(mvxFragment);
        }

        protected virtual void OnFragmentResume(MvxFragment mvxFragment)
        {

        }

        protected virtual void SetOnClickListenerToFragment(MvxFragment fragment)
        {
            if (ItemClick != null)
            {
                var listener = new ARViewPagerPageClickListener(
                    fragment,
                    (obj) =>
                    {
                        ExecuteCommandOnItem(ItemClick, GetItemPosition((Java.Lang.Object)obj));
                    });

                fragment.View.SetOnClickListener(listener);
            }
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, object dataContext)
        {
            if (command == null)
                return;

            var item = dataContext;
            if (item == null)
                return;

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }

        #endregion

        #region Public

        public override Fragment GetItem(int position)
        {
            Fragment fragment = null;
            object item = GetItemDataContext(position);

            if (FragmentCreator != null)
                fragment = FragmentCreator(position, item as IMvxViewModel);

            var layoutId = ItemTemplateId;

            if (fragment == null && TemplateSelector != null)
                layoutId = TemplateSelector(position, item as IMvxViewModel);

            if (fragment == null)
                fragment = new CommonViewPagerFragment(layoutId);

            if (fragment is MvxFragment mvxFragment)
                mvxFragment.BindingContext = new MvxAndroidBindingContext(_context, _bindingContext.LayoutInflaterHolder, item);

            return fragment;
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(GetTitleFunc?.Invoke(position) ?? string.Empty);
        }

        public override Java.Lang.Object InstantiateItem(Android.Views.ViewGroup container, int position)
        {
            var fragment = base.InstantiateItem(container, position);
            var mvxFragment = fragment as MvxFragment;
            if (mvxFragment != null)
            {
                if (mvxFragment.ViewModel == null)
                    mvxFragment.DataContext = GetItemDataContext(position);

                if (mvxFragment.View == null)
                    mvxFragment.StartCalled += MvxFragment_StartCalled;
                else
                    OnFragmentViewCreated(mvxFragment);

                mvxFragment.ResumeCalled += MvxFragment_ResumeCalled;
            }

            return fragment;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            var fragment = @object as MvxFragment;

            if (fragment != null)
                DestroyMvxFragment(fragment);

            base.DestroyItem(container, position, @object);
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            var result = base.IsViewFromObject(view, @object);
            return result;
        }

        public override void SetPrimaryItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            base.SetPrimaryItem(container, position, @object);
        }
        #endregion

    }
}
