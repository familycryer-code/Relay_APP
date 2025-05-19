using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using System.IO;
//using SineDisplayGraph; 

namespace RelayControlLibrary
{
    public static class RelayModeFunctions
    {
        public const byte DC4 = 0x0D;
        public const char _AdjustCalibrationOpCode = 'A';
        public const char _CloseOpCode = 'C';
        public const char _CloseCurveOpCode = 'C';
        public const char _DeadNetworkOpCode = 'D';
        public const char _InsensitiveOpCode = 'I';
        public const char _ModeOpCode = 'M';
        public const char _MagnitudeOpCode = 'M';
        public const char _NoCurveOpCode = 'N';
        public const char _NormalRecloseOpCode = 'N';
        public const char _RelaxCloseOpCode = 'r';
        public const char _RelaxCircleOpCode = 's';
        public const char _OffsetAngleOpCode = 'O';
        public const char _RemoteTripOpCode = 'R';
        public const char _WattVarTripOpCode = 'W';
        public const char _SensitiveOpCode = 'S';
        public const char _TripOpCode = 'T';
        public const char _TimeDelayOpCode = 'T';
        public const char _TripCurveOpCode = 'T';
        public const char _UpdateCurveOpCode = 'U';
        public const char _PumpModeOpCode = 'G';
        public const char _CircleCloseOpCode = 'C';
        public const char _DNPControlOpCode = 'D';
        public const char _DNPDataRequestOpCode = 'U';
        public const char _PermissiveClose = 'L';
        public const char _AdaptiveTripOpCode = 'P';

        #region Trip Mode Functions

        public static char CharRepresentationOf(TripModes tM)
        {
            switch (tM)
            {
                case TripModes.Sensitive:
                    return _SensitiveOpCode;

                case TripModes.Insensitive:
                    return _InsensitiveOpCode;

                case TripModes.TimeDelay:
                    return _TimeDelayOpCode;

                case TripModes.RemoteTrip:
                    return _RemoteTripOpCode;

                case TripModes.WattVar:
                    return _WattVarTripOpCode;
                default:
                    return '\0';
            }
        }

        public static byte ByteRepresentationOf(TripModes tM)
        {
            switch (tM)
            {
                case TripModes.Sensitive:
                    return (byte)_SensitiveOpCode;

                case TripModes.Insensitive:
                    return (byte)_InsensitiveOpCode;

                case TripModes.TimeDelay:
                    return (byte)_TimeDelayOpCode;

                //case TripModes.Adaptive:
                //    return (byte)_AdaptiveTripOpCode;

                case TripModes.RemoteTrip:
                    return (byte)_RemoteTripOpCode;
                case TripModes.WattVar:
                    return (byte)_WattVarTripOpCode;
                default:
                    return 0;
            }
        }



        public static byte ByteFrom(TripCurveTypes tCT)
        {

            switch (tCT)
            {
                case TripCurveTypes.OffsetAngle:
                    return (byte)_OffsetAngleOpCode;
                case TripCurveTypes.Magnitude:
                    return (byte)_MagnitudeOpCode;
                case TripCurveTypes.WattVar:
                    return (byte)_WattVarTripOpCode;
                case TripCurveTypes.NoCurve:
                default:
                    return (byte)_NoCurveOpCode;
            }
        }

