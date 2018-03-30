using System;
using Android.Runtime;

namespace AppRopio.Base.Droid.Map
{
    public static class GooglePlayServicesUtil
    {
        /// <summary>
        /// Проверка на наличие GooglePlayService
        /// Вытащено из файла MobileBank\src\GooglePlayServices\obj\Debug\generated\src\Com.Google.Android.Gms.Common.GooglePlayServicesUtil.cs
        /// Возвращается statusCode:
        /// 0-SUCCESS, 
        /// 1-SERVICE_MISSING, 
        /// 2-SERVICE_VERSION_UPDATE_REQUIRED, 
        /// 3-SERVICE_DISABLED, 
        /// 4-SERVICE_INVALID
        /// </summary>

        private static IntPtr _classReference;
        private static IntPtr ClassReference
        {
            get
            {
                return JNIEnv.FindClass("com/google/android/gms/common/GooglePlayServicesUtil", ref _classReference);
            }
        }

        private static IntPtr _methodReference;

        public static StatusEnum GetGooglePlayServiceStatus(global::Android.Content.Context context)
        {
            if (_methodReference == IntPtr.Zero)
                _methodReference = JNIEnv.GetStaticMethodID(ClassReference, "isGooglePlayServicesAvailable", "(Landroid/content/Context;)I");

            int result = JNIEnv.CallStaticIntMethod(ClassReference, _methodReference, new JValue(context));

            return (StatusEnum)result;
        }

        public static string GetDescription(this StatusEnum status)
        {
            switch (status)
            {
                case StatusEnum.SUCCESS: return string.Empty;
                case StatusEnum.SERVICE_MISSING: return "Отсутствуют сервисы GooglePlay!";
                case StatusEnum.SERVICE_VERSION_UPDATE_REQUIRED: return "Установленная версия сервисов Google Play устарела";
                case StatusEnum.SERVICE_DISABLED: return "Установленная версия сервисов Google Play была отключена на этом устройстве";
                case StatusEnum.SERVICE_INVALID: return "Версия сервисов Google Play, установленных на данное устройство не является подлинной";
                default: return string.Empty;
            }
        }

        public enum StatusEnum
        {
            SUCCESS = 0,
            SERVICE_MISSING = 1,
            SERVICE_VERSION_UPDATE_REQUIRED = 2,
            SERVICE_DISABLED = 3,
            SERVICE_INVALID = 9
        }
    }
}
