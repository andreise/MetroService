using System;

namespace GraphService.Model
{

    /// <summary>
    /// Simple Graph Service
    /// </summary>
    /// <remarks>
    /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
    /// </remarks>
    public interface IGraphService
    {
        /// <summary>
        /// Checks the graph is a null graph
        /// </summary>
        /// <param name="inputXml">The graph description in xml format</param>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        bool IsNull(string inputXml);

        /// <summary>
        /// Checks the graph is a singleton graph
        /// </summary>
        /// <param name="inputXml">The graph description in the xml format</param>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        bool IsSingleton(string inputXml);

        /// <summary>
        /// Checks the graph is an empty, a null or a singleton graph
        /// </summary>
        /// <param name="inputXml">The graph description in the xml format</param>
        /// <returns>Returns True if the graph is an empty, a null or a singleton graph, otherwise returns False</returns>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        bool IsEmpty(string inputXml);

        /// <summary>
        /// Checks the graph is a complete, not a null and not a singleton graph
        /// </summary>
        /// <param name="inputXml">The graph description in the xml format</param>
        /// <returns>Returns True if the graph is a complete, not a null and not a singleton graph, otherwise returns False</returns>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        bool IsComplete(string inputXml);

        /// <summary>
        /// Calculates the graph connectivity markers
        /// </summary>
        /// <param name="inputXml">The graph description in the xml format</param>
        /// <param name="startVertexIndex">The start vertex index</param>
        /// <returns>Returns the graph connectivity markers</returns>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        /// <exception cref="InvalidOperationException">Throws is the graph is a null graph</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the start vertex index is less than zero or equals to or greater than the graph size</exception>
        bool[] GetConnectivityMarkers(string inputXml, int startVertexIndex);

        /// <summary>
        /// Checks the graph is a connected graph
        /// </summary>
        /// <param name="inputXml">The graph description in the xml format</param>
        /// <returns>Returns True if the graph is a connected graph, otherwise, returns False</returns>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        /// <exception cref="InvalidOperationException">Throws is the graph is a null graph</exception>
        bool IsConnected(string inputXml);

        /// <summary>
        /// Calculates the graph spanning forest
        /// </summary>
        /// <param name="inputXml">The graph description in the xml format</param>
        /// <param name="startVertexIndex">The start vertex index</param>
        /// <returns>Returns the graph spanning forest in the xml format</returns>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        /// <exception cref="InvalidOperationException">Throws if the graph is a null graph</exception>
        string GetSpanningForest(string inputXml, int startVertexIndex);

        /// <summary>
        /// Calculates a vertex deleting sequence for the connected graph
        /// </summary>
        /// <param name="inputXml">The graph description in the xml format</param>
        /// <returns>Returns a vertex deleting sequence for the connected graph</returns>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        /// <exception cref="InvalidOperationException">
        /// Throws if the graph is a null or is not a connected graph, or the graph is in an unexpected invalid state
        /// </exception>
        int[] GetVertexDeletingSequenceForConnectedGraph(string inputXml);
    }

}
