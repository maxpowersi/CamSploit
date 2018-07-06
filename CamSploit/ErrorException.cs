using System;

namespace CamSploit
{
    /// <summary>
    /// This class represents an error in the execution of the normal flow code (not for error in exploits)
    /// </summary>
    public class ErrorException : Exception
    {
        public ErrorException(string msg) :base (msg)
        {
            
        }
    }
}