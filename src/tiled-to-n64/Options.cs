using CommandLine;

namespace TiledToN64
{
    public class Options
    {
        private static class Help
        {
            public const string Verbose =
                "Set output to verbose messages.";

            public const string InputPath =
                "Input path to source image file.";

            public const string OutputPath =
                "Output path to source image file.";
        }


        [Option("verbose", Required = false, HelpText = Help.Verbose)]
        public bool Verbose { get; set; }

        [Option('i', "inputPath", Required = true, HelpText = Help.InputPath)]
        public string InputPath { get; set; } = string.Empty;

        [Option('o', "outputPath", Required = false, HelpText = Help.OutputPath)]
        public string OutputPath { get; set; } = string.Empty;


        public void PrintState()
        {
            Console.WriteLine("Options:");
            Console.WriteLine($"{nameof(Verbose)}: {Verbose}");
            Console.WriteLine($"{nameof(InputPath)}: {InputPath}");
            Console.WriteLine($"{nameof(OutputPath)}: {OutputPath}");
        }
    }
}