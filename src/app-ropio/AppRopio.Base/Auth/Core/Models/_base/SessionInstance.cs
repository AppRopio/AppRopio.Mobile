using System;

namespace AppRopio.Base.Auth.Core.Models
{
    public class SessionInstance<T>
        where T : class
	{
		private static readonly long _baseTimeTicks = DateTime.UtcNow.Ticks;

		private volatile int _lastAccessTime;
	    private volatile int _timeoutInSeconds = 120;

		private string _token;
		private bool _alive; 

		private T _currentProfile;

        private System.Threading.Timer _pingTimer;

        public bool TimeoutEnabled { get; set; }

		public string Token 
		{ 
			get 
			{ 
				CheckTimeout();
				return _token;
			}
		}

		public bool IsAlive 
		{
			get 
			{ 
				CheckTimeout();
				return _alive; 
			}
		}

        public void KillAlive()
        {
            _alive = false;
        }

		public int SessionTimeoutInSeconds 
		{
			get 
			{ 
				CheckTimeout();
				return _timeoutInSeconds; 
			}
			set 
			{
				if (_timeoutInSeconds == value)
					return;
				_timeoutInSeconds = value;
				CheckTimeout();
			}
		}

		public int SecondsToFinishByTimeout 
		{
			get 
			{
				CheckTimeout();
				if (!_alive)
					return 0;
				return _timeoutInSeconds - (Now() - _lastAccessTime);
			}
		}

		public T CurrentProfile 
		{
			get 
			{ 
				CheckTimeout();
				return _currentProfile; 
			}
		}

		public void CreateSession(string token)
		{
			if (_alive)
				return;
            _token = token;
			_lastAccessTime = Now();
		}

		public void Start(T profile)
		{
			if (_alive)
				return;
            
            if (profile == null)
                throw new ArgumentNullException (nameof(profile));
            
			_lastAccessTime = Now();
            _currentProfile = profile;
			_alive = true;

			NotifyStarted();
			NotifyCurrentProfileChanged();
		}

		public void Finish(SessionFinishReason reason)
		{
			if (!_alive)
				return;
            
			_token = null;
			_alive = false;
			_lastAccessTime = 0;
			_currentProfile = null;

			NotifyFinished (reason);
			if (reason == SessionFinishReason.Restart)
				NotifyCurrentProfileChanged();
		}

        public void ChangeProfile(T profile)
		{
			if (!IsAlive)
				return;
            if (_currentProfile != null && _currentProfile == profile)
				return;
            if (profile == null)
				return;
            _currentProfile = profile;
			NotifyCurrentProfileChanged();
		}

		public void ResetTimeout()
		{
			if (!_alive)
				return;
			_lastAccessTime = Now();
		}

        private void CheckTimeout()
		{
			if (!_alive || !TimeoutEnabled)
				return;
            var isElapsed = (Now () - _lastAccessTime) >= _timeoutInSeconds;
			if (isElapsed)
			{
                Finish(SessionFinishReason.Timeout);
				DoLogout();
			}
		}

		public event Action SessionStarted;
		public event Action<SessionFinishReason> SessionFinished;
		public event Action<T> CurrentProfileChanged;

		private void NotifyStarted()
		{
			var handler = SessionStarted;
			if (handler != null)
			{
				handler ();
			}
		}

		private void NotifyFinished(SessionFinishReason reason)
		{
			var handler = SessionFinished;
			if (handler != null)
			{
				handler (reason);
			}
		}

		private void NotifyCurrentProfileChanged()
		{
			var handler = CurrentProfileChanged;
			if (handler != null)
			{
				handler (_currentProfile);
			}
		}

		private static int Now()
		{
			return (int)(DateTime.UtcNow - new DateTime (_baseTimeTicks)).TotalSeconds;
		}

        private void DoLogout()
        {
            _alive = false;
		}
    }
}
