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
    // Why Static Class? Initial thought was to use this class from multiple places and since this class has only one purpose
    // which is manupulation with rules and rule database so better to use static class. No overhead of managing objects and fields
    public static class RuleManager
    {
        private static string RuleFileName = "RuleDataBase.xml"; // Name of data base which is xml file storing al rules details

        // Directory where rule data will be store
        private static string DATA_DIR { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"RuleEngine"); } }
        /// <summary>
        /// Full path of rule database including database name and extension
        /// </summary>
        public static string FullPath { get { return Uri.UnescapeDataString(Path.Combine(DATA_DIR, RuleFileName)); } }
        private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(RuleSets)); // Globle instance of XmlSerializer
        private static XmlDocument xmlDocument = new XmlDocument(); // Globle instance of XmlDocument

        /// <summary>
        /// Main class for rules have collection of rules
        /// </summary>
        public static RuleSets RuleSet = new RuleSets();
        /// <summary>
        /// Initialize RuleManager. Create XML database if not exist and load rule from database to ListRule collection for further use
        /// </summary>
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
            // Read rule database using MemoryStream and Deserialize XML to RuleSets object
            MemoryStream memStream = new MemoryStream(File.ReadAllBytes(FullPath));
            RuleSet = xmlSerializer.Deserialize(memStream) as RuleSets;
        }

        /// <summary>
        /// Write new rule and save it to rule database
        /// </summary>
        /// <param name="sngnal">Signal</param>
        /// <param name="dataType">Data Type</param>
        /// <param name="option">Option</param>
        /// <param name="value">Value</param>
        public static void WriteNewRule(string sngnal,string dataType, string option, string value)
        {
            // Add new rule and save it to rule database
            RuleSet.ListRule.Insert(0, new Rule { SignalID = GetUniqueID(), Signal = sngnal, DataType = dataType, Option = option, Value = value });

            SaveXMLFile();
        }

        /// <summary>
        /// Delete a rule from rule database, Update Signal ID and save it
        /// </summary>
        /// <param name="signalID">Unique Signal ID</param>
        public static void DeleteRule(int signalID)
        {
            RuleSet.ListRule.Remove(RuleSet.ListRule.Where(x => x.SignalID == signalID).FirstOrDefault());
            UpdateSignalID(); // After deleting rule update remaining rule's Signal ID
            //SaveXMLFile();
        }

        // Update Signal ID and Save it to rule database
        private static void UpdateSignalID()
        {
            int count = 0;
            foreach (Rule rule in RuleSet.ListRule)
            {
                rule.SignalID = RuleSet.ListRule.Count - count++;
            }
            SaveXMLFile();
        }

        private static void SaveXMLFile() // Logic for saving rule to database
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


        private static int GetUniqueID()// Logic for creating new unique Signal ID
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
