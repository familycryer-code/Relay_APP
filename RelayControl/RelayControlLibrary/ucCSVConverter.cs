using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RelayControlLibrary
{
    public partial class ucCSVConverterCSVFile : UserControl
    {
        public ucCSVConverterCSVFile()
        {
            InitializeComponent();
        }

        private SavedSingleEvent liveData;
        private string openDiagFileName;

        private float convertVoltage(string s)
        {
            const float ConvertValue = 4.0f;
            float retFloat;

            retFloat = ConvertValue * float.Parse(s);

            return retFloat;
        }

        private float convertCurrent(string s)
        {
            const float ConvertValue = 960f;//480000f; //maybe divided by 2500/5
            float retFloat;

            retFloat = ConvertValue * float.Parse(s);

            return retFloat;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialogCSVFile.ShowDialog();
            string[] allLines;

            if (dr == DialogResult.OK && this.openFileDialogCSVFile.FileName.Contains(".csv"))
            {
                this.openDiagFileName = this.openFileDialogCSVFile.FileName;
                try
                {
                    allLines = File.ReadAllLines(this.openFileDialogCSVFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Opening File", ex.Message);
                    return;
                }
                int i = 0, j = 0; //i to count through their data,j to be our respective point

                this.liveData = new SavedSingleEvent(8192);
                this.liveData.Time = DateTime.Now;
                this.liveData.ID = 1234;
                foreach (string s in allLines) //move through file one row at a time
                {
                    string[] allValues = s.Split(',');

                    try
                    {
                        if (i % 4 != 3) //skip every 4 one to "sync" up their sample rate with ours
                        {

                            liveData.VtA[j] = this.convertVoltage(allValues[7]);
                            liveData.VtB[j] = this.convertVoltage(allValues[8]);
                            liveData.VtC[j] = this.convertVoltage(allValues[9]);
                            liveData.VnA[j] = 0;
                            liveData.VnB[j] = 0;
                            liveData.VnC[j] = 0;

                            liveData.IA[j] = this.convertCurrent(allValues[13]);
                            liveData.IB[j] = this.convertCurrent(allValues[14]);
                            liveData.IC[j] = this.convertCurrent(allValues[15]);
                            j++;
                        }
                        i++;

                        if (j >= 8192)
                        {
                            break;
                            throw new Exception("Data Exceeded Live Data Length");
                        }
                    }
                    catch
                    {
                        break;
                    }

                }
                for (; j < 8192; j++)
                {
                    liveData.VtA[j] = 0;
                    liveData.VtB[j] = 0;
                    liveData.VtC[j] = 0;
                    liveData.VnA[j] = 0;
                    liveData.VnB[j] = 0;
                    liveData.VnC[j] = 0;

                    liveData.IA[j] = 0;
                    liveData.IB[j] = 0;
                    liveData.IC[j] = 0;
                }
                MessageBox.Show("Conversion Done");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveEventsDialog = new SaveFileDialog();
            saveEventsDialog.Title = "Save Live Data";
            saveEventsDialog.InitialDirectory = @"C:\DGI Systems\Relay\Saved Data";
            saveEventsDialog.Filter = "Live Data File |*.ldf";

            string fileName = this.openDiagFileName;
            string[] fileParts = fileName.Split('\\');

            fileName = fileParts[fileParts.Length - 1];
            fileName = fileName.Remove(fileName.Length - 4);
            saveEventsDialog.FileName = fileName;

            DialogResult dr = saveEventsDialog.ShowDialog();

            if (saveEventsDialog.FileName != "" && dr == DialogResult.OK)
            {
                using (Stream stream = File.Open(saveEventsDialog.FileName, FileMode.Create))
                {
                    BinaryFormatter bF = new BinaryFormatter();
                    bF.Serialize(stream, this.liveData);
                }
            }
        }
    }
}
