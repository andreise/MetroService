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

            bool[] processedVertexMarkers = new bool[this.Size];
            HashSet<int> reachedVertexSet = new HashSet<int>();

            Func<int, bool> isProcessed = vertexIndex => processedVertexMarkers[vertexIndex];

            Action<int> processAndAddToReached = vertexIndex =>
            {
                processedVertexMarkers[vertexIndex] = true;
                reachedVertexSet.Add(vertexIndex);
            };

            Func<int> removeAnyFromReached = () =>
            {
                int vertexIndex = reachedVertexSet.First();
                reachedVertexSet.Remove(vertexIndex);
                return vertexIndex;
            };

            processAndAddToReached(startVertexIndex);
            while (reachedVertexSet.Count > 0)
            {
                int currentVertexIndex = removeAnyFromReached();
                for (int otherVertexIndex = 0; otherVertexIndex < this.Size; otherVertexIndex++)
                    if (this.AdjacencyMatrix[currentVertexIndex, otherVertexIndex])
                        if (!isProcessed(otherVertexIndex))
                            processAndAddToReached(otherVertexIndex);
            }

            return processedVertexMarkers;
        }

        /// <summary>
        /// Checks the graph is a connected graph
        /// </summary>
        /// <returns>Returns True if the graph is a connected graph, otherwise, returns False</returns>
        /// <exception cref="InvalidOperationException">Throws is the graph is a null graph</exception>
        public virtual bool IsConnected() => this.GetConnectivityMarkers(0).Count(marker => !marker) == 0;

        /// <summary>
        /// Calculates the graph spanning forest
        /// </summary>
        /// <param name="startVertexIndex">The start vertex index</param>
        /// <returns>Returns the graph spanning forest</returns>
        /// <exception cref="InvalidOperationException">Throws if the graph is a null graph</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the start vertex index is less than zero or equals to or greater than the graph size</exception>
        public virtual IGraph GetSpanningForest(int startVertexIndex)
        {
            if (this.Size == 0)
                throw new InvalidOperationException("The graph is a null graph.");

            if (startVertexIndex < 0 || startVertexIndex >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(startVertexIndex), startVertexIndex, "The vertex index must be equal to or greater than zero and less than the graph size.");

            Graph spanningForest = new Graph(this.Size);

            bool[] processedVertexMarkers = new bool[this.Size];
            Stack<Tuple<int, int>> reachedVertexStack = new Stack<Tuple<int, int>>(); // Item1: vertex index, Item2: from reached vertex index

            do // external cycle for spanning forest searching
            {

                reachedVertexStack.Push(new Tuple<int, int>(startVertexIndex, -1));

                do // internal cycle for spanning tree searching
                {
                    Tuple<int, int> currentVertexInfo = reachedVertexStack.Pop();

                    if (processedVertexMarkers[currentVertexInfo.Item1])
                        continue;

                    processedVertexMarkers[currentVertexInfo.Item1] = true;

                    if (currentVertexInfo.Item2 >= 0)
                        spanningForest.AdjacencyMatrix[currentVertexInfo.Item1, currentVertexInfo.Item2] = true;

                    for (int otherVertexIndex = 0; otherVertexIndex < this.Size; otherVertexIndex++)
                    {
                        if (processedVertexMarkers[otherVertexIndex])
                            continue;
                        if (this.AdjacencyMatrix[currentVertexInfo.Item1, otherVertexIndex])
                            reachedVertexStack.Push(new Tuple<int, int>(otherVertexIndex, currentVertexInfo.Item1));
                    }
                } while (reachedVertexStack.Count > 0);

            } while ((startVertexIndex = Array.FindIndex(processedVertexMarkers, marker => !marker)) >= 0);

            return spanningForest;
        }

        /// <summary>
        /// Calculates a vertex deleting sequence for the spanning tree
        /// </summary>
        /// <param name="spanningTree">The spanning tree</param>
        /// <returns>Returns a vertex deleting sequence for the spanning tree</returns>
        /// <exception cref="InvalidOperationException">
        /// Throws if the graph is in a some unexpected state; for example, the graph is not a spanning tree
        /// </exception>
        private static int[] GetSpanningTreeVertexDeletingSequenceHelper(IGraph spanningTree)
        {
            bool[] processedVertexMarkers = new bool[spanningTree.Size];
            List<int> processedVertexSequence = new List<int>(spanningTree.Size);

            Action<int> addToProcessed = vertexIndex =>
            {
                processedVertexMarkers[vertexIndex] = true;
                processedVertexSequence.Add(vertexIndex);
            };

            Func<int> getAnyLeafIndex = () =>
            {
                for (int vertexIndex = 0; vertexIndex < spanningTree.Size; vertexIndex++)
                    if (!processedVertexMarkers[vertexIndex])
                        if (spanningTree.Vertices[vertexIndex].Degree == 1)
                            return vertexIndex;
                return -1;
            };

            Func<int, int> getLeafAdjacentIndex = leafIndex =>
            {
                for (int otherVertexIndex = 0; otherVertexIndex < spanningTree.Size; otherVertexIndex++)
                    if (spanningTree.AdjacencyMatrix[leafIndex, otherVertexIndex])
                        return otherVertexIndex;
                return -1;
            };

            Func<int> getAnyVertexIndex = () => Array.FindIndex(processedVertexMarkers, marker => !marker);

            while (processedVertexSequence.Count < spanningTree.Size - 1)
            {
                int leafIndex = getAnyLeafIndex();
                if (leafIndex < 0)
                    throw new InvalidOperationException("Unexpected Exception: No leaf vertex is found.");

                int leafAdjacentIndex = getLeafAdjacentIndex(leafIndex);
                if (leafAdjacentIndex < 0)
                    throw new InvalidOperationException("Unexpected Exception: No leaf adjacent vertex is found.");

                spanningTree.AdjacencyMatrix[leafIndex, leafAdjacentIndex] = false;
                addToProcessed(leafIndex);
            }

            if (spanningTree.Size > 0)
            {
                int lastVertexIndex = getAnyVertexIndex();
                addToProcessed(lastVertexIndex);
            }

            return processedVertexSequence.ToArray();
        }

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
        public virtual int[] GetConnectedGraphVertexDeletingSequence(int startVertexIndex)
        {
            if (this.Size == 0)
                throw new InvalidOperationException("The graph is a null graph.");

            if (startVertexIndex < 0 || startVertexIndex >= this.Size)
                throw new ArgumentOutOfRangeException(nameof(startVertexIndex), startVertexIndex, "The vertex index must be equal to or greater than zero and less than the graph size.");

            if (!this.IsConnected())
                throw new InvalidOperationException("The graph is not a connected graph.");

            IGraph spanningTree = this.GetSpanningForest(startVertexIndex);
            return GetSpanningTreeVertexDeletingSequenceHelper(spanningTree);
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
                for (int otherVertexIndex = 0; otherVertexIndex < this.Size; otherVertexIndex++)
                    if (this.AdjacencyMatrix[vertexIndex, otherVertexIndex])
                        if (vertexIndex == otherVertexIndex)
                        {
                            if (this.IsLoopGraph)
                                this.Vertices[vertexIndex].Degree += 2;
                        }
                        else
                        {
                            this.Vertices[vertexIndex].Degree++;
                        }
            }
        }

    }

}
