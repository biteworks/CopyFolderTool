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
            
            if (App.mArgs.Length > 0)
            {
                String[] args = App.mArgs;
                fieldSource.Text = @args[0];
            }
        }
        private void copyToServer(object sender, RoutedEventArgs e)
        {
            string command;
            string sourcePath = @fieldSource.Text;
            string destinationPath = @fieldDestination.Text;

            //Optionen
            string optionE = "";
            string optionXO = "";
            string optionMOV = "";
            string standardOptions = " /MT:8 /FFT";

            if (option_E.IsChecked == true)
            {
                optionE = " /E";
            }

            if (option_XO.IsChecked == true)
            {
                optionXO = " /XO";
            }

            if (option_MOV.IsChecked == true)
            {
                optionXO = " /MOV";
            }

            if (sourcePath.Length != 0 && destinationPath.Length != 0)
            {
                command = "/K ROBOCOPY \"" + @sourcePath + "\" \"" + @destinationPath + "\" "  + optionE + optionXO + optionMOV + standardOptions;
                System.Diagnostics.Process process = System.Diagnostics.Process.Start("CMD.exe", command);
                process.WaitForExit();
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
    }
}
