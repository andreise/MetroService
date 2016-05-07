using System;
using System.Collections.Generic;

namespace GraphModel
{

    /// <summary>
    /// Simple Graph (Optionally Directed or Loop-graph)
    /// </summary>
    /// <remarks>
    /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
    /// </remarks>
    public interface IGraph<TEdgeChangedEventArgs, TAllEdgesSettedEventArgs>
        where TEdgeChangedEventArgs: EventArgs, IEdgeChangedEventArgs
        where TAllEdgesSettedEventArgs: EventArgs, IAllEdgesSettedEventArgs
    {
        /// <summary>
        /// The Graph Size
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Is the graph implementation a directed
        /// </summary>
        bool IsDirected { get; }

        /// <summary>
        /// Is the graph implementation a loop-graph
        /// </summary>
        bool IsLoopGraph { get; }

        /// <summary>
        /// Is the graph a null graph
        /// </summary>
        bool IsNull { get; }

        /// <summary>
        /// Is the graph a singleton graph
        /// </summary>
        bool IsSingleton { get; }

        /// <summary>
        /// Checks the graph is an empty, a null or a singleton graph
        /// </summary>
        /// <returns>Returns True if the graph is an empty, a null or a singleton graph, otherwise returns False</returns>
        bool IsEmpty();

        /// <summary>
        /// Checks the graph is a complete, not a null and not a singleton graph
        /// </summary>
        /// <returns>Returns True if the graph is a complete, not a null and not a singleton graph, otherwise returns False</returns>
        bool IsComplete();

        /// <summary>
        /// Calculates the graph connectivity markers
        /// </summary>
        /// <param name="startVertexIndex">The start vertex index</param>
        /// <returns>Returns the graph connectivity markers</returns>
        /// <exception cref="InvalidOperationException">Throws is the graph is a null graph</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the start vertex index is less than zero or equals to or greater than the graph size</exception>
        bool[] GetConnectivityMarkers(int startVertexIndex);

        /// <summary>
        /// Checks the graph is a connected graph
        /// </summary>
        /// <returns>Returns True if the graph is a connected graph, otherwise, returns False</returns>
        /// <exception cref="InvalidOperationException">Throws is the graph is a null graph</exception>
        bool IsConnected();

        /// <summary>
        /// Calculates the graph spanning forest
        /// </summary>
        /// <param name="startVertexIndex">The start vertex index</param>
        /// <returns>Returns the graph spanning forest</returns>
        /// <exception cref="InvalidOperationException">Throws if the graph is a null graph</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the start vertex index is less than zero or equals to or greater than the graph size</exception>
        IGraph<TEdgeChangedEventArgs, TAllEdgesSettedEventArgs> GetSpanningForest(int startVertexIndex);

        /// <summary>
        /// Calculates a vertex deleting sequence for the connected graph
        /// </summary>
        /// <param name="startVertexIndex">The start vertex index</param>
        /// <returns>Returns a vertex deleting sequence for the connected graph</returns>
        /// <exception cref="InvalidOperationException">
        /// Throws if the graph is a null or is not a connected graph, or the graph is in a some unexpected state
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if the start vertex index is less than zero or equals to or greater than the graph size
        /// </exception>
        int[] GetConnectedGraphVertexDeletingSequence(int startVertexIndex);

        /// <summary>
        /// The Adjacency Matrix
        /// </summary>
        IAdjacencyMatrix AdjacencyMatrix { get; }

        /// <summary>
        /// The Vertex List
        /// </summary>
        IReadOnlyList<IVertex> Vertices { get; }

        /// <summary>
        /// Does the empty graph if graph is not a null and is not a singleton graph,
        /// otherwise does nothing
        /// </summary>
        void DoEmpty();

        /// <summary>
        /// Does the complete graph if graph is not a null and is not a singleton graph,
        /// otherwise does nothing
        /// </summary>
        void DoComplete();

        /// <summary>
        /// Edge Changed Event
        /// </summary>
        event EventHandler<TEdgeChangedEventArgs> EdgeChanged;

        /// <summary>
        /// All Edges Setted Event
        /// </summary>
        event EventHandler<TAllEdgesSettedEventArgs> AllEdgesSetted;
    }

}
