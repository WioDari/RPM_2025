using System;
using System.Windows.Threading;

namespace MusicPlus.Services
{
    public class SessionService
    {
        private DispatcherTimer? _sessionTimer;
        private DispatcherTimer? _inactivityTimer;
        private DispatcherTimer? _warningTimer;
        private DispatcherTimer? _guestReminderTimer;
        private DateTime _lastActivity;
        private DateTime _sessionStart;
        private bool _isGuest;
        
        public event EventHandler? SessionExpired;
        public event EventHandler? InactivityTimeout;
        public event EventHandler? SessionWarning;
        public event EventHandler? GuestLoginReminder;

        private const int SESSION_DURATION_MINUTES = 60;
        private const int INACTIVITY_TIMEOUT_MINUTES = 5;
        private const int WARNING_BEFORE_EXPIRY_MINUTES = 10;
        private const int GUEST_REMINDER_INTERVAL_MINUTES = 5;

        public void StartSession(bool isGuest = false)
        {
            _isGuest = isGuest;
            _sessionStart = DateTime.Now;
            _lastActivity = DateTime.Now;

            if (!isGuest)
            {
                _sessionTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMinutes(SESSION_DURATION_MINUTES)
                };
                _sessionTimer.Tick += SessionTimer_Tick;
                _sessionTimer.Start();

                _warningTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMinutes(SESSION_DURATION_MINUTES - WARNING_BEFORE_EXPIRY_MINUTES)
                };
                _warningTimer.Tick += WarningTimer_Tick;
                _warningTimer.Start();

                _inactivityTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMinutes(INACTIVITY_TIMEOUT_MINUTES)
                };
                _inactivityTimer.Tick += InactivityTimer_Tick;
                ResetInactivityTimer();
            }
            else
            {
                _guestReminderTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMinutes(GUEST_REMINDER_INTERVAL_MINUTES)
                };
                _guestReminderTimer.Tick += GuestReminderTimer_Tick;
                _guestReminderTimer.Start();
            }
        }

        public void UpdateActivity()
        {
            _lastActivity = DateTime.Now;
            if (_inactivityTimer != null)
            {
                ResetInactivityTimer();
            }
        }

        public void StopSession()
        {
            _sessionTimer?.Stop();
            _inactivityTimer?.Stop();
            _warningTimer?.Stop();
            _guestReminderTimer?.Stop();
            
            _sessionTimer = null;
            _inactivityTimer = null;
            _warningTimer = null;
            _guestReminderTimer = null;
        }

        public TimeSpan GetRemainingSessionTime()
        {
            if (_isGuest)
                return TimeSpan.Zero;
            
            var elapsed = DateTime.Now - _sessionStart;
            var remaining = TimeSpan.FromMinutes(SESSION_DURATION_MINUTES) - elapsed;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }

        public TimeSpan GetInactivityTime()
        {
            return DateTime.Now - _lastActivity;
        }

        private void ResetInactivityTimer()
        {
            _inactivityTimer?.Stop();
            _inactivityTimer?.Start();
        }

        private void SessionTimer_Tick(object? sender, EventArgs e)
        {
            _sessionTimer?.Stop();
            SessionExpired?.Invoke(this, EventArgs.Empty);
        }

        private void InactivityTimer_Tick(object? sender, EventArgs e)
        {
            var inactivity = GetInactivityTime();
            if (inactivity.TotalMinutes >= INACTIVITY_TIMEOUT_MINUTES)
            {
                _inactivityTimer?.Stop();
                InactivityTimeout?.Invoke(this, EventArgs.Empty);
            }
        }

        private void WarningTimer_Tick(object? sender, EventArgs e)
        {
            _warningTimer?.Stop();
            SessionWarning?.Invoke(this, EventArgs.Empty);
        }

        private void GuestReminderTimer_Tick(object? sender, EventArgs e)
        {
            GuestLoginReminder?.Invoke(this, EventArgs.Empty);
        }
    }
}
