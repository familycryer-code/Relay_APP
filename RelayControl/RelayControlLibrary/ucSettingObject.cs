using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelayControlLibrary
{
    public partial class ucSettingObject : UserControl
    {
        public ucSettingObject(SettingsObject sO)
        {
            InitializeComponent();

            this.toolTip = new ToolTip();
            this.toolTip.SetToolTip(this, sO.ToolTip);

            this.initializeSingleNumbericValue(sO);
        }

        public ucSettingObject(DropDownBoxSettingObject dDBSO)
        {
            InitializeComponent();
            this.initializeEnableValue(dDBSO);
            this.toolTip = new ToolTip();
            this.toolTip.SetToolTip(this, dDBSO.ToolTip);
        }

        private SettingBoxTypes type;

        private void initializeSingleNumbericValue(SettingsObject sO)
        {
            try
            {
                Label name = new Label();
                Label units = new Label();
                NumericUpDown upDown = new NumericUpDown();

                this.type = SettingBoxTypes.Numeric;

                this.SuspendLayout();

                name.Text = sO.Name;
                units.Text = sO.Name;

                if (sO.Minimum > upDown.Maximum)
                {
                    upDown.Maximum = sO.Maximum;
                    upDown.Value = sO.Minimum;
                    upDown.Minimum = sO.Minimum;
                }
                else
                {
                    upDown.Value = sO.Minimum;
                    upDown.Minimum = sO.Minimum;
                    upDown.Maximum = sO.Maximum;
                }

                name.Location = new Point(3, 3);
                name.AutoSize = true;
                upDown.Location = new Point(160, 3);
                units.Location = new Point(upDown.Location.X + upDown.Size.Width, 3);

                this.Controls.Add(name);
                this.Controls.Add(units);
                this.Controls.Add(upDown);
                name.Show();
                units.Show();
                upDown.Show();

                this.PerformLayout();
                this.ResumeLayout();

            }
            catch (Exception ex)
            {
                throw new Exception("Error Creating Setting Box: " + sO.Name, ex);
            }
        }

        private void initializeEnableValue(DropDownBoxSettingObject dDBSO)
        {
            try
            {
                Label name;
                ComboBox cB;
                Point p = new Point(3, 5);

                foreach (SettingsObject sO in dDBSO.DataList)
                {
                    name = new Label();
                    name.Location = p;
                    name.Text = sO.Name;

                    cB = new ComboBox();
                    foreach (string s in sO.DropDownValues)
                    {
                        cB.Items.Add(s);
                    }
                    cB.SelectedIndex = 0;
                    cB.Location = new Point(p.X + 157, p.Y);
                    this.Controls.Add(name);
                    this.Controls.Add(cB);
                    this.Size = new Size(this.Size.Width, this.Size.Height + 25);
                    name.Show();
                    cB.Show();

                    p = new Point(p.X, p.Y + 25);
                }
                this.Size = new Size(this.Size.Width, this.Size.Height - 21); //Did this because it was one too many not sure why 21
            }
            catch (Exception ex)
            {
                throw new Exception("Error Creating Drop Down Box", ex);
            }
        }

        private System.Windows.Forms.ToolTip toolTip;



        public decimal Value
        {
            set
            {
                if (this.type == SettingBoxTypes.Numeric)
                {
                    foreach (object o in this.Controls)
                    {
                        try
                        {
                            NumericUpDown nUP = (NumericUpDown)o;
                            nUP.Value = value;
                            return;
                        }
                        catch { }
                    }
                }
                else if (this.type == SettingBoxTypes.Enables)
                {
                    foreach (object o in this.Controls)
                    {
                        try
                        {
                            ComboBox cB = (ComboBox)o;
                            int tempValue = (int)value;

                            switch (cB.Items.Count)
                            {
                                case 2:
                                    cB.SelectedIndex = tempValue & 1;
                                    tempValue >>= 1;
                                    break;
                                case 3:
                                case 4:
                                    cB.SelectedIndex = tempValue & 3;
                                    tempValue >>= 2;
                                    break;
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                    cB.SelectedIndex = tempValue & 7;
                                    tempValue >>= 3;
                                    break;
                                case 9:
                                case 10:
                                case 11:
                                case 12:
                                case 13:
                                case 14:
                                case 15:
                                case 16:
                                    cB.SelectedIndex = tempValue & 15;
                                    tempValue >>= 4;
                                    break;
                                default:
                                    throw new Exception("Bad Count Number on Combo Box " + this.Name);
                            }
                        }
                        catch { }
                    }

                }
                throw new Exception("Trouble Setting Value");

            }
            get
            {
                if (this.type == SettingBoxTypes.Numeric)
                {
                    foreach (object o in this.Controls)
                    {
                        try
                        {
                            NumericUpDown nUP = (NumericUpDown)o;
                            return nUP.Value;
                        }
                        catch { }
                    }
                }
                else
                {
                    decimal temp = 0;
                    int shiftAmount = 0;

                    foreach (object o in this.Controls)
                    {
                        try
                        {
                            ComboBox cB = (ComboBox)o;

                            temp += (cB.SelectedIndex << shiftAmount);
                            switch (cB.Items.Count)
                            {
                                case 2:
                                    shiftAmount++;
                                    break;
                                case 3:
                                case 4:
                                    shiftAmount += 2;
                                    break;
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                    shiftAmount += 3;
                                    break;
                                case 9:
                                case 10:
                                case 11:
                                case 12:
                                case 13:
                                case 14:
                                case 15:
                                case 16:
                                    shiftAmount += 4;
                                    break;
                                default:
                                    throw new Exception("Bad Count Number on Combo Box " + this.Name);
                            }
                        }
                        catch { }
                    }
                    return temp;
                }
                throw new Exception("Trouble Setting Value");
            }
        }

    }

    public class SettingsObject
    {
        public SettingsObject()
        {
        }

        public SettingsObject(string name, string units, decimal min, decimal max)
        {
            this.Name = name + ":";
            this.Units = units + "*";
            this.Maximum = max;
            this.Minimum = min;
        }
        public SettingsObject(string name, string units, decimal min, decimal max, string toolTip)
        {
            this.Name = name + ":";
            this.Units = units + "*";
            this.Maximum = max;
            this.Minimum = min;
            this.ToolTip = toolTip;
        }

        public string ToolTip;
        public string Name;
        public string Units;
        public decimal Minimum;
        public decimal Maximum;
        public List<string> DropDownValues = new List<string>();
    }

    public class DropDownBoxSettingObject
    {
        public DropDownBoxSettingObject()
        {
        }

        public DropDownBoxSettingObject(List<SettingsObject> l)
        {

        }
        public string ToolTip = "";
        public List<SettingsObject> DataList = new List<SettingsObject>();
    }

    public enum SettingBoxTypes
    {
        Numeric,
        Enables
    }
}
