using System;
using System.Text;
using System.Xml;
using static System.FormattableString;
using GraphModel;

namespace GraphService.Model
{

    internal sealed class GraphXmlFormatException : Exception
    {
        public GraphXmlFormatException(string message) : base(message) { }
    }

    /// <summary>
    /// Simple Graph Service
    /// </summary>
    /// <remarks>
    /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
    /// </remarks>
    public class GraphService : IGraphService
    {

        /// <summary>
        /// Loads a graph instance from the XML format string
        /// </summary>
        /// <param name="inputXml"></param>
        /// <returns>A new graph instance</returns>
        /// <exception cref="ArgumentNullException">Throws if the inputXml is null</exception>
        /// <exception cref="ArgumentException">Throws if the inputXml format is invalid</exception>
        /// <remarks>Supports directed or loop-graphs if the IGraph implements a directed or a loop-graph</remarks>
        public virtual IGraph LoadGraphFromXml(string inputXml)
        {
            if ((object)inputXml == null)
                throw new ArgumentNullException(nameof(inputXml));

            Func<string, string> getInvalidInputFormatMessage = description => Invariant($"The input xml is in the invalid format: {description}.");

            try
            {
                XmlDocument inputXmlDoc = new XmlDocument();
                inputXmlDoc.LoadXml(inputXml);

                XmlNode graphSizeNode = inputXmlDoc.SelectSingleNode("/Graph/Size/text()");
                if ((object)graphSizeNode == null)
                    throw new GraphXmlFormatException(getInvalidInputFormatMessage("the graph size element not found"));

                int graphSize = XmlConvert.ToInt32(graphSizeNode.Value);
                IGraph graph = GraphFactory.CreateSimpleGraph(graphSize);

                int maxEdgeCount;
                if (graph.IsDirected && graph.IsLoopGraph)
                    maxEdgeCount = graph.Size * graph.Size;
                else if (graph.IsDirected)
                    maxEdgeCount = graph.Size * (graph.Size - 1);
                else if (graph.IsLoopGraph)
                    maxEdgeCount = (graph.Size * (graph.Size - 1)) / 2 + graph.Size;
                else
                    maxEdgeCount = (graph.Size * (graph.Size - 1)) / 2;

                XmlNodeList edgeNodes = inputXmlDoc.SelectNodes("/Graph/Edges/Edge");

                if (edgeNodes.Count > maxEdgeCount)
                    throw new ArgumentException(Invariant($"This graph cannot contains more than {maxEdgeCount} edges."), nameof(inputXml));

                foreach (XmlElement edgeElem in edgeNodes)
                {
                    Func<string, string> getNoVertexElemMessage = vertexName =>
                        getInvalidInputFormatMessage(Invariant($"the edge element contains no {vertexName} child element"));

                    Func<string, XmlElement> getVertexElement = vertexName =>
                    {
                        var vertexElem = edgeElem[vertexName];
                        if ((object)vertexElem == null)
                            throw new GraphXmlFormatException(getNoVertexElemMessage(vertexName));
                        return vertexElem;
                    };

                    XmlElement vertex1Elem = getVertexElement("Vertex1");
                    int vertex1 = XmlConvert.ToInt32(vertex1Elem.InnerText);
                    XmlElement vertex2Elem = getVertexElement("Vertex2");
                    int vertex2 = XmlConvert.ToInt32(vertex2Elem.InnerText);

                    graph.AdjacencyMatrix[vertex1, vertex2] = true;
                }

                return graph;
            }
            catch (Exception e) when (
                e is GraphXmlFormatException ||
                e is XmlException ||
                e is InvalidCastException ||
                e is FormatException ||
                e is OverflowException ||
                e is ArgumentException
            )
            {
                throw new ArgumentException(
                    getInvalidInputFormatMessage(Invariant($"{e.Message} ({e.GetType()})")),
                    nameof(inputXml),
                    e
                );
            }
        }

        /// <summary>
        /// Saves the graph to a XML format string 
        /// </summary>
        /// <param name="graph">The graph</param>
        /// <exception cref="ArgumentNullException">Throws if the graph is null</exception>
        /// <returns>A new XML format string which descriptions the graph</returns>
        /// <remarks>Supports directed or loop-graphs if the IGraph implements a directed or a loop-graph</remarks>
        public virtual string SaveGraphToXml(IGraph graph)
        {
            if ((object)graph == null)
                throw new ArgumentNullException("graph");

            StringBuilder builder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(builder, new XmlWriterSettings() { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Graph");
                writer.WriteStartElement("Edges");

                writer.WriteElementString("Size", XmlConvert.ToString(graph.Size));

                Action<int, int> processEdge = (row, column) =>
                {
                    if (!graph.AdjacencyMatrix[row, column])
                        return;

                    writer.WriteStartElement("Edge");
                    writer.WriteElementString("Vertex1", XmlConvert.ToString(row));
                    writer.WriteElementString("Vertex2", XmlConvert.ToString(column));
                    writer.WriteEndElement(); // Edge
                };

                if (graph.IsDirected)
                {
                    for (int row = 0; row < graph.Size; row++)
                        for (int column = 0; column < graph.Size; column++)
                            processEdge(row, column);
                }
                else if (graph.IsLoopGraph)
                {
                    for (int row = 0; row < graph.Size; row++)
                        for (int column = row; column < graph.Size; column++)
                            processEdge(row, column);
                }
                else
                {
                    for (int row = 0; row < graph.Size - 1; row++)
                        for (int column = row + 1; column < graph.Size; column++)
                            processEdge(row, column);
                }

                writer.WriteEndElement(); // Edges
                writer.WriteEndElement(); // Graph
                writer.WriteEndDocument();
            }

            return builder.ToString();
        }

    }
}
