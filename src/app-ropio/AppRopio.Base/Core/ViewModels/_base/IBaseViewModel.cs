using MvvmCross.ViewModels;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.Base.Core.ViewModels
{
    /// <summary>
    /// Интерфейс для базовой ViewModel
    /// </summary>
    public interface IBaseViewModel : IMvxViewModel<IMvxBundle>, IMvxNotifyPropertyChanged
    {
        /// <summary>
        /// Задает или возвращает тип навигации
        /// </summary>
        /// <value>Тип навигации</value>
        NavigationType VmNavigationType { get; set; }

        /// <summary>
        /// Задает или возвращает величину показывающую, что эта
        /// <see cref="T:AppRopio.Base.Core.ViewModels.IBaseViewModel"/> загружается.
        /// </summary>
        /// <value><c>true</c> если загружается; иначе, <c>false</c>.</value>
        bool Loading { get; set; }

        /// <summary>
        /// Название экрана
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Задает или возвращает название ViewModel
        /// </summary>
        /// <value>Название ViewModel</value>
        string ViewModelTitle { get; set; }

        /// <summary>
        /// Pause all actions in this View Model instance. Method called when ViewModel stayed in navigation stack, but not presented.
        /// </summary>
        void Pause();

        /// <summary>
        /// Unbind этого экземпляра ViewModel
        /// </summary>
        void Unbind();
    }
}