using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CamSploit
{
    public static class CamLoader
    {
        public static IEnumerable<Camera> LoadFromTextFile(string filePath)
        {
            using (var file = new TxtFile(filePath))
            {
                file.Open();
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var splitted = line.Split (':');

                    var cam = new Camera(splitted[0], splitted[1]);
                    yield return cam;
                }

                file.Close();
            }
        }
        
        public static IEnumerable<Camera> LoadFromShodanJsonFile(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    dynamic json = JObject.Parse(line);

                    var cam = new Camera(json.http.host, json.port)
                    {
                        Country = json.location.country_name,
                        City = json.location.city,
                        Description = json.title
                    };

                    yield return cam;
                }

                fileReader.Close();
            }
        }
        
        public static IEnumerable<Camera> LoadFromHost(string ip, string port)
        {
            yield return new Camera(ip, port);
        }        
    }
}