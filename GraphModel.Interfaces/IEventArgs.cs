using System;

namespace GraphModel
{

    /// <summary>
    /// Graph Edge Changed Event Args
    /// </summary>
    public interface IEdgeChangedEventArgs
    {
        /// <summary>
        /// The first vertex index
        /// </summary>
        int FirstVertexIndex { get; }

        /// <summary>
        /// The second vertex index
        /// </summary>
        int SecondVertexIndex { get; }

        /// <summary>
        /// New Edge Value
        /// </summary>
        bool NewEdgeValue { get; }
    }


    /// <summary>
    /// Graph All Edges Setted Event Args
    /// </summary>
    public interface IAllEdgesSettedEventArgs
    {
        /// <summary>
        /// New Edge Value
        /// </summary>
        bool NewEdgeValue { get; }
    }

}