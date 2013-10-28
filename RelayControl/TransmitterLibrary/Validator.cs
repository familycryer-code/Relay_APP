using System;
using System.Windows.Forms; // For TextBox, and MessageBox controls

namespace TransmitterLibrary
{
	/// <summary>
	/// Provides static methods for validating data.
	/// </summary>
	public class Validator
	{
    /// <summary>
    /// The title that will appear in the dialog boxes.
    /// </summary>
    private static string Title = "Entry Error";

    /// <summary>
    /// Default constructor.
    /// </summary>
		public Validator()
		{
		}
    
		/// <summary>
		/// Checks whether the user entered data into a text box
		/// </summary>
		/// <param name="textBox">The text box control to be validated.</param>
		/// <returns>True if the user has entered data.</returns>
    public static bool IsPresent(TextBox textBox)
		{
			if (textBox.Text.Trim() == "")
			{
        MessageBox.Show("'" + textBox.Tag + "'" + " is a required field. ", Title,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				textBox.Focus();
				return false;
			}
			return true;
		}
    public static bool IsNonZero(TextBox textBox)
    {
      if (Convert.ToUInt32(textBox.Text) == 0)
      {
        MessageBox.Show("'" + textBox.Tag.ToString() + "' is a non-zero integer.", Title,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
      return true;
    }
    public static bool IsUInt16(TextBox textBox)
    {
      try
      {
        Convert.ToUInt16(textBox.Text);
        return true;
      }
      catch (FormatException formatExp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a integer number.\n" + formatExp.Message, "Entry Error",
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
      catch (OverflowException overExp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a integer number.\n" + overExp.Message, "Entry Error",
         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
      catch (Exception exp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a integer number.\n" + exp.Message, "Entry Error",
         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
    }

    public static bool IsUInt32(TextBox textBox)
    {
      try
      {
        Convert.ToUInt32(textBox.Text);
        return true;
      }
      catch (FormatException formatExp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a integer number.\n" + formatExp.Message, Title,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
      catch (OverflowException overExp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a integer number.\n" + overExp.Message, Title,
         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
      catch (Exception exp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a integer number.\n" + exp.Message, Title,
         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
    }

		/// <summary>
		/// Checks whether the user entered a Integer number into a text box
		/// </summary>
		/// <param name="textBox">The text box control to be validated.</param>
		/// <returns>True if the user has entered a valid integer number.</returns>
		private static bool IsInteger(TextBox textBox)
		{
      try
      {
        Convert.ToInt16(textBox.Text);
        return true;
      }
      catch (FormatException formatExp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a integer number.\n" + formatExp.Message, Title,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
      catch (OverflowException overExp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a integer number.\n" + overExp.Message, Title,
         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
      catch (Exception exp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a integer number.\n" + exp.Message, Title,
         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
    }

    private static bool IsSingle(TextBox textBox)
    {
      try
      {
        Convert.ToSingle(textBox.Text);
        return true;
      }
      catch (FormatException formatExp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a number.\n" + formatExp.Message, Title,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
      catch (OverflowException overExp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a number.\n" + overExp.Message, Title,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
      catch (Exception Exp)
      {
        MessageBox.Show("'" + textBox.Tag + "'" + " must be a number.\n" + Exp.Message, Title,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        textBox.Focus();
        return false;
      }
    }

		/// <summary>
		/// Validate the value of vault's specifications.
		/// </summary>
		/// <param name="textBox"></param>
		/// <returns>True if every field is OK.</returns>
    private static bool IsParameterOK(TextBox textBox)
    {
      string tag = textBox.Tag.ToString();
      Single single_value = 0.0f;
      int value = 0;
      if (tag == "VNA Calibration" || tag == "VNB Calibration" || tag == "VNC Calibration" ||
        tag == "VTA Calibration" || tag == "VTB Calibration" || tag == "VTC Calibration")
        single_value = Convert.ToSingle(textBox.Text);
      else
        value = Convert.ToInt16(textBox.Text);
      if (tag == "ID")
        if (value >= 1 && value <= 1023)// [1, 1023]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 1 ~ 1023)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "SRT_ID")
        if (value >= 1 && value <= 16383) // [1, 16383]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 1 ~ 16383)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "CT Multiplier")
        if (value >= 80 && value <= 399)// [80, 399]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 80 ~ 399)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "Current Threshold")
        if (value <= 200)
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 0 ~ 200)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "Voltage Threshold")
        if (value <= 200)
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 0 ~ 200)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "Analog Threshold")
        if (value <= 127)
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 0 ~ 127)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "TYPE2 Message Frequency")
        if (value >= 1 && value <= 23)// [1, 23]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 1 ~ 23)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "CONFIG Message Frequency")
        if (value >= 1 && value <= 23)// [1, 23]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 1 ~ 23)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "MUXBOX Message Frequency")
        if (value >= 1 && value <= 60)// [1, 60]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 1 ~ 60)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "CONFIG Burst Interval" || tag == "ALARM Burst Interval")
        if (value >= 1 && value <= 255)// [1, 255]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 1 ~ 255)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "CONFIG Burst Count" || tag == "ALARM Burst Count")
        if (value >= 2 && value <= 8)// [2, 8]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : 2 ~ 8)", Title,
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "IA Calibration" || tag == "IB Calibration" || tag == "IC Calibration" || tag == "Temperature Calibration")
        if (value >= -127 && value <= 127) // [-127, 127]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : -127 ~ 127)", Title,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "Phase A Calibration" || tag == "Phase B Calibration"
        || tag == "Phase C Calibration")
        if (value >= -50 && value <= 50) // [-50, 50]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : -50 ~ 50)", Title,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "VNA Calibration" || tag == "VNB Calibration" || tag == "VNC Calibration" ||
        tag == "VTA Calibration" || tag == "VTB Calibration" || tag == "VTC Calibration")
        if (single_value >= -10 && single_value <= 10) //[-10, 10]
          return true;
        else
        {
          MessageBox.Show("'" + tag + "'" + " is NOT valid! (range : -10 ~ 10)", Title,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          textBox.Focus();
          return false;
        }
      else if (tag == "X1 AMP" || tag == "X2 AMP" || tag == "X3 AMP" ||
              tag == "X1 AMP on Clamp" || tag == "X2 AMP on Clamp" || tag == "X3 AMP on Clamp")
        return true;
      else
        return true;
    }


    public static bool IsMyIntegerBoxOK(TextBox textBox)
    {
      return (Validator.IsPresent(textBox) &&
              Validator.IsInteger(textBox) &&
              Validator.IsParameterOK(textBox));
    }
    public static bool IsMyFloatBoxOK(TextBox textBox)
    {
      return (Validator.IsPresent(textBox) &&
              Validator.IsSingle(textBox) &&
              Validator.IsParameterOK(textBox));
    }

  }
}
