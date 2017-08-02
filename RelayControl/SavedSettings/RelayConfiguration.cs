using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using RelayControlLibrary;

namespace SavedSettings
{
    [Serializable()]
    public class RelayConfiguration : ISerializable
    {
        public RelayConfiguration()
        {
        }

        public string Name;
        public TripModeSavedStateV4 TripSettings = new TripModeSavedStateV4();
        public CloseModeSaveStateV4 CloseSettings = new CloseModeSaveStateV4(); //was "v1" (no version#)
        public PumpModeSavedStateV2 PumpSettings = new PumpModeSavedStateV2();
        public int CTRatio;
        public int RelayType;
        public int Phasing;
        [OptionalField]
        public SafeServiceSavedState SafeServiceSettings = new SafeServiceSavedState();
        [OptionalField]
        public DNPSavedState DNPSettings = new DNPSavedState();
        [OptionalField]
        public DNPEventState DNPEventSettings = new DNPEventState();
        [OptionalField]
        public DNPDeadBands DNPDeadBandSettings = new DNPDeadBands();

        public RelayConfiguration(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.Name = (string)info.GetValue("Name", typeof(string));
                this.TripSettings = (TripModeSavedStateV4)info.GetValue("Trip Settings", typeof(TripModeSavedStateV4));
                this.CloseSettings = (CloseModeSaveStateV4)info.GetValue("Close Settings", typeof(CloseModeSaveStateV4));
                this.PumpSettings = (PumpModeSavedStateV2)info.GetValue("Pump Settings", typeof(PumpModeSavedStateV2));
                try
                {
                    this.SafeServiceSettings = (SafeServiceSavedState)info.GetValue("Safe Service Settings", typeof(SafeServiceSavedState));
                }
                catch
                {
                    this.SafeServiceSettings = new SafeServiceSavedState();
                }
                this.CTRatio = (UInt16)info.GetValue("CTRatio", typeof(UInt16));
                this.RelayType = (int)info.GetValue("Relay Type", typeof(int));
                this.Phasing = (int)info.GetValue("Phasing", typeof(int));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Overall Single Saved Setting V4.", ex);
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
                info.AddValue("Safe Service Settings", this.SafeServiceSettings);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Overall Sing Saved Setting Get Object Data", ex);
            }
        }
    }
}
