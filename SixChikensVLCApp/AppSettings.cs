using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnLib
{

    
    public class AppConfig
    {
        public string URL1;
        public string URL2;
        public string URL3;
        public string URL4;
        public string URL5;
        public string URL6;
    }

    public sealed class AppSettings
    {
        private static AppSettings instance = null;
        private static readonly object padlock = new object();
        AppConfig m_config;
        string m_fileName;

        AppSettings()
        {
        }
        public AppConfig Config
        {
            get
            {
                return m_config;
            }
        }

        public void Default()
        {
            
        }
         
        public string Save()
        {
            try
            {
               
                string json = JsonConvert.SerializeObject(m_config);
                string jsonFormatted = JValue.Parse(json).ToString(Formatting.Indented);                    
                File.WriteAllText(m_fileName, jsonFormatted);
               
                return "ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }
        
        public string Load(string fileName, bool defaultIfNeed = true)
        {
            try
            {
                m_fileName = fileName;
                if (File.Exists(fileName) == false)
                {
                    m_config = new AppConfig();
                    if (defaultIfNeed == true)
                        Default();
                    Save();
                    return "file not found";
                }
                string text = File.ReadAllText(m_fileName);
                m_config = JsonConvert.DeserializeObject<AppConfig>(text);
                if (m_config == null)
                {
                    m_config = new AppConfig();
                    if (defaultIfNeed == true)
                        Default();
                    Save();
                }
                 

                return "ok";
            }
            catch (Exception err)
            {
                m_config = new AppConfig();
                return err.Message;
            }
        }
      
        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new AppSettings();
                        }
                    }
                }
                return instance;
            }
        }
    }

}
