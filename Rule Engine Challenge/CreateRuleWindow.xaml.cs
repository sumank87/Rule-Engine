using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rule_Engine_Challenge
{
    /// <summary>
    /// Interaction logic for CreateRuleWindow.xaml
    /// </summary>
    public partial class CreateRuleWindow : Window
    {
        List<string> ListSignal = new List<string> { "-- Select Signal --", "ATL1", "ATL2", "ATL3", "ATL4", "ATL5", "ATL6", "ATL7", "ATL8", "ATL9","ATL10" };
        List<string> ListValueType = new List<string> { "-- Select Value Type --", "Integer", "Datetime", "String" };
        List<string> ListIntegerOptions = new List<string> { "-- Choose an Option --","should be greater than", "should not be greater than","should be less than", "should not be less than", "should be equal to", "should not be equal to" };
        List<string> ListDateAndTimeOptions = new List<string> { "-- Choose an Option --", "should be in", "should not be in"};
        List<string> ListStringOptions = new List<string> { "-- Choose an Option --", "should be", "should not be" };
        List<string> ListStringValues = new List<string> { "-- Choose an Option --", "HIGH", "LOW" };
        List<string> ListDateAndTimeValues = new List<string> { "-- Choose an Option --", "future", "past" };
        MainWindow mainWindow;
        public CreateRuleWindow(MainWindow mainWin)
        {
            InitializeComponent();
            mainWindow = mainWin;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboSignal.ItemsSource = ListSignal;
            ComboSignal.SelectedItem = ListSignal[0];
            ComboDataType.ItemsSource = ListValueType;
            ComboDataType.SelectedItem = ListValueType[0];
            Owner = mainWindow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (ComboDataType.SelectedIndex == 1)
                RuleManager.WriteNewRule(ComboSignal.SelectedValue.ToString(), ComboDataType.SelectedValue.ToString(), ComboOptions.SelectedValue.ToString(), TextboxValue.Text);
            else
                RuleManager.WriteNewRule(ComboSignal.SelectedValue.ToString(), ComboDataType.SelectedValue.ToString(), ComboOptions.SelectedValue.ToString(), ComboValue.SelectedValue.ToString());
            bool IsContinue = CheckBoxContinue.IsChecked == null ? false : (bool)CheckBoxContinue.IsChecked;
            ComboSignal.SelectedIndex = 0;
            ComboDataType.SelectedIndex = 0;
            ComboOptions.SelectedIndex = 0;
            ComboValue.SelectedIndex = 0;
            TextboxValue.Text = string.Empty;
            if (!IsContinue)
            {
                Hide();
            }
        }

        private void ComboDataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboDataType.SelectedIndex == 1)
            {
                ComboOptions.IsEnabled = true;
                ComboValue.Visibility = Visibility.Collapsed;
                TextboxValue.Visibility = Visibility.Visible;
                ComboOptions.ItemsSource = ListIntegerOptions;
                ComboOptions.SelectedItem = ListIntegerOptions[0];
            }
            else if (ComboDataType.SelectedIndex == 2)
            {
                ComboOptions.IsEnabled = true;
                ComboValue.Visibility = Visibility.Visible;
                TextboxValue.Visibility = Visibility.Collapsed;
                ComboOptions.ItemsSource = ListDateAndTimeOptions;
                ComboOptions.SelectedItem = ListDateAndTimeOptions[0];
                ComboValue.ItemsSource = ListDateAndTimeValues;
                ComboValue.SelectedItem = ListDateAndTimeValues[0];
            }
            else if (ComboDataType.SelectedIndex == 3)
            {
                ComboOptions.IsEnabled = true;
                ComboValue.Visibility = Visibility.Visible;
                TextboxValue.Visibility = Visibility.Collapsed;
                ComboOptions.ItemsSource = ListStringOptions;
                ComboOptions.SelectedItem = ListStringOptions[0];
                ComboValue.ItemsSource = ListStringValues;
                ComboValue.SelectedItem = ListStringValues[0];
            }
            else
            {
                ComboOptions.IsEnabled = false;
            }

            EnableCreateButton();
        }

        private void ComboOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboOptions.SelectedIndex == 0)
                GridValue.IsEnabled = false;
            else
                GridValue.IsEnabled = true;
            EnableCreateButton();
        }

        private void TextboxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (char.IsNumber(c) || !TextboxValue.Text.Equals(string.Empty))
            {
                double num = 0.0;
                if (double.TryParse(string.Format(TextboxValue.Text + e.Text), out num))
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            else
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void EnableCreateButton()
        {
            if (ComboSignal.SelectedIndex == 0 || ComboDataType.SelectedIndex == 0 || ComboOptions.SelectedIndex == 0)
            {
                BtnCreate.IsEnabled = false;
            }
            else if ((ComboDataType.SelectedIndex==1 && TextboxValue.Text ==string.Empty) || (ComboDataType.SelectedIndex != 1 && ComboValue.SelectedIndex==0))
            {
                BtnCreate.IsEnabled = false;
            }
            else
            {
                BtnCreate.IsEnabled = true;
            }
        }

        private void TextboxValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableCreateButton();
        }

        private void ComboSignal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableCreateButton();
        }

        private void ComboValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableCreateButton();
        }
    }
}
