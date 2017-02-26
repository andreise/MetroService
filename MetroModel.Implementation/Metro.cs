using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using static System.FormattableString;

namespace MetroModel
{
    using GraphServiceReference;

    public enum TaskCode
    {
        ClosingSequence
    }

    public static class ArgPrefixes
    {
        public const string Task = "/task:";
        public const string InputFile = "/in:";
        public const string OutputFile = "/out:";
    }

    internal sealed class Metro : IMetro
    {
        private const int MaxStations = 1000;

        public TaskCode TaskCode { get; }

        string IMetro.TaskCode => this.TaskCode.ToString();

        public string InputFileName { get; }

        public string OutputFileName { get; }

        private Metro(
            TaskCode taskCodeName,
            string inputFileName,
            string outputFileName
        )
        {
            this.TaskCode = taskCodeName;
            this.InputFileName = inputFileName;
            this.OutputFileName = outputFileName;
        }

        private static string GetArgValue(string[] args, string argPrefix)
        {
            string arg = Array.Find(args, item => (object)item != null && item.StartsWith(argPrefix, StringComparison.OrdinalIgnoreCase));
            return arg?.Substring(startIndex: argPrefix.Length);
        }

        private static TaskCode? ParseTaskCode(string value)
        {
            try
            {
                return (TaskCode)Enum.Parse(typeof(TaskCode), value, true);
            }
            catch (Exception e) when (e is ArgumentException || e is OverflowException)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new Metro instance
        /// </summary>
        /// <param name="args">Input parameters encoded in the string array</param>
        /// <returns>Returns a new Metro instance</returns>
        /// <exception cref="ArgumentNullException">Throws if the args is null</exception>
        /// <exception cref="ArgumentException">Throws if any arg value is undefined or unknown</exception>
        public static Metro Create(string[] args)
        {
            if ((object)args == null)
                throw new ArgumentNullException(nameof(args));

            string taskCodeString = GetArgValue(args, ArgPrefixes.Task);
            if (string.IsNullOrEmpty(taskCodeString))
                throw new ArgumentException("The task code name parameter value not specified.", nameof(args));

            TaskCode? taskCode = ParseTaskCode(taskCodeString);
            if (taskCode == null)
                throw new ArgumentException(Invariant($"The specified task code name parameter value is invalid ({taskCodeString})."), nameof(args));
            if (!Enum.IsDefined(typeof(TaskCode), taskCode))
                throw new ArgumentException(Invariant($"The specified task code name parameter value is unknown ({taskCode})."), nameof(args));

            string inputFileName = GetArgValue(args, ArgPrefixes.InputFile);
            if (string.IsNullOrEmpty(inputFileName))
                throw new ArgumentException("The input file name parameter value is undefined", nameof(args));

            string outputFileName = GetArgValue(args, ArgPrefixes.OutputFile);
            if (string.IsNullOrEmpty(outputFileName))
                throw new ArgumentException("The output file name parameter value is undefined", nameof(args));

            return new Metro((TaskCode)taskCode, inputFileName, outputFileName);
        }

        private static string LoadInputTextToXml(string sourcePath)
        {
            try
            {
                using (var reader = File.OpenText(sourcePath))
                {
                    Func<string, string> getUnexpectedFormatMessage = description => Invariant($"The input file has an unexpected format: {description}.");

                    const int maxEmptyLinesBeforeHeader = 100;
                    int emptyLinesBeforeHeader = 0;
                    string header;
                    while ((object)(header = reader.ReadLine()) != null && string.IsNullOrWhiteSpace(header))
                    {
                        emptyLinesBeforeHeader++;
                        if (emptyLinesBeforeHeader > maxEmptyLinesBeforeHeader)
                            throw new UnexpectedFileFormatException(getUnexpectedFormatMessage("the file header is empty"));
                    }
                    if ((object)header == null)
                        throw new UnexpectedFileFormatException(getUnexpectedFormatMessage("the file is empty"));

                    Func<string, string[]> splitLine = line => line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                    string[] headerItems = splitLine(header);
                    if (headerItems.Length == 0)
                        throw new UnexpectedFileFormatException(getUnexpectedFormatMessage("the file header is empty"));
                    if (headerItems.Length == 1)
                        throw new UnexpectedFileFormatException(getUnexpectedFormatMessage("the file header contains only one parameter instead two"));
                    if (headerItems.Length > 2)
                        throw new UnexpectedFileFormatException(getUnexpectedFormatMessage("the file header contains too much parameters"));

                    Func<string, int> ParseInt32 = s => Int32.Parse(s, NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, NumberFormatInfo.InvariantInfo);

                    Func<string, string, int> ParseInt32Parameter = (paramName, paramValueString) =>
                    {
                        try
                        {
                            return ParseInt32(paramValueString);
                        }
                        catch (Exception e) when (e is ArgumentException || e is FormatException || e is OverflowException)
                        {
                            throw new UnexpectedFileFormatException(
                                getUnexpectedFormatMessage($"the '{paramName}' parameter value ({paramValueString}) has an unexpected format."),
                                e
                            );
                        }
                    };

                    int stationCount = ParseInt32Parameter(nameof(stationCount), headerItems[0]);
                    if (stationCount < 1)
                        throw new UnexpectedFileFormatException(getUnexpectedFormatMessage("the metro must contains at least one station"));
                    //if (stationCount < 2)
                    //    throw new UnexpectedFileFormatException(getUnexpectedFormatMessage("the metro must contains at least two stations"));
                    if (stationCount > MaxStations)
                        throw new UnexpectedFileFormatException(getUnexpectedFormatMessage(Invariant($"a metro should not contains more than {MaxStations} stations")));

                    int metroLineCount = ParseInt32Parameter(nameof(metroLineCount), headerItems[1]);
                    int maxMetroLineCount = (stationCount * (stationCount - 1)) / 2;

                    if (metroLineCount > maxMetroLineCount)
                        throw new UnexpectedFileFormatException(getUnexpectedFormatMessage(Invariant(
                            $"a metro containing {stationCount} stations cannot contains lines more than {maxMetroLineCount}"
                        )));

                    var builder = new StringBuilder();
                    using (var writer = XmlWriter.Create(builder, new XmlWriterSettings() { Indent = true }))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Graph");
                        writer.WriteElementString("Size", XmlConvert.ToString(stationCount));
                        writer.WriteStartElement("Edges");

                        string edgeLine;
                        int edgesProcessed = 0;
                        while (edgesProcessed < metroLineCount && (object)(edgeLine = reader.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(edgeLine))
                                continue;

                            edgesProcessed++;

                            string[] edgeLineItems = splitLine(edgeLine);
                            if (edgeLineItems.Length == 0)
                                throw new UnexpectedFileFormatException(getUnexpectedFormatMessage(Invariant($"the metro line {edgesProcessed} description is empty and contains no station numbers")));
                            if (edgeLineItems.Length == 1)
                                throw new UnexpectedFileFormatException(getUnexpectedFormatMessage(Invariant($"the metro line {edgesProcessed} description is incomplete and contains only one parameter instead two")));
                            if (edgeLineItems.Length > 2)
                                throw new UnexpectedFileFormatException(getUnexpectedFormatMessage(Invariant($"the metro line {edgesProcessed} description is unexpected and contains more than two parameters")));

                            int station1 = ParseInt32Parameter(Invariant($"the first station of the line ({edgesProcessed})"), edgeLineItems[0]);
                            int station2 = ParseInt32Parameter(Invariant($"the second station of the line ({edgesProcessed})"), edgeLineItems[1]);
                            if (station1 == station2)
                                throw new UnexpectedFileFormatException(getUnexpectedFormatMessage(Invariant($"the metro line ({edgesProcessed}) cannot be connected with itself")));
                            if (station1 == 0 || station2 == 0)
                                throw new UnexpectedFileFormatException(getUnexpectedFormatMessage(Invariant($"the metro station cannot has the zero number (the metro line: {edgesProcessed})")));
                            if (station1 > stationCount || station2 > stationCount)
                                throw new UnexpectedFileFormatException(getUnexpectedFormatMessage(Invariant($"the metro station cannot has the number greater than the metro station count (the metro line: {edgesProcessed})")));

                            writer.WriteStartElement("Edge");
                            writer.WriteElementString("Vertex1", XmlConvert.ToString(station1 - 1));
                            writer.WriteElementString("Vertex2", XmlConvert.ToString(station2 - 1));
                            writer.WriteEndElement(); // Edge
                        }
                        if (edgesProcessed < metroLineCount)
                            throw new UnexpectedFileFormatException(getUnexpectedFormatMessage(Invariant(
                                $"unexpected end of file: only {edgesProcessed} metro lines processed instead of expected {metroLineCount} lines"
                            )));

                        writer.WriteEndElement(); // Edges
                        writer.WriteEndElement(); // Graph
                        writer.WriteEndDocument();
                    }
                    return builder.ToString();
                }
            }
            catch (Exception e) when (
                e is ArgumentException ||
                e is NotSupportedException ||
                e is UnauthorizedAccessException ||
                e is IOException
            )
            {
                throw new FileLoadingException(Invariant($"The exception was thrown during loading the file '{sourcePath}'."), e);
            }
        }

