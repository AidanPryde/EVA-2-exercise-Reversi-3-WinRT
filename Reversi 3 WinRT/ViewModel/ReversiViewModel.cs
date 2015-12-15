
using Reversi_WinRT.Model;

using System;
using System.Collections.ObjectModel;
using Windows.UI.Core;

namespace Reversi_WinRT.ViewModel
{
    public class ReversiViewModel : ViewModelBase
    {

        #region Fields

        CoreDispatcher dispatcher;

        /// <summary>
        /// The model of the application.
        /// </summary>
        private ReversiGameModel _model;

        /// <summary>
        /// To follow whos turn it is when updating the view.
        /// </summary>
        private Boolean _isPlayer1TurnOn;

        /// <summary>
        /// Did we saved our game? Used to safe exit.
        /// </summary>
        private Boolean _saveMenuItemEnabled;

        /// <summary>
        /// The maximum minimum size of or grid in the view, that holds the cells.
        /// </summary>
        private Int32 _tableSizeOfCells;

        /// <summary>
        /// To set pass button to enabled or disabled.
        /// </summary>
        private Boolean _passButtonEnabled;
        /// <summary>
        /// To set pause button to enabled or disabled.
        /// </summary>
        private Boolean _pauseButtonEnabled;

        /// <summary>
        /// To set pause button text from 'Pause' to 'Unpause' and reversed.
        /// </summary>
        private String _pauseText;

        /// <summary>
        /// To set player 1 used time.
        /// </summary>
        private Int32 _player1Time;
        /// <summary>
        /// To set palyer 2 used time.
        /// </summary>
        private Int32 _player2Time;

        /// <summary>
        /// To update the status bar with the players points.
        /// </summary>
        private Int32 _player1Points;
        /// <summary>
        /// To update the status bar with the players points.
        /// </summary>
        private Int32 _player2Points;

        /// <summary>
        /// To follow if we can safly exit the game.
        /// </summary>
        private Boolean _saved;

        #endregion

        #region Properties

        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand ExitApplicationCommand { get; private set; }

        public DelegateCommand RulesCommand { get; private set; }

        public DelegateCommand AboutCommand { get; private set; }

        public DelegateCommand PassCommand { get; private set; }

        public DelegateCommand PauseCommand { get; private set; }

        public ObservableCollection<ReversiCell> Cells { get; private set; }

        public Boolean SaveMenuItemEnabled
        {
            get
            {
                return _saveMenuItemEnabled;
            }
        }

        public Boolean SmallMenuItemChecked //TODO: Can we do it with less invoke? We do not refresh if there is no change, so maybe we do not have to look for a possibly better sollution.
        {
            get
            {
                return _model.TableSizeSetting == 10;
            }

            set
            {
                _model.TableSizeSetting = 10;
                OnPropertyChanged("SmallMenuItemChecked");
                OnPropertyChanged("MediumMenuItemChecked");
                OnPropertyChanged("LargeMenuItemChecked");
                OnPropertyChanged("SmallMenuItemEnabled");
                OnPropertyChanged("MediumMenuItemEnabled");
                OnPropertyChanged("LargeMenuItemEnabled");
            }
        }

        public Boolean MediumMenuItemChecked //TODO: Can we do it with less invoke? We do not refresh if there is no change, so maybe we do not have to look for a possibly better sollution.
        {
            get
            {
                return _model.TableSizeSetting == 20;
            }

            set
            {
                _model.TableSizeSetting = 20;
                OnPropertyChanged("SmallMenuItemChecked");
                OnPropertyChanged("MediumMenuItemChecked");
                OnPropertyChanged("LargeMenuItemChecked");
                OnPropertyChanged("SmallMenuItemEnabled");
                OnPropertyChanged("MediumMenuItemEnabled");
                OnPropertyChanged("LargeMenuItemEnabled");
            }
        }

        public Boolean LargeMenuItemChecked //TODO: Can we do it with less invoke? We do not refresh if there is no change, so maybe we do not have to look for a possibly better sollution.
        {
            get
            {
                return _model.TableSizeSetting == 30;
            }

            set
            {
                _model.TableSizeSetting = 30;
                OnPropertyChanged("SmallMenuItemChecked");
                OnPropertyChanged("MediumMenuItemChecked");
                OnPropertyChanged("LargeMenuItemChecked");
                OnPropertyChanged("SmallMenuItemEnabled");
                OnPropertyChanged("MediumMenuItemEnabled");
                OnPropertyChanged("LargeMenuItemEnabled");
            }
        }

        public Int32 TableSizeOfCells { get { return _tableSizeOfCells; } }

        public Boolean PassButtonEnabled { get { return _passButtonEnabled; } }

        public Boolean PauseButtonEnabled { get { return _pauseButtonEnabled; } }

        public String PauseText { get { return _pauseText; } }

        public Int32 Player1Time { get { return _player1Time; } }

        public Int32 Player2Time { get { return _player2Time; } }

        public Int32 Player1Points { get { return _player1Points; } }

        public Int32 Player2Points { get { return _player2Points; } }

        public Boolean Saved
        {
            get { return _saved; }
            set { _saved = value; }
        }

