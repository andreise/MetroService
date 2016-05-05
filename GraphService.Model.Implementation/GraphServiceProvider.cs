using System;

namespace GraphService.Model
{
    /// <summary>
    /// Graph Service Provider
    /// </summary>
    public static class GraphServiceProvider
    {
        /// <summary>
        /// The default graph service instance lazy init container
        /// </summary>
        private static readonly Lazy<IGraphService> defaultInstanceContainer;

        /// <summary>
        /// Static constructor
        /// </summary>
        static GraphServiceProvider()
        {
            // Static fields must be initialized in a static constructor, not in a place of the declaration,
            // for the guaranted initialization in a multithread work in services

            defaultInstanceContainer = new Lazy<IGraphService>(() => new GraphService());
        }

        /// <summary>
        /// The default graph service instance
        /// </summary>
        public static IGraphService Default
        {
            get { return defaultInstanceContainer.Value; }
        }

        /// <summary>
        /// Gets a graph service
        /// </summary>
        /// <returns>A graph service instance</returns>
        /// <remarks>
        /// Th method behaviour is implementation depended:
        /// the method returns the default instance, or a some already existing instance (for ex., from a some pool), or a new instance
        /// </remarks>
        public static IGraphService GetGraphService()
        {
            return Default;
        }

        /// <summary>
        /// Creates a new graph service instance
        /// </summary>
        /// <returns>Returns a new graph service instance</returns>
        public static IGraphService GetNewGraphService()
        {
            return new GraphService();
        }
    }
}
