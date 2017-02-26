namespace MetroModel
{

    /// <summary>
    /// Metro Model
    /// </summary>
    public interface IMetro
    {
        /// <summary>
        /// Task Code
        /// </summary>
        string TaskCode { get; }

        /// <summary>
        /// Input File Name
        /// </summary>
        string InputFileName { get; }

        /// <summary>
        /// Output File Name
        /// </summary>
        string OutputFileName { get; }

        /// <summary>
        /// Performs the task which has the specified code
        /// </summary>
        /// <exception cref="MetroException">Throws if an exception was thrown during task performing</exception>
        /// <exception cref="InvalidOperationException">Throws if the 'TaskCode' task support not implemented</exception>
        void PerformTask();
    }

}
