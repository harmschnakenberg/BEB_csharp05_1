using System;
using System.IO;
using System.Windows;

namespace BEB_csharp05
{
    public partial class MainWindow : Window
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //STARTWERTE SETZEN//

        /// <summary>
        /// Prüfe, ob die KonfigIni vorhanden ist. Fallsnicht erzeugen.
        /// Locale Pfade 
        /// </summary>
        public static void InitializeApp()
        {

            // Prüfen ob INi bereit ist, ggf erstellen.
            IO.CheckAndCreateIniFile(Var.IniPath);

            //Ini-Datei einlesen.
            ReadFromIni(Var.IniPath);

            //Ordner-Pfade ggf. erstellen.
            try
            {
                IO.CreateDirWithWriteAccess(Path.Combine(Var.AppDir, "Log"));
                IO.CreateDirWithWriteAccess(Var.LocDirInput);
                IO.CreateDirWithWriteAccess(Var.LocDirOperation);
                IO.CreateDirWithWriteAccess(Var.LocDirScales);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "InitializeApp() " + ex.Source);
            }

            //Datenbanktabellen ggf. erstellen.
            Sql sql = new Sql(Var.DbPath);
            sql.SqlDeclareTables();
        }
 
        public static void ReadFromIni(string iniPath)
        {
            if (!File.Exists(iniPath))
            {
                MessageBox.Show("Die INI-Datei '" + iniPath + "' existiert nicht.", "ReadFromIni()");
            }

            try
            {
                NativeMethods Ini;
                Ini = new NativeMethods(Var.IniPath);
                bool _GlobalLoggingAllowed;

                bool _TryParseSucessfull;
                _TryParseSucessfull = bool.TryParse(Ini["Allgemein"]["GlobalLoggingAllowed"], out _GlobalLoggingAllowed);
                if (_TryParseSucessfull)
                {
                    Var.GlobalLoggingAllowed = _GlobalLoggingAllowed;
                }
                else
                {
                    MessageBox.Show("'GlobalLoggingAllowed' war '" + Ini["Allgemein"]["GlobalLoggingAllowed"] + "' und konnte in INI nicht als BOOL erkannt werden und wird 'true' gesetzt. ", "ReadFromIni()");
                    Var.GlobalLoggingAllowed = true;
                }

                Var.SleepingMilliSec = Int32.Parse(Ini["Allgemein"]["SleepingMilliSec"].ToString());

                Var.DbPath = Ini["DB"]["DbPath"].ToString();
        

                Var.LocDirInput = Ini["DIR"]["LocDirInput"].ToString();
                Var.LocDirOperation = Ini["DIR"]["LocDirOperation"].ToString();
                Var.LocDirScales = Ini["DIR"]["LocDirScales"].ToString();
                Var.LocDirExcel= Ini["DIR"]["LocDirExcel"].ToString();

                Var.FtpDirInput = Ini["FTP"]["FtpDir_Input"].ToString();
                Var.FtpDirOperation = Ini["FTP"]["FtpDir_Operation"].ToString();
                Var.FtpDirScales = Ini["FTP"]["FtpDir_Scales"].ToString();

                Var.FtpUsername1 = Ini["FTP"]["Username1"].ToString();
                Var.FtpPassword1 = Ini["FTP"]["Password1"].ToString();

                IO.Log(2018062004, "INI-Datei wurde geladen.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Die INI-Datei wurde nicht vollständig geladen. Nähere Informationen in der aktuellen LOG-Datei bei Fehlernummer 2018062005", "Fehler INI-Werte lesen.", MessageBoxButton.OK, MessageBoxImage.Warning);
                IO.Log(2018062005, "INI wurde nicht geladen. " + ex.Message + " | " + ex.InnerException + " | " + ex.Source + " | " + ex.GetBaseException().ToString());
            }
        }

        /// <summary>
        /// Prüft die Existenz der aus der INI gelesenen lokalen Pfade und gibt in der GUI grün aus wenn vorhanden, ansonsten rot.
        /// </summary>
        private void ValidateLocalPaths()
        {
            status_IniPath.Fill = IO.CheckPath_GiveColor(Var.IniPath);
            status_DbPath.Fill = IO.CheckPath_GiveColor(Var.DbPath);
            status_LocDirInput.Fill = IO.CheckPath_GiveColor(Var.LocDirInput);
            status_LocDirOperation.Fill = IO.CheckPath_GiveColor(Var.LocDirOperation);
            status_LocDirScales.Fill = IO.CheckPath_GiveColor(Var.LocDirScales);
            status_LocDirExcel.Fill = IO.CheckPath_GiveColor(Var.LocDirExcel);
        }

        //ENDE STARTWERTE SETZEN//
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




    }
}
