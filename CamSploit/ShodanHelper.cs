using Shodan.Net.Models;

namespace CamSploit
{
    public static class ShodanHelper
	{
		public static SearchQueries Search(string apiKey, string query)
		{
			var client = new Shodan.Net.ShodanClient(apiKey);

            var task = client.SearchQueriesAsync(query);

            task.RunSynchronously();

            return task.Result;
        }
    }
}