        #endregion

        #region Events

        public event EventHandler NewGame;

        public event EventHandler LoadGame;

        public event EventHandler SaveGame;

        public event EventHandler ExitApplication;

        public event EventHandler ReadRules;

        public event EventHandler ReadAbout;

        #endregion

        #region Constructors

        /// <summary>
        /// Creaeting the reversi ViewModel.
        /// </summary>
        /// <param name="model">The Model type, which it will use.</param>
        public ReversiViewModel(ReversiGameModel model)
        {
            // Initialize what we have to.
            _model = model;
            _model.SetGameEnded += new EventHandler<ReversiSetGameEndedEventArgs>(Model_SetGameEnded);
            _model.UpdatePlayerTime += new EventHandler<ReversiUpdatePlayerTimeEventArgs>(Model_UpdatePlayerTime);
            _model.UpdateTable += new EventHandler<ReversiUpdateTableEventArgs>(Model_UpdateTable);

            NewGameCommand = new DelegateCommand(param => { OnNewGame(); });
            LoadGameCommand = new DelegateCommand(param => { OnLoadGame(); });
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitApplicationCommand = new DelegateCommand(param => OnExitApplication());

            RulesCommand = new DelegateCommand(param => OnReadRules());
            AboutCommand = new DelegateCommand(param => OnReadAbout());

            PassCommand = new DelegateCommand(param => OnPass());
            PauseCommand = new DelegateCommand(param => OnPause());

            Cells = new ObservableCollection<ReversiCell>();

            _saved = true;

            _saveMenuItemEnabled = false;
            OnPropertyChanged("SaveMenuItemEnabled");

            _passButtonEnabled = false;
            OnPropertyChanged("PassButtonEnabled");
            _pauseButtonEnabled = false;
            OnPropertyChanged("PauseButtonEnabled");

            _pauseText = "Pause";
            OnPropertyChanged("PauseText");

            _player1Time = 0;
            OnPropertyChanged("Player1Time");
            _player2Time = 0;
            OnPropertyChanged("Player2Time");

            _player1Points = 0;
            OnPropertyChanged("Player1Points");
            _player2Points = 0;
            OnPropertyChanged("Player2Points");

            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// When the cell is clicked this is called.
        /// </summary>
        /// <param name="index">The tab index of the button, which was clicked.</param>
        private void MakePutDown(Int32 index)
        {
            ReversiCell cell = Cells[index];

            if (cell.Enabled == true)
            {
                _model.PutDown(cell.X, cell.Y);
            }
        }

        public void SetButtonGridUp()
        {
            // If there is a size change.
            if (Cells.Count == 0 || Cells.Count != _model.ActiveTableSize * _model.ActiveTableSize)
            {
                Cells.Clear();
                for (Int32 i = 0; i < _model.ActiveTableSize; ++i)
                {
                    for (Int32 j = 0; j < _model.ActiveTableSize; ++j)
                    {
                        //TODO: Only create the new ones? It seams even harder then in WFA.
                        Cells.Add(new ReversiCell(new DelegateCommand(param => MakePutDown(Convert.ToInt32(param))), i, j, ((i * _model.ActiveTableSize) + j)));
                    }
                }
            }

            _tableSizeOfCells = _model.ActiveTableSize;
            OnPropertyChanged("TableSizeOfCells");
        }

        /// <summary>
        /// Update a cell.
        /// </summary>
        /// <param name="x">The X coordiante of the cell.</param>
        /// <param name="y">The Y coordiante of the cell.</param>
        /// <param name="value">The number which will tell us what to do. Read more about it in the model (_table).</param>
        public void UpdateCell(Int32 x, Int32 y, Int32 value)
        {
            Int32 index = ((x * _model.ActiveTableSize) + y);
            ReversiCell cell = Cells[index];

            switch (value)
            {
                case -1:
                    cell.Text = "";
                    cell.BackColorInt = 0; // Color.White;
                    cell.Enabled = false;
                    break;

                case 0:
                    cell.Text = "";
                    cell.BackColorInt = 2; // Color.YellowGreen;
                    cell.Enabled = false;
                    break;

                case 1:
                    cell.Text = "";
                    cell.BackColorInt = 1; // Color.Black;
                    cell.Enabled = false;
                    break;

                case 3:
                    if (!_isPlayer1TurnOn)
                    {
                        cell.Text = "o";
                        cell.TextColorInt = 1; // Color.Black;
                        cell.Enabled = true;
                    }
                    else
                    {
                        cell.Text = "";
                        cell.Enabled = false;
                    }

                    cell.BackColorInt = 2; // Color.YellowGreen;

                    break;

                case 6:

                    if (_isPlayer1TurnOn)
                    {
                        cell.Text = "o";
                        cell.TextColorInt = 0; // Color.White;
                        cell.Enabled = true;
                    }
                    else
                    {
                        cell.Text = "";
                        cell.Enabled = false;
                    }

                    cell.BackColorInt = 2; // Color.YellowGreen;

                    break;

                case 4:
                    cell.Text = "o";
                    if (_isPlayer1TurnOn)
                    {
                        cell.TextColorInt = 0; // Color.White;
                    }
                    else
                    {
                        cell.TextColorInt = 1; // Color.Black;
                    }
                    //cell.TextColorInt = 3 // Color.Gray;
                    cell.Enabled = true;
                    break;

                case 5:
                    cell.Text = "";
                    cell.BackColorInt = 2; // Color.YellowGreen;
                    cell.Enabled = false;

                    break;

                default:
                    throw new Exception("Model gave us a number, that we was not ready for, while updating the table view.");
            }
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Model invoked this. The game ended.
        /// </summary>
        /// <param name="sender">The model. We do not use this.</param>
        /// <param name="e">The data of the ended game. We do not use it here. Controll will hadle it.</param>
        private void Model_SetGameEnded(object sender, ReversiSetGameEndedEventArgs e)
        {
            _passButtonEnabled = false;
            OnPropertyChanged("PassButtonEnabled");
            _pauseButtonEnabled = false;
            OnPropertyChanged("PauseButtonEnabled");

            _saved = true;

            _saveMenuItemEnabled = false;
            OnPropertyChanged("SaveMenuItemEnabled");
        }

        /// <summary>
        /// Model invoked this. One of the player time has advanced.
        /// </summary>
        /// <param name="sender">The model. We do not use this.</param>
        /// <param name="e">The data, which help us update the view.</param>
        private async void Model_UpdatePlayerTime(object sender, ReversiUpdatePlayerTimeEventArgs e)
        {
            if (e.IsPlayer1TimeNeedUpdate)
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _player1Time = e.NewTime;
                        OnPropertyChanged("Player1Time");
                    });
            }
            else
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _player2Time = e.NewTime;
                        OnPropertyChanged("Player2Time");
                    });
            }
        }

        /// <summary>
        /// Model invoked this. The table needs update.
        /// </summary>
        /// <param name="sender">The model. We do not use this.</param>
        /// <param name="e">The data we need to update the talbe.</param>
        private void Model_UpdateTable(object sender, ReversiUpdateTableEventArgs e)
        {
            _saved = false;

            _saveMenuItemEnabled = true;
            OnPropertyChanged("SaveMenuItemEnabled");

            _player1Points = e.Player1Points;
            OnPropertyChanged("Player1Points");
            _player2Points = e.Player2Points;
            OnPropertyChanged("Player2Points");

            _isPlayer1TurnOn = e.IsPlayer1TurnOn;

            _passButtonEnabled = e.IsPassingTurnOn;
            OnPropertyChanged("PassButtonEnabled");

            if (e.UpdatedFieldsCount == 0)
            {
                _pauseButtonEnabled = true;
                OnPropertyChanged("PauseButtonEnabled");

                _pauseText = "Pause";
                OnPropertyChanged("PauseText");

                SetButtonGridUp();

                Int32 index = 0;
                for (Int32 x = 0; x < _model.ActiveTableSize; ++x)
                {
                    for (Int32 y = 0; y < _model.ActiveTableSize; ++y)
                    {
                        UpdateCell(x, y, e.UpdatedFieldsDatas[index]);
                        ++index;
                    }
                }
            }
            else
            {
                for (Int32 i = 0; i < e.UpdatedFieldsCount; i += 3)
                {
                    UpdateCell(e.UpdatedFieldsDatas[i], e.UpdatedFieldsDatas[i + 1], e.UpdatedFieldsDatas[i + 2]);
                }
            }
        }

        #endregion

        #region Event methods

        private void OnNewGame()
        {
            if (NewGame != null)
            {
                NewGame(this, EventArgs.Empty);
            }
        }

        private void OnLoadGame()
        {
            if (LoadGame != null)
            {
                LoadGame(this, EventArgs.Empty);
                if (_saved == false)
                {
                    _saveMenuItemEnabled = true;
                    OnPropertyChanged("SaveMenuItemEnabled");
                }
            }
        }

        private void OnSaveGame()
        {
            if (SaveGame != null)
            {
                SaveGame(this, EventArgs.Empty);
                if (_saved == true)
                {
                    _saveMenuItemEnabled = false;
                    OnPropertyChanged("SaveMenuItemEnabled");
                }
            }
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
            {
                ExitApplication(this, EventArgs.Empty);
            }
        }

        private void OnReadRules()
        {
            if (ReadRules != null)
            {
                ReadRules(this, EventArgs.Empty);
            }
        }

        private void OnReadAbout()
        {
            if (ReadAbout != null)
            {
                ReadAbout(this, EventArgs.Empty);
            }
        }

        private void OnPass()
        {
            _model.Pass();
            _saved = false;
            _saveMenuItemEnabled = true;
            OnPropertyChanged("SaveMenuItemEnabled");
        }

        private void OnPause()
        {
            if (_pauseText == "Pause")
            {
                _pauseText = "Unpause";
                _model.Pause();
                OnPropertyChanged("PauseText");
            }
            else
            {
                _pauseText = "Pause";
                _model.Unpause();
                OnPropertyChanged("PauseText");
            }
        }

        #endregion
    }
}
