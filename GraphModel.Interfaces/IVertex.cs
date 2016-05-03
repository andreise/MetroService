namespace GraphModel
{

    /// <summary>
    /// Simple Graph Vertex
    /// </summary>
    /// <remarks>
    /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
    /// </remarks>
    public interface IVertex
    {
        /// <summary>
        /// Vertex Index
        /// </summary>
        int Index { get; }

        /// <summary>
        /// The Vertex Degree (Valency, Order)
        /// </summary>
        /// <remarks>
        /// The number of graph edges meeting at a given node in a graph is called the order of that graph vertex
        /// </remarks>
        int Degree { get; }
    }

}
