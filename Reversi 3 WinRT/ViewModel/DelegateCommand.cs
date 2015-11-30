
using System;
using System.Windows.Input;

namespace Reversi_WinRT.ViewModel
{

    /// <summary>
    /// The general command class.
    /// </summary>
    public class DelegateCommand : ICommand
    {

        #region Fields

        /// <summary>
        /// The lambda expression, that will execute the action.
        /// </summary>
        private readonly Action<Object> _execute;

        /// <summary>
        /// The lambda expression, that will check the condition for the execution.
        /// </summary>
        private readonly Func<Object, Boolean> _canExecute;

        #endregion

        #region Contructors

        /// <summary>
        /// Creating the command.
        /// </summary>
        /// <param name="execute">The action, that should be executed.</param>
        public DelegateCommand(Action<Object> execute) :
            this(null, execute)
        {
        }

        /// <summary>
        /// Creating the command.
        /// </summary>
        /// <param name="canExecute">The condition for the execution.</param>
        /// <param name="execute">The action, that should be executed.</param>
        public DelegateCommand(Func<Object, Boolean> canExecute, Action<Object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>Returns true if this command can be executed; otherwise, false.</returns>
        public Boolean CanExecute(Object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            else
            {
                return _canExecute(parameter);
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(Object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled.");
            }

            _execute(parameter);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Event methodes

        /// <summary>
        /// Help to check if it is not null every time, when CanExecuteChanged is rased.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        #endregion

    }
}
