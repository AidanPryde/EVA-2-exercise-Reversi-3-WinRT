
using System;

namespace Reversi_WinRT.Model
{
    public class TimerElapsedEventArgs
    {
        /// <summary>
        /// Esemény bekövetkezési időpontjának lekérdezése.
        /// </summary>
        public DateTime SignalTime
        {
            get;

            private set;
        }

        /// <summary>
        /// Időmúlás eseményargumentum példányosítása.
        /// </summary>
        public TimerElapsedEventArgs()
        {
            SignalTime = DateTime.Now;
        }
    }

    /// <summary>
    /// Időmúlás eseménykezelő típusa.
    /// </summary>
    public delegate void ElapsedEventHandler(object sender, TimerElapsedEventArgs e);
}
