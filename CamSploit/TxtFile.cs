﻿using System;
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

        public void Open()
        {
            _streamReader = new StreamReader(_path);
        }

        public string ReadLine()
        {
            return _streamReader.ReadLine();
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

        public void Dispose()
        {
            if (_streamReader != null)
                _streamReader.Dispose();

            if (_streamWritter != null)
                _streamWritter.Dispose();
        }
    }
}