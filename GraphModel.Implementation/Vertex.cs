using System;
using static System.FormattableString;

namespace GraphModel
{

    /// <summary>
    /// Simple Graph Vertex
    /// </summary>
    /// <remarks>
    /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
    /// </remarks>
    internal class Vertex : IVertex
    {
        /// <summary>
        /// The Vertex Owner
        /// </summary>
        protected internal Graph Owner { get; }

        public int Index { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">The graph which is the vertex owner</param>
        /// <exception cref="ArgumentNullException">Throws if the vertex owner is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the vertex index is less than zero or equals to or greater than the owner size</exception>
        protected internal Vertex(Graph owner, int index)
        {
            if ((object)owner == null)
                throw new ArgumentNullException(nameof(owner));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "The vertex index must be equal to or greater than zero.");

            if (index >= owner.Size)
                throw new ArgumentOutOfRangeException(nameof(index), index, Invariant($"The vertex index must be less than the owner size ({owner.Size})."));

            this.Owner = owner;

            this.Index = index;

            this.Owner.EdgeChanged += (sender, e) =>
            {
                if (e.FirstVertexIndex == this.Index || e.SecondVertexIndex == this.Index)
                    if (e.NewEdgeValue)
                        this.Degree++;
                    else
                        this.Degree--;
            };
            this.Owner.AllEdgesSetted += (sender, e) => this.Degree = e.NewEdgeValue ? this.Owner.Size - 1 : 0;
        }

        /// <summary>
        /// The Vertex Degree (Valency, Order)
        /// </summary>
        /// <remarks>
        /// The number of graph edges meeting at a given node in a graph is called the order of that graph vertex
        /// </remarks>
        public int Degree { get; internal set; }

        /// <summary>
        /// Recalculates The Vertex Degree
        /// </summary>
        public void RecalcDegree()
        {
            this.Degree = 0;
            for (int otherVertexIndex = 0; otherVertexIndex < this.Owner.AdjacencyMatrix.Size; otherVertexIndex++)
                if (this.Owner.AdjacencyMatrix[this.Index, otherVertexIndex])
                    this.Degree++;
        }
    }

}
