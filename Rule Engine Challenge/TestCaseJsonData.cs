using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Rule_Engine_Challenge
{
    [DataContract]
    public class TestCaseJsonData
    {
        //[DataMember(Name = "sreamData")]
        public StreamData[] sreamData { get; set; }
        //public List<StreamData> results { get; set; }
    }

    [DataContract(Name = "StreamData")]
    public class StreamData
    {
        [DataMember(Name = "signal")]
        public string signal { get; set; }

        [DataMember(Name = "value_type")]
        public string value_type { get; set; }

        [DataMember(Name = "value")]
        public string value { get; set; }
    }
}