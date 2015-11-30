
using System;

namespace Reversi_WinRT.Model
{
    /// <summary>
    /// Type of the Reversi update player time event argument.
    /// </summary>
    public class ReversiUpdatePlayerTimeEventArgs : EventArgs
    {
        private Boolean _isPlayer1TimeNeedUpdate;
        private Int32 _newTime;

        /// <summary>
        /// Quary of the '_isPlayer1TimeNeedUpdate' field value.
        /// If it is true we need to update player 1 time. If it is false we need to update player 2 time.
        /// </summary>
        public Boolean IsPlayer1TimeNeedUpdate
        {
            get
            {
                return _isPlayer1TimeNeedUpdate;
            }
        }

        /// <summary>
        /// Quary of the '_newTime' field value. The new time value for the active player.
        /// </summary>
        public Int32 NewTime
        {
            get
            {
                return _newTime;
            }
        }

        /// <summary>
        /// Creating Reversi update player time event argument instance.
        /// </summary>
        /// <param name="isPlayer1TimeNeedUpdate">Whos time needs to be updated?</param>
        /// <param name="newTime">The new time for the player.</param>
        public ReversiUpdatePlayerTimeEventArgs(Boolean isPlayer1TimeNeedUpdate, Int32 newTime)
        {
            _isPlayer1TimeNeedUpdate = isPlayer1TimeNeedUpdate;
            _newTime = newTime;
        }
    }
}
