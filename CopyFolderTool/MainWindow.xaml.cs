using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows;

namespace CopyFolderTool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            if (App.mArgs != null && App.mArgs.Length > 0)
            {
                //String[] args = App.mArgs;
                fieldSource.Text = checkForBackslash(App.mArgs[0]);
            }
        }
        private void copyToServer(object sender, RoutedEventArgs e)
        {
            string command;
            string sourcePath = checkForBackslash(fieldSource.Text);
            string destinationPath = checkForBackslash(fieldDestination.Text);

            //Optionen
            string optionE = "";
            string optionXO = "";
            string optionMOV = "";
            string optionClose = "";
            string standardOptions = " /MT:8";

            if (option_E.IsChecked == true)
            {
                optionE = " /E";
            }

            if (option_XO.IsChecked == true)
            {
                optionXO = " /XO  /FFT";
            }

            if (option_MOV.IsChecked == true)
            {
                optionXO = " /MOV";
            }

            if(option_Shutdown.IsChecked == true)
            {
                optionClose = "/C ";
            }
            else
            {
                optionClose = "/K ";
            }

            if (sourcePath.Length != 0 && destinationPath.Length != 0)
            {
                btn_startCopying.IsEnabled = false;
                command = optionClose + "ROBOCOPY \"" + @sourcePath + "\" \"" + @destinationPath + "\" "  + optionE + optionXO + optionMOV + standardOptions;
                System.Diagnostics.Process process = System.Diagnostics.Process.Start("CMD.exe", command);
                process.WaitForExit();
                Application.Current.Shutdown();

                if (option_Shutdown.IsChecked == true)
                {
                    _ = System.Diagnostics.Process.Start("Shutdown", "-s -t 10");
                }
            }
            else
            {
                MessageBox.Show("Please enter destination path!");
            }
        }

        private void selectDestinationFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                fieldDestination.Text = dialog.FileName;
            }
        }

        private string checkForBackslash(string inputStr)
        {
            string lastChar = inputStr.Substring(inputStr.Length - 1);
            if (lastChar == "\\")
            {
                return inputStr.Remove(inputStr.Length - 1);
            }
            else
            {
                return inputStr;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
