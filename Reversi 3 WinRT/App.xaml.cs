
using Reversi_WinRT.Model;
using Reversi_WinRT.Persistence;
using Reversi_WinRT.ViewModel;
using Reversi_WinRT.View;

using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Foundation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Reversi_WinRT
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        #region Constants

        /// <summary>
        /// Array of the allowed table sizes. It is readonly.
        /// </summary>
        private readonly Int32[] _supportedGameTableSizesArray = new Int32[] { 10, 20, 30 };
        /// <summary>
        /// The default table size. It is readonly.
        /// </summary>
        private readonly Int32 _tableSizeDefaultSetting = 10;

        #endregion

        #region Fields

        private ReversiGameModel _model;
        private ReversiViewModel _viewModel;
        private FileOpenPicker _fileOpenPicker;
        private FileSavePicker _fileSavePicker;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            this.Suspending += OnSuspending;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Alkalmazás állapot betöltése.
        /// </summary>
        private async void LoadAppState()
        {
            // a betöltést az alkalmazás könyvtárából végezzük
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("lastgame.reversi");

            if (file == null) // amennyiben nem volt korábbi mentés
            {
                return;
            }

            try
            {
                await _model.LoadGame(file.Path);
            }
            catch { }
            // a hibát nem kell jeleznünk, csak elveszítjük a korábbi állapotot
        }

        /// <summary>
        /// Alkalmazás állapot mentése.
        /// </summary>
        private async void SaveAppState()
        {
            // a játékállást az alkalmazás könyvtárjába végezzük
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("lastgame.reversi", CreationCollisionOption.ReplaceExisting);

            try
            {
                await _model.SaveGame(file.Path);
            }
            catch { }
            // hiba esetén ne tegyünk semmit, mert már folyamatban van a felfüggesztés
        }

        #endregion

        #region App event handling

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _model = new ReversiGameModel(new ReversiFileDataAccess(_supportedGameTableSizesArray), _tableSizeDefaultSetting); // létrehozzuk a modellt perzisztanciával
            _model.SetGameEnded += new EventHandler<ReversiSetGameEndedEventArgs>(Model_SetGameEnded);

            _viewModel = new ReversiViewModel(_model); // létrehozzuk a nézetmodellt
            _viewModel.NewGame += ViewModel_NewGame;
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame); // kezeljük a nézetmodell eseményeit
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.ReadRules += new EventHandler(ViewModel_ReadRules);
            _viewModel.ReadAbout += new EventHandler(ViewModel_ReadAbout);
            // a kilépést most nem kell

            Frame rootFrame = new Frame(); // létrehozzuk az ablakkeretet
            rootFrame.DataContext = _viewModel; // erre állítjuk be a nézetmodellt

            Window.Current.Content = rootFrame; // a keretet állítjuk be tartalomnak

            // amennyiben nem a felhasználó zárta be az alkalmazást, be kell töltenünk a korábbi állapotot
            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                LoadAppState();
            }

            if (!rootFrame.Navigate(typeof(MainPage), args.Arguments)) // beállítjuk a nyitóképernyőt
            {
                throw new Exception("Failed to create initial page");
            }

            // amennyiben a rendszer fel akarja venni a saját parancsainkat
            SettingsPane.GetForCurrentView().CommandsRequested += new TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs>(SettingsPane_CommandsRequested);

            Window.Current.Activate(); // aktiváljuk az ablakot
        }

        /*
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        */

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

            SaveAppState(); // elmentjük az alkalmazás állapotát

            deferral.Complete();
        }


        /// <summary>
        /// Beállítások megtekintésének eseménykezelője.
        /// </summary>
        private void SettingsPane_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs e)
        {
            // feveszünk egy parancsot a beállításokhoz, amely megjeleníti az ablakot
            e.Request.ApplicationCommands.Add(new SettingsCommand("Settings", "Game", command =>
            {
                ReversiSettingsFlyout settingsFlyout = new ReversiSettingsFlyout();
                settingsFlyout.DataContext = _viewModel;
                settingsFlyout.Show(); // beállítások megjelenítése
            }));
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelése.
        /// </summary>
        private async void Model_SetGameEnded(object sender, ReversiSetGameEndedEventArgs e)
        {
            _model.Pause();

            MessageDialog dialog;

            if (e.Player1Points > e.Player2Points)
            {
                dialog = new MessageDialog("Player 1 won." + Environment.NewLine + "Player 1 points: " + e.Player1Points.ToString() + ", player 2 points: " + e.Player2Points.ToString() + ".");
            }
            else if (e.Player1Points < e.Player2Points)
            {
                dialog = new MessageDialog("Player 2 won." + Environment.NewLine + "Player 1 points: " + e.Player1Points.ToString() + ", player 2 points: " + e.Player2Points.ToString() + ".");
            }
            else
            {
                dialog = new MessageDialog("It is a tie." + Environment.NewLine + "Player 1 points: " + e.Player1Points.ToString() + ", player 2 points: " + e.Player2Points.ToString() + ".");
            }

            await dialog.ShowAsync();

            _model.Unpause();
        }

        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            _model.NewGame();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGame(object sender, EventArgs e)
        {
            _model.Pause();

            // fájl kiválasztó lap létrehozása
            if (_fileOpenPicker == null)
            {
                _fileOpenPicker = new FileOpenPicker();
                _fileOpenPicker.ViewMode = PickerViewMode.List;
                _fileOpenPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary; // dokumentumok könyvtárban
                _fileOpenPicker.FileTypeFilter.Add(".reversi"); // txt kiterjesztésű fájlok megjelenítése
            }

            // kiválasztjuk a fájlt
            StorageFile file = await _fileOpenPicker.PickSingleFileAsync();

            // nyithatunk új nézetet
            if (file != null)
            {
                MessageDialog dialog = null;

                try
                {
                    await _model.LoadGame(file.Path);
                }
                catch (ReversiDataException ex)
                {
                    dialog = new MessageDialog(ex.ReversiMessage + System.Environment.NewLine + System.Environment.NewLine + ex.ReversiInfo);
                }
                catch (Exception ex)
                {
                    dialog = new MessageDialog(ex.Message);
                }

                // ha nem sikerült a mentés
                if (dialog != null)
                {
                    await dialog.ShowAsync();
                    _viewModel.Saved = false;
                }
            }

            _model.Unpause();
        }
        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            _model.Pause();

            // fájl kiválasztó lap létrehozása
            if (_fileSavePicker == null)
            {
                _fileSavePicker = new FileSavePicker();
                _fileSavePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                _fileSavePicker.FileTypeChoices.Add("Reversi table.", new List<String> { ".reversi" }); // mentés formátuma
            }

            // fájl kiválasztása
            StorageFile file = await _fileSavePicker.PickSaveFileAsync(); // a célfájl betöltése

            if (file != null)
            {
                MessageDialog dialog = null;

                try
                {
                    await _model.SaveGame(file.Path);
                }
                catch (ReversiDataException ex)
                {
                    dialog = new MessageDialog(ex.ReversiMessage + System.Environment.NewLine + System.Environment.NewLine + ex.ReversiInfo);
                }
                catch (Exception ex)
                {
                    dialog = new MessageDialog(ex.Message);
                }

                // ha nem sikerült a mentés
                if (dialog != null)
                {
                    await dialog.ShowAsync();
                }
            }

            _viewModel.Saved = true;

            _model.Unpause();
        }

        private async void ViewModel_ReadRules(object sender, EventArgs e)
        {
            _model.Pause();

            MessageDialog dialog = new MessageDialog("Always the white starts the game. If he can he chooses a put down location, only if he can not he passes. Then the black do the same then the white again, and so on ... ." + Environment.NewLine + "You have to straddle the enemy put downs to make a put down and to make them yours. You can do it in all directions. The game ends if no one can make a put down. The player with the more put downs win.");

            await dialog.ShowAsync();

            _model.Unpause();
        }

        private async void ViewModel_ReadAbout(object sender, EventArgs e)
        {
            _model.Pause();

            MessageDialog dialog = new MessageDialog("Created by Peskó Márton. It was a assignment at" + Environment.NewLine + "Eötvös Loránd University http://www.elte.hu/" + Environment.NewLine + "Faculty of Informatics http://www.inf.elte.hu/english/Lapok/default.aspx" + Environment.NewLine + "Software Information Technologist BsC.major" + Environment.NewLine + "Software Development Specialisation" + Environment.NewLine + "in the Eseményvezérelt Alkalmazások Fejlesztése 2.exercise." + Environment.NewLine + "This program was created with Visual Studio Enterprise 2015 using a C# programing language with Microsoft .NET 4.6 framework. It is a Windows Runtime (WinRT) application. You can download the source code from https://github.com/AidanPryde/Reversi-exercise-EVA-2 webpage, for this program and in its other forms like as Windows Froms Application (WFA) or as Windows Presentation Foundation (WPF) impementations.");

            await dialog.ShowAsync();

            _model.Unpause();
        }

        #endregion
    }
}