        private static void SaveSequenceToFile(string destPath, int[] sequence)
        {
            try
            {
                using (var writer = File.CreateText(destPath))
                {
                    for (int i = 0; i < sequence.Length; i++)
                        writer.WriteLine((sequence[i] + 1).ToString(NumberFormatInfo.InvariantInfo));
                }
            }
            catch (Exception e) when (
                e is ArgumentException ||
                e is NotSupportedException ||
                e is UnauthorizedAccessException ||
                e is IOException
            )
            {
                throw new FileSavingException(Invariant($"The exception was thrown during saving the file '{destPath}'."), e);
            }
        }

        private void PerformClosingSequenceTask()
        {
            using (var graphService = new GraphService())
            {
                string inputXml = LoadInputTextToXml(this.InputFileName);
                var result = graphService.GetConnectedGraphVertexDeletingSequence(inputXml);
                if (!result.Success)
                    throw new GraphServiceErrorWrapperException(result.ErrorMessage);
                SaveSequenceToFile(this.OutputFileName, result.Sequence);
            }
        }

        /// <summary>
        /// Performs the task which has the specified code
        /// </summary>
        /// <exception cref="MetroException">Throws if an exception was thrown during task performing</exception>
        /// <exception cref="InvalidOperationException">Throws if the 'TaskCode' task support not implemented</exception>
        public void PerformTask()
        {
            switch (this.TaskCode)
            {
                case TaskCode.ClosingSequence:
                    this.PerformClosingSequenceTask();
                    break;

                default:
                    throw new InvalidOperationException(Invariant($"The '{this.TaskCode}' task support not implemented."));
            }
        }

    }
}
