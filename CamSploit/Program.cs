using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using ExploitMaker;

namespace CamSploit
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<Options>(args).WithParsed(Process);
            }
            catch (ErrorException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private static void Process(Options opts)
        {
            if (opts.GetInputType() == InputType.ListExploit)
            {
                string desc;
                if (opts.ShowExploit.ToUpper() == "ALL")
                {
                    desc = string.Join("\n", ExploitHelper.GetAllCommonName());
                }
                else
                {
                    var exploit = ExploitHelper.GetExploit(opts.ShowExploit);
                    desc = exploit == null ? Phrases.Invalid_Common_Name : exploit.Description;
                }

                Console.WriteLine(desc);
            }
            else
                using (var writter = new Writter(opts.Output))
                {
                    IEnumerable<Camera> cams = null;
                    switch (opts.GetInputType())
                    {
                        case InputType.SingleHost:
                            cams = CamLoader.LoadFromHost(opts.SingleHost);
                            break;
                        case InputType.ListHost:
                            cams = CamLoader.LoadFromTextFile(opts.ListHost);
                            break;
                        case InputType.Shodan:
                            cams = CamLoader.LoadFromShodanJsonFile(opts.ShodanFile);
                            break;
                         case InputType.None:
                             throw new ErrorException(Phrases.Invalid_Main_Action);
                    }

                    var exploits = opts.Exploits != null && opts.Exploits.Any() ? ExploitHelper.GetExploits(opts.Exploits) : ExploitHelper.GetAll();

                    foreach (var cam in cams)
                    {
                        foreach (var e in exploits)
                        {
                            writter.InitTest(e.CommonName, cam);

                            var credencials = e.Run(cam.UrlHttp);

                            if (credencials != null)
                                writter.TestSuccess(e.CommonName, cam, credencials);
                            else
                                writter.TestFailed(e.CommonName, cam);
                        }
                    }
                }
        }

    }
}