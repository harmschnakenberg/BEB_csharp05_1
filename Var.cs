using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;

namespace BEB_csharp05
{
    public class Var
    {

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //VARIABLENDEKLARATION//

        private static string appDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public static string AppDir
        {
            get
            {
                return appDir;
            }

            set
            {
                appDir = value;
                NotifyStaticPropertyChanged("AppDir");
            }
        }

        public static string IniPath { get; } = Path.Combine(AppDir, "Konfig", "BEB_Konfig.ini");

        private static string dbPath;
        public static string DbPath
        {
            get
            {
                return dbPath;
            }

            set
            {
                dbPath = value;
                NotifyStaticPropertyChanged("DbPath");
            }
        }

        // Log-Ordner ist fest im AppDir, Tägliche Logfiles
        public static string LogPath { get; } = Path.Combine(AppDir, "Log", "log_" + DateTime.Now.Year + "-" + DateTime.Now.Month.ToString("D" + 2) + "-" + DateTime.Now.Day.ToString("D" + 2) + ".txt");

        public static bool GlobalLoggingAllowed { get; set; } = false;
        public static bool CheckforDuplicateInDb { get; set; } = true;

        private static string locDirInput;
        public static string LocDirInput
        {
            get
            {
                return locDirInput;
            }

            set
            {
                if (!Directory.Exists(value))
                {
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(Path.GetFullPath(value)));
                    }
                    catch
                    {
                        IO.Log(2018062110, "ungültig LocDirInput: " + value);
                        value = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop); ;
                    }
                }
                locDirInput = value;
                NotifyStaticPropertyChanged("LocDirInput");
            }
        }

        private static string locDirOperation;
        public static string LocDirOperation
        {
            get
            {
                return locDirOperation;
            }

            set
            {
                if (!Directory.Exists(value) && value.Length > 3)
                {
                    try
                    {
                        Directory.CreateDirectory(Path.GetFullPath(value));
                    }
                    catch
                    {
                        IO.Log(2018062109, "ungültig LocDirOperation: " + value);
                        value = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop); ;
                    }
                }
                locDirOperation = value;
                NotifyStaticPropertyChanged("LocDirOperation");
            }
        }

        private static string locDirScales;
        public static string LocDirScales
        {
            get
            {
                return locDirScales;
            }

            set
            {
                if (!Directory.Exists(value) && value.Length > 3)
                {
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(Path.GetFullPath(value)));
                    }
                    catch
                    {
                        IO.Log(2018062108, "ungültig LocDirScales: " + value);
                        value = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                }
                locDirScales = value;
                NotifyStaticPropertyChanged("LocDirScales");
            }

        }

        private static string locDirExcel;
        public static string LocDirExcel
        {
            get
            {
                return locDirExcel;
            }

            set
            {
                if (!Directory.Exists(value) && value.Length > 3)
                {
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(Path.GetFullPath(value)));
                    }
                    catch
                    {
                        IO.Log(2018062805, "ungültig LocDirExcel: " + value);
                        value = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                }
                locDirExcel = value;
                NotifyStaticPropertyChanged("LocDirExcel");
            }

        }
        
        private static string ftpDirInput;
        public static string FtpDirInput
        {
            get
            {
                return ftpDirInput;
            }

            set
            {
                if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    // Wenn letztes Zeichen ein '/', dann Zeichen entfernen.
                    //IO.Log(2018062801, "value.LastIndexOf(' / ') " + value.LastIndexOf("/") + " value.Length " + value.Length);
                    if (value.LastIndexOf("/") == value.Length - 1) 
                    {
                        ftpDirInput = value.Remove(value.Length-1, 1);
                    }
                    else
                    {
                        ftpDirInput = value;
                    }                   
                }
                NotifyStaticPropertyChanged("FtpDirInput");
            }
        }

        private static string ftpDirOperation;
        public static string FtpDirOperation
        {
            get
            {
                return ftpDirOperation;
            }

            set
            {
                if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    // Wenn letztes Zeichen ein '/', dann Zeichen entfernen.
                    //IO.Log(2018062802, "value.LastIndexOf(' / ') " + value.LastIndexOf("/") + " value.Length " + value.Length);
                    if (value.LastIndexOf("/") == value.Length - 1)
                    {
                        ftpDirOperation = value.Remove(value.Length - 1, 1);
                    }
                    else
                    {
                        ftpDirOperation = value;
                    }
                }
                NotifyStaticPropertyChanged("FtpDirOperation");
            }
        }

        private static string ftpDirScales;
        public static string FtpDirScales
        {
            get
            {
                return ftpDirScales;
            }

            set
            {
                if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    // Wenn letztes Zeichen ein '/', dann Zeichen entfernen.
                    //IO.Log(2018062803, "value.LastIndexOf(' / ') " + value.LastIndexOf("/") + " value.Length " + value.Length);
                    if (value.LastIndexOf("/") == value.Length - 1)
                    {
                        ftpDirScales = value.Remove(value.Length - 1, 1);
                    }
                    else
                    {
                        ftpDirScales = value;
                    }
                }
                NotifyStaticPropertyChanged("FtpDirScales");
            }
        }

        public static string FtpUsername1 { get; set; }
        public static string FtpPassword1 { get; set; }

        private static readonly string modBusPort = "COM1";
        public static string ModBusPort
        {
            get
            {
                return modBusPort;
            }

            set
            {

                ModBusPort = value;
                NotifyStaticPropertyChanged("ModBusPort");
            }
        }

        private static float taskProgress1;
        public static float TaskProgress1
        {
            get { return taskProgress1; }
            set
            {
                taskProgress1 = value;
                NotifyStaticPropertyChanged("TaskProgress1");
            }
        }

        private static float taskProgress2;
        public static float TaskProgress2
        {
            get { return taskProgress2; }
            set
            {
                taskProgress2 = value;
                NotifyStaticPropertyChanged("TaskProgress2");
            }
        }

        private static IEnumerable<string> listView1Content = new List<string>() { "leer", "leer", "leer" };
        public static IEnumerable<string> ListView1Content
        {
            get
            {
                return listView1Content;
            }
            set
            {
                listView1Content = value;
                NotifyStaticPropertyChanged("ListView1Content");
            }
        }

        private static DataTable myDataTable = null;
        public static DataTable MyDataTable
        {
            get
            {
                return myDataTable;
            }

            set
            {
                myDataTable = value;

                
                //NotifyPropertyChanged();
                NotifyStaticPropertyChanged("MyDataTable");
            }
        }

        private static DateTime firstDayofCurrentMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day);
        public static DateTime FirstDayofCurrentMonth
        {
            get
            {
                return firstDayofCurrentMonth;
            }

            set
            {
                firstDayofCurrentMonth = value;
                //NotifyPropertyChanged();
                NotifyStaticPropertyChanged("FirstDayofCurrentMonth");
            }
        }

        private static DateTime startTimeFtpAutoRead = DateTime.Now.AddMinutes(1);
        public static DateTime StartTimeFtpAutoRead
        {
            get
            {
                return startTimeFtpAutoRead;
            }
            set
            {
                if (value.GetType() == typeof(DateTime))
                startTimeFtpAutoRead = value;
                NotifyStaticPropertyChanged("StartTimeFtpAutoRead");
            }
        }

        private static int hourTimePeriodForNewFtpUpdate = 24;
        public static int HourTimePeriodForNewFtpUpdate
        {
            get
            {
                return hourTimePeriodForNewFtpUpdate;
            }
            set
            {
                hourTimePeriodForNewFtpUpdate = value;
                StartTimeFtpAutoRead = DateTime.Now.AddHours(value);
                NotifyStaticPropertyChanged("HourTimePeriodForNewFtpUpdate");
            }
        }
        
        private static string sqlSelectedColumns = "*";
        public static string SqlSelectedColumns
        {
            get
            {
                return sqlSelectedColumns;
            }

            set
            {
                if (value.Length > 3)
                {
                    sqlSelectedColumns = value;
                    NotifyStaticPropertyChanged("SqlSelectedColumns");
                }
            }
        }

        private static string sqlSelectedGroupBy = "";
        public static string SqlSelectedGroupBy
        {
            get
            {
                return sqlSelectedGroupBy;
            }

            set
            {
                if (value.Length > 3)
                {
                    sqlSelectedGroupBy = " GROUP BY " + value;
                }
                else
                {
                    sqlSelectedGroupBy = "";
                }
                NotifyStaticPropertyChanged("SqlSelectedGroupBy");
            }
        }

        private static string sqlSelectedTimeVon = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd HH':'mm':'ss");
        public static string SqlSelectedTimeVon
        {
            get
            {
                return sqlSelectedTimeVon;
            }

            set
            {
                if (value.Length > 15)
                {
                    sqlSelectedTimeVon = value;
                }
                NotifyStaticPropertyChanged("SqlSelectedTimeVon");
            }
        }

        private static string sqlSelectedTimeBis = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1).ToString("yyyy-MM-dd HH':'mm':'ss");
        public static string SqlSelectedTimeBis
        {
            get
            {
                return sqlSelectedTimeBis;
            }

            set
            {
                {
                    sqlSelectedTimeBis = value;
                }
                NotifyStaticPropertyChanged("SqlSelectedTimeBis");
            }
        }

        private static string sqlCommand = "SELECT TOP(1) FROM Input";
        public static string SqlCommand
        {
            get
            {
                return sqlCommand;
            }
            set
            {
                if (value.Length > 18) // mindestens 18 Zeichen, damit SQL-Befehl ausgeführt wird.
                {
                    sqlCommand = value;
                    NotifyStaticPropertyChanged("SqlCommand");

                    Sql sql = new Sql(Var.DbPath);
                    MyDataTable = sql.GetDataTable(value);
                }
            }
        }

        public static void SqlAssembleCommand()
        {
            if (SqlSelectedColumns != null &&
                SqlSelectedTimeVon != null &&
                SqlSelectedTimeBis != null &&
                SqlSelectedGroupBy != null
               )
            {

                string locSelectedColumns = SqlSelectedColumns.Replace("Min(Zeit) AS Zeit", "'Summe' AS Zeit");

                SqlCommand = "SELECT " + locSelectedColumns +
                        " WHERE Zeit BETWEEN '" + SqlSelectedTimeVon +
                        "' AND '" + SqlSelectedTimeBis + "'" +
                        " UNION " +
                        " SELECT " + SqlSelectedColumns +
                        " WHERE Zeit BETWEEN '" + SqlSelectedTimeVon + "' AND '" + SqlSelectedTimeBis + "' " +
                        SqlSelectedGroupBy + 
                        " ORDER BY Zeit";
            }
        }

        internal static string ArchiveFile_Input { get; set; } = Path.Combine(Var.AppDir, "Konfig", "Input.archiv");
        internal static string ArchiveFile_Operation { get; set; } = Path.Combine(Var.AppDir, "Konfig", "Operation.archiv");
        internal static string ArchiveFile_Scales { get; set; } = Path.Combine(Var.AppDir, "Konfig", "Scales.archiv");

        private static int maxFilesCount = 3;
        public static int MaxFilesCount
        {
            get
            {
                return maxFilesCount;
            }
            set
            {
                maxFilesCount = value;
                NotifyStaticPropertyChanged("MaxFilesCount");
            }
        }

        private static string currentLogFileContent = "leer";
        public static string CurrentLogFileContent
        {
            get
            {
                return currentLogFileContent;
            }
            set
            {
                currentLogFileContent = value;
                NotifyStaticPropertyChanged("CurrentLogFileContent");
            }
        }

        private static Visibility controlVisibility = Visibility.Collapsed;
        public static Visibility ControlVisibility
        {
            get
            {
                return controlVisibility;
            }
            set
            {
                controlVisibility = value;
                NotifyStaticPropertyChanged("ControlVisibility");
            }
        }

        internal static int SleepingMilliSec { get; set; } = 100;

        //ENDE VARIABLENDEKLARATION//
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}