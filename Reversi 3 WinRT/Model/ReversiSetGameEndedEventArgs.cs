
using System;

namespace Reversi_WinRT.Model
{
    /// <summary>
    /// Type of the Reversi set game ended event argument.
    /// </summary>
    public class ReversiSetGameEndedEventArgs : EventArgs
    {
        private Int32 _player1Points;
        private Int32 _player2Points;

        /// <summary>
        /// Quary of the '_player1Points' field value. Player 1 points.
        /// </summary>
        public Int32 Player1Points
        {
            get
            {
                return _player1Points;
            }
        }

        /// <summary>
        /// Quary of the '_player2Points' field value. Player 2 points.
        /// </summary>
        public Int32 Player2Points
        {
            get
            {
                return _player2Points;
            }
        }

        /// <summary>
        /// Creating Reversi set game ended event argument instance.
        /// </summary>
        /// <param name="player1Points">Player 1 points.</param>
        /// <param name="player2Points">Player 2 points.</param>
        public ReversiSetGameEndedEventArgs(Int32 player1Points, Int32 player2Points)
        {
            _player1Points = player1Points;
            _player2Points = player2Points;
        }
    }
}
