using System;
using static System.FormattableString;

namespace GraphModel
{

    /// <summary>
    /// Graph Edge Changed Event Args
    /// </summary>
    public class EdgeChangedEventArgs : EventArgs, IEdgeChangedEventArgs
    {
        /// <summary>
        /// The first vertex index
        /// </summary>
        public int FirstVertexIndex { get; }

        /// <summary>
        /// The second vertex index
        /// </summary>
        public int SecondVertexIndex { get; }

        /// <summary>
        /// New Edge Value
        /// </summary>
        public bool NewEdgeValue { get; }

        /// <summary>
        /// Checks vertex indexes are correct
        /// </summary>
        /// <param name="firstVertexIndex">The first vertex index</param>
        /// <param name="secondVertexIndex">The second vertex index</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the the first vertex index and the second vertex is equal to or less than zero</exception>
        protected virtual void CheckVertexIndexes(int firstVertexIndex, int secondVertexIndex)
        {
            Action<string, int> checkIndex = (indexName, index) =>
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(indexName, "The vertex index must be equal to or greater than zero.");
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
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if the the first vertex index and the second vertex is equal to or less than zero,
        /// or if the the first vertex index and the second vertex index are equal
        /// </exception>
        protected internal EdgeChangedEventArgs(int firstVertexIndex, int secondVertexIndex, bool newEdgeValue) : base()
        {
            this.CheckVertexIndexes(firstVertexIndex, secondVertexIndex);

            this.FirstVertexIndex = firstVertexIndex;
            this.SecondVertexIndex = secondVertexIndex;
            this.NewEdgeValue = newEdgeValue;
        }
    }

    /// <summary>
    /// Graph All Edges Setted Event Args
    /// </summary>
    public class AllEdgesSettedEventArgs : EventArgs, IAllEdgesSettedEventArgs
    {
        /// <summary>
        /// New Edge Value
        /// </summary>
        public bool NewEdgeValue { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newEdgeValue">The new edge value</param>
        protected internal AllEdgesSettedEventArgs(bool newEdgeValue) : base()
        {
            this.NewEdgeValue = newEdgeValue;
        }

    }

}
