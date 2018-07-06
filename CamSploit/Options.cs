using System.Collections.Generic;
using CommandLine;

namespace CamSploit
{
    public class Options
    {
        [Option("rhost", Required = true, HelpText = "Single host in format IP:Port, example 192.168.0.1:80",SetName = "a")]
        public string SingleHost { get; set; }

        [Option("rhost-list", Required = true, HelpText = "Text file with one single full host (IP:Port) per line.", SetName = "b")]
        public string ListHost { get; set; }

        [Option("rhost-shodan-file", Required = true, HelpText = "JSON Shodan data file, example: data.json", SetName = "c")]
        public string ShodanFile { get; set; }

        [Option("show-exploit", Required = true, HelpText = "Show all exploits in the application or the description of one exploit.", SetName = "d")]
        public string ShowExploit { get; set; }

        [Option("output", Required = false, Default = "output.camsploit.txt", HelpText = "Output file (it is optional).")]
        public string Output { get; set; }

        [Option("exploits", Required = false, HelpText = "List of exploits separated by spaces, example CVE_2018_9995 Default_Password_CeNova", Separator = ',')]
        public IEnumerable<string> Exploits { get; set; }

        public InputType GetInputType()
        {
            if (ShowExploit != null)
                return InputType.ListExploit;

            if (SingleHost != null)
                return InputType.SingleHost;

            if (ListHost != null)
                return InputType.ListHost;

            return ShodanFile != null ? InputType.Shodan : InputType.None;
        }
    }
}