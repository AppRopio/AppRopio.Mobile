namespace AppRopio.Base.API
{
    public static class ApiSettings
    {
        public static bool DebugServiceEnabled =>
#if DEBUG 
        true;
#else
        false;
#endif
    }
}
