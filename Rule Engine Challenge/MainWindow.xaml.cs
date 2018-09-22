using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;

namespace Rule_Engine_Challenge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog openDialog;
        CreateRuleWindow createNewWindow;
        // Store results
        ObservableCollection<ResultData> Result = new ObservableCollection<ResultData>();
        
        public MainWindow()
        {
            InitializeComponent();
            openDialog = new OpenFileDialog(); // Initilize OpenFileDialog which will be used for selecting test case json file
            createNewWindow = new CreateRuleWindow(this); // CreateRuleWindow will let user create new rule(s)
            RuleManager.InitializeRuleManager(); // Initilize RuleManager, a class which handles rules
            ListViewRules.DataContext = RuleManager.RuleSet.ListRule; // ListRule is a object version of rules
            ListViewResult.DataContext = Result; // Result is ObservableCollection of ResultData
            //RuleManager.WriteNewRule("ALT3","Integer","GreterThan","300.00");
        }

        private void BtnChoose_Click(object sender, RoutedEventArgs e)
        {
            TxtbError.Visibility = Visibility.Collapsed; // Collapse error if already there
            openDialog.Filter = "Json File|*.json"; // Filter file of type json
            openDialog.Title = "Choose Test Case File";
            bool? IsOpenClicked = openDialog.ShowDialog();
            if (IsOpenClicked == true)
            {
                // Display file name on mainwindow
                TxtbFileName.Text = openDialog.SafeFileName;
            }
        }

        private void BtnRunTest_Click(object sender, RoutedEventArgs e)
        {
            if (openDialog.FileName.Equals(string.Empty))
            {
                // Can't procced with a test case file
                TxtbError.Visibility = Visibility.Visible;
                TxtbError.Text = Properties.Resources.CannotproceedwithoutTestcase;
                return;
            }
            if(RuleManager.RuleSet.ListRule.Count==0)
            {
                // If there is not rule created yeat
                TxtbError.Visibility = Visibility.Visible;
                TxtbError.Text = Properties.Resources.YouHaveNotCreatedRuleYet;
                return;
            }
            TxtbError.Visibility = Visibility.Collapsed; // Hile error, Everything looks good
            MemoryStream stream = new MemoryStream(File.ReadAllBytes(openDialog.FileName)); // Resd test case file using MemoryStream
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(StreamData[]));
            StreamData[] result = serializer.ReadObject(stream) as StreamData[]; // Convery test case file into StreamData object array
            stream.Flush(); // Clear memory stream
            Result.Clear(); // Clear before adding new result
            ValidateStreamData(result);
            //var date = DateTime.Parse("2017-07-26 16:35:11", CultureInfo.CurrentCulture);
        }

        #region Main Algorithm

        private void ValidateStreamData(StreamData []streamData)
        {
            foreach(StreamData data in streamData)
            {
                // Find all maching rules for a Signal with a Value Type
                ObservableCollection<Rule> listOfMatchingRules = new ObservableCollection<Rule>(RuleManager.RuleSet.ListRule.Where(n => (n.Signal == data.signal && n.DataType==data.value_type)));
                foreach(Rule rule in listOfMatchingRules) // Validate for all rules for a signal with a value type
                {
                    if (!IsValid(data, rule))
                    {
                        // If a StreamData is against a rule add them to Result list
                        Result.Add(new ResultData { Signal = data.signal, Option = rule.Option, ValueRule = rule.Value, ValueStream = data.value });
                    }
                }
            }
        }

        private bool IsValid(StreamData streamData, Rule ruleData)
        {
            // Choose comparison method based on Value Type of Signal Data 
            switch (streamData.value_type)
            {
                case "Integer": return IntegerCompare(ruleData.Option, double.Parse(streamData.value), double.Parse(ruleData.Value));
                case "Datetime": return DatetimeCompare(ruleData.Option, streamData.value, ruleData.Value);
                case "String": return StringCompare(ruleData.Option, streamData.value, ruleData.Value);
            }
            return false;
        }

        public bool IntegerCompare<T>(string option, T dataValue, T ruleValue) where T : IComparable<T>
        {
            // Compare data based on options
            switch (option)
            {
                case "should not be greater than": return dataValue.CompareTo(ruleValue) < 0;
                case "should be greater than": return dataValue.CompareTo(ruleValue) > 0;
                case "should be less than": return dataValue.CompareTo(ruleValue) < 0;
                case "should not be less than": return dataValue.CompareTo(ruleValue) > 0;
                case "should be equal to": return dataValue.Equals(ruleValue);
                case "should not be equal to": return !dataValue.Equals(ruleValue);
                default: return false;
            }
        }

        public bool DatetimeCompare(string option, string dataValue, string ruleValue) 
        {
            // Compare data based on options
            switch (option)
            {
                case "should be in": 
                    if(ruleValue.Equals("future"))
                        return DateTime.Compare(DateTime.Parse(dataValue, CultureInfo.CurrentCulture), DateTime.Now) > 0;
                    else
                        return DateTime.Compare(DateTime.Parse(dataValue, CultureInfo.CurrentCulture), DateTime.Now) < 0;
                case "should not be in":
                    if (ruleValue.Equals("future"))
                        return DateTime.Compare(DateTime.Parse(dataValue, CultureInfo.CurrentCulture), DateTime.Now) < 0;
                    else
                        return DateTime.Compare(DateTime.Parse(dataValue, CultureInfo.CurrentCulture), DateTime.Now) > 0;
                default: return false;
            }
        }

        public bool StringCompare<T>(string option, T dataValue, T ruleValue) where T : IComparable<T>
        {
            // Compare data based on options
            switch (option)
            {
                case "should be": return dataValue.Equals(ruleValue);
                case "should not be": return !dataValue.Equals(ruleValue);
                default: return false;
            }
        }

        #endregion

        private void BtnCreateNew_Click(object sender, RoutedEventArgs e)
        {
            createNewWindow.Show();
            //RuleManager.WriteNewRule("ALT3", "Integer", "Greter", "20.24");
            //RuleManager.WriteNewRule("ALT4", "String", "Greter", "Suman");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            Application.Current.Shutdown();
        }

        private void BtnCross_Click(object sender, RoutedEventArgs e)
        {
            // Delete a rule
            RuleManager.DeleteRule((int)(sender as Button).Tag);
        }
    }
}
