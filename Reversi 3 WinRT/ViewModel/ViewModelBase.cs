
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Reversi_WinRT.ViewModel
{
    /// <summary>
    /// The base class of ViewModell class.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        #region Constructors

        /// <summary>
        /// Creating the base class of ViewModell class.
        /// </summary>
        protected ViewModelBase() { }

        #endregion

        #region Methode

        /// <summary>
        /// Changing the property with checking.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
