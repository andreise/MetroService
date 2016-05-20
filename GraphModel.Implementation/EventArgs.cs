using System;

namespace GraphModel
{

    /// <summary>
    /// Graph Edge Changed Event Args
    /// </summary>
    internal class EdgeChangedImplEventArgs : EdgeChangedEventArgs
    {

        /// <summary>
        /// Checks argument values are correct
        /// </summary>
        /// <param name="firstVertexIndex">The first vertex index</param>
        /// <param name="secondVertexIndex">The second vertex index</param>
        /// <param name="newEdgeValue">The new edge value</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the first vertex index or the second vertex is equal to or less than zero</exception>
        protected override void CheckArgs(int firstVertexIndex, int secondVertexIndex, bool newEdgeValue)
        {
            Action<string, int> checkIndex = (indexName, index) =>
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(indexName, index, "The vertex index must be equal to or greater than zero.");
            };
            checkIndex(nameof(firstVertexIndex), firstVertexIndex);
            checkIndex(nameof(secondVertexIndex), secondVertexIndex);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="firstVertexIndex">The first vertex index</param>
        /// <param name="secondVertexIndex">The second vertex index</param>
        /// <param name="newEdgeValue">The new edge value</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the first vertex index or the second vertex is equal to or less than zero</exception>
        protected internal EdgeChangedImplEventArgs(int firstVertexIndex, int secondVertexIndex, bool newEdgeValue) : base(firstVertexIndex, secondVertexIndex, newEdgeValue)
        {
        }

    }

    /// <summary>
    /// Graph All Edges Setted Event Args
    /// </summary>
    internal class AllEdgesSettedImplEventArgs : AllEdgesSettedEventArgs
    {

        /// <summary>
        /// Checks argument values are correct
        /// </summary>
        /// <param name="newEdgeValue">The new edge value</param>
        protected override void CheckArgs(bool newEdgeValue)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newEdgeValue">The new edge value</param>
        protected internal AllEdgesSettedImplEventArgs(bool newEdgeValue) : base(newEdgeValue)
        {
        }

    }

}
