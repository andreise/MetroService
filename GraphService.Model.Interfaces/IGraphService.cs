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
        IGraph LoadGraphFromXml(string inputXml);

        string SaveGraphToXml(IGraph graph);
    }

}
