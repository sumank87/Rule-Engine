using System.Runtime.Serialization;

namespace Rule_Engine_Challenge
{
    [DataContract]
    public class TestCaseJsonData // Not being used as of now
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