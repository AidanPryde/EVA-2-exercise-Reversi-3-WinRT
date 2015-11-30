
using System;

namespace Reversi_WinRT.Model
{
    /// <summary>
    /// Type of the Reversi update table event argument.
    /// </summary>
    public class ReversiUpdateTableEventArgs : EventArgs
    {
        private Int32 _updatedFieldsCount;
        private Int32[] _updatedFieldsDatas;
        private Int32 _player1Points;
        private Int32 _player2Points;
        private Boolean _isPlayer1TurnOn;
        private Boolean _isPassingTurnOn;


        /// <summary>
        /// Quary of the '_updatedFieldsCount' field value. If its value 0,
        /// then the '_updatedFieldsDatas' the hole table values without the coordinates.
        /// Otherwise it the size on the '_updatedFieldsDatas'. One unite is two coordinate and a data value.
        /// </summary>
        public Int32 UpdatedFieldsCount
        {
            get
            {
                return _updatedFieldsCount;
            }
        }

        /// <summary>
        /// Quary of the '_updatedFieldsDatas' field value. It may contain the hole talve values without the coordinates, or
        /// its format is two coordinate, then the value.
        /// </summary>
        public Int32[] UpdatedFieldsDatas
        {
            get
            {
                return _updatedFieldsDatas;
            }
        }

        /// <summary>
        /// Quary of the '_player1Points' field value. The actual points of player 1 has.
        /// </summary>
        public Int32 Player1Points
        {
            get
            {
                return _player1Points;
            }
        }

        /// <summary>
        /// Quary of the '_player2Points' field value. The actual points of player 2 has.
        /// </summary>
        public Int32 Player2Points
        {
            get
            {
                return _player2Points;
            }
        }

        /// <summary>
        /// Quary of the '_isPassingTurnOn' field value.
        /// If '_updatedFieldsCount' is 0, then indicate which player's turn it is: true means player 1, false means playe 2.
        /// If '_updatedFieldsCount' is NOT 0, then indicate if it is a passing turn. False if it is someone's normal turn. True means the player has to pass.
        /// </summary>
        public Boolean IsPlayer1TurnOn
        {
            get
            {
                return _isPlayer1TurnOn;
            }
        }

        /// <summary>
        /// Quary of the '_isPassingTurnOn' field value.
        /// If '_updatedFieldsCount' is 0, then indicate which player's turn it is: true means player 1, false means playe 2.
        /// If '_updatedFieldsCount' is NOT 0, then indicate if it is a passing turn. False if it is someone's normal turn. True means the player has to pass.
        /// </summary>
        public Boolean IsPassingTurnOn
        {
            get
            {
                return _isPassingTurnOn;
            }
        }

        /// <summary>
        /// Creating Reversi update table event argument instance.
        /// If the "updatedFieldsCount" parameter equels to 0,
        /// then we send the new values only, without the coordinates and the Booleans to tell which player's turn it is
        /// and if it is a passing turn.
        /// Otherwise we send the data as: X, Y, data, X, Y, data, ... . with the Boleans indicating if it is a passing turn
        /// and which player turn it is.
        /// The sent datas are changed datas and datas for possible put downs for each players.
        /// Both case we send the player points.
        /// </summary>
        /// <param name="updatedFieldsCount">The updated fields and possible put downs fields count.</param>
        /// <param name="updatedFieldsDatas">The updated fields and possible put downs fields data.</param>
        /// <param name="player1Points">The points of player 1 has.</param>
        /// <param name="player2Points">The points of player 2 has.</param>
        /// <param name="isPlayer1TurnOn">Indicate the type of the turn or one of the player which turn it is.</param>
        /// <param name="isPassingTurnOn">Indicate the type of the turn or one of the player which turn it is.</param>
        public ReversiUpdateTableEventArgs(Int32 updatedFieldsCount, Int32[] updatedFieldsDatas,
            Int32 player1Points, Int32 player2Points, Boolean isPlayer1TurnOn, Boolean isPassingTurnOn)
        {
            _updatedFieldsCount = updatedFieldsCount;
            _updatedFieldsDatas = updatedFieldsDatas;
            _player1Points = player1Points;
            _player2Points = player2Points;
            _isPlayer1TurnOn = isPlayer1TurnOn;
            _isPassingTurnOn = isPassingTurnOn;
        }
    }
}
