namespace GraphService
{

    /// <summary>
    /// Graph Service Result
    /// </summary>
    public class WebServiceResult
    {

        /// <summary>
        /// Is method executed successfully
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }

    }

    /// <summary>
    /// GetConnectedGraphVertexDeletingSequence Result
    /// </summary>
    public class GetConnectedGraphVertexDeletingSequenceResult : WebServiceResult
    {

        /// <summary>
        /// Connected Graph Vertex Deleting Sequence
        /// </summary>
        public int[] Sequence { get; set; }

    }

}