        public static char CharFrom(TripCurveTypes tCT)
        {
            char result;
            if (tCT == TripCurveTypes.OffsetAngle)
                result = _OffsetAngleOpCode;
            else if (tCT == TripCurveTypes.Magnitude)
                result = _MagnitudeOpCode;
            else if (tCT == TripCurveTypes.NoCurve)
                result = _NoCurveOpCode;
            else
                throw new Exception("You never cease to amaze me.  CharFrom(TripCurveTypes)");

            return result;
        }
        public static TripModes TripModeFrom(char c)
        {
            switch (c)
            {
                case 'I':
                    return TripModes.Insensitive;
                case 'S':
                    return TripModes.Sensitive;
                case 'T':
                    return TripModes.TimeDelay;
                case 'W':
                    return TripModes.WattVar;
              //  case 'A':
              //      return TripModes.Adaptive;
                default:
                    throw new Exception("Unrecognized Character");
            }
        }
        public static TripModes TripModeFrom(string s)
        {
            TripModes tM;

            if (s == "Sensitive" || s == "S")
            {
                tM = TripModes.Sensitive;
            }
            else if (s == "Insensitive" || s == "I")
            {
                tM = TripModes.Insensitive;
            }
            else if (s == "Time Delay" || s == "T")
            {
                tM = TripModes.TimeDelay;
            }
            else if (s == "Remote Trip" || s == "R")
            {
                tM = TripModes.RemoteTrip;
            }
            else if (s == "Watt-Var" || s == "W")
            {
                tM = TripModes.WattVar;
            }
           /* else if (s == "Adaptive" || s == "A")
            {
                tM = TripModes.Adaptive;
            }*/
            else
            {
                throw new Exception("Unrecognized Input String");
            }

            return tM;
        }

        public static string StringRepresentationOf(TripModes tM)
        {
            switch (tM)
            {
                case TripModes.Insensitive:
                    return "Insensitive";
                case TripModes.RemoteTrip:
                    return "Remote Trip";
                case TripModes.Sensitive:
                    return "Sensitive";
                case TripModes.TimeDelay:
                    return "Time Delay";
                case TripModes.WattVar:
                    return "Watt-Var";
             //   case TripModes.Adaptive:
             //       return "Adaptive";
                default:
                    throw new Exception("Bad Trip Mode Value");
            }
        }
        public static byte ByteRepresentationOf(Phases p)
        {
            switch (p)
            {
                case Phases.A:
                    return (byte)'A';
                case Phases.B:
                    return (byte)'B';
                case Phases.C:
                    return (byte)'C';
                case Phases.Effective:
                    return (byte)'E';
                case Phases.NegativeSeq:
                    return (byte)'N';
                case Phases.PositiveSeq:
                    return (byte)'P';
                case Phases.TotalAverage:
                    return (byte)'T';
                default:
                    throw new Exception("Unrecognized Phase");
            }
        }

        public static byte ByteRepresentationOf(PhaseTypes pT)
        {
            switch (pT)
            {
                case PhaseTypes.Current:
                    return (byte)'I';
                case PhaseTypes.DifferentialVoltage:
                    return (byte)'D';
                case PhaseTypes.NetworkVoltage:
                    return (byte)'N';
                case PhaseTypes.TransformerVoltage:
                    return (byte)'T';
                case PhaseTypes.Power:
                    return (byte)'P';
                default:
                    throw new Exception("Unrecognized Phase Type");
            }
        }

        public static Phases PhaseFrom(string s)
        {
            switch (s)
            {
                case "PhaseA":
                case "A":
                    return Phases.A;
                case "PhaseB":
                case "B":
                    return Phases.B;
                case "PhaseC":
                case "C":
                    return Phases.C;
                default:
                    throw new Exception(s + " is not a recognized Phase string");
            }
        }

        public static Phases PhaseFrom(char c)
        {
            switch (c)
            {
                case 'A':
                case 'a':
                    return Phases.A;
                case 'B':
                case 'b':
                    return Phases.B;
                case 'C':
                case 'c':
                    return Phases.C;
                case 'T':
                case 't':
                    return Phases.TotalAverage;
                case 'p':
                case 'P':
                    return Phases.PositiveSeq;
                case 'N':
                case 'n':
                    return Phases.NegativeSeq;
                case 'E':
                case 'e':
                    return Phases.Effective;
                default:
                    throw new Exception(c + " is not a recognized Phase string");
            }
        }

        public static PhaseTypes PhaseTypesFrom(string s)
        {
            switch (s)
            {
                case "NetworkVoltage":
                    return PhaseTypes.NetworkVoltage;
                case "TransformerVoltage":
                    return PhaseTypes.TransformerVoltage;
                case "Current":
                    return PhaseTypes.Current;
                case "DifferentialVoltage":
                    return PhaseTypes.DifferentialVoltage;
                default:
                    throw new Exception(s + " is not a valid Phase Type String");
            }
        }

