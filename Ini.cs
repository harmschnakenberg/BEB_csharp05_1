using BEB_csharp05;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace BEB_csharp05
{
    // Source: http://dotnet-snippets.de/dns/ini---dateien-lesen-und-schreiben-SID60.aspx
    // Changed by JB: http://jbquerier.blogspot.com
    /// <summary>
    /// Klasse zum Schreiben von Ini-Dateien.
    /// </summary>
    public class NativeMethods
    {
        // JB: accessor changed.
        /// <summary>
        /// Pfad der Ini-Datei.
        /// </summary>
        private readonly string path = Var.IniPath;

        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(
              string section, 
              string key, 
              string val, 
              string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
             string section,
             string key,
             string def,
             StringBuilder retVal, 
             int size,
             string filePath);

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="INIPath">Pfad zur Ini-Datei</param>
        public NativeMethods(string INIPath)
        {
            path = INIPath;
        }

        // Part by JB
        /// <summary>
        /// Sektion der Ini-Datei
        /// </summary>
        /// <param name="Section">Name der Sektion</param>
        /// <returns>Sektion</returns>
        public Section this[string Section]
        {
            get { return new Section(Section, this); }
        }

        // Part by JB
        /// <summary>
        /// Sektion-Struktur
        /// </summary>
        public class Section
        {
            private readonly string Name;
            private NativeMethods File;

            /// <summary>
            /// Konstruktor für die Sektion
            /// </summary>
            /// <param name="name">Name der Sektion</param>
            /// <param name="file">Verweis auf die Ini-Datei</param>
            public Section(string name, NativeMethods file)
            {
                Name = name;
                File = file;
            }

            /// <summary>
            /// Schreibt oder ermittelt einen Ini-Wert.
            /// </summary>
            /// <param name="Key">Bezeichner des Wertes</param>
            /// <returns>String des Wertes</returns>
            public string this[string Key]
            {
                get { return File.IniReadValue(Name, Key); }
                set { if (value != this[Key]) File.IniWriteValue(Name, Key, value); }
            }
        }

        // JB: accessor changed.
        /// <summary>
        /// Schreibt einen Ini-Wert
        /// </summary>
        /// <param name="Section">Name der Sektion</param>
        /// <param name="Key">Bezeichner des Wertes</param>
        /// <param name="Value">Wert</param>
        protected void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        // JB: accessor changed.
        /// <summary>
        /// Ermittelt einen Ini-Wert
        /// </summary>
        /// <param name="Section">Name der Sektion</param>
        /// <param name="Key">Bezeichner des Wertes</param>
        /// <returns>Wert</returns>
        protected string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
    }
}

// Usage-Sample:
public partial class MainWindow : Window
{
    private NativeMethods Settings;

    public MainWindow()
    {
        //InitializeComponent();
        Settings = new NativeMethods(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Demo.ini");
    }

    private void IniWrite_Click(object sender, RoutedEventArgs e)
    {
        Settings["DemoSection"]["DemoCaption"] = "DemoValue";
    }

    private void IniRead_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(Settings["DemoSection"]["DemoCaption"]);
    }

}