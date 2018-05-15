using System;
using System.Collections;
using System.IO;

namespace CamSploit
{
    public class TxtFile : IDisposable
    {
        private readonly string _path;
        private StreamReader _streamReader;
        private StreamWriter _streamWritter;

        public TxtFile(string path)
        {
            _path = path;
        }

        public void Create()
        {
            _streamWritter = new StreamWriter(_path);
        }

        public void Write(string line)
        {
            _streamWritter.WriteLine(line);
        }

        public void WriteEntity(IEnumerable list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            foreach (var obj in list)
                Write(obj.ToString());
        }

        public void WriteLineBreak()
        {
            _streamWritter.WriteLine();
        }

        public void WriteTabulation()
        {
            _streamWritter.Write("\t");
        }

        public void Open()
        {
            _streamReader = new StreamReader(_path);
        }

        public string ReadAll()
        {
            return _streamReader.ReadToEnd();
        }

        public string ReadLine()
        {
            return _streamReader.ReadLine();
        }

        public string[] ReadLineSplitted(string separator)
        {
            var line = _streamReader.ReadLine();
            return line == null ? null : line.Split(separator.ToCharArray(), StringSplitOptions.None);
        }

        public bool IsEndOfFile
        {
            get { return _streamReader != null && _streamReader.EndOfStream; }
        }

        public void Close()
        {
            if (_streamReader != null)
                _streamReader.Close();

            if (_streamWritter != null)
            {
                _streamWritter.Flush();
                _streamWritter.Close();
            }
        }

        public void FlushChanges()
        {
            if (_streamWritter != null)
                _streamWritter.Flush();
        }

        public void Dispose()
        {
            if (_streamReader != null)
                _streamReader.Dispose();

            if (_streamWritter != null)
                _streamWritter.Dispose();
        }
    }
}