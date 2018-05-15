using System.IO;
using System.Collections.Generic;
using Shodan.Net.Models;
using Newtonsoft.Json.Linq;

namespace CamSploit
{
    public static class ShodanHelper
	{
		public static IEnumerable<Camera> LoadJsonResult(string filePath)
		{
			using (var fileReader = new StreamReader(filePath))
			{
				string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    dynamic json = JObject.Parse(line);

                    var cam = new Camera(json.http.host, json.port);
                    cam.Country = json.location.country_name;
                    cam.City = json.location.city;
                    cam.Description = json.title;

                    yield return cam;
                }

				fileReader.Close();
			}
		}

		public static SearchQueries Search(string apiKey, string query)
		{
			var client = new Shodan.Net.ShodanClient(apiKey);

            var task = client.SearchQueriesAsync(query);

            task.RunSynchronously();

            return task.Result;
        }
    }
}