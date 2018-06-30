using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BEB_csharp05
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Wird mit DispatcherTimer timer aus MainWindow getriggert. Prüft, ob die Zeit AutoReadFtpStartTime abgelaufen ist, setzt sie ggf. neu und startet das Laden aus FTP-Verzeichnis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            if (DateTime.Now.CompareTo(Var.StartTimeFtpAutoRead) > 0)
            {
                IO.Log(2018062905, "Aktualisierung von Ftp/Datei/Datenbank (Letzte Prüfung: " + Var.StartTimeFtpAutoRead + ")");
                Var.StartTimeFtpAutoRead = DateTime.Now.AddHours(Var.HourTimePeriodForNewFtpUpdate);

                LoadFtp(Var.FtpDirInput, Var.LocDirInput);
                LoadDir(Var.LocDirInput, Var.ArchiveFile_Input, IO.WriteToDb_Input);
                LoadFtp(Var.FtpDirOperation, Var.LocDirOperation);
                LoadDir(Var.LocDirOperation, Var.ArchiveFile_Operation, IO.WriteToDb_Operation);

                LoadDir(Var.LocDirScales, Var.ArchiveFile_Scales, IO.WriteToDb_Scales);
            }


        }

        private async void LoadFtp(string ftpDir, string locDir)
        {
            // Cancellation token
            CancellationTokenSource cts = new CancellationTokenSource();

            // The Progress<T> constructor captures our UI context,
            //  so the lambda will be run on the UI thread.
            var progress1 = new Progress<int>(percent1 =>
            {
                Var.TaskProgress1 = percent1;
            });
            var progressIndex = new Progress<int>(i =>
            {
                ListView1.SelectedIndex = i;
            });
            var progress2 = new Progress<int>(percent2 =>
            {
                Var.TaskProgress2 = percent2;
            });

            /* Create Object Instance */
            Ftp ftpClient = new Ftp(ftpDir, Var.FtpUsername1, Var.FtpPassword1);

            /* Get Contents of a Directory (Names Only) */
            IEnumerable<string> filesToLoad = ftpClient.DirectoryListSimple("").Except(IO.ListFileNamesFromFolder(locDir, "*.csv")).Take(Var.MaxFilesCount);
            await Task.Run(() => Var.ListView1Content = filesToLoad);

            // DoProcessing is run on the thread pool.
            await Task.Run(() => Ftp.LoadFtpFiles(filesToLoad, ftpDir, locDir, progress1, progress2, progressIndex));

            Var.TaskProgress1 = 0;
            Var.TaskProgress2 = 0;
        }

        private async void LoadDir(string locDir, string archiveFile, IO.WriteToDb writeToDb)
        {
            // The Progress<T> constructor captures our UI context,
            //  so the lambda will be run on the UI thread.
            var progress1 = new Progress<int>(percent1 =>
            {
                Var.TaskProgress1 = percent1;
            });
            var progressInt = new Progress<int>(i =>
            {
                ListView1.SelectedIndex = i;
            });
            var progress2 = new Progress<int>(percent2 =>
            {
                Var.TaskProgress2 = percent2;
            });

            // Dateien aus dem Ordner, die in die DB geschrieben werden sollen.
            IEnumerable<string> filesToLoad = IO.ListFilesFromFolder(locDir, archiveFile).Take(Var.MaxFilesCount);

            List<string> fileNamesToLoad = new List<string>();
            foreach (string filePath in filesToLoad)
            {
                fileNamesToLoad.Add(Path.GetFileName(filePath));
            }

            ListView1.ItemsSource = fileNamesToLoad;

            // DoProcessing is run on the thread pool.
            //await IO.LoadDirToDb(IO.WriteToDb_Input, filesToLoad, progress1, progress2);
            await IO.LoadDirToDb(writeToDb, filesToLoad, progress1, progress2);

            //Clear the field.
            Var.TaskProgress1 = 0;
            Var.TaskProgress2 = 0;
            ListView1.ItemsSource = null;
        }

        // CancellationTokenSource cts;
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            // Request cancellation.
            cts.Cancel();
            IO.Log(20180626, "Cancellation set in token source...");
            Thread.Sleep(2500);
            // Cancellation should have happened, so call Dispose.
            cts.Dispose();
        }

        private void Button_ReadIni_Click(object sender, RoutedEventArgs e)
        {
            // Ini neu einladen und initialisieren
            InitializeApp();
        }

        private void Button_CheckFtpDir_Click(object sender, RoutedEventArgs e)
        {
            status_FtpDirInput.Fill = Ftp.DoesFtpDirectoryExist(Var.FtpDirInput);
            status_FtpDirOperation.Fill = Ftp.DoesFtpDirectoryExist(Var.FtpDirOperation);
            status_FtpDirScales.Fill = Ftp.DoesFtpDirectoryExist(Var.FtpDirScales);
        }

        private void Button_Click_LoadFtpInput(object sender, RoutedEventArgs e)
        {
            LoadFtp(Var.FtpDirInput, Var.LocDirInput);
            /*
            // Cancellation token
            CancellationTokenSource cts = new CancellationTokenSource();

            // The Progress<T> constructor captures our UI context,
            //  so the lambda will be run on the UI thread.
            var progress1 = new Progress<int>(percent1 =>
            {
                Var.TaskProgress1 = percent1;
            });
            var progressIndex = new Progress<int>(i =>
            {
                ListView1.SelectedIndex = i;
            });
            var progress2 = new Progress<int>(percent2 =>
            {
                Var.TaskProgress2 = percent2;
            });

            // Create Object Instance 
            Ftp ftpClient = new Ftp(Var.FtpDirInput, Var.FtpUsername1, Var.FtpPassword1);

            // Get Contents of a Directory (Names Only) 
            IEnumerable<string> filesToLoad = ftpClient.DirectoryListSimple("").Except(IO.ListFileNamesFromFolder(Var.LocDirInput, "*.csv")).Take(Var.MaxFilesCount);
            await Task.Run(() => Var.ListView1Content = filesToLoad);

            // DoProcessing is run on the thread pool.
            await Task.Run(() => Ftp.LoadFtpFiles(filesToLoad, Var.FtpDirInput, Var.LocDirInput,  progress1, progress2, progressIndex));

            Var.TaskProgress1 = 0;
            Var.TaskProgress2 = 0;
            */
        }

        private void Button_Click_LoadFtpOperation(object sender, RoutedEventArgs e)
        {
            LoadFtp(Var.FtpDirOperation, Var.LocDirOperation);
        }

        public void Button_Click_LoadDirInput(object sender, EventArgs e)
        {
            LoadDir(Var.LocDirInput, Var.ArchiveFile_Input, IO.WriteToDb_Input);
            /*
            // The Progress<T> constructor captures our UI context,
            //  so the lambda will be run on the UI thread.
            var progress1 = new Progress<int>(percent1 =>
            {
                Var.TaskProgress1 = percent1;
            });
            var progressInt = new Progress<int>(i =>
            {
                ListView1.SelectedIndex = i;
            });
            var progress2 = new Progress<int>(percent2 =>
            {
                Var.TaskProgress2 = percent2;
            });

            // Dateien aus dem Ordner, die in die DB geschrieben werden sollen.
            IEnumerable<string> filesToLoad = IO.ListFilesFromFolder(Var.LocDirInput, Var.ArchiveFile_Input).Take(Var.MaxFilesCount);

            List<string> fileNamesToLoad = new List<string>();
            foreach (string filePath in filesToLoad)
            {
                fileNamesToLoad.Add(Path.GetFileName(filePath));
            }

            ListView1.ItemsSource = fileNamesToLoad;

            // DoProcessing is run on the thread pool.
            await IO.LoadDirToDb(IO.WriteToDb_Input, filesToLoad, progress1, progress2);

            //Clear the field.
            Var.TaskProgress1 = 0;
            Var.TaskProgress2 = 0;
            ListView1.ItemsSource = null;
            */
        }

        public void Button_Click_LoadDirOperation(object sender, EventArgs e)
        {
            LoadDir(Var.LocDirOperation, Var.ArchiveFile_Operation, IO.WriteToDb_Operation);
            /*
            // The Progress<T> constructor captures our UI context,
            //  so the lambda will be run on the UI thread.
            var progress1 = new Progress<int>(percent1 =>
            {
                Var.TaskProgress1 = percent1;
            });
            var progressInt = new Progress<int>(i =>
            {
                ListView1.SelectedIndex = i;
            });
            var progress2 = new Progress<int>(percent2 =>
            {
                Var.TaskProgress2 = percent2;
            });

            // Dateien aus dem Ordner, die in die DB geschrieben werden sollen.
            IEnumerable<string> filesToLoad = IO.ListFilesFromFolder(Var.LocDirOperation, Var.ArchiveFile_Operation).Take(Var.MaxFilesCount);

            List<string> fileNamesToLoad = new List<string>();
            foreach (string filePath in filesToLoad)
            {
                fileNamesToLoad.Add(Path.GetFileName(filePath));
            }
            ListView1.ItemsSource = fileNamesToLoad;

            // DoProcessing is run on the thread pool.
            await IO.LoadDirToDb(IO.WriteToDb_Operation, filesToLoad, progress1, progress2);

            //Clear the field.
            Var.TaskProgress1 = 0;
            Var.TaskProgress2 = 0;
            ListView1.ItemsSource = null;
            */
        }

        public void Button_Click_LoadDirScales(object sender, EventArgs e)
        {
            LoadDir(Var.LocDirScales, Var.ArchiveFile_Scales, IO.WriteToDb_Scales);
            /*
            // The Progress<T> constructor captures our UI context,
            //  so the lambda will be run on the UI thread.
            var progress1 = new Progress<int>(percent1 =>
            {
                Var.TaskProgress1 = percent1;
            });
            var progressInt = new Progress<int>(i =>
            {
                ListView1.SelectedIndex = i;
            });
            var progress2 = new Progress<int>(percent2 =>
            {
                Var.TaskProgress2 = percent2;
            });

            // Dateien aus dem Ordner, die in die DB geschrieben werden sollen.
            IEnumerable<string> filesToLoad = IO.ListFilesFromFolder(Var.LocDirScales, Var.ArchiveFile_Scales).Take(Var.MaxFilesCount);

            List<string> fileNamesToLoad = new List<string>();
            foreach (string filePath in filesToLoad)
            {
                fileNamesToLoad.Add(Path.GetFileName(filePath));
            }
            ListView1.ItemsSource = fileNamesToLoad;

            // DoProcessing is run on the thread pool.
            await IO.LoadDirToDb(IO.WriteToDb_Scales, filesToLoad, progress1, progress2);

            //Clear the field.
            Var.TaskProgress1 = 0;
            Var.TaskProgress2 = 0;
            ListView1.ItemsSource = null;
            */
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////
   
        private void ComboBox_Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comboBox_Table.SelectedIndex)
            {
                case 1:
                    Var.SqlSelectedColumns = "MIN(Zeit) AS Zeit, ROUND(AVG(Gasfüllstand_ges),0) AS [Gasfüllstand ges %], ROUND(AVG(Gasfüllstand_F1),0) AS [Gasfüllstand F1 %], ROUND(AVG(Gasfüllstand_F2),0) AS [Gasfüllstand F2 %], ROUND(AVG(Gasfüllstand_NG),0) AS [Gasfüllstand NG %], ROUND(AVG(Gasfüllstand_GPL),0) AS [Gasfüllstand GPL %], ROUND(AVG(Gasanalyse_CH4),1) AS [Gasanalyse CH4 %], ROUND(AVG(Gasanalyse_O2),1) AS [Gasanalyse O2 %], ROUND(AVG(Gasanalyse_H2S),0) AS [Gasanalyse H2S ppm], ROUND(AVG(BHKW_kW),0) AS [BHKW kW] FROM Operation";
                    Var.ControlVisibility = Visibility.Collapsed;
                    break;
                case 2:
                    Var.SqlSelectedColumns = "MIN(Zeit) AS Zeit, SchlagNr AS [Schlag-Nr], Sorte AS [Sorte], Kunde_Name AS [Kunde], SUM(Nettogewicht) FROM Scales ";
                    Var.ControlVisibility = Visibility.Visible;
                    break;
                default:
                    Var.SqlSelectedColumns = "Min(Zeit) AS Zeit, SUM(Annahme) AS Annahme, SUM(Feststoff) AS Feststoff, ROUND( CAST(SUM(Annahme) AS FLOAT)* 100.0  / ( SUM(Annahme) + SUM(Feststoff) ), 1) || '%' AS Gülleanteil FROM Input";
                    Var.ControlVisibility = Visibility.Collapsed;
                    break;
            }

            Var.SqlAssembleCommand();
        }

        private void ComboBox_Group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comboBox_Group.SelectedIndex)
            {
                case 0:
                    //Alles
                    Var.SqlSelectedGroupBy = "strftime('%Y', Zeit), strftime('%m', Zeit), strftime('%d', Zeit), strftime('%H', Zeit), strftime('%M', Zeit)";
                    break;
                case 1:
                    //Stunden
                    Var.SqlSelectedGroupBy = "strftime('%Y', Zeit), strftime('%m', Zeit), strftime('%d', Zeit), strftime('%H', Zeit)";
                    break;
                case 2:
                    //Tage
                    Var.SqlSelectedGroupBy = "strftime('%Y', Zeit), strftime('%m', Zeit), strftime('%d', Zeit)";
                    break;
                case 3:
                    //Monate
                    Var.SqlSelectedGroupBy = "strftime('%Y', Zeit), strftime('%m', Zeit)";
                    break;
                case 4:
                    //Jahre
                    Var.SqlSelectedGroupBy = "strftime('%Y', Zeit)";
                    break;
                default:
                    Var.SqlSelectedGroupBy = "";
                    break;
                    
            }
            Var.SqlAssembleCommand();
        }

        private void DatePickerVon_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerVon == null || comboBox_EndDatePattern == null || datePickerBis == null)
            {
                return;
            }

            try
            {
                DateTime von = datePickerVon.SelectedDate.GetValueOrDefault();
                Var.SqlSelectedTimeVon = von.ToString("yyyy-MM-dd HH':'mm':'ss");
                datePickerBis.IsEnabled = false;

                //if (comboBox_EndDatePattern == null) 

                switch (comboBox_EndDatePattern.SelectedIndex)
                {
                    case 1:
                        Var.SqlSelectedTimeBis = von.AddDays(1).ToString("yyyy-MM-dd HH':'mm':'ss");
                        break;
                    case 2:
                        Var.SqlSelectedTimeBis = von.AddMonths(1).ToString("yyyy-MM-dd HH':'mm':'ss");
                        break;
                    case 3:
                        Var.SqlSelectedTimeBis = von.AddYears(1).ToString("yyyy-MM-dd HH':'mm':'ss");
                        break;
                    default:
                        datePickerBis.IsEnabled = true;
                        Var.SqlSelectedTimeBis = datePickerBis.SelectedDate.Value.ToString("yyyy-MM-dd HH':'mm':'ss");
                        break;
                }

                datePickerBis.SelectedDate = Convert.ToDateTime(Var.SqlSelectedTimeBis);
                Var.SqlAssembleCommand();
            }
            catch (NullReferenceException exception)
            {
                MessageBox.Show(exception.Message + "\r\n" + exception.InnerException + "\r\n" + exception.StackTrace, " Null-Referenz-Fehler");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\r\n" + exception.InnerException + "\r\n" + exception.Source);
            }
        }

        private void DatePickerBis_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Var.SqlSelectedTimeBis = datePickerBis.SelectedDate.Value.ToString("yyyy-MM-dd HH':'mm':'ss");
            if (datePickerBis.IsEnabled)
            {
                Var.SqlAssembleCommand();
            }
        }

        private void ComboBox_Sorte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_Kunde_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_tableToExcel_Click(object sender, RoutedEventArgs e)
        {
            string targetPath = Path.Combine(Var.LocDirExcel, "BEB_" + Var.SqlSelectedTimeVon.Substring(0,10) + "_" + Var.SqlSelectedTimeBis.Substring(0,10) + ".xls");
            IO.Log(2018062806, "Speichere Excel-Datei: " + targetPath);

            ExcelClass.ExportToExcel(Var.MyDataTable, targetPath);
            System.Diagnostics.Process.Start(Var.LocDirExcel);
        }

        private void Button_ListFilesFromFolder_Click(object sender, RoutedEventArgs e)
        {
            switch (ComboBox_ListToShow.SelectedIndex)
            {
                case 0:
                    ListView1.ItemsSource = IO.ListFileNamesFromFolder(Var.LocDirInput, "*.csv");
                    break;
                case 1:
                    Ftp ftp1 = new Ftp(Var.FtpDirInput, Var.FtpUsername1, Var.FtpPassword1);
                    ListView1.ItemsSource = ftp1.DirectoryListSimple("");
                    break;
                case 2:
                    ListView1.ItemsSource = IO.ListFileNamesFromFolder(Var.LocDirOperation, "*.csv");
                    break;
                case 3:
                    Ftp ftp3 = new Ftp(Var.FtpDirOperation, Var.FtpUsername1, Var.FtpPassword1);
                    ListView1.ItemsSource = ftp3.DirectoryListSimple("");
                    break;
                case 4:
                    ListView1.ItemsSource = IO.ListFileNamesFromFolder(Var.LocDirScales, "*.csv");
                    break;
                case 5:
                    Ftp ftp5 = new Ftp(Var.FtpDirScales, Var.FtpUsername1, Var.FtpPassword1);
                    ListView1.ItemsSource = ftp5.DirectoryListSimple("");
                    break;
                default:
                    ListView1.ItemsSource = null;
                    break;
            }
        }

        public void Tab_Log_GotFocus(object sender, RoutedEventArgs e)
        {
            //Lade Logfile
            if (File.Exists(Var.LogPath))
            {
                Var.CurrentLogFileContent = File.ReadAllText(Var.LogPath);
            }
        }

        public void Tab_Tabelle_GotFocus(object sender, RoutedEventArgs e)
        {
            //lade die Tabelle
            Var.SqlAssembleCommand();
        }

    }
}