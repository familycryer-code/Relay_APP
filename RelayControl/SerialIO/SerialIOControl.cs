using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace CharlieSerialIO
{
    public static class SerialIOControl
    {
        public static void OpenSerialPort(System.IO.Ports.SerialPort serialPort)
        {
            try
            {
                serialPort.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Error Opening Port:\n" + e.Message);
            }
        }

        public static void CloseSerialPort(System.IO.Ports.SerialPort serialPort)
        {
            if(serialPort.IsOpen)
                serialPort.Close();
            
        }

        public static void SendCompleteArray(char[] cArray, System.IO.Ports.SerialPort serialPort)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Write(cArray, 0, cArray.Length);
                }
                catch (Exception e)
                {
                    throw new Exception("Error Writing to Port:\n" + e.Message);
                }
                finally
                {
                    CloseSerialPort(serialPort);
                }
            }
            else
            {
                throw new Exception("Port Is Not Open");
            }
        }

        public static void SendCompleteArray(byte[] bArray, System.IO.Ports.SerialPort serialPort)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Write(bArray, 0, bArray.Length);
                }
                catch (Exception e)
                {
                    throw new Exception("Error Writing to Port:\n" + e.Message);
                }
                finally
                {
                    CloseSerialPort(serialPort);
                }
            }
            else
            {
                throw new Exception("Port Is Not Open");
            }
        }

    }

    public class MySerialPort:SerialPort
    {
        public void SendPacket(char[] array)
        {

        }

        public void SendPacket(byte[] array)
        {
        }
    }
}
