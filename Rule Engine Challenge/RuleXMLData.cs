using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Rule_Engine_Challenge
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class RuleSets
    {
        private ObservableCollection<Rule> ruleField = new ObservableCollection<Rule>();

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Rule")]
        public ObservableCollection<Rule> ListRule
        {
            get
            {
                return this.ruleField;
            }
            set
            {
                this.ruleField = value;
            }
        }

        //private RuleSetsRule[] ruleField;

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("Rule")]
        //public RuleSetsRule[] Rule
        //{
        //    get
        //    {
        //        return this.ruleField;
        //    }
        //    set
        //    {
        //        this.ruleField = value;
        //    }
        //}
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Rule: INotifyPropertyChanged
    {
        private int signalIdField;

        private string signalField;

        private string dataTypeField;

        private string optionField;

        private string valueField;

        /// <remarks/>
        public int SignalID
        {
            get
            {
                return this.signalIdField;
            }
            set
            {
                this.signalIdField = value;
                OnPropertyChanged("SignalID");
            }
        }

        /// <remarks/>
        public string Signal
        {
            get
            {
                return this.signalField;
            }
            set
            {
                this.signalField = value;
            }
        }

        /// <remarks/>
        public string DataType
        {
            get
            {
                return this.dataTypeField;
            }
            set
            {
                this.dataTypeField = value;
            }
        }

        /// <remarks/>
        public string Option
        {
            get
            {
                return this.optionField;
            }
            set
            {
                this.optionField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }


}