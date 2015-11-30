
using System;
using Windows.System.Threading;

namespace Reversi_WinRT.Model
{
    public class Timer : IDisposable
    {
        private ThreadPoolTimer _timer; // The build-in timer.
        private TimeSpan _interval;
        private Boolean _autoReset;
        private Boolean _enabled;

        /// <summary>
        /// Ismételt eseményjelzés lekérdezése, vagy beállítása.
        /// </summary>
        public Boolean AutoReset
        {
            get
            {
                return _autoReset;
            }

            set
            {
                _autoReset = value;
            }
        }

        /// <summary>
        /// Bekapcsolt állapot lekérdezése, vagy beállítása.
        /// </summary>
        public Boolean Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (value)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }
        }

        /// <summary>
        /// Időintervallum lekérdezése, vagy beállítása (ezred másodpercben).
        /// </summary>
        public Double Interval
        {
            get
            {
                return _interval.TotalMilliseconds;
            }

            set
            {
                Stop();
                _interval = TimeSpan.FromMilliseconds(value);
                Start();
            }
        }

        /// <summary>
        /// Idő eltelésének eseménye.
        /// </summary>
        public ElapsedEventHandler Elapsed;

        /// <summary>
        /// Időzítő példányosítása.
        /// </summary>
        public Timer()
        {
            _interval = TimeSpan.FromMilliseconds(1000);
            _enabled = false;
            _autoReset = true;
        }

        /// <summary>
        /// Időzítő példányosítása.
        /// </summary>
        /// <param name="interval">Az időintervallum (ezred másodpercben).</param>
        public Timer(Double interval, Boolean autoReset)
        {
            _interval = TimeSpan.FromMilliseconds(interval);
            _enabled = false;
            _autoReset = autoReset;
        }

        /// <summary>
        /// Időzítő megsemmisítése.
        /// </summary>
        ~Timer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Időzítő indítása.
        /// </summary>
        public void Start()
        {
            if (_timer != null)
            {
                _timer.Cancel();
            }

            if (_autoReset)
            {
                _timer = ThreadPoolTimer.CreatePeriodicTimer(Timeout, _interval);
            }
            else
            {
                _timer = ThreadPoolTimer.CreateTimer(Timeout, _interval);
            }

            _enabled = true;
        }

        /// <summary>
        /// Időzítő leállítása.
        /// </summary>
        public void Stop()
        {
            _timer.Cancel();
            _timer = null;
            _enabled = false;
        }

        /// <summary>
        /// Erőforrások felszabadítása.
        /// </summary>
        public void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Erőforrások felszabadítása.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Idő lejártának eseménykezelője.
        /// </summary>
        private void Timeout(ThreadPoolTimer timer)
        {
            if (Elapsed != null)
            {
                Elapsed(this, new TimerElapsedEventArgs());
            }

            if (!_autoReset)
            {
                _enabled = false;
            }
        }

        /// <summary>
        /// Megsemmisítés végrehajtása.
        /// </summary>
        /// <param name="disposing">Felügyelt erőforrások megsemmisítése.</param>
        private void Dispose(Boolean disposing)
        {
            if (_timer != null)
            {
                _timer.Cancel();
            }

            if (disposing)
            {
                _timer = null;
            }
        }
    }
}