        public static PhaseTypes PhaseTypesFrom(char c)
        {
            switch (c)
            {
                case 'N':
                case 'n':
                    return PhaseTypes.NetworkVoltage;
                case 'T':
                case 't':
                    return PhaseTypes.TransformerVoltage;
                case 'I':
                case 'i':
                    return PhaseTypes.Current;
                case 'D':
                case 'd':
                    return PhaseTypes.DifferentialVoltage;
                default:
                    throw new Exception(c.ToString() + " is not a valid Phase Type String");
            }
        }

        public static byte[] BytePacketFor(Phases p, PhaseTypes pT, Int16 i)
        {
            byte[] packet = new byte[6];

            packet[0] = (byte)_AdjustCalibrationOpCode;
            packet[1] = ByteRepresentationOf(p);
            packet[2] = ByteRepresentationOf(pT);
            packet[3] = (byte)(i >> 8);
            packet[4] = (byte)i;
            packet[5] = (byte)DC4;

            return packet;
        }
        public static char[] CharPacketFor(TripModeDefinition tMD)
        {
            char[] returnArray = new char[8];

            returnArray[0] = _ModeOpCode;
            returnArray[1] = _TripOpCode;
            returnArray[2] = CharRepresentationOf(tMD.Mode);
            returnArray[3] = (char)tMD.TimeDelayHighByte;
            returnArray[4] = (char)tMD.TimeDelayLowByte;
            returnArray[5] = (char)tMD.SensitiveTimeDelayHighByte;
            returnArray[5] = (char)tMD.SensitiveTimeDelayLowByte;
            returnArray[6] = (char)DC4;
            return returnArray;
        }

        public static byte[] BytePacketFor(TripModeDefinition tMD)
        {
            byte[] returnArray = new byte[8];

            returnArray[0] = (byte)_ModeOpCode; // M
            returnArray[1] = (byte)_TripOpCode; // T
            returnArray[2] = ByteRepresentationOf(tMD.Mode); // T - Time Delay
            returnArray[3] = tMD.TimeDelayHighByte;
            returnArray[4] = tMD.TimeDelayLowByte;
            returnArray[5] = (byte)tMD.ExtendedDelay;//tMD.SensitiveTimeDelayHighByte;
            returnArray[6] = tMD.SensitiveTimeDelayLowByte;
            returnArray[7] = (byte)DC4;
            return returnArray;
        }

        public static byte ByteFrom(Frequencies f)
        {
            switch (f)
            {
                case Frequencies.Blue:
                    return 0x01;
                case Frequencies.Green:
                    return 0x02;
                case Frequencies.Red:
                    return 0x00;
                case Frequencies.Yellow:
                    return 0x03;
                default:
                    throw new Exception(f.ToString() + " is a bad value for Transmitter Frequency.");
            }
        }

