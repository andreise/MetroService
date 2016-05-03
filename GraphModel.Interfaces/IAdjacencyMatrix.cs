using System;

namespace GraphModel
{

    /// <summary>
    /// Simple Graph Adjacency Matrix (Optionally Directed or Loop-graph)
    /// </summary>
    /// <remarks>
    /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
    /// </remarks>
    public interface IAdjacencyMatrix
    {
        ///// <summary>
        ///// The Matrix Size
        ///// </summary>
        //int Size { get; }

        /// <summary>
        /// Is the edge between vertices presents
        /// </summary>
        /// <param name="row">The first vertex index</param>
        /// <param name="column">The second vertex index</param>
        /// <returns>Returns True if the edge between vertexes presents, otherwise returns False</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if the graph is a null graph,
        /// or if the row or the column is less than zero or equal to or greater than the graph size,
        /// or if the graph implements no-loop graph and the row and the column are equals and associated value is True
        /// </exception>
        bool this[int row, int column] { get; set; }
    }

}
