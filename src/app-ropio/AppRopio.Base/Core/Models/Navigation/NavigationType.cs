namespace AppRopio.Base.Core.Models.Navigation
{
    /// <summary>
    /// Тип навигации, используется в Presenter'ах для определения стилия навигации
    /// </summary>
    public enum NavigationType
    {
        /// <summary>
        /// Отсутствует. Для стартовых ViewModel'ей
        /// </summary>
        None = 0,

        /// <summary>
        /// Push-навигация. Запрещена повторная навигация на ViewModel такого же типа
        /// </summary>
        Push = 1,

        /// <summary>
        /// Push-навигация. Разрешена повторная навигация на ViewModel такого же типа
        /// </summary>
        DoublePush = 2,

        /// <summary>
        /// Push-навигация на Root'овые ViewModel'и. Запрещена повторная навигация на ViewModel такого же типа
        /// </summary>
        ClearAndPush = 3,

        /// <summary>
        /// Push-навигация на Root'овые ViewModel'и. Разрешена повторная навигация на ViewModel такого же типа
        /// </summary>
        DoubleClearAndPush = 4,

        /// <summary>
        /// Модальная навигация
        /// </summary>
        PresentModal = 5,

        /// <summary>
        /// Внутри какого-либо экрана
        /// </summary>
        InsideScreen = 6
    }
}
