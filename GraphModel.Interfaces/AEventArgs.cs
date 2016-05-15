using System;

namespace GraphModel
{

    /// <summary>
    /// Graph Edge Changed Event Args
    /// </summary>
    public abstract class AEdgeChangedEventArgs : EventArgs
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
        /// Checks argument values are correct
        /// </summary>
        /// <param name="firstVertexIndex">The first vertex index</param>
        /// <param name="secondVertexIndex">The second vertex index</param>
        /// <param name="newEdgeValue">The new edge value</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if argument values are in an invalid state</exception>
        protected abstract void CheckArgs(int firstVertexIndex, int secondVertexIndex, bool newEdgeValue);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="firstVertexIndex">The first vertex index</param>
        /// <param name="secondVertexIndex">The second vertex index</param>
        /// <param name="newEdgeValue">The new edge value</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if argument values are in an invalid state</exception>
        public AEdgeChangedEventArgs(int firstVertexIndex, int secondVertexIndex, bool newEdgeValue)
        {
            this.CheckArgs(firstVertexIndex, secondVertexIndex, newEdgeValue);

            this.FirstVertexIndex = firstVertexIndex;
            this.SecondVertexIndex = secondVertexIndex;
            this.NewEdgeValue = newEdgeValue;
        }

    }


    /// <summary>
    /// Graph All Edges Setted Event Args
    /// </summary>
    public abstract class AAllEdgesSettedEventArgs : EventArgs
    {

        /// <summary>
        /// New Edge Value
        /// </summary>
        public bool NewEdgeValue { get; }

        /// <summary>
        /// Checks argument values are correct
        /// </summary>
        /// <param name="newEdgeValue">The new edge value</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if argument values are in an invalid state</exception>
        protected abstract void CheckArgs(bool newEdgeValue);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newEdgeValue">The new edge value</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if argument values are in an invalid state</exception>
        public AAllEdgesSettedEventArgs(bool newEdgeValue)
        {
            this.CheckArgs(newEdgeValue);

            this.NewEdgeValue = newEdgeValue;
        }

    }

}