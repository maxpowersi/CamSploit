namespace CamSploit
{
    public class Credencial
    {
        public Credencial(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public override string ToString()
        {
            return Username +":" + Password;
        }
    }
}