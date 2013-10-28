using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FileIO
{
    public class FileIO
    {
        /// <summary>
        /// Constructor for a FileIO object
        /// </summary>
        /// <param name="filePath">Full path of the file to edit or create</param>
        public FileIO(string filePath)
        {
            this.FilePath = filePath;
            //this.OpenFile();
        }

        public string FilePath;

        private FileStream myFileStream;

        private bool fileIsOpen = false;

        public static FileStream OpenFile(string filePath)
        {
            try
            {
                return File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            catch
            {
                throw new Exception("Error Opening or Creating File");
            }
        }

        public bool OpenFile()
        {
            try
            {
                myFileStream = File.Open(this.FilePath, FileMode.OpenOrCreate);
                this.fileIsOpen = true;
                return true;
            }
            catch
            {
                throw new Exception("Error Opening or Creating File");
               
            }
        }

        public static bool AddLine(FileStream localFS, string stringToAdd)
        {

            StreamWriter localStreamWriter = new StreamWriter(localFS);
            
            localStreamWriter.BaseStream.Seek(0, SeekOrigin.End);
            localStreamWriter.WriteLine(stringToAdd);

            localStreamWriter.Close();
            return true;
        }

        public bool AddLine(string stringToAdd)
        {
            if (this.fileIsOpen)
            {
                StreamWriter localStreamWriter = new StreamWriter(myFileStream);
                localStreamWriter.WriteLine(stringToAdd);
                localStreamWriter.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AddString(FileStream fS, string stringToAdd)
        {
            if (fS.CanWrite)
            {
                StreamWriter localSW = new StreamWriter(fS);
                localSW.BaseStream.Seek(0, SeekOrigin.End);
                localSW.Write(stringToAdd);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DeleteFile(string filePath)
        {
            if(File.Exists(filePath))
            {
                DialogResult deleteConfirm = MessageBox.Show("Delete " + filePath + "?", "Confirm Delete?", MessageBoxButtons.YesNo);
                if(deleteConfirm == DialogResult.Yes)
                {
                    File.Delete(filePath);  
                }
            }
            else
            {
                MessageBox.Show("File Does Not Exist");
            }

            return;    
        }
    }
}
