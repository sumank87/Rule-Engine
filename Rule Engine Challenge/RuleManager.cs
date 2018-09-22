using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Rule_Engine_Challenge
{
    public static class RuleManager
    {
        private static string RuleFileName = "RuleDataBase.xml";

        private static string DATA_DIR { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"RuleEngine"); } }
        public static string FullPath { get { return Uri.UnescapeDataString(Path.Combine(DATA_DIR, RuleFileName)); } }
        private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(RuleSets)); // 
        private static XmlDocument xmlDocument = new XmlDocument();

        public static RuleSets RuleSet = new RuleSets();
        public static void InitializeRuleManager()
        {
            if (!File.Exists(FullPath))
            {
                Directory.CreateDirectory(DATA_DIR);  // create directory
                XmlTextWriter xtw;
                xtw = new XmlTextWriter(FullPath, Encoding.UTF8);
                //xtw.WriteStartDocument();
                xtw.WriteStartElement("RuleSets");
                xtw.WriteEndElement();
                xtw.Close();
            }
            LoadRuleSets();
        }

        private static void LoadRuleSets()
        {
            MemoryStream memStream = new MemoryStream(File.ReadAllBytes(FullPath));
            RuleSet = xmlSerializer.Deserialize(memStream) as RuleSets;
        }

        public static void WriteNewRule(string sngnal,string dataType, string option, string value)
        {
            RuleSet.ListRule.Insert(0, new Rule { SignalID = GetUniqueID(), Signal = sngnal, DataType = dataType, Option = option, Value = value });

            SaveXMLFile();
        }

        public static void DeleteRule(int signalID)
        {
            RuleSet.ListRule.Remove(RuleSet.ListRule.Where(x => x.SignalID == signalID).FirstOrDefault());
            UpdateSignalID();
            //SaveXMLFile();
        }

        private static void UpdateSignalID()
        {
            int count = 0;
            foreach (Rule rule in RuleSet.ListRule)
            {
                rule.SignalID = RuleSet.ListRule.Count - count++;
            }
            SaveXMLFile();
        }

        private static void SaveXMLFile()
        {
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, RuleSet);
                xmlStream.Position = 0;
                //Loads the XML document from the specified string.
                xmlDocument.Load(xmlStream);
                xmlDocument.Save(FullPath);
            }
        }

        private static int GetUniqueID()
        {
            if (RuleSet.ListRule.Count.Equals(0))
                return 1;
            else
                return RuleSet.ListRule[0].SignalID + 1;
        }

        //public static void WriteNewRule(string signal,string dataType,string option, string value)
        //{
        //    XmlDocument xd = new XmlDocument();
        //    FileStream fileStream = new FileStream(FullPath, FileMode.Open);
        //    xd.Load(fileStream);
        //    XmlElement rootElement = xd.CreateElement("Rule");
        //    rootElement.AppendChild(CreateChildNode(xd, "Signal", signal));
        //    rootElement.AppendChild(CreateChildNode(xd, "DataType", dataType));
        //    rootElement.AppendChild(CreateChildNode(xd, "Option", option));
        //    rootElement.AppendChild(CreateChildNode(xd, "Value", value));
        //    xd.DocumentElement.AppendChild(rootElement);
        //    fileStream.Close();
        //    xd.Save(FullPath);
        //}

        //private static XmlElement CreateChildNode(XmlDocument xd, string key, string value)
        //{
        //    XmlElement childElement = xd.CreateElement(key);
        //    XmlText ChildText = xd.CreateTextNode(value);
        //    childElement.AppendChild(ChildText);
        //    return childElement;
        //}
    }
}