        /// <summary>
        /// Returns the packet for communication
        /// </summary>
        /// <param name="tCD">Trip Curve Definition</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte[] BytePacketFor(TripCurveDefinition tCD, int index)
        {
            byte[] returnArray = new byte[14];
            index += 0x30;

            returnArray[0] = (byte)_TripOpCode; // 'T'
            returnArray[1] = (byte)index;
            returnArray[2] = ByteFrom(tCD.CurveType);
            returnArray[3] = tCD.OffsetHighByte;
            returnArray[4] = tCD.OffsetLowByte;
            returnArray[5] = tCD.TiltHighByte;
            returnArray[6] = tCD.TiltLowByte;
            returnArray[7] = tCD.CodomainMaxHighByte;
            returnArray[8] = tCD.CodomainMaxLowByte;
            returnArray[9] = tCD.CodomainMinHighByte;
            returnArray[10] = tCD.CodomainMinLowByte;
            returnArray[11] = tCD.MagnitudeHighByte;
            returnArray[12] = tCD.MagnitudeLowByte;
            returnArray[13] = (byte)DC4;

            if (dataBackupR.dataBackup_fromRelay == true)
            {
                // writes to 12 bytes T0_byte1 to T0_byte12 in master uP
                // these 12 bytes correspond to the byte packet refering to APP contents as seen on line 381-394 in RelayModeFunctions.cs
                string lineRead;
                StreamReader sr = new StreamReader("C:\\DGI Systems\\Relay\\Saved Data\\test_fileRead.txt");
                int c = 1;
                int skipFwdBy = 0;
                
                switch (index)
                {
                    case 48:    // T0
                        skipFwdBy = 20;
                        break;
                    case 49:    // T1
                        skipFwdBy = 32;
                        break;
                    case 50:    // T2
                        break;
                    case 51:    // T3
                        break;
                    case 52:    // T4
                        break;
                    default:
                        break;
                }

                while (c <= skipFwdBy)
                {// skip through first 20 data bytes 
                    lineRead = sr.ReadLine(); //Read the next line
                    c++;
                }

                returnArray[0] = (byte)_TripOpCode; // 'T'
                //returnArray[1] = (byte)index;
                for (int cnt = 1; cnt <= 12; cnt++)
                {
                    lineRead = sr.ReadLine(); //Read the next line
                    if ((cnt % 2) != 0)//odd 
                        returnArray[cnt+1] = Convert.ToByte(lineRead);
                    else
                        returnArray[cnt - 1] = Convert.ToByte(lineRead);
                }
                returnArray[13] = (byte)DC4;
                sr.Close();
                if (index == 2)
                { 
                    dataBackupR.dataBackup_fromRelay = false; 
                }
                Thread.Sleep(4000);   // 1 second delay
            }

            return returnArray;
        }

        public static TripModes TripMode(string s)
        {
            TripModes returnMode;

            if (s == TripModes.Insensitive.ToString())
                returnMode = TripModes.Insensitive;
            else if (s == TripModes.Sensitive.ToString())
                returnMode = TripModes.Sensitive;
            else //if (s = 
                returnMode = TripModes.TimeDelay;

            return returnMode;
        }

        #endregion

        #region Close Mode Functions

        public static char CharRepresentationOf(CloseModes cM)
        {
            switch (cM)
            {
                case CloseModes.CircleClose:
                    return _DeadNetworkOpCode;
                case CloseModes.Normal:
                    return _NormalRecloseOpCode;
                default:
                    return '\0';
            }
        }

        public static byte ByteRepresentationOf(CloseModes cM)
        {
            switch (cM)
            {
                case CloseModes.CircleClose:
                    return (byte)_CircleCloseOpCode;
                case CloseModes.Normal:
                default:
                    return (byte)_NormalRecloseOpCode;
            }
        }

        public static byte ByteRepresentationOf(CloseModeDefinition cMD)
        {
            switch (cMD.CloseMode)
            {
                case CloseModes.CircleClose:
                    return (byte)_CircleCloseOpCode;
                case CloseModes.Normal:
                default:
                    return (byte)_NormalRecloseOpCode;
                case CloseModes.RelaxClose:
                    return (byte)_RelaxCloseOpCode;
                case CloseModes.CircleAndRelax:
                    return (byte)_RelaxCircleOpCode;
                case CloseModes.PermissiveClose:
                    return (byte)_PermissiveClose;
            }
        }
        public static byte[] BytePacketFor(CloseModeDefinition cMD)
        {
            byte[] returnArray = new byte[8];

            returnArray[0] = (byte)_ModeOpCode;
            returnArray[1] = (byte)_CloseOpCode;
            returnArray[2] = ByteRepresentationOf(cMD);
            returnArray[3] = cMD.TimeDelayHighByte;
            returnArray[4] = cMD.TimeDelayLowByte;
            returnArray[5] = (byte)(cMD.DataBits >> 8);
            returnArray[6] = (byte)cMD.DataBits;
            returnArray[7] = (byte)DC4;

            return returnArray;

        }

