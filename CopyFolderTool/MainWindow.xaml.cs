using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace CopyFolderTool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string strSettingsFilePath = System.IO.Path.Combine(strWorkPath, "Paths.config");

        public MainWindow()
        {
            InitializeComponent();
            
            if (App.mArgs != null && App.mArgs.Length > 0)
            {
                fieldSource.Text = checkForBackslash(App.mArgs[0]);
                int filesCount = getFilesCount(fieldSource.Text);
                if(filesCount > 0)
                {
                    fileCountNotice.Text = "Source directory contains " + filesCount + " files.";
                }
            }

            readLogfilePath();
        }
        private void StartRobocopy(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;

            string command;
            string sourcePath;
            string destinationPath;
            string logPath;

            if (String.IsNullOrEmpty(fieldSource.Text) || String.IsNullOrEmpty(fieldDestination.Text))
            {
                MessageBox.Show("Please enter valid paths!", "Path missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                sourcePath = checkForBackslash(fieldSource.Text);
                destinationPath = checkForBackslash(fieldDestination.Text);
            }

            //Optionen
            string optionE = "";
            string optionXO = "";
            string optionMOV = "";
            string optionLog = "";
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

            if (option_Logfile.IsChecked == true)
            {
                if (String.IsNullOrEmpty(fieldLogfile.Text))
                {
                    MessageBox.Show("Please enter paths for log file.", "Path missing", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    logPath = checkForBackslash(fieldLogfile.Text);
                    // Pfad Probleme
                    optionLog = " /LOG:\"" + logPath + "\\CopyFolderTool_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log\" /TEE /NDL";
                }
            }

            if(option_Shutdown.IsChecked == true)
            {
                optionClose = "/C ";
            }
            else
            {
                optionClose = "/K ";
            }

            if (button.Name == "btn_startCopying")
            {
                btn_startCopying.IsEnabled = false;
                command = optionClose + "ROBOCOPY \"" + @sourcePath + "\" \"" + @destinationPath + "\" " + optionE + optionXO + optionMOV + optionLog + standardOptions;
                System.Diagnostics.Process process = System.Diagnostics.Process.Start("CMD.exe", command);
                process.WaitForExit();
                Application.Current.Shutdown();
                btn_startComparing.IsEnabled = true;

                if (option_Shutdown.IsChecked == true)
                {
                    _ = System.Diagnostics.Process.Start("Shutdown", "-s -t 10");
                }
            }
            else if (button.Name == "btn_startComparing")
            {
                btn_startComparing.IsEnabled = false;
                command = optionClose + "ROBOCOPY \"" + @sourcePath + "\" \"" + @destinationPath + "\" " + "/L /NJH /NJS /NP /NS";
                System.Diagnostics.Process process = System.Diagnostics.Process.Start("CMD.exe", command);
                process.WaitForExit();
                btn_startComparing.IsEnabled = true;
            }

        }

        private void selectFolder(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                switch (button.Name)
                {
                    case "btn_SourcePath":
                        fieldSource.Text = dialog.FileName;
                        int filesCount = getFilesCount(fieldSource.Text);
                        if (filesCount > 0)
                        {
                            fileCountNotice.Text = "Source directory contains " + filesCount + " files.";
                        }
                        break;
                    case "btn_DestinationPath":
                        fieldDestination.Text = dialog.FileName;
                        break;
                    case "btn_Logfile":
                        fieldLogfile.Text = dialog.FileName;
                        saveLogfilePath(fieldLogfile.Text);
                        break;
                    default:
                        Console.WriteLine("Wrong Button");
                        break;
                }
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

        private static int getFilesCount(string path)
        {
            try
            {
                var filesCount = Directory.GetFiles(@path, "*.*", SearchOption.AllDirectories).Count();
                return filesCount;
            }
            catch
            {
                return 0;
            }
        } 

        private void readLogfilePath()
        {
            if (!File.Exists(strSettingsFilePath))
            {
                using (StreamWriter sw = File.CreateText(strSettingsFilePath))
                {
                    sw.WriteLine("C:\\Logs\\");
                }
                fieldLogfile.Text = "C:\\Logs\\";
            }
            else {
                string[] lines = File.ReadAllLines(strSettingsFilePath);
                fieldLogfile.Text = lines[0];
            }
            return;
        }
        private void saveLogfilePath(string path)
        {
            File.WriteAllText(strSettingsFilePath, path);
            return;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
