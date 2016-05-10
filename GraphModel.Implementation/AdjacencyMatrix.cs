using System;
using static System.FormattableString;

namespace GraphModel
{

    /// <summary>
    /// Simple Graph Adjacency Matrix
    /// </summary>
    /// <remarks>
    /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
    /// </remarks>
    internal class AdjacencyMatrix : IAdjacencyMatrix
    {
        /// <summary>
        /// The Matrix Owner
        /// </summary>
        protected internal Graph Owner { get; }

        /// <summary>
        /// The Matrix Size
        /// </summary>
        public int Size { get; }

        //protected readonly bool[][] edges;
        protected bool[][] Edges { get; }

        /// <summary>
        /// Init an Edge Matrix
        /// </summary>
        /// <returns>Returns an Edge Matrix</returns>
        protected virtual bool[][] InitEdges()
        {
            bool[][] edges = new bool[this.Size == 0 ? 0 : this.Size - 1][];
            for (int i = 0; i < edges.Length; i++)
                edges[i] = new bool[i + 1];
            return edges;
        }

        /// <summary>
        /// Edge Changed Event
        /// </summary>
        public event EventHandler<EdgeChangedEventArgs> EdgeChanged;

        /// <summary>
        /// Raises Edge Changed Event
        /// </summary>
        /// <param name="e">Event Args</param>
        protected virtual void OnEdgeChanged(EdgeChangedEventArgs e) => this.EdgeChanged?.Invoke(this, e);

        /// <summary>
        /// All Edges Setted Event
        /// </summary>
        public event EventHandler<AllEdgesSettedEventArgs> AllEdgesSetted;

        /// <summary>
        /// Raises All Edges Setted Event
        /// </summary>
        /// <param name="e">Event Args</param>
        protected virtual void OnAllEdgesSetted(AllEdgesSettedEventArgs e) => this.AllEdgesSetted?.Invoke(this, e);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">The graph which is the matrix owner</param>
        /// <param name="size">The matrix size</param>
        /// <exception cref="ArgumentNullException">Throws if the owner is null</exception>
        /// <exception cref="ArgumentException">Throws if the owner already has an adjacency matrix</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the matrix size is less than zero</exception>
        protected internal AdjacencyMatrix(
            Graph owner,
            int size
        )
        {
            if ((object)owner == null)
                throw new ArgumentNullException(nameof(owner));

            if ((object)owner.AdjacencyMatrix != null)
                throw new ArgumentException("The owner already has an adjacency matrix.", nameof(owner));

            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), size, "The matrix size must be greater than zero.");

            this.Owner = owner;
            this.Size = size;
            this.Edges = this.InitEdges();
        }

        /// <summary>
        /// Does the function for each edge until the function returns True
        /// </summary>
        /// <param name="func">The function with the first and the second edge indexes</param>
        /// <returns>Returns True the graph size is less than two or if each edge was processed by the function, otherwise returns False</returns>
        /// <exception cref="ArgumentNullException">Throws if the function is null</exception>
        protected internal bool ForEachEdgeHelper(Func<int, int, bool> func)
        {
            if ((object)func == null)
                throw new ArgumentNullException(nameof(func));

            for (int i = 0; i < this.Edges.Length; i++)
                for (int j = 0; j < this.Edges[i].Length; j++)
                    if (!func(i, j))
                        return false;
            return true;
        }

        /// <summary>
        /// Init Edges Helper
        /// </summary>
        /// <param name="value">The value</param>
        protected internal void FillEdgesHelper(bool value)
        {
            bool changed = false;

            this.ForEachEdgeHelper(
                (i, j) =>
                {
                    if (this.Edges[i][j] != value)
                    {
                        this.Edges[i][j] = value;
                        changed = true;
                    }
                    return true;
                }
            );

            if (changed)
                OnAllEdgesSetted(new AllEdgesSettedEventArgs(value));
        }

        /// <summary>
        /// Is Empty Or Complete Graph Helper
        /// </summary>
        /// <param name="isExpectedEdgeValue">Expected edge value test function</param>
        /// <returns>Returns True if the the graph size is less than two or if each edge has an exprected value, otherwise returns False</returns>
        /// <exception cref="ArgumentNullException">Throws if the isExpectedEdgeValue function is null</exception>
        protected internal bool IsEmptyOrCompleteHelper(Func<bool, bool> isExpectedEdgeValue)
        {
            if ((object)isExpectedEdgeValue == null)
                throw new ArgumentNullException(nameof(isExpectedEdgeValue));

            return this.ForEachEdgeHelper(
                (i, j) => isExpectedEdgeValue(this.Edges[i][j])
            );
        }

        private void CheckIndex(string indexName, int index)
        {
            if (index < 0 || index >= this.Size)
                throw new ArgumentOutOfRangeException(indexName, index, Invariant($"The '{indexName}' must be equal to or greater than zero and less than the matrix size."));
        }

        private void CheckNullGraphAndRowAndColumn(int row, int column)
        {
            if (this.Size == 0)
                throw new ArgumentOutOfRangeException("The graph is a null graph.");

            this.CheckIndex(nameof(row), row);
            this.CheckIndex(nameof(column), column);
        }

        private Tuple<int, int> GetIndexesFromRowAndColumn(int row, int column) =>
            column > row ?
            Tuple.Create(column - 1, row) :
            Tuple.Create(row - 1, column);

        /// <summary>
        /// Is the edge between vertices presents
        /// </summary>
        /// <param name="row">The first vertex index</param>
        /// <param name="column">The second vertex index</param>
        /// <returns>Returns True if the edge between vertexes presents, otherwise returns False</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if the graph is a null graph,
        /// or if the row or the column is less than zero or equal to or greater than the matrix size,
        /// or if the row and the column are equals and associated value is True
        /// </exception>
        public virtual bool this[int row, int column]
        {
            get
            {
                this.CheckNullGraphAndRowAndColumn(row, column);
                if (row == column)
                    return false;
                var indexes = GetIndexesFromRowAndColumn(row, column);
                return this.Edges[indexes.Item1][indexes.Item2];
            }

            set
            {
                this.CheckNullGraphAndRowAndColumn(row, column);
                if (row == column)
                {
                    if (value)
                        throw new ArgumentOutOfRangeException(
                            Invariant($"{nameof(row)}/{nameof(column)}"),
                            row,
                            Invariant($"A simple graph cannot contain graph loops. A vertex cannot be connected with itself (the vertex index: {row}).")
                        );
                    return;
                }
                var indexes = GetIndexesFromRowAndColumn(row, column);
                if (this.Edges[indexes.Item1][indexes.Item2] != value)
                {
                    this.Edges[indexes.Item1][indexes.Item2] = value;
                    this.OnEdgeChanged(new EdgeChangedEventArgs(Math.Min(row, column), Math.Max(row, column), value));
                }
            }
        }
    }

}
