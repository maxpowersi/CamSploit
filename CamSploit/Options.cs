using System.Collections.Generic;
using CommandLine;

namespace CamSploit
{
  //test
    public class Options
    {
        [Option("target-host", Required = true, HelpText = "Text file with one single full host (IP:Port) per line", SetName = "a")]
        public string SingleHost { get; set; }

        [Option("list-host", Required = true, HelpText = "Single host in format IP:Port, example 192.168.0.1:80", SetName = "b")]
        public string ListHost { get; set; }

        [Option("shodan-file", Required = true, HelpText = "JSON Shodan data file, example: data.json", SetName = "c")]
        public string ShodanFile { get; set; }

        [Option("list-exploit", Required = true, HelpText = "Show all exploits in the application", SetName = "d")]
        public string ListExploit { get; set; }

        [Option("shodan-search", Required = true, HelpText = "Search one exploit in shodan", SetName = "e")]
        public string SearchShodan { get; set; }

        [Option("output", Required = false, Default = "output.camsploit.txt", HelpText = "Output file")]
        public string Output { get; set; }

        [Option("exploit", Required = false, HelpText = "List of exploits separated by spaces, example CVE_2018_9995 Default_Password_CeNova", Separator = ',')]
        public IEnumerable<string> Exploit { get; set; }

        public InputType GetInputType()
        {
            if (ListExploit != null)
                return InputType.ListExploit;

            if (SingleHost != null)
                return InputType.SingleHost;

            if (ListHost != null)
                return InputType.ListHost;

            return ShodanFile != null ? InputType.Shodan : InputType.None;
        }
    }
}
