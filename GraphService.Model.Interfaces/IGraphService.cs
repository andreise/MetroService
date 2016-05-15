using GraphModel;

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
        /// Loads a graph instance from the XML format string
        /// </summary>
        /// <param name="inputXml"></param>
        /// <returns>A new graph instance</returns>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        /// <remarks>Supports directed or loop-graphs if the IGraph implements a directed or a loop-graph</remarks>
        IGraph LoadGraphFromXml(string inputXml);

        /// <summary>
        /// Saves the graph to a XML format string 
        /// </summary>
        /// <param name="graph">The graph</param>
        /// <exception cref="ArgumentNullException">Throws if the graph is null</exception>
        /// <returns>A new XML format string which descriptions the graph</returns>
        /// <remarks>Supports directed or loop-graphs if the IGraph implements a directed or a loop-graph</remarks>
        string SaveGraphToXml(IGraph graph);
    }

}
