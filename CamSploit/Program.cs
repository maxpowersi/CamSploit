using System;
using System.Collections.Generic;
using System.Linq;
using CamSploit.CVEs;
using CommandLine;

namespace CamSploit
{
	public static class Program
	{
		static void Main(string[] args)
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
			//For ListExploit main action
			if (opts.GetInputType() == InputType.ListExploit)
				ProcessListExploit(opts.ListHost.ToUpper());
			else
			{
				//For others main action
				using (var writter = new Writter(opts.Output))
				{
					foreach (var cam in GetCams(opts))
					{
						foreach (var e in GetExploits(opts.Exploit))
						{
							writter.InitTest(e.CommonName, cam);

							var credencials = e.Exploit(cam.UrlHttp);

							if (credencials != null)
								writter.TestSuccess(e.CommonName, cam, credencials);
							else
								writter.TestFailed(e.CommonName, cam);
						}
					}
				}
			}
		}

		private static IEnumerable<Camera> GetCams(Options opts)
		{
			switch (opts.GetInputType())
			{
				case InputType.SingleHost:
					return  CamLoader.LoadFromHost(opts.SingleHost);
				case InputType.ListHost:
					return CamLoader.LoadFromTextFile(opts.ListHost);
				case InputType.Shodan:
					return CamLoader.LoadFromShodanJsonFile(opts.ShodanFile);
				default:
					throw new Exception(Phrases.Invalid_Main_Action);
			}
		}

		private static IEnumerable<IExploit> GetExploits(IEnumerable<string> exploits)
		{
			//TODO
			return exploits.Select(x => x.ToUpper()).Contains("ALL") ? ExploitHelper.GetAll() : ExploitHelper.GetExploits(exploits);
		}

		private static void ProcessListExploit(string commonName)
		{
			string desc;
			if (commonName == "ALL")
				desc = string.Join("\n", ExploitHelper.GetAllCommonName());
			else
			{
				var exploit = ExploitHelper.GetExploit(commonName);
				desc = exploit == null ? Phrases.Invalid_Common_Name : exploit.Description;
			}

			Console.WriteLine(desc);
		}
	}
}