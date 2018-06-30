using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BEB_csharp05
{
    class IO
    {
        public static void CreateDirWithWriteAccess(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    DirectorySecurity securityRules = new DirectorySecurity();
                    SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
                    securityRules.AddAccessRule(new FileSystemAccessRule(sid, FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.CreateDirectory(path, securityRules);

                    IO.Log(2018062506, Directory.GetAccessControl(path).ToString());
                }
                catch (Exception ex)
                {
                    IO.Log(2018062506, "Ordner <" + path + "> konnte nicht mit Schreibrechten erstellt werden. | " + ex.Message);
                }
            }
        }

        public static void Log(long faultNumber, string logText)
        {
            if (!Var.GlobalLoggingAllowed) { return; }

            CreateDirWithWriteAccess(Path.Combine(Var.AppDir, "Log"));

            StreamWriter sw = File.AppendText(Var.LogPath);
            try
            {
                sw.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + faultNumber.ToString("D" + 5) + "\t" + logText);
            }
            catch
            {
                MessageBox.Show("Logging nicht möglich. Schreibrechte im Stammordner prüfen.", "Log()", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// Prüft Ob die Datei locIniPath existiert. Wenn nicht, wird sie neu erstellt und mit Standardwerten beschrieben.
        /// </summary>
        /// <param name="locIniPath"></param>
        public static void CheckAndCreateIniFile(string locIniPath)
        {

            try
            {
                string locIniDir = Path.GetDirectoryName(locIniPath);
                Directory.CreateDirectory(locIniDir);
            }
            catch (Exception ex)
            {
                IO.Log(2018062107, ex.GetType() + ex.Message);
            }


            if (!File.Exists(locIniPath))
            {
                FileInfo FI = new FileInfo(locIniPath);
                FileStream FS = FI.Create();
                FS.Close();

                NativeMethods Ini;
                Ini = new NativeMethods(locIniPath);
                Ini["Allgemein"]["GlobalLoggingAllowed"] = "true";
                Ini["Allgemein"]["SleepingMilliSec"] = "100";

                Ini["DIR"]["LocDirInput"] = Path.Combine(Var.AppDir, "Data", "Input");
                Ini["DIR"]["LocDirOperation"] = Path.Combine(Var.AppDir, "Data", "Operation");
                Ini["DIR"]["LocDirScales"] = Path.Combine(Var.AppDir, "Data", "Scales");
                Ini["DIR"]["LocDirExcel"] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BEB_Excel");

                Ini["FTP"][";FtpDir_Input"] = @"ftp://192.168.1.100/SAMP04";
                Ini["FTP"][";FtpDir_Operation"] = @"ftp://192.168.1.100/SAMP03";

                Ini["FTP"]["FtpDir_Input"] = @"ftp://localhost/input/";
                Ini["FTP"]["FtpDir_Operation"] = @"ftp://localhost/operation/";
                Ini["FTP"]["FtpDir_Scales"] = @"ftp://localhost/scales/";
                Ini["FTP"]["Username1"] = "ftpuser";
                Ini["FTP"]["Password1"] = "ftpuser";

                Ini["DB"]["DbPath"] = Path.Combine(Var.AppDir, "Konfig", "BEBDB.sqlite");

                Ini["MODBUS"]["Com-Port"] = "COM1";
            }
        }

        /// <summary>
        /// Überprüft den Pfad FilePath und gibt wenn vorhanden den Brush Green wieder. Ansonsten Red. 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns>Grün oder Rot</returns>
        public static System.Windows.Media.Brush CheckPath_GiveColor(string FilePath)
        {
            if (File.Exists(FilePath) || Directory.Exists(FilePath))
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Red;
            }

        }

        /// <summary>
        /// List die Dateinamen von CSV-Dateien aus locDirPath und gibt sie wenn sie nicht in der Datei locListFilePath sind als IEnumerable string  aus. 
        /// </summary>
        /// <param name="locDirPath">Pfad zum Ordner, der die Dateien enthält</param>
        /// <param name="locListFilePath">Pfad zu Textdatei, die Ausnahmen enthält.</param>
        /// <returns>Liste der CSV-Dateinamen aus locDirPath ohne die Ausnahmen aus locListFilePath</returns>
        public static IEnumerable<string> ListFilesFromFolder(string locDirPath, string locListFilePath)
        {
            List<string> logFilesList = new List<string> { "" };

            if (File.Exists(locListFilePath))
            {
                // Read each line of the file into a string array. Each element of the array is one line of the file.
                string[] lines = File.ReadAllLines(locListFilePath);

                // Go through the file contents by using a foreach loop.
                if (lines.Length > 0)
                {
                    foreach (string line in lines)
                    {
                        logFilesList.Add(Path.Combine(locDirPath, line.Trim()));
                    }
                }
            }
            else
            {
                IO.Log(2018062206, "Datei '" + locListFilePath + "' existiert nicht.");
                logFilesList = new List<string>() { "" };
            }

            try
            {
                //Lese das Verzeichnis, schließe Ausnahmen aus.
                return Directory.EnumerateFiles(locDirPath, "*.csv").Except(logFilesList);
            }
            catch (Exception ex)
            {
                IO.Log(2018062207, ex.Message + " | " + ex.Source);
                return null;
            }
        }

        /// <summary>
        /// Listet die Dateinamen aus dem Ordner locDirPath mit Endung, mit filter 
        /// </summary>
        /// <param name="locDirPath"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<string> ListFileNamesFromFolder(string locDirPath, string filter)
        {
            var dirList = Directory.GetFiles(locDirPath, filter);
            List<string> liste = new List<string>();
            foreach (var file in dirList)
            {
                liste.Add(Path.GetFileName(file));
            }
            return liste;
        }

        public static void WriteToDb_Input(string sourceFilePath, IProgress<int> progress)
        {
            bool keinFehler = true;
            try
            {
                Sql sql = new Sql(Var.DbPath);
                int lineInFile = 0;
                //nur die Zahl aus dem Dateinamen verwenden
                string dateiname = Path.GetFileNameWithoutExtension(sourceFilePath).Substring(2);

                // Quelle: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-read-from-a-text-file
                // Read each line of the file into a string array. Each element of the array is one line of the file.
                //string[] lines = File.ReadAllLines(sourceFilePath); // @"C:\Users\Public\TestFolder\WriteLines2.txt"

                var reader = File.OpenText(sourceFilePath);
                var fileText = reader.ReadToEnd();
                string[] lines = fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int totLines = lines.Count();

                // Go through the file contents by using a foreach loop.
                foreach (string line in lines)
                {
                    ++lineInFile;
                    if (progress != null) progress.Report(lineInFile);

                    if (line.Length < 2) continue;
                    string[] items = line.Replace("\"", "").Split(','); // Entferne Anführungszeichen, teile beim ','

                    // Wenn das erste Zeichen keine Zahl ist: Überspringe die Zeile.       
                    if (!char.IsNumber(items[0].ToCharArray()[0])) continue;

                    string zeit = "20" + items[0].Replace('/', '-') + " " + items[1];
                    int annahme = 0, annahme1 = 0, annahme2 = 0;
                    int feststoff = 0, feststoff1 = 0, feststoff2 = 0;

                    if (int.TryParse(items[2], out annahme1)) annahme += annahme1;
                    if (int.TryParse(items[3], out annahme2)) annahme += annahme2;
                    //if (int.TryParse(items[6], out annahme3)) annahme += annahme3;  // Gülle in Nachgärer gepumpt.

                    if (int.TryParse(items[4], out feststoff1)) feststoff += feststoff1;
                    if (int.TryParse(items[5], out feststoff2)) feststoff += feststoff2;

                    Dictionary<string, string> data = new Dictionary<string, string>
                        {
                            { "Zeit", zeit },
                            { "Datei", dateiname + "-" + lineInFile.ToString("D3") },
                            { "Annahme", annahme.ToString() },
                            { "Feststoff", feststoff.ToString() }
                        };

                    if (Var.CheckforDuplicateInDb)
                    {
                        // nur eintragen, wenn Zeitstempel noch nicht vorhanden?
                        if (sql.ExecuteScalar("SELECT 1 FROM Input WHERE Zeit ='" + zeit + "'").Length > 0) continue;
                    }

                    try
                    {
                        if (!sql.Insert("Input", data))
                        {
                            keinFehler = false;
                            IO.Log(2018062208, "Die Zeile " + lineInFile.ToString() + " aus Datei '" + sourceFilePath + "' konnte nicht in die Datenbank übernommen werden.");
                        }
                    }
                    catch (Exception exception)
                    {
                        IO.Log(2018062202, "Items.Lenght:" + items.Length + " | " + exception.Message + " | " + exception.InnerException + " | " + exception.Source);
                    }
                }

                //Dateinamen in Protokolldatei anhängen. 
                if (keinFehler)
                {
                    using (StreamWriter sw = File.AppendText(Var.ArchiveFile_Input))
                    {
                        sw.WriteLine(Path.GetFileName(sourceFilePath));
                    }
                }
            }
            catch (FieldAccessException exception)
            {
                IO.Log(2018062204, "FieldAccessException: " + exception.Message + "\r\n" + exception.InnerException);

            }
            catch (IndexOutOfRangeException exception)
            {
                IO.Log(2018062205, exception.Message + " | Datei: '" + sourceFilePath + "' entspricht nicht dem erwarteten Schema.");
            }
            catch (Exception exception)
            {
                IO.Log(2018062203,
                                "Datei: " + sourceFilePath + "\r\n" +
                                exception.Message + "\r\n" +
                                exception.GetType() + "\r\n" +
                                exception.InnerException);
            }
        }

        public static void WriteToDb_Operation(string sourceFilePath, IProgress<int> progress)
        {
            bool keinFehler = true;
            try
            {
                Sql sql = new Sql(Var.DbPath);
                int lineInFile = 0;
                //nur die Zahl aus dem Dateinamen verwenden
                string dateiname = Path.GetFileNameWithoutExtension(sourceFilePath).Substring(2);

                // Quelle: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-read-from-a-text-file
                // Read each line of the file into a string array. Each element of the array is one line of the file.

                var reader = File.OpenText(sourceFilePath);
                var fileText = reader.ReadToEnd();
                string[] lines = fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int totLines = lines.Count();

                // Go through the file contents by using a foreach loop.
                foreach (string line in lines)
                {
                    ++lineInFile;
                    if (progress != null) progress.Report(lineInFile);

                    if (line.Length < 2) continue;
                    string[] items = line.Replace("\"", "").Split(','); // Entferne Anführungszeichen, teile beim ','

                    // Wenn das erste Zeichen keine Zahl ist: Überspringe die Zeile.       
                    if (!char.IsNumber(items[0].ToCharArray()[0])) continue;

                    string zeit = "20" + items[0].Replace('/', '-') + " " + items[1];

                    Dictionary<string, string> data = new Dictionary<string, string>
                        {
                            { "Zeit", zeit },
                            { "Gasfüllstand_ges", items[2] },
                            { "Gasfüllstand_F1", items[3] },
                            { "Gasfüllstand_F2", items[4] },
                            { "Gasfüllstand_NG", items[5] },
                            { "Gasfüllstand_GPL", items[6] },
                            { "Gasanalyse_CH4", items[7] },
                            { "Gasanalyse_O2", items[8] },
                            { "Gasanalyse_H2S", items[9] },
                            { "BHKW_kW", items[10] },
                            { "Temp_F1", items[12] },
                            { "Temp_F2", items[13] },
                            { "Temp_NG", items[14] }
                        };

                    if (Var.CheckforDuplicateInDb)
                    {
                        // nur eintragen, wenn Zeitstempel noch nicht vorhanden?
                        if (sql.ExecuteScalar("SELECT 1 FROM Operation WHERE Zeit ='" + zeit + "'").Length > 0) continue;
                    }

                    try
                    {
                        if (!sql.Insert("Operation", data))
                        {
                            keinFehler = false;
                            IO.Log(2018062208, "Die Zeile " + lineInFile.ToString() + " aus Datei '" + sourceFilePath + "' konnte nicht in die Datenbank übernommen werden.");
                        }
                    }
                    catch (Exception exception)
                    {
                        IO.Log(2018062701, "Items.Lenght:" + items.Length + " | " + exception.Message + " | " + exception.InnerException + " | " + exception.Source);
                    }
                }

                //Dateinamen in Protokolldatei anhängen. 
                if (keinFehler)
                {
                    using (StreamWriter sw = File.AppendText(Var.ArchiveFile_Operation))
                    {
                        sw.WriteLine(Path.GetFileName(sourceFilePath));
                    }
                }
            }
            catch (FieldAccessException exception)
            {
                IO.Log(2018062702, "FieldAccessException: " + exception.Message + "\r\n" + exception.InnerException);

            }
            catch (IndexOutOfRangeException exception)
            {
                IO.Log(2018062703, exception.Message + " | Datei: '" + sourceFilePath + "' entspricht nicht dem erwarteten Schema.");
            }
            catch (Exception exception)
            {
                IO.Log(2018062704,
                                "Datei: " + sourceFilePath + "\r\n" +
                                exception.Message + "\r\n" +
                                exception.GetType() + "\r\n" +
                                exception.InnerException);
            }
        }

        public static void WriteToDb_Scales(string sourceFilePath, IProgress<int> progress)
        {
            bool keinFehler = true;
            try
            {
                Sql sql = new Sql(Var.DbPath);
                int lineInFile = 0;
                //nur die Zahl aus dem Dateinamen verwenden
                string dateiname = Path.GetFileNameWithoutExtension(sourceFilePath);

                // Quelle: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-read-from-a-text-file
                // Read each line of the file into a string array. Each element of the array is one line of the file.

                StreamReader reader = new StreamReader(sourceFilePath, Encoding.Default );
                var fileText = reader.ReadToEnd();
 
                IO.Log(123456, fileText);

                string[] lines = fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int totLines = lines.Count();

                // Go through the file contents by using a foreach loop.
                foreach (string line in lines)
                {
                    ++lineInFile;
                    if (progress != null) progress.Report(lineInFile);

                    if (line.Length < 2) continue;
                    string[] items = line.Replace("\"", "").Split(';'); // Entferne Anführungszeichen, teile beim ','

                    // Wenn das erste Zeichen keine Zahl ist: Überspringe die Zeile.       
                    if (!char.IsNumber(items[0].ToCharArray()[0])) continue;

                    string[] datum = items[19].Split('.');
                    string zeit = "20" + datum[2].ToString() + "-" + datum[1].ToString() + "-" + datum[0].ToString() + " " + items[20];

                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                            { "LfdNr", items[0] },
                            { "Datei", dateiname },
                            { "SchlagNr", items[2] },
                            { "Sorte", items[11].Trim() },
                            { "Nettogewicht", items[13] },
                            { "Kunde_Name", items[8].Trim() },
                            { "Kennzeichen", items[3].Trim() },
                            { "Erst_Gewicht", items[17] },
                            { "Tara_Hand", items[18].ToString() },
                            { "Zweit_Gewicht", items[21] },
                            { "Zeit", zeit }
                    };

                    /*
                     *      [Scales] (
                            [LfdNr]         INTEGER NOT NULL PRIMARY KEY,
                            [Datei]         TEXT NOT NULL,
                            [SchlagNr]      INT,
                            [Sorte]         TEXT NOT NULL,
                            [Nettogewicht]  INT,
                            [Kunde_Name]    TEXT NOT NULL,
                            [Erst_Gewicht]  INT,
                            [Tara_Hand]     TEXT,
                            [Zweit_Gewicht] INT,
                            [Zeit]          TEXT NOT NULL
                            )");
                    */
                    
                    if (Var.CheckforDuplicateInDb)
                    {
                        // nur eintragen, wenn Zeitstempel noch nicht vorhanden?
                        if (sql.ExecuteScalar("SELECT 1 FROM Operation WHERE Zeit ='" + zeit + "'").Length > 0)
                        {
                            continue;
                        }
                    }

                    try
                    {
                        if (!sql.Insert("Scales", data))
                        {
                            keinFehler = false;
                            IO.Log(2018062908, "Die Zeile " + lineInFile.ToString() + " aus Datei '" + sourceFilePath + "' konnte nicht in die Datenbank übernommen werden.");
                        }
                    }
                    catch (Exception exception)
                    {
                        IO.Log(2018062909, "Items.Lenght:" + items.Length + " | " + exception.Message + " | " + exception.InnerException + " | " + exception.Source);
                    }
                }

                //Dateinamen in Protokolldatei anhängen. 
                if (keinFehler)
                {
                    using (StreamWriter sw = File.AppendText(Var.ArchiveFile_Scales))
                    {
                        sw.WriteLine(Path.GetFileName(sourceFilePath));
                    }
                }
            }
            catch (FieldAccessException exception)
            {
                IO.Log(2018062910, "FieldAccessException: " + exception.Message + "\r\n" + exception.InnerException);

            }
            catch (IndexOutOfRangeException exception)
            {
                IO.Log(2018062911, exception.Message + " | Datei: '" + sourceFilePath + "' entspricht nicht dem erwarteten Schema.");
            }
            catch (Exception exception)
            {
                IO.Log(2018062912,
                                "Datei: " + sourceFilePath + "\r\n" +
                                exception.Message + "\r\n" +
                                exception.GetType() + "\r\n" +
                                exception.InnerException);
            }
        }


        public delegate void WriteToDb(string sourceFilePath, IProgress<int> progress);

        public static Task LoadDirToDb(WriteToDb writeToDb, IEnumerable<string> filesToLoad, IProgress<int> progress1, IProgress<int> progress2)
        {

            int i = 0;
            int maxi = filesToLoad.Count();

            Task task = Task.Run(() =>
            {
                foreach (string file in filesToLoad)
                {
                    ++i;
                    //if (progress1 != null)
                    {

                        int locProgress = Convert.ToInt32(i * 100 / maxi);
                        progress1.Report(locProgress);
                    }
                    writeToDb(file, progress2);
                }
            });
            return task;
        }


    }
}
