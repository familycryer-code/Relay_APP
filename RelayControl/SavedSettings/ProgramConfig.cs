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
    public class ProgramConfig
    {
        public ProgramConfig()
        {
            this.initializeConfig();
        }

        public ProgramConfigData Data = new ProgramConfigData();

        private const string _configFileName = @"C:\DGI Systems\Relay\Saved Data\config.sav";
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

                if (!File.Exists(_configFileName))
                {
                    //File.Create(_configFileName);

                    using (Stream sW = File.Create(_configFileName))
                    {
                        BinaryFormatter bF = new BinaryFormatter();

                        this.Data = new ProgramConfigData();
                        bF.Serialize(sW, this.Data);
                    }
                }
                else
                {
                    using (Stream sR = File.OpenRead(_configFileName))
                    {
                        BinaryFormatter bF = new BinaryFormatter();
                        this.Data = (ProgramConfigData)bF.Deserialize(sR);
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
                using (Stream sW = File.OpenWrite(_configFileName))
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

    public class ProgramConfigData : ISerializable
    {
        public ProgramConfigData()
        {

        }

        public bool AutoLoadEnabled = true;
        public ProgramConfigData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                this.AutoLoadEnabled = (bool)info.GetValue("EnableAutoLoad", typeof(bool));
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
                info.AddValue("EnableAutoLoad", this.AutoLoadEnabled);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ProgramConfig Data Get Object Data", ex);
            }
        }
    }
}
