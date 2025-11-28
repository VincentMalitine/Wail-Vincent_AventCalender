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
using System.Windows.Threading;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;

namespace Wail_Vincent_AventCalender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Debut de déclaration du player audio
        SoundPlayer bgm = new SoundPlayer(@"BGM\bgm1.wav");
        // Fin de déclaration du player audio

        // Debut de propriété du décompte de Noël
        public string NoelTimer { get; private set; } = string.Empty;
        // Fin de propriété du décompte de Noël

        // Debut de timer de mise à jour
        private readonly DispatcherTimer countdownTimer = new DispatcherTimer();
        // Fin de timer de mise à jour

        // Debut de flag boucle saisie genre
        bool continuer = true;
        // Fin de flag boucle saisie genre

        // Debut de constructeur
        public MainWindow()
        {
            InitializeComponent();

            // Debut de initialisation du timer
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
            MettreAJourNoelTimer();
            CountDownTextBox.Text = NoelTimer; // première valeur affichée
            countdownTimer.Start();
            // Fin de initialisation du timer

            // Debut de saisie prénom
            var prenom = Interaction.InputBox(
                "Entrez votre prénom :",
                "Bienvenue",
                ""
            ).Trim();
            // Fin de saisie prénom

            // Debut de boucle saisie genre
            while (continuer)
            {
                var genre = Interaction.InputBox(
                    "Entrez votre genre (M pour Masculin ou F pour Féminin) :",
                    "Genre",
                    ""
                ).Trim().ToUpperInvariant();

                if (genre == "M" || genre == "F")
                {
                    continuer = false;
                }
                else
                {
                    MessageBox.Show("Veuillez entrer M ou F");
                }
            }
            // Fin de boucle saisie genre

            // Debut de affichage salutation
            if (string.IsNullOrWhiteSpace(prenom))
            {
                MessageBox.Show("Bonjour !");
            }
            else
            {
                MessageBox.Show($"Bonjour {prenom} !");
            }
            // Fin de affichage salutation

            // Debut de lancement musique
            bgm.PlayLooping();
            // Fin de lancement musique

            // Debut de trace debug
            Debug.WriteLine($"Décompte initial Noël: {NoelTimer}");
            // Fin de trace debug
        }
        // Fin de constructeur

        // Debut de gestion tick timer
        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            MettreAJourNoelTimer();
            CountDownTextBox.Text = NoelTimer; // mise à jour affichage
        }
        // Fin de gestion tick timer

        // Debut de mise à jour du string NoelTimer
        private void MettreAJourNoelTimer()
        {
            var maintenant = DateTime.Now;
            int annee = (maintenant.Month == 12 && maintenant.Day > 25) ? maintenant.Year + 1 : maintenant.Year;
            var prochainNoel = new DateTime(annee, 12, 25, 0, 0, 0);

            if (maintenant >= prochainNoel)
            {
                prochainNoel = new DateTime(annee + 1, 12, 25, 0, 0, 0);
            }

            var reste = prochainNoel - maintenant;
            NoelTimer = $"{reste.Days}j {reste.Hours:D2}:{reste.Minutes:D2}:{reste.Seconds:D2}";
        }
        // Fin de mise à jour du string NoelTimer

        // Debut de nettoyage fermeture fenêtre
        protected override void OnClosed(System.EventArgs e)
        {
            countdownTimer.Stop();
            bgm.Stop();
            bgm.Dispose();
            base.OnClosed(e);
        }
        // Fin de nettoyage fermeture fenêtre
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