using System;
using MvvmCross.Core.Views;

namespace AppRopio.Base.iOS.Views
{
    public interface IUnbindable : IMvxView
    {
        /// <summary>
        /// Pause all actions in this View instance. Method called when View stayed in navigation stack, but not presented.
        /// </summary>
        void Pause();

        /// <summary>
        /// Unbind this View instance. Method called when View removed from navigation stack.
        /// </summary>
        void Unbind();
    }
}

