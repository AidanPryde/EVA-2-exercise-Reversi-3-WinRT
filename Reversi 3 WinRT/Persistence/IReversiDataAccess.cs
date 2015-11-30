
using System;
using System.Threading.Tasks;

namespace Reversi_WinRT.Persistence
{
    /// <summary>
    /// Reversi file handler interface.
    /// </summary>
    public interface IReversiDataAccess
    {
        Int32[] SupportedGameTableSizesArray
        {
            get;
        }

        /// <summary>
        /// Loading file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>All the data to recover the game state. The game table size, the put downs and the players game times.</returns>
        Task<ReversiGameDescriptiveData> Load(String path);

        /// <summary>
        /// Saving file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="data">All the data to recover the game state. The game table size, the put downs and the players game times.</param>
        Task Save(String path, ReversiGameDescriptiveData data);
    }
}
