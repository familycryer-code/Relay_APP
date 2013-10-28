using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RelayControlLibrary;

namespace SavedSettings
{
    public class SavedSettingsOldVersions
    {
    }

       [Serializable()]

    public class CloseModeSaveState : ISerializable 
    {
        public CloseModeSaveState()
        {
        }

        public string Name;
        public bool CircleCloseEnabled;
        public int CloseDelay;
        public decimal RecloseVolts;
        public int PhaseDetectionAngle;
        public decimal PhaseDetectionOffset;
        public int TiltAngle;

        public CloseModeSaveState(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.CircleCloseEnabled = (bool)info.GetValue("Circle Close Enabled", typeof(bool));
                this.CloseDelay = (int)info.GetValue("Close Delay", typeof(int));
                this.RecloseVolts = (decimal)info.GetValue("Reclose Volts", typeof(decimal));
                this.PhaseDetectionAngle = (int)info.GetValue("Phase Detection Angle", typeof(int));
                this.PhaseDetectionOffset = (decimal)info.GetValue("Phase Detection Offset", typeof(decimal));
                this.TiltAngle = (int)info.GetValue("Tilt Angle", typeof(int));
            }
            catch (Exception ex)
            {
                throw new Exception("Error In Close Mode Save State Contructor.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Circle Close Enabled", this.CircleCloseEnabled);
                info.AddValue("Close Delay", this.CloseDelay);
                info.AddValue("Reclose Volts", this.RecloseVolts);
                info.AddValue("Phase Detection Offset", this.PhaseDetectionOffset);
                info.AddValue("Phase Detection Angle", this.PhaseDetectionAngle);
                info.AddValue("Tilt Angle", this.TiltAngle);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Close Mode GetObjectData", ex);
            }
        }
    }

    [Serializable()]

    public class CloseModeSaveStateV4 : ISerializable
    {
        public CloseModeSaveStateV4()
        {
        }

        public string Name;
        public bool CircleCloseEnabled;
        public int CloseDelay;
        public decimal RecloseVolts;
        public int PhaseDetectionAngle;
        public decimal PhaseDetectionOffset;
        public int TiltAngle;
        public bool BlockedOverride;

        public CloseModeSaveStateV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.CircleCloseEnabled = (bool)info.GetValue("Circle Close Enabled", typeof(bool));
                this.CloseDelay = (int)info.GetValue("Close Delay", typeof(int));
                this.RecloseVolts = (decimal)info.GetValue("Reclose Volts", typeof(decimal));
                this.PhaseDetectionAngle = (int)info.GetValue("Phase Detection Angle", typeof(int));
                this.PhaseDetectionOffset = (decimal)info.GetValue("Phase Detection Offset", typeof(decimal));
                this.TiltAngle = (int)info.GetValue("Tilt Angle", typeof(int));
                this.BlockedOverride = info.GetBoolean("Blocked Override");
            }
            catch (Exception ex)
            {
                throw new Exception("Error In Close Mode Save State Contructor.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Circle Close Enabled", this.CircleCloseEnabled);
                info.AddValue("Close Delay", this.CloseDelay);
                info.AddValue("Reclose Volts", this.RecloseVolts);
                info.AddValue("Phase Detection Offset", this.PhaseDetectionOffset);
                info.AddValue("Phase Detection Angle", this.PhaseDetectionAngle);
                info.AddValue("Tilt Angle", this.TiltAngle);
                info.AddValue("Blocked Override", this.BlockedOverride);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Close Mode GetObjectData", ex);
            }
        }
    }

     [Serializable()]

    public class PumpModeSavedState : ISerializable
    {
        public PumpModeSavedState()
        {
        }

        public string Name;
        public bool EnablePumpProtect;
        public bool NeverReclose;
        public int CycleLimit;
        public int PumpTime;
        public int PumpProtectTime;

        public PumpModeSavedState(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.EnablePumpProtect = (bool)info.GetValue("Enable Pump Protect", typeof(bool));
                this.NeverReclose = (bool)info.GetValue("Never Reclose", typeof(bool));
                this.CycleLimit = (int)info.GetValue("Cycle Limit", typeof(int));
                this.PumpTime = (int)info.GetValue("Pump Time", typeof(int));
                this.PumpProtectTime = (int)info.GetValue("Pump Protect Time", typeof(int));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Pump Mode Saved State v1", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", Name);
                info.AddValue("Enable Pump Protect", EnablePumpProtect);
                info.AddValue("Never Reclose", NeverReclose);
                info.AddValue("Cycle Limit", CycleLimit);
                info.AddValue("Pump Time", PumpTime);
                info.AddValue("Pump Protect Time", PumpProtectTime);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Getting Object Data in Pump Mode Saving", ex);
            }
        }

    }

    [Serializable()]

    public class PumpModeSaveObject : ISerializable
    {
        public PumpModeSaveObject()
        {
        }
        public List<PumpModeSavedState> SavedStates = new List<PumpModeSavedState>();

        public PumpModeSaveObject(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.SavedStates = (List<PumpModeSavedState>)info.GetValue("Saved States", typeof(List<PumpModeSavedState>));
            }
            catch
            {
                this.SavedStates = null;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Saved States", this.SavedStates);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Saving Data In Pump Mode Settings.", ex);
            }
        }

        public void AddSavedState(PumpModeSavedState pMSS)
        {
            int i = 0;

            for (; i < SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name == pMSS.Name || this.SavedStates[i].Name == null)
                {
                    this.SavedStates[i] = pMSS;
                    break;
                }
            }

            if (i == SavedStates.Count)
            {
                this.SavedStates.Add(pMSS);
            }

            this.SavedStates.Sort(delegate(PumpModeSavedState pMSS1, PumpModeSavedState pMSS2) { return pMSS1.Name.CompareTo(pMSS2.Name); });
        }

        private bool sameName(PumpModeSavedState pMSS, string s)
        {
            if (pMSS.Name == s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveSavedState(PumpModeSavedState pMSS)
        {
            if (this.SavedStates == null)
                return;
            this.SavedStates.Remove(pMSS);
        }

        public void RemoveSavedState(string name)
        {
            if (this.SavedStates == null)
                return;

            for (int i = 0; i < this.SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name.Equals(name))
                {
                    this.SavedStates.Remove(this.SavedStates[i]);
                    break;
                }
            }
        }

    }

    [Serializable()]

    public class PumpModeSavedStateV2 : ISerializable
    {
        public PumpModeSavedStateV2()
        {
        }

        public string Name;
        public bool NeverReclose;
        public int CycleLimit;
        public int PumpTime;
        public int PumpProtectTime;
        public int MotorTimeout;
        public byte MotorCycleLimit;
        public bool EnableRelayCycle;
        public bool EnableMotorTimeout;
        public bool EnableMotorCycle;

        public PumpModeSavedStateV2(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.MotorTimeout = (int)info.GetValue("Motor Timeout", typeof(int));
                this.NeverReclose = (bool)info.GetValue("Never Reclose", typeof(bool));
                this.CycleLimit = (int)info.GetValue("Cycle Limit", typeof(int));
                this.PumpTime = (int)info.GetValue("Pump Time", typeof(int));
                this.PumpProtectTime = (int)info.GetValue("Pump Protect Time", typeof(int));
                this.EnableRelayCycle = (bool)info.GetValue("Enable Relay Cycle", typeof(bool));
                this.EnableMotorTimeout = (bool)info.GetValue("Enable Motor Timeout", typeof(bool));
                this.EnableMotorCycle = (bool)info.GetValue("Enable Motor Cycle", typeof(bool));
                this.MotorCycleLimit = (byte)info.GetValue("Motor Cycle Limit", typeof(byte));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Pump Mode Saved State V2", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", Name);
                info.AddValue("Never Reclose", NeverReclose);
                info.AddValue("Cycle Limit", CycleLimit);
                info.AddValue("Pump Time", PumpTime);
                info.AddValue("Pump Protect Time", PumpProtectTime);
                info.AddValue("Enable Relay Cycle", this.EnableRelayCycle);
                info.AddValue("Enable Motor Timeout", this.EnableMotorTimeout);
                info.AddValue("Enable Motor Cycle", this.EnableMotorCycle);
                info.AddValue("Motor Timeout", this.MotorTimeout);
                info.AddValue("Motor Cycle Limit", this.MotorCycleLimit);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Getting Object Data in Pump Mode Saving", ex);
            }
        }
    }

    [Serializable()]

    public class PumpModeSaveObjectV2 : ISerializable
    {
        public PumpModeSaveObjectV2()
        {
        }

        public List<PumpModeSavedStateV2> SavedStates = new List<PumpModeSavedStateV2>();

        public PumpModeSaveObjectV2(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.SavedStates = (List<PumpModeSavedStateV2>)info.GetValue("Saved States", typeof(List<PumpModeSavedState>));
            }
            catch
            {
                this.SavedStates = null;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Saved States", this.SavedStates);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Saving Data In Pump Mode Settings.", ex);
            }
        }

        public void AddSavedState(PumpModeSavedStateV2 pMSS)
        {
            int i = 0;

            for (; i < SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name == pMSS.Name || this.SavedStates[i].Name == null)
                {
                    this.SavedStates[i] = pMSS;
                    break;
                }
            }

            if (i == SavedStates.Count)
            {
                this.SavedStates.Add(pMSS);
            }

            this.SavedStates.Sort(delegate(PumpModeSavedStateV2 pMSS1, PumpModeSavedStateV2 pMSS2) { return pMSS1.Name.CompareTo(pMSS2.Name); });
        }

        private bool sameName(PumpModeSavedStateV2 pMSS, string s)
        {
            if (pMSS.Name == s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveSavedState(PumpModeSavedStateV2 pMSS)
        {
            if (this.SavedStates == null)
                return;
            this.SavedStates.Remove(pMSS);
        }

        public void RemoveSavedState(string name)
        {
            if (this.SavedStates == null)
                return;

            for (int i = 0; i < this.SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name.Equals(name))
                {
                    this.SavedStates.Remove(this.SavedStates[i]);
                    break;
                }
            }
        }
    }
     [Serializable()]

    public class TripModeSavedState : ISerializable
    {
        public TripModeSavedState()
        {
        }

        public string Name;
        public TripModes TripMode;
        public int SensitiveTripDelay;
        public int ExtendedTimeDelay;
        public int TimeDelay;
        public decimal SensitiveTrip;
        public int TiltAngle;
        public decimal InsensitiveCurrent;
        public decimal WattVarCurrent;
        public int WattVarAngle;
        public int GullWingAngle;
        public bool GullWingEnabled;

        public TripModeSavedState(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripMode = (TripModes)info.GetValue("Trip Mode", typeof(TripModes));
                this.SensitiveTripDelay = (int)info.GetValue("Sensitive Trip Delay", typeof(int));
                this.ExtendedTimeDelay = (int)info.GetValue("Extended Time Delay", typeof(int));
                this.TimeDelay = (int)info.GetValue("Time Delay", typeof(int));
                this.SensitiveTrip = (Decimal)info.GetValue("Sensitive Trip", typeof(decimal));
                this.TiltAngle = (int)info.GetValue("Tilt Angle", typeof(int));
                this.InsensitiveCurrent = (Decimal)info.GetValue("Insensitive Current", typeof(decimal));
                this.WattVarAngle = (int)info.GetValue("Watt Var Angle", typeof(int));
                this.WattVarCurrent = (Decimal)info.GetValue("Watt Var Current", typeof(decimal));
                this.GullWingAngle = (int)info.GetValue("Gull Wing Angle", typeof(int));
                this.GullWingEnabled = (bool)info.GetValue("Gull Wing Enabled", typeof(bool));
            }
            catch (Exception ex)
            {
                throw new Exception ("Error Instantiating Trip Mode Saved State", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", Name);
                info.AddValue("Trip Mode", TripMode);
                info.AddValue("Sensitive Trip Delay", SensitiveTripDelay);
                info.AddValue("Extended Time Delay", ExtendedTimeDelay);
                info.AddValue("Time Delay", TimeDelay);
                info.AddValue("Sensitive Trip", SensitiveTrip);
                info.AddValue("Tilt Angle", TiltAngle);
                info.AddValue("Insensitive Current", InsensitiveCurrent);
                info.AddValue("Watt Var Current", WattVarCurrent);
                info.AddValue("Watt Var Angle", WattVarAngle);
                info.AddValue("Gull Wing Angle", GullWingAngle);
                info.AddValue("Gull Wing Enabled", GullWingEnabled);
            }
            catch (Exception ex)
            {
                throw new Exception ("Error Getting Object Data in Trip Mode Saving", ex);
            }
        }


    }

    [Serializable()]

    public class TripModeSavedStateV4 : ISerializable
    {
        public TripModeSavedStateV4()
        {
        }

        public string Name;
        public TripModes TripMode;
        public int SensitiveTripDelay;
        public int ExtendedTimeDelay;
        public int TimeDelay;
        public decimal SensitiveTrip;
        public int TiltAngle;
        public decimal InsensitiveCurrent;
        public decimal WattVarCurrent;
        public int WattVarAngle;
        public int GullWingAngle;
        public bool GullWingEnabled;
        public int TripStyle;
        public bool TripOnPowerDown = true;

        public TripModeSavedStateV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripMode = (TripModes)info.GetValue("Trip Mode", typeof(TripModes));
                this.SensitiveTripDelay = (int)info.GetValue("Sensitive Trip Delay", typeof(int));
                this.ExtendedTimeDelay = (int)info.GetValue("Extended Time Delay", typeof(int));
                this.TimeDelay = (int)info.GetValue("Time Delay", typeof(int));
                this.SensitiveTrip = (Decimal)info.GetValue("Sensitive Trip", typeof(decimal));
                this.TiltAngle = (int)info.GetValue("Tilt Angle", typeof(int));
                this.InsensitiveCurrent = (Decimal)info.GetValue("Insensitive Current", typeof(decimal));
                this.WattVarAngle = (int)info.GetValue("Watt Var Angle", typeof(int));
                this.WattVarCurrent = (Decimal)info.GetValue("Watt Var Current", typeof(decimal));
                this.GullWingAngle = (int)info.GetValue("Gull Wing Angle", typeof(int));
                this.GullWingEnabled = (bool)info.GetValue("Gull Wing Enabled", typeof(bool));
                this.TripStyle = (int)info.GetValue("Trip Style", typeof(int));
                this.TripOnPowerDown = (bool)info.GetValue("Trip On Power Down", typeof(bool));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Trip Mode Saved State", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", Name);
                info.AddValue("Trip Mode", TripMode);
                info.AddValue("Sensitive Trip Delay", SensitiveTripDelay);
                info.AddValue("Extended Time Delay", ExtendedTimeDelay);
                info.AddValue("Time Delay", TimeDelay);
                info.AddValue("Sensitive Trip", SensitiveTrip);
                info.AddValue("Tilt Angle", TiltAngle);
                info.AddValue("Insensitive Current", InsensitiveCurrent);
                info.AddValue("Watt Var Current", WattVarCurrent);
                info.AddValue("Watt Var Angle", WattVarAngle);
                info.AddValue("Gull Wing Angle", GullWingAngle);
                info.AddValue("Gull Wing Enabled", GullWingEnabled);
                info.AddValue("Trip Style", TripStyle);
                info.AddValue("Trip On Power Down", TripOnPowerDown);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Getting Object Data in Trip Mode Saving", ex);
            }
        }
    }

    [Serializable()]

    public class SaveObject : ISerializable
    {
        public SaveObject()
        {
        }

        //public int NumberOfObjects;
        public List<TripModeSavedStateV4> SavedStates = new List<TripModeSavedStateV4>();

        public SaveObject(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                //this.NumberOfObjects = (int)info.GetValue("Number Of Objects", typeof(int));
                this.SavedStates = (List<TripModeSavedStateV4>)info.GetValue("Saved States", typeof(List<TripModeSavedStateV4>));
            }
            catch //(Exception ex)
            {
                this.SavedStates = null;
                //throw new Exception("Error in deserializing of Save Object in Trip Mode Settings.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Saved States", this.SavedStates);
                //info.AddValue("Number Of Objects", this.NumberOfObjects);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Saving Data In Trip Mode Settings.", ex);
            }
        }

        public void AddSavedState(TripModeSavedStateV4 tSS)
        {
            int i = 0;

            for (; i < SavedStates.Count; ++i)
            {
                if(this.SavedStates[i].Name == tSS.Name || this.SavedStates[i].Name == null)
                {
                    this.SavedStates[i] = tSS;
                    break;
                }
            }

            if(i == SavedStates.Count)
            {
                this.SavedStates.Add(tSS);
            }

            //Sort list alphabetically
            this.SavedStates.Sort(delegate(TripModeSavedStateV4 tSS1, TripModeSavedStateV4 tSS2) { return tSS1.Name.CompareTo(tSS2.Name); });
        }

        private bool sameName(TripModeSavedState tSS, string s)
        {
            if(tSS.Name == s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveSavedState(TripModeSavedStateV4 tSS)
        {
            if(this.SavedStates == null)
                return;
            this.SavedStates.Remove(tSS);
        }

        public void RemoveSavedState(string name)
        {
            if(this.SavedStates == null)
                return;

            for(int i = 0; i < this.SavedStates.Count; ++i)
            {
                if(this.SavedStates[i].Name.Equals(name))
                {
                    this.SavedStates.Remove(this.SavedStates[i]);
                    break;
                }
            }
        }

    }

    [Serializable()]

    public class SaveObjectV4 : ISerializable
    {
        public SaveObjectV4()
        {
        }

        //public int NumberOfObjects;
        public List<TripModeSavedState> SavedStates = new List<TripModeSavedState>();

        public SaveObjectV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                //this.NumberOfObjects = (int)info.GetValue("Number Of Objects", typeof(int));
                this.SavedStates = (List<TripModeSavedState>)info.GetValue("Saved States", typeof(List<TripModeSavedState>));
            }
            catch //(Exception ex)
            {
                this.SavedStates = null;
                //throw new Exception("Error in deserializing of Save Object in Trip Mode Settings.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Saved States", this.SavedStates);
                //info.AddValue("Number Of Objects", this.NumberOfObjects);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Saving Data In Trip Mode Settings.", ex);
            }
        }

        public void AddSavedState(TripModeSavedState tSS)
        {
            int i = 0;

            for (; i < SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name == tSS.Name || this.SavedStates[i].Name == null)
                {
                    this.SavedStates[i] = tSS;
                    break;
                }
            }

            if (i == SavedStates.Count)
            {
                this.SavedStates.Add(tSS);
            }

            //Sort list alphabetically
            this.SavedStates.Sort(delegate(TripModeSavedState tSS1, TripModeSavedState tSS2) { return tSS1.Name.CompareTo(tSS2.Name); });
        }

        private bool sameName(TripModeSavedState tSS, string s)
        {
            if (tSS.Name == s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveSavedState(TripModeSavedState tSS)
        {
            if (this.SavedStates == null)
                return;
            this.SavedStates.Remove(tSS);
        }

        public void RemoveSavedState(string name)
        {
            if (this.SavedStates == null)
                return;

            for (int i = 0; i < this.SavedStates.Count; ++i)
            {
                if (this.SavedStates[i].Name.Equals(name))
                {
                    this.SavedStates.Remove(this.SavedStates[i]);
                    break;
                }
            }
        }

    }
    
    [Serializable()]

    public class SavedSettingsv2 : ISerializable
    {
        public SavedSettingsv2()
        {
        }

        public List<SavedSettingv2> Settings = new List<SavedSettingv2>();

        public SavedSettingsv2(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Settings = (List<SavedSettingv2>)info.GetValue("Settings", typeof(List<SavedSettingv2>));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Saved Settings.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Settings", this.Settings);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Saved Settings Get Object Data", ex);
            }
        }

        public void AddState(SavedSettingv2 sS)
        {
            int i = 0;
            for (i = 0; i < this.Settings.Count; ++i)
            {
                if (this.Settings[i].Name == sS.Name)
                {
                    this.Settings[i] = sS;
                    break;
                }
            }

            if (i == this.Settings.Count)
            {
                this.Settings.Add(sS);
            }

            this.Settings.Sort(delegate(SavedSettingv2 sS1, SavedSettingv2 sS2) { return sS1.Name.CompareTo(sS2.Name); });
        }

        public void DeleteState(string name)
        {
            foreach (SavedSettingv2 sS in this.Settings)
            {
                if (sS.Name == name)
                {
                    this.Settings.Remove(sS);
                    break;
                }
            }
        }

        public void DeleteState(SavedSettingv2 sS)
        {
            try
            {
                if (this.Settings.Count > 0)
                {
                    this.Settings.Remove(sS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Deleting Saved State", ex);
            }
        }

    }

    [Serializable()]

    public class SavedSettingsV3 : ISerializable
    {
        public SavedSettingsV3()
        {
        }

        public List<SavedSettingV3> Settings = new List<SavedSettingV3>();

        public SavedSettingsV3(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Settings = (List<SavedSettingV3>)info.GetValue("Settings", typeof(List<SavedSettingV3>));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Saved Settings.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Settings", this.Settings);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Saved Settings Get Object Data", ex);
            }
        }

        public void AddState(SavedSettingV3 sS)
        {
            int i = 0;
            for (i = 0; i < this.Settings.Count; ++i)
            {
                if (this.Settings[i].Name == sS.Name)
                {
                    this.Settings[i] = sS;
                    break;
                }
            }

            if (i == this.Settings.Count)
            {
                this.Settings.Add(sS);
            }

            this.Settings.Sort(delegate(SavedSettingV3 sS1, SavedSettingV3 sS2) { return sS1.Name.CompareTo(sS2.Name); });
        }

        public void DeleteState(string name)
        {
            foreach (SavedSettingV3 sS in this.Settings)
            {
                if (sS.Name == name)
                {
                    this.Settings.Remove(sS);
                    break;
                }
            }
        }

        public void DeleteState(SavedSettingV3 sS)
        {
            try
            {
                if (this.Settings.Count > 0)
                {
                    this.Settings.Remove(sS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Deleting Saved State", ex);
            }
        }

    }

    [Serializable()]

    public class SavedSettingsV4 : ISerializable
    {
        public SavedSettingsV4()
        {
        }

        public List<SavedSettingV4> Settings = new List<SavedSettingV4>();

        public SavedSettingsV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Settings = (List<SavedSettingV4>)info.GetValue("Settings", typeof(List<SavedSettingV4>));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Saved Settings.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Settings", this.Settings);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Saved Settings Get Object Data", ex);
            }
        }

        public void AddState(SavedSettingV4 sS)
        {
            int i = 0;
            for (i = 0; i < this.Settings.Count; ++i)
            {
                if (this.Settings[i].Name == sS.Name)
                {
                    this.Settings[i] = sS;
                    break;
                }
            }

            if (i == this.Settings.Count)
            {
                this.Settings.Add(sS);
            }

            this.Settings.Sort(delegate(SavedSettingV4 sS1, SavedSettingV4 sS2) { return sS1.Name.CompareTo(sS2.Name); });
        }

        public void DeleteState(string name)
        {
            foreach (SavedSettingV4 sS in this.Settings)
            {
                if (sS.Name == name)
                {
                    this.Settings.Remove(sS);
                    break;
                }
            }
        }

        public void DeleteState(SavedSettingV4 sS)
        {
            try
            {
                if (this.Settings.Count > 0)
                {
                    this.Settings.Remove(sS);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Deleting Saved State", ex);
            }
        }

    }

    [Serializable()]
    public class SavedSettingv2 : ISerializable
    {
        public SavedSettingv2()
        {
        }

        public string Name;
        public TripModeSavedState TripSettings = new TripModeSavedState();
        public CloseModeSaveState CloseSettings = new CloseModeSaveState();
        public PumpModeSavedState PumpSettings = new PumpModeSavedState();
        public int CTRatio;
        public int RelayType;
        public int Phasing;

        public SavedSettingv2(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripSettings = (TripModeSavedState)info.GetValue("Trip Settings", typeof(TripModeSavedState));
                this.CloseSettings = (CloseModeSaveState)info.GetValue("Close Settings", typeof(CloseModeSaveState));
                this.PumpSettings = (PumpModeSavedState)info.GetValue("Pump Settings", typeof(PumpModeSavedState));
                this.CTRatio = (UInt16)info.GetValue("CTRatio", typeof(UInt16));
                this.RelayType = (char)info.GetValue("Relay Type", typeof(char));
                this.Phasing = (UInt16)info.GetValue("Phasing", typeof(UInt16));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Single Saved Setting v2.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Trip Settings", this.TripSettings);
                info.AddValue("Close Settings", this.CloseSettings);
                info.AddValue("Pump Settings", this.PumpSettings);
                info.AddValue("CTRatio", this.CTRatio);
                info.AddValue("Relay Type", this.RelayType);
                info.AddValue("Phasing", this.Phasing);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Sing Saved Setting Get Object Data", ex);
            }
        }
    }

    [Serializable()]
    public class SavedSettingV3 : ISerializable
    {
        public SavedSettingV3()
        {
        }

        public string Name;
        public TripModeSavedState TripSettings = new TripModeSavedState();
        public CloseModeSaveState CloseSettings = new CloseModeSaveState();
        public PumpModeSavedStateV2 PumpSettings = new PumpModeSavedStateV2();
        public int CTRatio;
        public int RelayType;
        public int Phasing;

        public SavedSettingV3(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripSettings = (TripModeSavedState)info.GetValue("Trip Settings", typeof(TripModeSavedState));
                this.CloseSettings = (CloseModeSaveState)info.GetValue("Close Settings", typeof(CloseModeSaveState));
                this.PumpSettings = (PumpModeSavedStateV2)info.GetValue("Pump Settings", typeof(PumpModeSavedStateV2));
                this.CTRatio = (UInt16)info.GetValue("CTRatio", typeof(UInt16));
                this.RelayType = (char)info.GetValue("Relay Type", typeof(char));
                this.Phasing = (UInt16)info.GetValue("Phasing", typeof(UInt16));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Single Saved Setting V3.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Trip Settings", this.TripSettings);
                info.AddValue("Close Settings", this.CloseSettings);
                info.AddValue("Pump Settings", this.PumpSettings);
                info.AddValue("CTRatio", this.CTRatio);
                info.AddValue("Relay Type", this.RelayType);
                info.AddValue("Phasing", this.Phasing);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Sing Saved Setting Get Object Data", ex);
            }
        }
    }

    [Serializable()]
    public class SavedSettingV4 : ISerializable
    {
        public SavedSettingV4()
        {
        }

        public string Name;
        public TripModeSavedStateV4 TripSettings = new TripModeSavedStateV4();
        public CloseModeSaveStateV4 CloseSettings = new CloseModeSaveStateV4(); //was "v1" (no version#)
        public PumpModeSavedStateV2 PumpSettings = new PumpModeSavedStateV2();
        public int CTRatio;
        public int RelayType;
        public int Phasing;

        public SavedSettingV4(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripSettings = (TripModeSavedStateV4)info.GetValue("Trip Settings", typeof(TripModeSavedStateV4));
                this.CloseSettings= (CloseModeSaveStateV4)info.GetValue("Close Settings", typeof(CloseModeSaveStateV4));
                this.PumpSettings = (PumpModeSavedStateV2)info.GetValue("Pump Settings", typeof(PumpModeSavedStateV2));
                this.CTRatio = (UInt16)info.GetValue("CTRatio", typeof(UInt16));
                this.RelayType = (char)info.GetValue("Relay Type", typeof(char));
                this.Phasing = (UInt16)info.GetValue("Phasing", typeof(UInt16));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Single Saved Setting V3.", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Name", this.Name);
                info.AddValue("Trip Settings", this.TripSettings);
                info.AddValue("Close Settings", this.CloseSettings);
                info.AddValue("Pump Settings", this.PumpSettings);
                info.AddValue("CTRatio", this.CTRatio);
                info.AddValue("Relay Type", this.RelayType);
                info.AddValue("Phasing", this.Phasing);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Sing Saved Setting Get Object Data", ex);
            }
        }
    }
}
