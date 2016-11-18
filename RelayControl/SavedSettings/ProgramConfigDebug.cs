using System;
using System.Collections.Generic;
using System.IO;
#if DEBUG
//using System.Linq;
#endif
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
//using System.Threading.Tasks;

namespace SavedSettings
{
    public class ProgramConfigDebug
    {
        public ProgramConfigDebug()
        {
            this.initializeConfig();
        }

        public ProgramConfigDataDebug Data = new ProgramConfigDataDebug();

        private const string _configFileNameDebug = @"C:\DGI Systems\Relay\Saved Data\configDebug.sav";
        private const string _configFilePath = @"C:\DGI Systems\Relay\Saved Data\";

        private void initializeConfig()
        {
            this.initializeConfigFile();
        }

        private void initializeConfigFile()
        {
            try
            {
                if (!Directory.Exists(_configFilePath))
                {
                    Directory.CreateDirectory(_configFilePath);
                }

                if (!File.Exists(_configFileNameDebug))
                {
                    //File.Create(_configFileName);

                    using (Stream sW = File.Create(_configFileNameDebug))
                    {
                        BinaryFormatter bF = new BinaryFormatter();

                        this.Data = new ProgramConfigDataDebug();
                        bF.Serialize(sW, this.Data);
                    }
                }
                else
                {
                    using (Stream sR = File.OpenRead(_configFileNameDebug))
                    {
                        BinaryFormatter bF = new BinaryFormatter();
                        this.Data = (ProgramConfigDataDebug)bF.Deserialize(sR);
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Initializing Config File", ex);
            }
        }

        public void SaveConfigFile()
        {
            try
            {
                using (Stream sW = File.OpenWrite(_configFileNameDebug))
                {
                    BinaryFormatter bF = new BinaryFormatter();

                    bF.Serialize(sW, this.Data);
                }
            }
            catch (Exception ex)
            {
                this.errorHandler("Error Saving Config File", ex);
            }
        }

        private void errorHandler(string title, Exception ex)
        {
            throw ex;
        }
    }

    [Serializable]

    public class ProgramConfigDataDebug : ISerializable
    {
        public ProgramConfigDataDebug()
        {

        }

        public bool ReprogramBoot = false;
        public ProgramConfigDataDebug(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.ReprogramBoot = (bool)info.GetValue("ReprogramBoot", typeof(bool));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Instantiating Config File Data", ex);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                info.AddValue("ReprogramBoot", this.ReprogramBoot);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ProgramConfig Data Get Object Data", ex);
            }
        }
    }
}
