using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace RelayControl
{
    public partial class TCPConnectionForm : Form
    {
        public TCPConnectionForm()
        {
            InitializeComponent();
            populateIPAddressesComboBox();
        }

        public TCPConnectionForm(IPAddress iPAddress, int Port)
        {
            InitializeComponent();
            ipAddressControl.IPAddress = iPAddress;
            numericUpDownPort.Value = Port;
            populateIPAddressesComboBox();
        }

        public IPAddress IPAddress;
        public int Port;

        private List<IPAddressInfo> iPAddressInfos = new List<IPAddressInfo>();
        private static string _savedDirectory = @".\Saved Data\";
        private static string _addressesFile = _savedDirectory + "ip_addresses.json";

        private void populateIPAddressesComboBox()
        {
            try
            {
                if (File.Exists(_addressesFile))
                {
                    using (StreamReader sR = File.OpenText(_addressesFile))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        var json = sR.ReadToEnd();
                        iPAddressInfos = (List<IPAddressInfo>)JsonConvert.DeserializeObject<List<IPAddressInfo>>
                            (json, new IPAddressConverter());
                    }
                    // If there is nothing in the file, just return to avoid error.
                    if (iPAddressInfos == null)
                    {
                        iPAddressInfos = new List<IPAddressInfo>();
                        return;
                    }
                    comboBoxIPAddresses.DataSource = iPAddressInfos;
                }
                comboBoxIPAddresses.SelectedIndex = -1;
                comboBoxIPAddresses.ResetText();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error in IPAddress Save File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSetIP_Click(object sender, EventArgs e)
        {
            IPAddress = ipAddressControl.IPAddress;
            Port = (int)numericUpDownPort.Value;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonSaveIP_Click(object sender, EventArgs e)
        {
            saveIPAddress();
        }

        private void saveIPAddress()
        {
            IPAddressInfo info = new IPAddressInfo();
            // Check for valid data
            if (textBoxIPLabel.Text == "" || textBoxIPLabel.Text == null)
            {
                MessageBox.Show("Please give IP Address a name");
                return;
            }
            info.IPAddress = ipAddressControl.IPAddress;
            info.Port = (int)numericUpDownPort.Value;
            info.Name = textBoxIPLabel.Text;
            // Check if it exists
            var valueExists = iPAddressInfos.Find(x => x.Name == info.Name);

            // if it does exist, remove it
            if (valueExists != null)
            {
                // Check to make sure they want to override it
                var result = MessageBox.Show(
                    String.Format("Are you sure you want to overwrite {0}?", info.Name), "Overwrite?", MessageBoxButtons.YesNo);

                if (result != DialogResult.Yes)
                    return;
                iPAddressInfos.Remove(valueExists);
            }

            iPAddressInfos.Add(info);

            writeIPInfosToFile();
            populateIPAddressesComboBox();
        }

        private void writeIPInfosToFile()
        {
            var json = JsonConvert.SerializeObject(iPAddressInfos,
                Formatting.Indented, new IPAddressConverter());
            if (!Directory.Exists(_savedDirectory))
                Directory.CreateDirectory(_savedDirectory);

            File.WriteAllText(_addressesFile, json);
        }

        private void buttonDeleteIP_Click(object sender, EventArgs e)
        {
            var info = (IPAddressInfo)comboBoxIPAddresses.SelectedItem;

            // Check to make sure something was selected and that it exists
            if (info == null)
                return;
            // Check to make sure they want to override it
            var result = MessageBox.Show(
                String.Format("Are you sure you want to delete {0}?", info.Name), "Delete?", MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
                return;
            iPAddressInfos.Remove(info);
            writeIPInfosToFile();
            populateIPAddressesComboBox();
        }


        private void comboBoxIPAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If -1 is selected, nothing is selected so ignore it.
            if (comboBoxIPAddresses.SelectedIndex == -1)
                return;
            populateInputs((IPAddressInfo)comboBoxIPAddresses.SelectedItem);
        }

        private void populateInputs(IPAddressInfo addressInfo)
        {
            ipAddressControl.IPAddress = addressInfo.IPAddress;
            numericUpDownPort.Value = addressInfo.Port;
        }
    }

    public class IPAddressInfo
    {
        public IPAddress IPAddress;
        public int Port;
        public string Name;

        public static IPAddressInfo GetIPAddressInfo(string formattedString)
        {
            IPAddressInfo returnVal = new IPAddressInfo();

            var strings = formattedString.Split(' ');

            returnVal.Name = strings[0].Replace(" ", String.Empty);
            returnVal.Port = Convert.ToInt32(strings[5]);
            IPAddress.TryParse(strings[3], out returnVal.IPAddress);
            return returnVal;
        }

        public override string ToString()
        {
            return String.Format("{0} - IPAddress: {1} : {2}", Name, IPAddress, Port);
        }
    }

    class IPAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPAddress));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IPAddress returnValue = new IPAddress(0);
            IPAddress.TryParse((string)reader.Value, out returnValue);

            return returnValue;
        }
    }
}
