
using Reversi_WinRT.Model;
using Reversi_WinRT.Persistence;

using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Reversi_WinRT_Test
{
    [TestClass]
    public class ReversiGameModelTest
    {
        //AssertEx _assertEx;

        private readonly String documentsFolder = "C:\\Users\\Márton\\Documents\\Resources";

        private readonly Int32[] _supportedGameTableSizesArray = new Int32[] { 4, 6, 8, 10, 20, 30 };
        private readonly Int32 _tableSizeDefaultSetting = 10;

        private ReversiFileDataAccess _dataAccess;

        private ReversiGameModel _model;

        /// <summary>
        /// True if we just check if the events was invoked.
        /// If it is False we still check if it was invoked, but do other stuff too.
        /// </summary>
        private Boolean _simpleEvents = true;

        private Boolean _eventSetGameEndedInvoked;
        private Boolean _eventUpdatePlayerTimeInvoked;
        private Boolean _eventUpdateTableInvoked;

        /// <summary>
        /// We brute force throuth all the possible game.
        /// [0,0] -> Step counter (x).
        /// [x,0] -> Not finished possible put downs count at the x. step.
        /// </summary>
        private Int32[,] _remainingSteps;
        private Int32 _maximumPossiblePutDownsSize;
        private Int32 _maximumReversedPutDownsSize;
        private Int32 _possibleGameCount;
        /// <summary>
        /// [0] -> Player 1 wins count.
        /// [1] -> Player 2 wins count.
        /// [2] -> Draws count.
        /// </summary>
        private Int32[] _possibleResults;
        /// <summary>
        /// The nuber of passes from the first put down to the last in one game play.
        /// </summary>
        private Int32 _currentPassingCount;
        private Int32 _maximumPassingCount;

        /// <summary>
        /// The checnged data sent from model.
        /// </summary>
        private ReversiUpdateTableEventArgs _lastUpdateArg;

        [TestMethod]
        public void ReversiGameModelNewGameInitializeOddTest()
        {
            Int32[] wrongGameTableSizesArray = new Int32[] { 10, 15, 20 }; // <- 15

            _dataAccess = new ReversiFileDataAccess(wrongGameTableSizesArray);

            Assert.ThrowsException<ReversiModelException>(() => _model = new ReversiGameModel(_dataAccess, _tableSizeDefaultSetting));
        }

        [TestMethod]
        public void ReversiGameModelNewGameInitializeTooSmallTest()
        {
            Int32[] wrongGameTableSizesArray = new Int32[] { 10, 2, 20 }; // <- 2

            _dataAccess = new ReversiFileDataAccess(wrongGameTableSizesArray);
            Assert.ThrowsException<ReversiModelException>(() => _model = new ReversiGameModel(_dataAccess, _tableSizeDefaultSetting));
        }

        [TestInitialize]
        public void Initialize()
        {
            _dataAccess = new ReversiFileDataAccess(_supportedGameTableSizesArray);

            _model = new ReversiGameModel(_dataAccess, _tableSizeDefaultSetting);

            _model.UpdateTable += new EventHandler<ReversiUpdateTableEventArgs>(model_UpdateTable);
            _model.SetGameEnded += new EventHandler<ReversiSetGameEndedEventArgs>(model_SetGameEnded);
            _model.UpdatePlayerTime += new EventHandler<ReversiUpdatePlayerTimeEventArgs>(model_UpdatePlayerTime);
        }

        [TestMethod]
        public async void ReversiGameModelBeforeNewGameTest()
        {
            _eventSetGameEndedInvoked = false;
            _eventUpdatePlayerTimeInvoked = false;
            _eventUpdateTableInvoked = false;

            _model.Pass();
            _model.Pause();
            _model.Unpause();
            _model.PutDown(0, 0);

            Assert.IsFalse(_eventSetGameEndedInvoked);
            Assert.IsFalse(_eventUpdatePlayerTimeInvoked);
            Assert.IsFalse(_eventUpdateTableInvoked);

            _model.NewGame();

            await WaitMethod(2);

            Assert.IsTrue(_eventUpdatePlayerTimeInvoked);

            _model.Pause();
            _eventUpdatePlayerTimeInvoked = false;

            await WaitMethod(2);

            Assert.IsFalse(_eventUpdatePlayerTimeInvoked);

            _model.Unpause();
            _eventUpdatePlayerTimeInvoked = false;

            await WaitMethod(2);

            Assert.IsTrue(_eventUpdatePlayerTimeInvoked);
        }

        [TestMethod]
        public async Task ReversiGameModelBeforeNewGameSaveTest()
        {
            await _model.SaveGame("");
        }

        [TestMethod]
        public void ReversiGameModelNewGameSizeTest()
        {
            Assert.AreEqual(10, _model.TableSizeSetting);
            Assert.AreEqual(0, _model.ActiveTableSize);

            _model.TableSizeSetting = 20;

            Assert.AreEqual(20, _model.TableSizeSetting);
            Assert.AreEqual(0, _model.ActiveTableSize);

            _model.TableSizeSetting = 15;

            Assert.AreEqual(20, _model.TableSizeSetting);
            Assert.AreEqual(0, _model.ActiveTableSize);

            _simpleEvents = true;
            _model.NewGame();

            Assert.AreEqual(20, _model.TableSizeSetting);
            Assert.AreEqual(20, _model.ActiveTableSize);

            _model.TableSizeSetting = 10;

            Assert.AreEqual(10, _model.TableSizeSetting);
            Assert.AreEqual(20, _model.ActiveTableSize);

            _model.TableSizeSetting = 15;

            Assert.AreEqual(10, _model.TableSizeSetting);
            Assert.AreEqual(20, _model.ActiveTableSize);
        }

        [TestMethod]
        public async Task ReversiGameModelNewGameLoadTestOk0Step()
        {
            _simpleEvents = true;
            await _model.LoadGame(documentsFolder + "\\ok 0 step.reversi");
        }
        /*
        [TestMethod]
        public void ReversiGameModelNewGameLoadEmptyFileTest()
        {
            // Zero or one line file.
            _simpleEvents = true;
            Assert.ThrowsException<FormatException>(async () => await _model.LoadGame(documentsFolder + "\\empty file.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadLessPutDownThenPutDownSizeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<NullReferenceException>(async () => await _model.LoadGame(documentsFolder + "\\less put down then put down size.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadNoPlayer2TimePutDownSizeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<IndexOutOfRangeException>(async () => await _model.LoadGame(documentsFolder + "\\no player 2 time and put down size.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadNoPlayersTimePutDownSizeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<IndexOutOfRangeException>(async () => await _model.LoadGame(documentsFolder + "\\no players time and put down size.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadNoPutDownSizeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<IndexOutOfRangeException>(async () => await _model.LoadGame(documentsFolder + "\\no put down size.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongOddPutDownSizeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong odd put down size.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongePlayer1TimeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong player 1 time.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongePlayer2TimeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong player 2 time.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongePlayersTimeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong players time.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStepMinus1Instead3Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame("../../../Resources/wrong step -1 instead 3 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStepMinus1Instead6Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step -1 instead 6 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStepMinus1InsteadPassTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step -1 instead pass.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep0Instead3Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 0 instead 3 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep0Instead6Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 0 instead 6 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep1Instead3Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 1 instead 3 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep1Instead6Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 1 instead 6 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep1InsteadPassTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 1 instead pass.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep3Instead6Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 3 instead 6 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep3InsteadPassTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 3 instead pass.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep5Instead3Or4Test()
        {
            _simpleEvents = true;
            Debug.WriteLine("../../../Resources/wrong step 5 instead 3 or 4.reversi");
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 5 instead 3 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep5Instead6Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 5 instead 6 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep6Instead3Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 6 instead 3 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStep6InsteadPassTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step 6 instead pass.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStepPassInstead3Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step pass instead 3 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeStepPassInstead6Or4Test()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong step pass instead 6 or 4.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeTableSizeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong table size.reversi"));
        }

        [TestMethod]
        public void ReversiGameModelNewGameLoadWrongeTooBigPutDownSizeTest()
        {
            _simpleEvents = true;
            Assert.ThrowsException<ReversiDataException>(async () => await _model.LoadGame(documentsFolder + "\\wrong too big put down size.reversi"));
        }
        */
        [TestMethod]
        public void ReversiGameModelAllPossibleSenario()
        {
            //TODO: Maybe it can be faster with a graph, where the vertexes would be the states of the games and the edges would be the put downs.
            // That way we could prevent to replay the same game plays, but we would have to compare the game states.
            // The same edges with different orders may create different game states.
            // OR we can make a step back functionality somehow.

            _simpleEvents = false;

            _model.TableSizeSetting = 4;

            // Init the array of doom and the other stuffs.
            _remainingSteps = new Int32[(_model.TableSizeSetting * _model.TableSizeSetting * 2) + 1, (_model.TableSizeSetting * _model.TableSizeSetting * 2) + 1];

            _remainingSteps[0, 0] = 1;
            _remainingSteps[1, 0] = 8;

            Int32 halfSize = _model.TableSizeSetting / 2;

            _remainingSteps[1, 1] = halfSize;
            _remainingSteps[1, 2] = halfSize - 2;
            _remainingSteps[1, 3] = halfSize + 1;
            _remainingSteps[1, 4] = halfSize - 1;
            _remainingSteps[1, 5] = halfSize - 1;
            _remainingSteps[1, 6] = halfSize + 1;
            _remainingSteps[1, 7] = halfSize - 2;
            _remainingSteps[1, 8] = halfSize;

            _maximumPossiblePutDownsSize = 0;
            _maximumReversedPutDownsSize = 0;
            _possibleGameCount = 0;
            _maximumPassingCount = 0;
            _possibleResults = new Int32[] { 0, 0, 0 };

            // Go for it. See you in 100 years.
            while (_remainingSteps[0, 0] != 0)
            {
                _model.NewGame();
                //Debug.Print(_possibleGameCount.ToString());
            }


            Debug.WriteLine(_maximumPossiblePutDownsSize);
            Debug.WriteLine(_maximumReversedPutDownsSize);
            Debug.WriteLine(_possibleGameCount);
            Debug.WriteLine(_maximumPassingCount);
            Debug.WriteLine(_possibleResults[0]);
            Debug.WriteLine(_possibleResults[1]);
            Debug.WriteLine(_possibleResults[2]);


            /*
            Assert.AreEqual(0, _maximumPossiblePutDownsSize); // 9
            Assert.AreEqual(0, _maximumReversedPutDownsSize); // 6
            Assert.AreEqual(0, _possibleGameCount); // 60060
            Assert.AreEqual(0, _maximumPassingCount); // 4
            Assert.AreEqual(0, _possibleResults[0]); // 24632
            Assert.AreEqual(0, _possibleResults[1]); // 30116
            Assert.AreEqual(0, _possibleResults[2]); // 5312
            */
        }

        /// <summary>
        /// We update the _remainingSteps array in a brute mode.
        /// If we have no possible put downs in the row, we create them and step if we can.
        /// If we have at least one possible put down, we make that and step.
        /// </summary>
        private void updateStepsArray()
        {
            if (_lastUpdateArg.UpdatedFieldsCount != 0) // Not a new game.
            {
                ++(_remainingSteps[0, 0]); // Next step.

                if (_remainingSteps[_remainingSteps[0, 0], 0] == 0) // No step to replay.
                {
                    if (_lastUpdateArg.IsPassingTurnOn)
                    {
                        _remainingSteps[_remainingSteps[0, 0], 0] += 2;
                        _remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0] - 1] = -1;
                        _remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0]] = -1;


                        int possbilePutDownsSize = 0;
                        int reversedPutDownsSize = 0;
                        for (Int32 i = 0; i < _lastUpdateArg.UpdatedFieldsCount; i += 3)
                        {
                            if (_lastUpdateArg.UpdatedFieldsDatas[i + 2] != 1 && _lastUpdateArg.UpdatedFieldsDatas[i + 2] != -1)
                            {
                                possbilePutDownsSize += 3;
                            }
                            else
                            {
                                reversedPutDownsSize += 3;
                            }
                        }

                        if (_maximumPossiblePutDownsSize < possbilePutDownsSize)
                        {
                            _maximumPossiblePutDownsSize = possbilePutDownsSize;
                        }

                        if (_maximumReversedPutDownsSize < reversedPutDownsSize)
                        {
                            _maximumReversedPutDownsSize = reversedPutDownsSize;
                        }

                        ++_currentPassingCount;

                        if (_maximumPassingCount < _currentPassingCount)
                        {
                            _maximumPassingCount = _currentPassingCount;
                        }

                        _model.Pass();
                    }
                    else
                    {
                        Int32 checkFor = 3;
                        if (_lastUpdateArg.IsPlayer1TurnOn)
                        {
                            checkFor = 6;
                        }

                        int possbilePutDownsSize = 0;
                        int reversedPutDownsSize = 0;
                        for (Int32 i = 0; i < _lastUpdateArg.UpdatedFieldsCount; i += 3)
                        {
                            if (_lastUpdateArg.UpdatedFieldsDatas[i + 2] != 1 && _lastUpdateArg.UpdatedFieldsDatas[i + 2] != -1)
                            {
                                possbilePutDownsSize += 3;

                                if (_lastUpdateArg.UpdatedFieldsDatas[i + 2] == checkFor || _lastUpdateArg.UpdatedFieldsDatas[i + 2] == 4)
                                {
                                    // The new possible steps.
                                    _remainingSteps[_remainingSteps[0, 0], 0] += 2;
                                    _remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0] - 1] = _lastUpdateArg.UpdatedFieldsDatas[i];
                                    _remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0]] = _lastUpdateArg.UpdatedFieldsDatas[i + 1];
                                }
                            }
                            else
                            {
                                reversedPutDownsSize += 3;
                            }
                        }

                        if (_maximumPossiblePutDownsSize < possbilePutDownsSize)
                        {
                            _maximumPossiblePutDownsSize = possbilePutDownsSize;
                        }

                        if (_maximumReversedPutDownsSize < reversedPutDownsSize)
                        {
                            _maximumReversedPutDownsSize = reversedPutDownsSize;
                        }

                        if (_remainingSteps[_remainingSteps[0, 0], 0] != 0)
                        {
                            // One of the possible step, we just added.
                            _model.PutDown(_remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0] - 1], _remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0]]);
                        }
                        else
                        {
                            ; // Last put down. Game ended.
                        }
                    }
                }
                else
                {
                    // We replay the steps we took before. (We can not step back, thats why we do this.)
                    if (_remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0] - 1] == -1 && _remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0]] == -1)
                    {
                        _model.Pass();
                    }
                    else
                    {
                        _model.PutDown(_remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0] - 1], _remainingSteps[_remainingSteps[0, 0], _remainingSteps[_remainingSteps[0, 0], 0]]);
                    }
                }
            }
            else
            {
                _model.Unpause(); // Little hack to make this work.
                // The first step after a newGame().
                _model.PutDown(_remainingSteps[1, _remainingSteps[1, 0] - 1], _remainingSteps[1, _remainingSteps[1, 0]]);
            }
        }

        /// <summary>
        /// We finished a game. We delete the last put downs backwords, then if it not ended
        /// (_remainingSteps[0,0] != 0) we set _remainingSteps[0,0] to 1.
        /// </summary>
        /// <param name="e"></param>
        private void removeStep(ReversiSetGameEndedEventArgs e)
        {
            if (e.Player1Points > e.Player2Points)
            {
                _possibleResults[0] += 1;
            }
            else if (e.Player1Points < e.Player2Points)
            {
                _possibleResults[1] += 1;
            }
            else
            {
                _possibleResults[2] += 1;
            }

            while (_remainingSteps[_remainingSteps[0, 0], 0] == 0)
            {
                --(_remainingSteps[0, 0]);
                if (_remainingSteps[_remainingSteps[0, 0], 0] != 0)
                {
                    _remainingSteps[_remainingSteps[0, 0], 0] -= 2;
                }

                if (_remainingSteps[0, 0] == 0)
                {
                    break;
                }
            }

            if (_remainingSteps[0, 0] != 0)
            {
                _remainingSteps[0, 0] = 1;
                _currentPassingCount = 0;
            }

            ++_possibleGameCount;
        }

        private void model_UpdateTable(Object sender, ReversiUpdateTableEventArgs e)
        {
            _eventUpdateTableInvoked = true;

            if (!_simpleEvents)
            {
                _lastUpdateArg = e;
                updateStepsArray();
            }
        }

        private void model_SetGameEnded(Object sender, ReversiSetGameEndedEventArgs e)
        {
            _eventSetGameEndedInvoked = true;

            if (!_simpleEvents)
            {
                removeStep(e);
            }
        }

        private void model_UpdatePlayerTime(Object sender, ReversiUpdatePlayerTimeEventArgs e)
        {
            _eventUpdatePlayerTimeInvoked = true;
        }

        /// <summary>
        /// To delay without the thread being frozen.
        /// </summary>
        /// <param name="time">The amount of time in seconds we want to delay.</param>
        /// <returns>Nothing.</returns>
        async Task WaitMethod(Int32 time)
        {
            await Task.Delay((Int32)TimeSpan.FromSeconds(time).TotalMilliseconds);
        }
    }
}