        public static char[] CharPacketFor(CloseModes cM)
        {
            char[] returnArray = new char[5];

            returnArray[0] = (char)(returnArray.Length - 1);
            returnArray[1] = _ModeOpCode;
            returnArray[2] = _CloseOpCode;
            returnArray[3] = CharRepresentationOf(cM);
            returnArray[4] = (char)DC4;

            return returnArray;
        }

        public static CloseModes CloseModeFrom(string s)
        {
            if (s == "Cricle Close" || s == "C")
                return CloseModes.CircleClose;
            else if (s == "Normal" || s == "N")
                return CloseModes.Normal;
            else
                throw new Exception("Input String Not Recognized For Close Mode");
        }

        public static byte[] BytePacketFor(CloseCurveDefinition cCD)
        {
            byte[] result = new byte[10];

            result[0] = (byte)_CloseCurveOpCode;
            result[1] = cCD.RecloseVoltsByteHigh;
            result[2] = cCD.RecloseVoltsByteLow;
            result[3] = cCD.TiltAngleTangentHighByte;
            result[4] = cCD.TiltAngleTangentLowByte;
            result[5] = cCD.PhasingOffsetHighByte;
            result[6] = cCD.PhasingOffsetLowByte;
            result[7] = cCD.PhaseDetectTangentHighByte;
            result[8] = cCD.PhaseDetectTangentLowByte;
            result[9] = (byte)DC4;

            return result;
        }

        #endregion

        public static byte HighByte(int i)
        {
            return (byte)(i >> 8);
        }

        public static byte LowByte(int i)
        {
            return (byte)(0x00FF & i);
        }

        public static byte HighByte(decimal d)
        {
            d = Math.Round(d, 1);
            d *= 10;

            return HighByte((int)d);
        }

        public static byte LowByte(decimal d)
        {
            d = Math.Round(d, 1);
            d *= 10;

            return LowByte((int)d);
        }

        public static Color GetPhaseColor(PhasorTypes pT)
        {
            switch (pT)
            {
                case PhasorTypes.IA:
                    return Color.Pink;
                case PhasorTypes.IB:
                    return Color.LightGreen;
                case PhasorTypes.IC:
                    return Color.LightBlue;
                case PhasorTypes.IN:
                    return Color.DarkRed;
                case PhasorTypes.IP:
                    return Color.DarkSeaGreen;
                case PhasorTypes.PA:
                    return Color.DarkRed;
                case PhasorTypes.PB:
                    return Color.DarkGreen;
                case PhasorTypes.PC:
                    return Color.DarkBlue;
                case PhasorTypes.PT:
                    return Color.DarkOrange;
                case PhasorTypes.VnA:
                    return Color.DarkRed;
                case PhasorTypes.VnB:
                    return Color.DarkGreen;
                case PhasorTypes.VnC:
                    return Color.Navy;
                case PhasorTypes.VtA:
                    return Color.Red;
                case PhasorTypes.VtB:
                    return Color.Green;
                case PhasorTypes.VtC:
                    return Color.Blue;
                case PhasorTypes.VtN:
                    return Color.Black;
                case PhasorTypes.VtP:
                    return Color.SteelBlue;
                case PhasorTypes.Ieff:
                    return Color.Purple;
                case PhasorTypes.VdN:
                    return Color.Pink;
                case PhasorTypes.VdP:
                    return Color.OrangeRed;
                case PhasorTypes.VdA:
                    return Color.Red;
                case PhasorTypes.VdB:
                    return Color.Green;
                case PhasorTypes.VdC:
                    return Color.Blue;
                default:
                    return Color.Purple;

            }
        }

