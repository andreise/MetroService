#define DEBUG

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static System.FormattableString;

namespace GraphModel
{
    using IGraph = IGraph<EdgeChangedEventArgs, AllEdgesSettedEventArgs>;

    /// <summary>
    /// Simple Graph
    /// </summary>
    /// <remarks>
    /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
    /// </remarks>
    internal class Graph: IGraph
    {
        /// <summary>
        /// The Graph Size
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Is the graph implementation a directed.
        /// Returns False
        /// </summary>
        public bool IsDirected
        {
            get { return false; }
        }

        /// <summary>
        /// Is the graph implementation a loop-graph.
        /// Returns False
        /// </summary>
        public bool IsLoopGraph
        {
            get { return false; }
        }

        /// <summary>
        /// The Adjacency Matrix
        /// </summary>
        protected internal AdjacencyMatrix AdjacencyMatrix { get; }

        /// <summary>
        /// The Adjacency Matrix
        /// </summary>
        IAdjacencyMatrix IGraph.AdjacencyMatrix { get { return this.AdjacencyMatrix; } }

        /// <summary>
        /// The Vertex List
        /// </summary>
        protected internal IReadOnlyList<Vertex> Vertices { get; }

        /// <summary>
        /// The Vertex List
        /// </summary>
        IReadOnlyList<IVertex> IGraph.Vertices { get { return this.Vertices; } }

        /// <summary>
        /// Edge Changed Event
        /// </summary>
        public event EventHandler<EdgeChangedEventArgs> EdgeChanged;

        /// <summary>
        /// Raises Edge Changed Event
        /// </summary>
        /// <param name="e">The Event Args</param>
        protected virtual void OnEdgeChanged(EdgeChangedEventArgs e) => this.EdgeChanged?.Invoke(this, e);

        /// <summary>
        /// All Edges Setted Event
        /// </summary>
        public event EventHandler<AllEdgesSettedEventArgs> AllEdgesSetted;

        /// <summary>
        /// Raises All Edges Setted Event
        /// </summary>
        /// <param name="e">The Event Args</param>
        protected virtual void OnAllEdgesSetted(AllEdgesSettedEventArgs e) => this.AllEdgesSetted?.Invoke(this, e);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size">The graph size</param>
        ///// <exception cref="ArgumentOutOfRangeException">Throws if the graph size is less than zero</exception>
        private Graph(int size)
        {
            //if (size < 0)
            //    throw new ArgumentOutOfRangeException(nameof(size), size, "The graph size must be greater than zero.");

            this.Size = size;

            this.AdjacencyMatrix = new AdjacencyMatrix(this, this.Size);
            this.AdjacencyMatrix.EdgeChanged += (sender, e) => this.OnEdgeChanged(e);
            this.AdjacencyMatrix.AllEdgesSetted += (sender, e) => this.OnAllEdgesSetted(e);

            var vertexList = new Vertex[this.Size];
            for (int i = 0; i < vertexList.Length; i++)
                vertexList[i] = new Vertex(this, i);
            this.Vertices = new ReadOnlyCollection<Vertex>(vertexList);
        }

        /// <summary>
        /// Creates a simple graph
        /// </summary>
        /// <param name="size">The graph size</param>
        /// <returns>Returns a new simple graph</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the graph size is less than zero</exception>
        public static Graph Create(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), size, "The graph size must be greater than zero.");

