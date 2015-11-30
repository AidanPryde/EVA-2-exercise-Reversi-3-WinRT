
using System;

namespace Reversi_WinRT.ViewModel
{

    /// <summary>
    /// The reversi game-cell class.
    /// </summary>
    public class ReversiCell : ViewModelBase
    {

        #region Fields

        /// <summary>
        /// If false, it still can be clicked, but no model action will be called.
        /// </summary>
        private Boolean _enabled;

        /// <summary>
        /// We have two state now 'o' Or ''. It is to show if is there a possible put down.
        /// </summary>
        private String _text;

        /// <summary>
        /// To distinguish the player 1 and player 2 possible put downs.
        /// </summary>
        private Int32 _textColorInt;

        /// <summary>
        /// To see what kind of the cell is.
        /// </summary>
        private Int32 _backColorInt;

        /// <summary>
        /// The X coordinate of the cell in the grid.
        /// </summary>
        private Int32 _x;

        /// <summary>
        /// The Y coordinate of the cell in the grid.
        /// </summary>
        private Int32 _y;

        /// <summary>
        /// The tabindex in the grib.
        /// </summary>
        private Int32 _index;

        /// <summary>
        /// What to do when clicked?
        /// </summary>
        private DelegateCommand _putDownCommand;

        #endregion

        #region Properties

        public Boolean Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (_enabled != value) // Only refresh if has to.
                {
                    _enabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public String Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (_text != value) // Only refresh if has to.
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 TextColorInt
        {
            get
            {
                return _textColorInt;
            }

            set
            {
                if (_textColorInt != value) // Only refresh if has to.
                {
                    _textColorInt = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 BackColorInt
        {
            get
            {
                return _backColorInt;
            }

            set
            {
                if (_backColorInt != value) // Only refresh if has to.
                {
                    _backColorInt = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 X
        {
            get
            {
                return _x;
            }
        }

        public Int32 Y
        {
            get
            {
                return _y;
            }
        }
        public Int32 Index
        {
            get
            {
                return _index;
            }
        }

        public DelegateCommand PutDownCommand
        {
            get
            {
                return _putDownCommand;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="putDown">The delegate to do, when clicked.</param>
        /// <param name="x">The X coordinate of the cell in the grid.</param>
        /// <param name="y">The Y coordinate of the cell in the grid.</param>
        /// <param name="index">The tabindex in the grib.</param>
        /// <param name="enabled">If false, it still can be clicked, but no model action will be called.</param>
        /// <param name="text">We have two state now 'o' Or ''. It is to show if is there a possible put down.</param>
        /// <param name="textColorInt">To distinguish the player 1 and player 2 possible put downs.</param>
        /// <param name="backColorInt">To see what kind of the cell is.</param>
        public ReversiCell(DelegateCommand putDown, Int32 x, Int32 y, Int32 index, Boolean enabled = false, String text = "", Int32 textColorInt = 0, Int32 backColorInt = 0)
        {
            _putDownCommand = putDown;

            _x = x;
            _y = y;

            _index = index;

            _enabled = enabled;
            _text = text;
            _textColorInt = textColorInt;
            _backColorInt = backColorInt;
        }

        #endregion

    }
}