        public static PhasorTypes PhasorTypeFrom(string s)
        {
            switch (s)
            {
                case "IA":
                    return PhasorTypes.IA;
                case "IB":
                    return PhasorTypes.IB;
                case "IC":
                    return PhasorTypes.IC;
                case "Ieff":
                    return PhasorTypes.Ieff;
                case "IN":
                    return PhasorTypes.IN;
                case "IP":
                    return PhasorTypes.IP;
                case "PA":
                    return PhasorTypes.PA;
                case "PB":
                    return PhasorTypes.PB;
                case "PC":
                    return PhasorTypes.PC;
                case "PT":
                    return PhasorTypes.PT;
                case "VdA":
                    return PhasorTypes.VdA;
                case "VdB":
                    return PhasorTypes.VdB;
                case "VdC":
                    return PhasorTypes.VdC;
                case "VdN":
                    return PhasorTypes.VdN;
                case "VdP":
                    return PhasorTypes.VdP;
                case "VdT":
                    return PhasorTypes.VdT;
                case "VnA":
                    return PhasorTypes.VnA;
                case "VnB":
                    return PhasorTypes.VnB;
                case "VnC":
                    return PhasorTypes.VnC;
                case "VnN":
                    return PhasorTypes.VnN;
                case "VnP":
                    return PhasorTypes.VnP;
                case "VtA":
                    return PhasorTypes.VtA;
                case "VtB":
                    return PhasorTypes.VtB;
                case "VtC":
                    return PhasorTypes.VtC;
                case "VtN":
                    return PhasorTypes.VtN;
                case "VtP":
                    return PhasorTypes.VtP;
                default:
                    return PhasorTypes.None;
            }
        }

        public static PhasorTypes PhasorTypeFrom(char type, char phase)
        {
            PhasorTypes phasorType = PhasorTypes.None;

            switch (type)
            {
                case 'D':
                    switch (phase)
                    {
                        case 'A':
                            phasorType = PhasorTypes.VdA;
                            break;
                        case 'B':
                            phasorType = PhasorTypes.VdB;
                            break;
                        case 'C':
                            phasorType = PhasorTypes.VdC;
                            break;
                        case 'N':
                            phasorType = PhasorTypes.VdN;
                            break;
                        case 'P':
                            phasorType = PhasorTypes.VdP;
                            break;
                        case 'T':
                            phasorType = PhasorTypes.VdT;
                            break;
                        default:
                            phasorType = PhasorTypes.None;
                            break;
                    }
                    break;
                case 'I':
                    switch (phase)
                    {
                        case 'A':
                            phasorType = PhasorTypes.IA;
                            break;
                        case 'B':
                            phasorType = PhasorTypes.IB;
                            break;
                        case 'C':
                            phasorType = PhasorTypes.IC;
                            break;
                        case 'E':
                            phasorType = PhasorTypes.Ieff;
                            break;
                        case 'N':
                            phasorType = PhasorTypes.IN;
                            break;
                        case 'P':
                            phasorType = PhasorTypes.IP;
                            break;
                        default:
                            phasorType = PhasorTypes.None;
                            break;
                    }
                    break;
                case 'T':
                    switch (phase)
                    {
                        case 'A':
                            phasorType = PhasorTypes.VtA;
                            break;
                        case 'B':
                            phasorType = PhasorTypes.VtB;
                            break;
                        case 'C':
                            phasorType = PhasorTypes.VtC;
                            break;
                        case 'N':
                            phasorType = PhasorTypes.VtN;
                            break;
                        case 'P':
                            phasorType = PhasorTypes.VtP;
                            break;
                        default:
                            phasorType = PhasorTypes.None;
                            break;
                    }
                    break;
                case 'N':
                    switch (phase)
                    {
                        case 'A':
                            phasorType = PhasorTypes.VnA;
                            break;
                        case 'B':
                            phasorType = PhasorTypes.VnB;
                            break;
                        case 'C':
                            phasorType = PhasorTypes.VnC;
                            break;
                        case 'N':
                            phasorType = PhasorTypes.VnN;
                            break;
                        case 'P':
                            phasorType = PhasorTypes.VnP;
                            break;
                        default:
                            phasorType = PhasorTypes.None;
                            break;
                    }
                    break;
                case 'P':
                    switch (phase)
                    {
                        case 'A':
                            phasorType = PhasorTypes.PA;
                            break;
                        case 'B':
                            phasorType = PhasorTypes.PB;
                            break;
                        case 'C':
                            phasorType = PhasorTypes.PC;
                            break;
                        case 'T':
                            phasorType = PhasorTypes.PT;
                            break;
                        default:
                            phasorType = PhasorTypes.None;
                            break;
                    }
                    break;
                case 'H':
                    switch (phase)
                    {
                        case 'A':
                            phasorType = PhasorTypes.IA;
                            break;
                        case 'B':
                            phasorType = PhasorTypes.IB;
                            break;
                        case 'C':
                            phasorType = PhasorTypes.IC;
                            break;
                        default:
                            phasorType = PhasorTypes.None;
                            break;
                    }
                    break;
                default:
                    phasorType = PhasorTypes.None;
                    break;
            }

            return phasorType;
        }

