using System;
using System.IO;
using System.Reflection;
using ExploitMaker;

namespace CamSploit
{
    public class Writter : IWritter
    {
        private TxtFile _txtFile;

        public Writter(string outputPath)
        {
            if (!Path.IsPathRooted(outputPath))
                outputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), outputPath);
            
            Init(outputPath);
        }

        public void Dispose()
        {
            _txtFile.Dispose();
        }

        public void InitTest(string cve, Camera cam)
        {
            Console.WriteLine(Phrases.Init_Test, cve, cam.Address);
        }

        public void TestFailed(string cve, Camera cam)
        {
            Console.WriteLine(Phrases.Test_File, cam.Address, cve);

            _txtFile?.Write(cam + ",,," + cve);
        }

        public void TestSuccess(string cve, Camera cam, Credencial cred)
        {
            Console.WriteLine(Phrases.Test_Success, cam.Address, cve, cred);

            _txtFile?.Write(cam + "," + cred.Username + "," + cred.Password + "," + cve);
        }

        public void LogError(string cve, Camera cam, string error)
        {
            Console.WriteLine(error, cam.Address, cve);
        }

        private void Init(string output)
        {
            _txtFile = new TxtFile(output);
            _txtFile.Create();
        }
    }
}