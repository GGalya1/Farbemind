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
            Knopf_Starten.IsEnabled = false;
            Knopf_Raten.IsEnabled = true;
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

        private void EllipseSchwarzFarben(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;
            ContextMenu cm = mi.Parent as ContextMenu;
            Ellipse el = cm.PlacementTarget as Ellipse;
            el.Fill = Brushes.Black; //Ellipse hat den ContextMenu, Contextmenu hat MenuItem
        }
    }
}
