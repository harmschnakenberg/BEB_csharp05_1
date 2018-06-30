using System;
using System.Windows;
//using System.Windows.Shapes;
using System.Windows.Threading;

namespace BEB_csharp05
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Var var = new Var();

        public MainWindow()
        {
            // Inhalt der Klasse Var für XAML berietstellen
            DataContext = var;
            // Programm initialisieren
            InitializeComponent();
            // Variablen, Pfade usw. initialisieren
            InitializeApp();
            IO.Log(2018062209, "Anwendung wurde gestartet.");
            //Prüfe die ausgelesenen lokalen Pfade und zeige grün wenn sie existieren, sonst rot            
            ValidateLocalPaths();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (TimerTick);
            timer.Start();

            datePickerBis.SelectedDate = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1);
            //Fenstergröße
            Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight * 0.9;
            Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth * 0.5;
        }

       

        
 
    }

}
