using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using ExploitMaker;
using ExploitMaker.Cam;
using ExploitMaker.Exceptions;
using ExploitMaker.Modules;

namespace CamSploit
{
    public static class Program
    {
        private const string InvalidMainAction = "Invalid main action, plase select one of the following actions: rhost, list-rhost, shodan-file or show-exploit";

        private const string InvalidCommonName = "The entered exploit was not found";
        
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
                Console.WriteLine(ex);
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
                    desc = exploit == null ? InvalidCommonName : exploit.Description;
                }

                Console.WriteLine(desc);
                return;
            }

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
                        throw new
                            ErrorException(InvalidMainAction);
                }

                var exploits = opts.Exploits != null && opts.Exploits.Any()
                    ? ExploitHelper.GetExploits(opts.Exploits)
                    : ExploitHelper.GetAll();

                if (cams == null)
                    return;

                var enumerable = exploits as Module[] ?? exploits.ToArray();
                foreach (var cam in cams)
                {
                    foreach (var e in enumerable)
                    {
                        try
                        {
                            writter.InitTest(e.CommonName, cam);

                            var exploitResult = e.Run(cam);

                            writter.LogResult(e.CommonName, cam, exploitResult);
                        }
                        catch (ExploitFailException ex)
                        {
                            writter.ExploitExecutionFailed(ex);
                        }
                        catch (ExploituUreachableTargetException ex)
                        {
                            writter.ExploitExecutionFailedUnreachableTarget(ex);
                        }
                    }
                }
            }
        }
    }
}