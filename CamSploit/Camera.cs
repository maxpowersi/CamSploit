namespace CamSploit
{
	public class Camera
	{
		public Camera (string host, string port)
		{
			Port = port;
			Host = host;
		}

		public string Host { get; set; }

		public string Port { get; set; }

        public string UrlHttp { get { return "http://" + Host + ":" + Port; } }

		public string Address { get { return Host + ":" + Port; } }

        public string Country { get; set; }

        public string City { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Host + "," + Port + "," + UrlHttp + "," + Description + "," + Country + "," + City;
        }
    }
}