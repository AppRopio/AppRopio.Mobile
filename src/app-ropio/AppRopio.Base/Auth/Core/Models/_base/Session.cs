using System;
using AppRopio.Models.Auth.Responses;

namespace AppRopio.Base.Auth.Core.Models
{
    public static class Session
    {
        private static readonly SessionInstance<User> _instance = new SessionInstance<User>();
        private static SessionInstance<User> Instance
        {
            get { return _instance; }
        }

        public static string Token { get { return Instance.Token; } }
        public static int SessionTimeoutInSeconds
        {
            get { return Instance.SessionTimeoutInSeconds; }
            set { Instance.SessionTimeoutInSeconds = value; }
        }

        public static int SecondsToFinishByTimeout { get { return Instance.SecondsToFinishByTimeout; } }
        public static bool IsAlive { get { return Instance.IsAlive; } }
        public static User CurrentProfile { get { return Instance.CurrentProfile; } }

        public static event Action SessionStarted
        {
            add { Instance.SessionStarted += value; }
            remove { Instance.SessionStarted -= value; }
        }

        public static event Action<SessionFinishReason> SessionFinished
        {
            add { Instance.SessionFinished += value; }
            remove { Instance.SessionFinished -= value; }
        }

        public static event Action<User> CurrentProfileChanged
        {
            add { Instance.CurrentProfileChanged += value; }
            remove { Instance.CurrentProfileChanged -= value; }
        }

        public static void Start(string token)
        {
            Instance.CreateSession(token);
        }

        public static void Restart(string token)
        {
            if (Instance.IsAlive)
                Instance.Finish(SessionFinishReason.Restart);

            Start(token);
        }

        public static void Finish()
        {
            Instance.Finish(SessionFinishReason.Logout);
        }

        public static void ChangeUserTo(User user)
        {
            Instance.Start(user);
            Instance.ChangeProfile(user);
        }

        public static void CheckTimeout (bool isCheck)
        {
            Instance.TimeoutEnabled = isCheck;
        }

        public static void ResetTimeout()
        {
            Instance.ResetTimeout();
        }
    }
}
