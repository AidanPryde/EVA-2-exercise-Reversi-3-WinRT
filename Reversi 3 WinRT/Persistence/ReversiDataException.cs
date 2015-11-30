
using System;

namespace Reversi_WinRT.Persistence
{
    /// <summary>
    /// The type of the Reversi data access exception.
    /// </summary>
    public class ReversiDataException : Exception
    {

        #region Fields

        private String _info;
        private String _message;

        #endregion

        #region Properties

        public String ReversiMessage
        {
            get
            {
                return _message;
            }
        }

        public String ReversiInfo
        {
            get
            {
                return _info;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create Reversi data access exception instance.
        /// </summary>
        public ReversiDataException(String message, String info)
        {
            _message = message;
            _info = info;
        }

        #endregion

    }
}
