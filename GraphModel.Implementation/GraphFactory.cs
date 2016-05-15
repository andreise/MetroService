namespace GraphModel
{
 
    /// <summary>
    /// Graph Factory
    /// </summary>
    public static class GraphFactory
    {
        /// <summary>
        /// Creates a simple graph
        /// </summary>
        /// <param name="size">The graph size</param>
        /// <returns>Returns a new simple graph</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the graph size equals to or less than zero</exception>
        /// <remarks>
        /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
        /// </remarks>
        public static IGraph CreateSimpleGraph(int size)
        {
            return Graph.Create(size);
        }
    }

}
