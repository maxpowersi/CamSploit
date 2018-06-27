using System;
using System.IO;
using System.Reflection;
using ExploitMaker;

namespace CamSploit
{
    public class Writter : IWritter
    {
        private readonly StreamWriter _txtFile;

        public Writter(string outputPath)
        {
            if (!Path.IsPathRooted(outputPath))
                outputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), outputPath);

            _txtFile = new StreamWriter(outputPath);
        }

        public void Dispose()
        {
            _txtFile.Dispose();
        }

        public void InitTest(string cve, Camera cam)
        {
            Console.WriteLine(Phrases.Init_Test, cve, cam.Address);
        }

        public void TestSuccess(string cve, Camera cam, Credencial cred)
        {
          
            if (string.IsNullOrEmpty(cred.Username))
                cred.Username = "{null}";
            
            if (string.IsNullOrEmpty(cred.Password))
                cred.Password = "{null}";
            
            Console.WriteLine(Phrases.Test_Success, cam.Address, cve, cred);
            
            _txtFile.WriteLine(string.Join(',', cam, cred.Username, cred.Password, cve, "Success",cred.Message));
            
            _txtFile.Flush();
        }

        public void TestFailed(string cve, Camera cam)
        {
            Console.WriteLine(Phrases.Test_File, cam.Address, cve);
            
            _txtFile.WriteLine(string.Join(',', cam, "null", "null", cve, "Fail", ""));
            _txtFile.Flush();
        }

        public void TestFailedMsg(string cve, Camera cam, string error)
        {
            Console.WriteLine(error);

            if (string.IsNullOrEmpty(error))
                error = "";
            
            _txtFile.WriteLine(string.Join(',', cam, "null", "null", cve, "Fail", error.Replace(',', ' ')));
            _txtFile.Flush();
        }
    }
}