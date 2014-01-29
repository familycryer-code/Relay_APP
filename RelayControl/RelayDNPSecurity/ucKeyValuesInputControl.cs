using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayDNPSecurity
{
    public partial class ucKeyValuesInputControl : UserControl
    {
        public ucKeyValuesInputControl(int numberOfValues, string groupBoxName)
        {
            InitializeComponent();
            if(numberOfValues % 16 != 0)
                throw new Exception("Number of Values must be divisible by 16");

            this.generateBoxes(numberOfValues);
            this.setGroupBox(numberOfValues, groupBoxName);

            this.Size = this.groupBox.Size;
            this.Controls.Add(this.groupBox);
            this.keyLength = numberOfValues;
        }

        private static int _boxWidth = 30;
        private static int _boxHeight = 20;
        private static int _locationOffsetX = 5;
        private static int _locationOffsetY = 15;
        private static int _spacingX = 5;
        private static int _spacingY = 5;

        private GroupBox groupBox;
        private int keyLength;
        #region Initialization

        private void generateBoxes(int numberOfValues)
        {
            for (int i = 0; i < numberOfValues; ++i)
            {
                TextBox workingTB;
                Point boxLocation = new Point(_locationOffsetX + (i%16 * (_boxWidth + _spacingX)), _locationOffsetY + ((i / 16) * (_boxHeight + _spacingY)));

                workingTB = new TextBox();
                workingTB.Height = _boxHeight;
                workingTB.Width = _boxWidth;
                workingTB.Location = boxLocation;
                workingTB.TextChanged += textBoxDataChanged;

                this.Controls.Add(workingTB);
            }
        }

        private void setGroupBox(int numberOfValues, string groupBoxName)
        {
            this.groupBox = new GroupBox();
            this.groupBox.Text = groupBoxName;
            this.groupBox.Height = ((numberOfValues / 16) + 1) * (_boxHeight + _spacingY) - _spacingY;
            this.groupBox.Width = 16 * (_spacingX + _boxWidth) + _locationOffsetX;
            this.groupBox.Location = new Point(0, 0);
        }

        #endregion

        public void SetKey(byte[] dataArray)
        {
            if (dataArray.Length % 16 != 0 || dataArray.Length == 0 || dataArray == null || dataArray.Length > this.keyLength)
            {
                throw new Exception("Key Length Invalid");
            }

            int i = 0;

            foreach (object o in this.Controls)
            {    
                TextBox tB = new TextBox();
                try
                {
                    tB = (TextBox)o;
                }
                catch
                {
                    continue;
                    // because it isn't a textbox
                }

                StringBuilder hexString = new StringBuilder(2);
                if (i == dataArray.Length)
                    break;
                hexString.AppendFormat("{0:x2}", dataArray[i++]);

                tB.Text = hexString.ToString();
            }
        }

        #region Validation

        public byte[] GetKey()
        {
            byte[] returnArray = null;

            if(!this.KeyDataValid()){
                return new byte[0];
            }

            returnArray = getKeyData();
            return returnArray;
        }



        public bool KeyDataValid()
        {
            if (!this.properNumberOfBoxesFilled())
            {
                throw new Exception("Boxes must be filled consecutively and must be a multiple of 16");
            }

            if (!this.dataInBoxesValid())
            {
                throw new Exception("Invalid Data in Boxes");
            }

            return true;
        }

        private bool properNumberOfBoxesFilled()
        {
            int numberOfConsecutiveBoxesFilled = 0;
            bool incompleteBoxFound = false;

            foreach (object o in this.Controls)
            {
                TextBox tB = new TextBox();
                try
                {
                    tB = (TextBox)o;
                }
                catch
                {
                    //Do nothing because it isn't a TextBox
                    //Just suppressing the error
                }

                try
                {
                    if (tB.Text.Length == 2)
                    {
                        if (!incompleteBoxFound)
                            numberOfConsecutiveBoxesFilled++;
                        else
                        {
                            return false;
                        }
                    }
                    else
                        incompleteBoxFound = true;
                }
                catch
                {
                    return false;
                }
            }

            if (numberOfConsecutiveBoxesFilled % 16 == 0 && numberOfConsecutiveBoxesFilled != 0)
                return true;
            else
                return false;
        }

        private bool dataInBoxesValid()
        {
            bool returnValue = false;
            TextBox workingTextBox = null;

            foreach (object o in this.Controls)
            {
                try
                {
                    workingTextBox = (TextBox)o;
                }
                catch
                {
                    //Do nothing because it isn't a TextBox
                    //Just suppressing the error
                }

                try
                {
                    // Check to see if we have passed the last filled box
                    if (workingTextBox.Text.Length == 0)
                        break;
                    if (!this.singleBoxDataIsValid(workingTextBox.Text))
                        throw new Exception();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private bool singleBoxDataIsValid(string p)
        {
            try
            {
                Convert.ToByte(p, 16);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private byte[] getKeyData()
        {
            TextBox workingTextBox = null;
            byte[] returnArray;
            List<byte> values = new List<byte>();

            foreach (object o in this.Controls)
            {
                try
                {
                    workingTextBox = (TextBox)o;
                }
                catch
                {
                    //Do nothing because it isn't a TextBox
                    //Just suppressing the error
                    break;
                }

                try
                {
                    // Check to see if we have passed the last filled box
                    if (workingTextBox.Text.Length != 2)
                        break;

                    values.Add(Convert.ToByte(workingTextBox.Text, 16));
                }
                catch
                {
                    return null;
                }
            }
            returnArray = new byte[values.Count];

            for (int i = 0; i < returnArray.Length; ++i)
            {
                returnArray[i] = values[i];
            }

            return returnArray;
        }

        private byte singleBoxByteValue(string p)
        {
            throw new NotImplementedException();
        }

        private void textBoxDataChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox workingTB = (TextBox)sender;
                string workingString = workingTB.Text;

                if (workingString.Length == 2)
                {
                    TextBox tempBox = this.getTextBoxAfter(workingTB);
                    tempBox.Focus();
                    tempBox.SelectAll();
                }
                else if (workingString.Length > 2)
                {
                    workingTB.Text = workingString.Substring(0, 2);
                }
            }
            catch
            {
                //TODO
            }
        }
        #endregion

        private TextBox getTextBoxAfter(TextBox tB)
        {
            TextBox returnBox = null;

            if (this.Controls.Count > 2) //2 is a guess, is Controls Empty when we initialize this?
            {
                if (tB == null)
                {
                    return (TextBox)this.Controls[0];
                }
                else
                {
                    foreach (object o in this.Controls)
                    {
                        TextBox workingTB = null;
                        try
                        {
                            workingTB = (TextBox)o;
                        }
                        catch
                        {
                            throw new Exception("Not in a Key Value TextBox");
                        }

                        try
                        {
                            if (workingTB == tB)
                            {
                                returnBox = (TextBox)this.Controls[this.Controls.IndexOf(tB) + 1];
                                break;
                            }
                        }
                        catch
                        {
                            // Last TextBox
                            returnBox = tB;
                        }
                    }
                }
            }

            if (returnBox != null)
                return returnBox;
            else
                throw new Exception("Error getting Next Text Box in Key Values");
        }
    }
}
