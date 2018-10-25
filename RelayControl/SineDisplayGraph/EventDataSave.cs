using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using RelayControlLibrary;

namespace SineDisplayGraph
{
    [Serializable()]

    class AllEventDataSave : ISerializable
    {
        public AllEventDataSave()
        {
        }

        public List<EventSaveData> Events = new List<EventSaveData>(8);
        public LiveSaveData LiveData = new LiveSaveData();


        public AllEventDataSave(SerializationInfo info, StreamingContext ctxt)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
        }

    }

    [Serializable()]

    class EventSaveData : ISerializable
    {
        public EventSaveData()
        {
        }

        int EventNumber;
        EventTypes Type;
        DateTime EventTime;
        float[] VtA = new float[2048];
        float[] VtB = new float[2048];
        float[] VtC = new float[2048];
        float[] VnA = new float[2048];
        float[] VnB = new float[2048];
        float[] VnC = new float[2048];
        float[] IA = new float[2048];
        float[] IB = new float[2048];
        float[] IC = new float[2048];

        public EventSaveData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.EventNumber = (int)info.GetValue("Event Number", typeof(int));
                this.Type = (EventTypes)info.GetValue("Type", typeof(EventTypes));
                this.EventTime = (DateTime)info.GetValue("Event Time", typeof(DateTime));
                this.VtA = (float[])info.GetValue("VtA", typeof(float[]));
                this.VtB = (float[])info.GetValue("VtB", typeof(float[]));
                this.VtC = (float[])info.GetValue("VtC", typeof(float[]));
                this.VnA = (float[])info.GetValue("VnA", typeof(float[]));
                this.VnB = (float[])info.GetValue("VnB", typeof(float[]));
                this.VnC = (float[])info.GetValue("VnC", typeof(float[]));
                this.IA = (float[])info.GetValue("IA", typeof(float[]));
                this.IB = (float[])info.GetValue("IB", typeof(float[]));
                this.IC = (float[])info.GetValue("IC", typeof(float[]));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Retrieving Event Saved Data for Event " + this.EventNumber.ToString(), ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("Event Number", this.EventNumber);
                info.AddValue("Type", this.Type);
                info.AddValue("Event Time", this.EventTime);
                info.AddValue("VtA", this.VtA);
                info.AddValue("VtB", this.VtB);
                info.AddValue("VtC", this.VtC);
                info.AddValue("VnA", this.VnA);
                info.AddValue("VnB", this.VnB);
                info.AddValue("VnC", this.VnC);
                info.AddValue("IA", this.IA);
                info.AddValue("IB", this.IB);
                info.AddValue("IC", this.IC);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Setting Event Data for Event " + this.EventNumber.ToString(), ex);
            }
        }
    }

    [Serializable()]

    class LiveSaveData : ISerializable
    {
        public LiveSaveData()
        {
        }

        float[] VtA = new float[8192];
        float[] VtB = new float[8192];
        float[] VtC = new float[8192];
        float[] VnA = new float[8192];
        float[] VnB = new float[8192];
        float[] VnC = new float[8192];
        float[] IA = new float[8192];
        float[] IB = new float[8192];
        float[] IC = new float[8192];

        public LiveSaveData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.VtA = (float[])info.GetValue("VtA", typeof(float[]));
                this.VtB = (float[])info.GetValue("VtB", typeof(float[]));
                this.VtC = (float[])info.GetValue("VtC", typeof(float[]));
                this.VnA = (float[])info.GetValue("VnA", typeof(float[]));
                this.VnB = (float[])info.GetValue("VnB", typeof(float[]));
                this.VnC = (float[])info.GetValue("VnC", typeof(float[]));
                this.IA = (float[])info.GetValue("IA", typeof(float[]));
                this.IB = (float[])info.GetValue("IB", typeof(float[]));
                this.IC = (float[])info.GetValue("IC", typeof(float[]));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Retrieving Live Saved Data", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("VtA", this.VtA);
                info.AddValue("VtB", this.VtB);
                info.AddValue("VtC", this.VtC);
                info.AddValue("VnA", this.VnA);
                info.AddValue("VnB", this.VnB);
                info.AddValue("VnC", this.VnC);
                info.AddValue("IA", this.IA);
                info.AddValue("IB", this.IB);
                info.AddValue("IC", this.IC);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Setting Live Data", ex);
            }
        }
    }
}