            return new Graph(size);
        }

        /// <summary>
        /// Checks the graph is a null graph
        /// </summary>
        public bool IsNull
        {
            get { return this.Size == 0; }
        }

        /// <summary>
        /// Checks the graph is a singleton graph
        /// </summary>
        public bool IsSingleton
        {
            get { return this.Size == 1; }
        }

        /// <summary>
        /// Checks the graph is an empty, a null or a singleton graph
        /// </summary>
        /// <returns>Returns True if the graph is an empty, a null or a singleton graph, otherwise returns False</returns>
        public bool IsEmpty() => this.AdjacencyMatrix.IsEmptyOrCompleteHelper(value => !value);

        /// <summary>
        /// Checks the graph is a complete, not a null and not a singleton graph
        /// </summary>
        /// <returns>Returns True if the graph is a complete, not a null and not a singleton graph, otherwise returns False</returns>
        public bool IsComplete() => this.Size >= 2 && this.AdjacencyMatrix.IsEmptyOrCompleteHelper(value => value);

        /// <summary>
        /// Calculates the graph connectivity markers
        /// </summary>
        /// <param name="startVertexIndex">The start vertex index</param>
        /// <returns>Returns the graph connectivity markers</returns>
        /// <exception cref="InvalidOperationException">Throws is the graph is a null graph</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the start vertex index is less than zero or equals to or greater than the graph size</exception>
        public virtual bool[] GetConnectivityMarkers(int startVertexIndex)
        {
            if (this.Size == 0)
                throw new InvalidOperationException("The graph is a null graph.");

            if (startVertexIndex < 0 || startVertexIndex >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(startVertexIndex), startVertexIndex, "The vertex index must be equal to or greater than zero and less than the graph size.");

            bool[] markers = new bool[this.Size]; // Vertex markers (False: vertex is unreached; True: vertex is reached)
            HashSet<int> reachedAndUprocessedSet = new HashSet<int>(); // Reached and unprocessed vertex set

            Action<int> pushToReached = vertexIndex =>
            {
                markers[vertexIndex] = true;
                reachedAndUprocessedSet.Add(vertexIndex);
            };

            Func<int> popAnyFromReachedAndUprocessed = () =>
            {
                int vertexIndex = reachedAndUprocessedSet.First();
                reachedAndUprocessedSet.Remove(vertexIndex);
                return vertexIndex;
            };

            pushToReached(startVertexIndex);
            while (reachedAndUprocessedSet.Count > 0)
            {
                int currentVertexIndex = popAnyFromReachedAndUprocessed();
                for (int nextVertexIndex = 0; nextVertexIndex < this.Size; nextVertexIndex++)
                    if (this.AdjacencyMatrix[currentVertexIndex, nextVertexIndex])
                        if (!markers[nextVertexIndex])
                            pushToReached(nextVertexIndex);
            }

            return markers;
        }

        /// <summary>
        /// Checks the graph is a connected graph
        /// </summary>
        /// <returns>Returns True if the graph is a connected graph, otherwise, returns False</returns>
        /// <exception cref="InvalidOperationException">Throws is the graph is a null graph</exception>
        public virtual bool IsConnected() => this.GetConnectivityMarkers(0).Count(marker => !marker) == 0;

        /// <summary>
        /// Converts the graph to its spanning tree
        /// </summary>
        /// <param name="startVertexIndex">The start vertex index</param>
        /// <exception cref="InvalidOperationException">Throws if the graph is a null or is not a connected graph</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the start vertex index is less than zero or equals to or greater than the graph size</exception>
        public virtual void ConvertToSpanningTree(int startVertexIndex)
        {
            //if (this.Size == 0)
            //    throw new InvalidOperationException("The graph is a null graph.");

            //if (startVertexIndex < 0 || startVertexIndex >= this.Size)
            //    throw new ArgumentOutOfRangeException(nameof(startVertexIndex), startVertexIndex, "The vertex index must be equal to or greater than zero and less than the graph size.");

            if (!this.IsConnected())
                throw new InvalidOperationException("The graph is not a connected graph.");

            //var markers = new VertexConnectivityMarker[this.Size];
            //var pathDictionary = new Dictionary<int, int>(this.Size);
            //int currentVertexIndex = startVertexIndex;
            //markers[currentVertexIndex] = VertexConnectivityMarker.Reached;
            //pathDictionary.Add(currentVertexIndex, currentVertexIndex);
            //do
            //{
            //    int newCurrentVertexIndex = -1;
            //    for (int column = 0; column < this.Size; column++)
            //        if (this.AdjacencyMatrix[currentVertexIndex, column])
            //        {
            //            if (markers[column] == VertexConnectivityMarker.Unreached)
            //            {
            //                markers[column] = VertexConnectivityMarker.Reached;
            //                if (newCurrentVertexIndex < 0)
            //                {
            //                    newCurrentVertexIndex = column;
            //                    pathDictionary.Add(newCurrentVertexIndex, currentVertexIndex);
            //                }
            //            }
            //            else if (markers[column] == VertexConnectivityMarker.Processed && pathDictionary[column] == currentVertexIndex)
            //            {
            //                this.AdjacencyMatrix[currentVertexIndex, column] = false;
            //            }
            //        }
            //    markers[currentVertexIndex] = VertexConnectivityMarker.Processed;
            //    currentVertexIndex = newCurrentVertexIndex;
            //}
            //while (currentVertexIndex >= 0);
        }

        /// <summary>
        /// Converts the graph to its spanning tree and calculates a vertex deleting sequence for the connected graph
        /// </summary>
        /// <param name="startVertexIndex">The start vertex index</param>
        /// <returns>Returns a vertex deleting sequence for the connected graph</returns>
        /// <exception cref="InvalidOperationException">
        /// Throws if the graph is a null or is not a connected graph, or the graph is in an unexpected invalid state
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if the start vertex index is less than zero or equals to or greater than the graph size
        /// </exception>
        public virtual int[] ConvertToSpanningTreeAndGetVertexDeletingSequenceForConnectedGraph(int startVertexIndex)
        {
            this.ConvertToSpanningTree(startVertexIndex);

            HashSet<int> processedVertexSet = new HashSet<int>();
            int[] processedVertexSequence = new int[this.Size];

            Func<int> getFirstExternalVertexIndex = () =>
            {
                for (int i = 0; i < this.Size; i++)
                    if (!processedVertexSet.Contains(i))
                        if (this.Vertices[i].Degree == 1)
                            return i;
                return -1;
            };

            // Process all vertexes excluding a last vertex
            // Do nothing for a singleton graph
            while (processedVertexSet.Count < this.Size - 1)
            {
                int currentExternalVertexIndex = getFirstExternalVertexIndex();
                if (currentExternalVertexIndex < 0)
                    throw new InvalidOperationException("Unexpected Exception: External Vertex Not Found.");

                for (int column = 0; column < this.Size; column++)
                    if (this.AdjacencyMatrix[currentExternalVertexIndex, column])
                    {
                        this.AdjacencyMatrix[currentExternalVertexIndex, column] = false;
                        processedVertexSet.Add(currentExternalVertexIndex);
                        processedVertexSequence[processedVertexSet.Count - 1] = currentExternalVertexIndex;
                        break;
                    }
            }

            // Process the last unprocessed vertex (or the first vertex for a singleton graph)
            for (int i = 0; i < this.Size; i++)
                if (!processedVertexSet.Contains(i))
                {
                    processedVertexSequence[processedVertexSequence.Length - 1] = i;
                    break;
                }


            return processedVertexSequence;
        }

        /// <summary>
        /// Does the empty graph if graph is not a null and is not a singleton graph,
        /// otherwise does nothing
        /// </summary>
        public void DoEmpty() => this.AdjacencyMatrix.FillEdgesHelper(false);

        /// <summary>
        /// Does the complete graph if graph is not a null and is not a singleton graph,
        /// otherwise does nothing
        /// </summary>
        public void DoComplete() => this.AdjacencyMatrix.FillEdgesHelper(true);

        /// <summary>
        /// Recalculates vertex degrees
        /// </summary>
        public void RecalcVertexDegrees()
        {
            for (int vertexIndex = 0; vertexIndex < this.Size; vertexIndex++)
            {
                this.Vertices[vertexIndex].Degree = 0;
                for (int column = 0; column < this.Size; column++)
                    if (this.AdjacencyMatrix[vertexIndex, column])
                        this.Vertices[vertexIndex].Degree++;
            }
        }

    }

}
