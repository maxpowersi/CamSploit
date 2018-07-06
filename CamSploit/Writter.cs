using System;
using System.IO;
using System.Reflection;
using ExploitMaker;
using ExploitMaker.Exceptions;
using ExploitMaker.Modules;

namespace CamSploit
{
    public class Writter:IDisposable
    {
        private readonly StreamWriter _txtFile;

        private string _fail = "Fail";
        private string _notVulnerable = "NotVulnerable";
        private string _vulnerable= "Vulnerable";
        private string _unreachable = "Unreachable";
        
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

        public void InitTest(string module, Camera cam)
        {
            Console.WriteLine(Phrases.Init_Test, module, cam.Address);
        }

        public void LogResult(string module, Camera cam, ExploitResult exploitResult)
        {
            if (exploitResult.Result)
            {
                Console.WriteLine(exploitResult.ScreenMessage);
                
                _txtFile.WriteLine(string.Join(',', cam, exploitResult.Credencials.Username, exploitResult.Credencials.Password, module, _vulnerable, CleanString(exploitResult.Comment)));
                _txtFile.Flush();
            }
            else
            {
                Console.WriteLine(exploitResult.ScreenMessage);
                
                _txtFile.WriteLine(string.Join(',', cam, "", "", module, _notVulnerable, CleanString(exploitResult.Comment)));
                _txtFile.Flush();
            }
        }

        public void ExploitExecutionFailed(ExploitFailException ex)
        {
            Console.WriteLine(ex.ScreenMessage);
            
            _txtFile.WriteLine(string.Join(',', ex.Camera, "null", "null", ex.CommonName, _fail, CleanString(ex.Message)));
            _txtFile.Flush();
        }
        
        public void ExploitExecutionFailedUnreachableTarget(ExploituUreachableTargetException ex)
        {
            Console.WriteLine(ex.ScreenMessage);
            
            _txtFile.WriteLine(string.Join(',', ex.Camera, "null", "null", ex.CommonName, _unreachable, CleanString(ex.ScreenMessage)));
            _txtFile.Flush();
        }

        /// <summary>
        /// Remove some problematic characters, to writte the messages in csv files
        /// </summary>
        private string CleanString(string toClean)
        {
            return toClean.Replace(',', ' ').Replace('\n', ' ');
        }
    }
}