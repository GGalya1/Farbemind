using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Farbemind
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Ellipse> ellipses = new List<Ellipse>();
        List<TextBox> textBoxes = new List<TextBox>();
        int[] code = new int[4];// 0=Schwarz 1=Blau 2=Rot 3=Grün
        Random rnd = new Random();
        int runde = 0;

        public MainWindow()
        {
            InitializeComponent();

            //Spalten im Spielfeld erzeugen
            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(1, GridUnitType.Star); //anders formuliert: mach "<ColumnDefinition Width="1*"/>"
                Spielfeld.ColumnDefinitions.Add(column);//füge diese Spalte
            }

            Spielregeln.Text = "Spielregeln:\n"+
                                "Ein Code aus vier Farben soll erraten werden.\n"+
                                "1. Farben auswaehlen\n"+
                                "2. Raten Button betaetigen\n"+
                                "3. Auswertung:\n"+
                                "\tx - richtige Farbe und richtiger Ort,\n"+
                                "\to - richtige Farbe.\n"+
                                "Die Reiehnfolge der Auwertung spielt keine Rolle.\n";
        }

        /// <summary>
        /// Spiel startet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Knopf_Starten_Click(object sender, RoutedEventArgs e)
        {
            NeueZeile();
            foreach (int i in code)
            {
                code[i] = rnd.Next(0, 4);
            }
            //MessageBoxResult result = MessageBox.Show("{0} - {1} - {2} - {3}", code[0], code[1], code[2], code[3]);
            Knopf_Starten.IsEnabled = false;
            Knopf_Raten.IsEnabled = true;
        }
        private void Knopf_Raten_Click(object sender, RoutedEventArgs e)
        {
            int[] raten = new int[4];
            for (int i = 0; i < raten.Length; i++)
            {
                raten[i] = FarbeZuZahl(ellipses[i].Fill);
            }
            if (raten.Contains(-1))
            {
                MessageBox.Show("Bitte Farben für alle Kreise wählen.", "Fehler!");
            }
            else
                Raten(raten); //wenn es kein Fehler gibt/ alle Farben gewahlt wurden, dann gehen wir zur unseren Methode und arbeiten weiter
        }

        private void Raten(int[] raten)
        {
            string[] ausgabe = new string[4];
            for (int i = 0; i<raten.Length; i++)
            {
                if (raten[i] == code[i])
                {
                    ausgabe[i] = "X";
                }
                else if (code.Contains(raten[i]))
                {
                    ausgabe[i] = "O";
                }
                else ausgabe[i] = " ";
            }
            Mischen(ausgabe); //hier gebe ich eine Referenz zu meinem Array, also er wird live verändert
        }

        public void Mischen(string[] stringArray) //hier wird Array verarbeitet und live verändert
        {
            int n = stringArray.Length;
            while (n > 1)
            {
                int k = rnd.Next(n--); //von letztem Element bis 1-em mit zufäligem Element tauschen + n-- -> n = n - 1
                string temp = stringArray[n];
                stringArray[n] = stringArray[k];
                stringArray[k] = temp;

            }
        }

        private int FarbeZuZahl(Brush color)
        {
            int zahl = -1;
            if (color == Brushes.Black)
            {
                zahl = 0;
            }
            else if (color == Brushes.Blue)
            {
                zahl = 1;
            }
            else if (color == Brushes.Red)
            {
                zahl = 2;
            }
            else if(color == Brushes.Green)
                zahl = 3;

            return zahl;
        }


        /// <summary>
        /// Dem Spielfeld wird eine neue Zeile hinzufügt
        /// </summary>
        private void NeueZeile()
        {
            //Neue Zeile im Grid einfügen
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(100, GridUnitType.Pixel);
            Spielfeld.RowDefinitions.Add(rowDefinition);

            //Ellipsen
            ellipses.Clear(); //Damit schon benutze Ellipse nicht weiter verwenden
            for (int i = 0; i < 5; i++)
            {
                //Ellipse definieren
                Ellipse ellipse = new Ellipse
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Gray
                };

                //Rechtklickmeü für Farbwahl
                ContextMenu contextMenu = new ContextMenu();

                MenuItem menuItemBlack = new MenuItem();
                menuItemBlack.Header = "Schwarz";
                menuItemBlack.Click += new RoutedEventHandler(EllipseSchwarzFarben); //mache das und sende information, wer das gemacht hat
                contextMenu.Items.Add(menuItemBlack);

                MenuItem menuItemRed = new MenuItem();
                menuItemRed.Header = "Rot";
                menuItemRed.Click += new RoutedEventHandler(EllipseRotFarben); //mache das und sende information, wer das gemacht hat
                contextMenu.Items.Add(menuItemRed);

                MenuItem menuItemGreen = new MenuItem();
                menuItemGreen.Header = "Grün";
                menuItemGreen.Click += new RoutedEventHandler(EllipseGruenFarben); //mache das und sende information, wer das gemacht hat
                contextMenu.Items.Add(menuItemGreen);

                MenuItem menuItemBlue = new MenuItem();
                menuItemBlue.Header = "Blau";
                menuItemBlue.Click += new RoutedEventHandler(EllipseBlauFarben); //mache das und sende information, wer das gemacht hat
                contextMenu.Items.Add(menuItemBlue);

                ellipse.ContextMenu = contextMenu;


                //Ellipse dem Grid hinzufügen
                Spielfeld.Children.Add(ellipse);
                Grid.SetRow(ellipse, runde);
                Grid.SetColumn(ellipse, i);
                ellipses.Add(ellipse); //in unserer Liste hinzufügen
            }

            //Textbox fuer Zeile/Erraten/Hinweis
            TextBox textBox = new TextBox
            {
                Background = Brushes.LightGray,
                IsReadOnly = true,
                FontSize = 40,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center //Beschreiben von TextBox
            };
            Spielfeld.Children.Add(textBox); //ein neues Textbox hinzufügen
            Grid.SetRow(textBox, runde);
            Grid.SetColumn(textBox, 4);
            textBoxes.Add(textBox);

            runde++;
        }


        /// <summary>
        /// Ellipse el = ((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget as Ellipse; - macht die Gleiche wie "MenuItem mi = e.Source as MenuItem; + ContextMenu cm = mi.Parent as ContextMenu;"
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseSchwarzFarben(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            ContextMenu cm = mi.Parent as ContextMenu;
            Ellipse el = cm.PlacementTarget as Ellipse; 
            el.Fill = Brushes.Black; //Ellipse hat den ContextMenu, Contextmenu hat MenuItem
        }
        private void EllipseRotFarben(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem; //sender soll als MenuItem betrachtet werden
            Ellipse el = ((ContextMenu)mi.Parent).PlacementTarget as Ellipse;
            el.Fill = Brushes.Red; //Ellipse hat den ContextMenu, Contextmenu hat MenuItem
        }
        private void EllipseGruenFarben(object sender, RoutedEventArgs e)
        {
            Ellipse el = ((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget as Ellipse;
            el.Fill = Brushes.Green; //Ellipse hat den ContextMenu, Contextmenu hat MenuItem
        }
        private void EllipseBlauFarben(object sender, RoutedEventArgs e)
        {
            ((Ellipse)((ContextMenu)((MenuItem)e.Source).Parent).PlacementTarget).Fill = Brushes.Blue; // macht die Gleiche wie //MenuItem mi = e.Source as MenuItem; + ContextMenu cm = mi.Parent as ContextMenu;
            
        }
    }
}
