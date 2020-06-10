using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class CommTradeConverter : UserControl
    {
        public CommTradeConverter()
        {
            InitializeComponent();
        }

        private SavedSingleEvent savedEvent;
        private const int _arraySize = 8192;
        private string lDFfilename;

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {

            if (savedEvent == null)
            {
                MessageBox.Show("Please Select File First");
                return;
            }
            SaveFileDialog saveLDFDialog = new SaveFileDialog();
            saveLDFDialog.Title = "Save Live Data";
            saveLDFDialog.InitialDirectory = @"C:\DGI Systems\Relay\Saved Data";
            saveLDFDialog.Filter = "Live Data File |*.ldf";
            saveLDFDialog.FileName = lDFfilename;
            saveLDFDialog.ShowDialog();

            if (saveLDFDialog.FileName != "")
            {
                using (Stream stream = File.Open(saveLDFDialog.FileName, FileMode.Create))
                {
                    BinaryFormatter bF = new BinaryFormatter();
                    bF.Serialize(stream, savedEvent);
                }
            }
        }


        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {

                openFileDialog.InitialDirectory = @"C:\Users\Charles\source\repos\work\relay\comtrade-converter\resources";
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    labelInputFileName.Text = openFileDialog.FileName;
                    setLDFFileName(Path.GetFileName(openFileDialog.FileName));
                    try
                    {
                        var fileStream = openFileDialog.OpenFile();
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            var fileContent = reader.ReadToEnd();
                            storeData(fileContent);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Opening File", ex.ToString());
                    }
                }
            }
        }

        private void setLDFFileName(string v)
        {
            // Strip off the end letter
            lDFfilename = v.Substring(0, v.Length - 4);
            lDFfilename += ".ldf";
        }

        private void storeData(string fileContent)
        {
            var lines = fileContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            savedEvent = new SavedSingleEvent(_arraySize);

            /*
             * Order Of Data from CommTrade is IA > IB > IC > VA > VB > VC
             */

            savedEvent.IA = getArray(lines[0]);
            savedEvent.IB = getArray(lines[1]);
            savedEvent.IC = getArray(lines[2]);
            savedEvent.VnA = getArray(lines[3]);
            savedEvent.VnB = getArray(lines[4]);
            savedEvent.VnC = getArray(lines[5]);

            savedEvent.ID = 1;
            savedEvent.Time = DateTime.UtcNow;
            savedEvent.Type = EventTypes.NoEvent;
        }

        private float[] getArray(string v)
        {
            float[] returnArray = new float[8192];

            var values = Array.ConvertAll(v.Split(','), float.Parse);

            for (int i = 0; i < values.Length && i < returnArray.Length; i++)
            {
                returnArray[i] = values[i];
            }

            return returnArray;
        }
    }
}
