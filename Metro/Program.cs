using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static System.FormattableString;
using MetroModel;

namespace Metro
{
    using Properties;

    static class Program
    {
        private static void OutputLine(string line = null) => Console.WriteLine(line);

        private static void OutputLines(params string[] lines)
        {
            if ((object)lines == null)
                return;

            for (int i = 0; i < lines.Length; i++)
                OutputLine(lines[i]);
        }

        private static string GetExecutableName()
        {
            string exeName;
            try
            {
                exeName = Path.GetFileName(Assembly.GetEntryAssembly().CodeBase);
            }
            catch (ArgumentException)
            {
                exeName = null;
            }
            return string.IsNullOrEmpty(exeName) ? "Metro.exe" : exeName;
        }

        private static void OutputAppTitle() => OutputLines("Metro Application / Graph Service Client", "");

        private static void OutputHelp()
        {
            string exeName = GetExecutableName();

            OutputLines(
                "The Application loads a metro scheme and gets a metro stations closing sequence to stay metro scheme as a connected graph until last station reached.",
                "",
                "Usage Syntax:",
                Invariant($"{exeName} {ArgPrefixes.Task}TaskName {ArgPrefixes.InputFile}InputFile {ArgPrefixes.OutputFile}OutputFile"),
                "Where the TaskName is a task name, the InputFile is an input file name and the OutputFile is an input file name.",
                "",
                Invariant($"Valid task names are: {TaskCode.ClosingSequence}."),
                "",
                "File names might contain relative or absolute paths.",
                "If a file name contains no path or contains a relative path, a current directory will be used.",
                "If the OutputFile contains a relative or an absolute path, the destination directory must exists.",
                "WARNING: If the OutputFile already exists and has no read-only attribute, ",
                "it will be overwritten by a new output file.",
                "",
                "Examples:",
                Invariant($"{exeName} {ArgPrefixes.Task}{TaskCode.ClosingSequence} {ArgPrefixes.InputFile}input.txt {ArgPrefixes.OutputFile}output.txt"),
                Invariant($"{exeName} {ArgPrefixes.Task}{TaskCode.ClosingSequence} {ArgPrefixes.InputFile}input.txt {ArgPrefixes.OutputFile}temp{Path.DirectorySeparatorChar}output.txt"),
                Invariant($"{exeName} {ArgPrefixes.Task}{TaskCode.ClosingSequence} {ArgPrefixes.InputFile}{Path.DirectorySeparatorChar}input.txt {ArgPrefixes.OutputFile}{Path.DirectorySeparatorChar}temp{Path.DirectorySeparatorChar}output.txt"),
                Invariant($"{exeName} {ArgPrefixes.Task}{TaskCode.ClosingSequence} {ArgPrefixes.InputFile}C{Path.VolumeSeparatorChar}{Path.DirectorySeparatorChar}temp{Path.DirectorySeparatorChar}input.txt {ArgPrefixes.OutputFile}C{Path.VolumeSeparatorChar}{Path.DirectorySeparatorChar}temp{Path.DirectorySeparatorChar}output.txt"),
                ""
            );
        }

        private static void ExitPrompt()
        {
            if (Debugger.IsAttached || Settings.Default.DebugMode)
            {
                OutputLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        static void Main(string[] args)
        {
            OutputAppTitle();

            IMetro metro;
            try
            {
                metro = MetroFactory.CreateMetro(args);
            }
            catch (ArgumentException)
            {
                OutputHelp();
                ExitPrompt();
                return;
            }

            try
            {
                OutputLine("Please wait...");
                metro.PerformTask();
                OutputLines(Invariant($"The result successfully written to the output file '{metro.OutputFileName}'."), "");
            }
            catch (Exception e)
            {
                // catches a MetroException, InvalidOperationException throwed by the Metro Model, or any Exception throwed by the web service
                string message = Invariant($"{e.Message} ({e.GetType()})");
                if ((object)e.InnerException != null)
                    message += Invariant($". Inner exception: {e.InnerException.Message} ({e.InnerException.GetType()})");
                OutputLines(message, "");
            }

            ExitPrompt();
        }
    }
}
