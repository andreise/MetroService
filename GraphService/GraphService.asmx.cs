using System;
using System.Web.Services;
using static System.FormattableString;

namespace GraphService
{
    using Model;

    /// <summary>
    /// Simple Graph Service
    /// </summary>
    /// <remarks>
    /// A simple graph is an unweighted, undirected graph containing no graph loops or multiple edges
    /// </remarks>
    [WebService(Namespace = "http://andreysergeev.org/graphservice")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GraphService : WebService
    {

        private static string GetErrorMessage(Exception e)
        {
            string message = Invariant($"{e.Message} ({e.GetType()})");
            if ((object)e.InnerException != null)
                message += Invariant($": {e.InnerException.Message} ({e.InnerException.GetType()})");
            return message;
        }

        /// <summary>
        /// Calculates the vertex deleting sequence for the connected graph
        /// </summary>
        /// <param name="inputXml">The graph description in the xml format</param>
        /// <returns>Returns GetConnectedGraphVertexDeletingSequenceResult instance</returns>
        [WebMethod]
        public GetConnectedGraphVertexDeletingSequenceResult GetConnectedGraphVertexDeletingSequence(string inputXml)
        {
            try
            {
                int[] sequence = GraphServiceProvider.Default.LoadGraphFromXml(inputXml).GetConnectedGraphVertexDeletingSequence(0);
                return new GetConnectedGraphVertexDeletingSequenceResult() { Success = true, ErrorMessage = null, Sequence = sequence };
            }
            catch (Exception e) when (e is ArgumentException || e is InvalidOperationException)
            {
                return new GetConnectedGraphVertexDeletingSequenceResult() { Success = false, ErrorMessage = GetErrorMessage(e), Sequence = null };
            }
        }
    }

}
