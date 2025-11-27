using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wail_Vincent_AventCalender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnEnterName_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnChooseGender_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox themeSelector && themeSelector.SelectedItem is ComboBoxItem selected)
            {
                string theme = selected.Content.ToString();

                // Logique pour changer le thème en fonction de la sélection
                MessageBox.Show($"Thème sélectionné : {theme}");
            }
        }

        private void BackgroundColorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BackgroundColorSelector.SelectedItem is ComboBoxItem selected)
            {
                string hex = selected.Tag.ToString();

                try
                {
                    var brush = (SolidColorBrush)new BrushConverter().ConvertFromString(hex);

                    // Change le fond de la fenêtre
                    this.Background = brush;
                }
                catch
                {
                    MessageBox.Show("Erreur : couleur invalide.");
                }
            }
        }

    }
}