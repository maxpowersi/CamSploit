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
            using (var writter = new Writter(opts.Output))
            {
                ExploitHelper.InitWritter(writter);
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
                                ErrorException(Phrases.Invalid_Main_Action);
                    }

                    var exploits = opts.Exploits != null && opts.Exploits.Any()
                        ? ExploitHelper.GetExploits(opts.Exploits)
                        : ExploitHelper.GetAll();

                    if (cams == null)
                        return;

                    var enumerable = exploits as Exploit[] ?? exploits.ToArray();
                    foreach (var cam in cams)
                    {
                        foreach (var e in enumerable)
                        {
                            try
                            {
                                ExploitHelper.Writter.InitTest(e.CommonName, cam);
                                
                                var cred = e.Run(cam);
                                
                                if(cred != null)
                                    ExploitHelper.Writter.TestSuccess(e.CommonName, cam, cred);
                                else
                                    ExploitHelper.Writter.TestFailed(e.CommonName, cam);
                            }
                            catch (ExploitFailException ex)
                            {
                                Console.WriteLine(ex);
                                ExploitHelper.Writter.TestFailedMsg(ex.CommonName, ex.Camera, ex.Message);
                            }
                        }
                    }
                }
            }
        }
    }
}