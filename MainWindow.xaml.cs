using System.Media;
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
using Microsoft.VisualBasic;

namespace Wail_Vincent_AventCalender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SoundPlayer bgm = new SoundPlayer(@"BGM\bgm1.wav");
        public MainWindow()
        {
            InitializeComponent();

            // Demande le prénom à l'utilisateur
            var prenom = Interaction.InputBox(
                "Entrez votre prénom :",
                "Bienvenue",
                ""
            ).Trim();

            var genre = Interaction.InputBox(
                "Entrez votre genre :",
                ""
            ).Trim();

            if (string.IsNullOrWhiteSpace(prenom))
            {
                MessageBox.Show("Bonjour !");
            }
            else
            {
                MessageBox.Show($"Bonjour {prenom} !");
            }
            // Lecture en boucle
            bgm.PlayLooping();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            // Arrête et libère la ressource audio à la fermeture
            bgm.Stop();
            bgm.Dispose();
            base.OnClosed(e);
        }
    }
}