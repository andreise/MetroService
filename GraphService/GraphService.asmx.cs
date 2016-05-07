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
            return Invariant($"{e.Message} ({e.GetType()})");
        }

        /// <summary>
        /// Calculates the vertex deleting sequence for the connected graph
        /// </summary>
        /// <param name="inputXml">The graph description in the xml format</param>
        /// <param name="result">The vertex deleting sequence for the connected graph if executed successfully, otherwise the null value</param>
        /// <param name="errorMessage">An error message if execution failed, otherwise the null value</param>
        /// <returns>Returns True if executed successfully, otherwise returns False</returns>
        [WebMethod]
        public bool GetConnectedGraphVertexDeletingSequence(string inputXml, out int[] result, out string errorMessage)
        {
            try
            {
                result = GraphServiceProvider.Default.GetConnectedGraphVertexDeletingSequence(inputXml);
                errorMessage = null;
                return true;
            }
            catch (Exception e)
            {
                result = null;
                errorMessage = GetErrorMessage(e);
                return false;
            }
        }
    }

}
