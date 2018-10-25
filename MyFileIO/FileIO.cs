using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MyFileIO
{
    public class MyFile
    {
        public MyFile(string path)
        {
            if (!File.Exists(path))
            {
                this.myFile = File.Create(path);
                this.Path = path;
                this.myFile.Close();
            }
            else
            {
                this.Path = path;
            }
        }

        private FileStream myFile;
        private StreamReader myStreamReader;
        private StreamWriter myStreamWriter;
        public string Path;
        public bool IsOpen = false;

        public void Open()
        {
            try
            {
                this.myFile = new FileStream(this.Path, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Opening File: " + this.Path + ex.Message, ex);
            }
            this.IsOpen = true;
        }

        public void Open(FileAccess fA)
        {
            try
            {
                this.myFile = new FileStream(this.Path, FileMode.Open, fA);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Opening File: " + this.Path + ex.Message, ex);
            }
            this.IsOpen = true;
        }

        public void Close()
        {
            if (this.IsOpen)
            {
                if (this.myStreamReader != null)
                    this.myStreamReader.Close();
                if (this.myStreamWriter != null)
                    this.myStreamWriter.Close();
                this.myFile.Close();
            }
            this.IsOpen = false;
        }

        public string ReadWholeFile()
        {
            string fileString;

            try
            {
                this.Open(FileAccess.Read);
                this.myStreamReader = new StreamReader(this.myFile);
            }
            catch
            {
                this.IsOpen = false;
                throw new Exception("Error In Read Access For File");
            }
            fileString = this.myStreamReader.ReadToEnd();
            this.Close();

            return fileString;
        }

        public void WriteWholeFile(string p)
        {
            try
            {
                this.Open(FileAccess.ReadWrite);
                this.myStreamWriter = new StreamWriter(this.myFile);
            }
            catch
            {
                this.IsOpen = false;
                throw new Exception("Error Accessing File For Write");
            }
            this.myStreamWriter.WriteLine(p);
            this.Close();
        }
    }
}