        public static double RadiansToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static double DegreesToRadians(double radians)
        {
            return radians * (Math.PI / 180.0);
        }

        public static Frequencies FrequencyFrom(byte b)
        {
            switch (b)
            {
                case 0x01:
                    return Frequencies.Blue;
                case 0x00:
                    return Frequencies.Red;
                case 0x02:
                    return Frequencies.Green;
                case 0x03:
                    return Frequencies.Yellow;
                default:
                    throw new Exception(b.ToString() + " is not a valid Frequency Value");
            }
        }

        public static DateTime DateFrom(long i)
        {
            //DateTime returnTime = new DateTime(2009, 7, 29, 0, 0, 1);
            //long intZeroTime = 633844224000000000;  //July 29, 2009 12:00AM

            i *= 10000000;                          //convert to seconds.   
            i += Constants.IntZeroTime;             //add base time to it


            return DateTime.FromBinary(i);

        }

        public static long BinaryDate(DateTime dT)
        {
            long i;
            UInt64 unsigned;

            i = dT.Ticks;
            unsigned = (UInt64)i;
            unsigned -= Constants.IntZeroTime;
            unsigned /= 10000000;

            return (long)unsigned;
        }

        /// <summary>
        /// Returns the real value based on the raw DAC register value.
        /// </summary>
        /// <param name="dACReading">Raw value from DAC, not shifted</param>
        /// <param name="cC">Calibration Constant</param>
        /// <returns>Real Float Value</returns>
        public static float CalibratedDACValue(Int16 dACReading, CalibrationConstant cC)
        {
            float returnValue;

            dACReading >>= 3; //shifted 3 due to nature of DAC
            if (dACReading >= 0x0800)
                dACReading = (Int16)(-dACReading);

            returnValue = (float)dACReading * (float)cC.RealValue;

            //might need a >> 10 here, or equivalent
            return returnValue;
        }

        public static bool IsCurrent(PhasorTypes phasorTypes)
        {
            if (phasorTypes == PhasorTypes.IA || phasorTypes == PhasorTypes.IB || phasorTypes == PhasorTypes.IC
                || phasorTypes == PhasorTypes.Ieff || phasorTypes == PhasorTypes.IN || phasorTypes == PhasorTypes.IP)
                return true;
            else
                return false;
        }

        public static bool IsPower(PhasorTypes phasorType)
        {
            if (phasorType == PhasorTypes.PA || phasorType == PhasorTypes.PB || phasorType == PhasorTypes.PC || phasorType == PhasorTypes.PT)
                return true;
            else
                return false;
        }

        public static uint ConvertTo6_10(decimal d)
        {
            decimal tempD = d / Constants.TenFracBits;

            return (uint)tempD;
        }

        public static decimal ConvertFrom6_10(uint i)
        {
            decimal tempD = (decimal)i * Constants.TenFracBits;

            return tempD;
        }

        public static uint ConvertTo8_8(decimal d)
        {
            decimal tempD = d / Constants.EightFracBits;

            return (uint)tempD;
        }

        public static decimal ConvertFrom8_8(uint i)
        {
            decimal tempD = (decimal)i * Constants.EightFracBits;

            return tempD;
        }
    }
}
