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
    /// Logique d'interaction pour la fenêtre principale de l'application Avent (WPF).
    /// Gère le décompte vers Noël, la musique de fond et quelques interactions utilisateur.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Lecteur audio pour la musique de fond en boucle.
        private readonly SoundPlayer bgm = new SoundPlayer(@"Ressources\BGM\bgm.wav");

        /// <summary>
        /// Chaîne représentant le décompte vers Noël au format "Dj HH:mm:ss".
        /// </summary>
        public string NoelTimer { get; private set; } = string.Empty;

        // Minuterie (1s) pour mettre à jour l'affichage du décompte.
        private readonly DispatcherTimer countdownTimer = new DispatcherTimer();

        // Flag de contrôle pour la boucle de saisie du genre.
        private bool continuer = true;

        /// <summary>
        /// Initialise la fenêtre, configure la minuterie, collecte les informations utilisateur,
        /// lance la musique et effectue une première mise à jour de l'UI.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Configuration et démarrage de la minuterie.
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
            MettreAJourNoelTimer();
            CountDownTextBlock.Text = NoelTimer;
            countdownTimer.Start();

            // Saisie du prénom (optionnelle).
            var prenom = Interaction.InputBox(
                "Entrez votre prénom :",
                "Bienvenue",
                ""
            ).Trim();

            // Saisie du genre avec validation (M/F).
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

            // Salutation en fonction du prénom saisi.
            if (string.IsNullOrWhiteSpace(prenom))
            {
                MessageBox.Show("Bonjour !");
            }
            else
            {
                MessageBox.Show($"Bonjour {prenom} !");
            }

            // Lancement de la musique en boucle.
            bgm.PlayLooping();

            // Trace de diagnostic (décompte initial).
            Debug.WriteLine($"Décompte initial Noël: {NoelTimer}");
        }

        /// <summary>
        /// Gestion du tick de la minuterie: recalcul du décompte et rafraîchissement de l'UI.
        /// </summary>
        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            MettreAJourNoelTimer();
            CountDownTextBlock.Text = NoelTimer;
        }

        /// <summary>
        /// Calcule le temps restant jusqu'au prochain 25 décembre et met à jour les éléments d'UI.
        /// Gère le cas où l'on est déjà le jour de Noël ou après.
        /// </summary>
        public void MettreAJourNoelTimer()
        {
            var maintenant = DateTime.Now;

            // Détermination de l'année cible pour Noël.
            int annee = (maintenant.Month == 12 && maintenant.Day > 25) ? maintenant.Year + 1 : maintenant.Year;
            var prochainNoel = new DateTime(annee, 12, 25, 0, 0, 0);

            // Si la date actuelle a atteint le 25/12, viser l'année suivante.
            if (maintenant >= prochainNoel)
            {
                prochainNoel = new DateTime(annee + 1, 12, 25, 0, 0, 0);
            }

            // Construction de la chaîne de décompte et mise à jour de l'UI.
            var reste = prochainNoel - maintenant;
            NoelTimer = $"{reste.Days}j {reste.Hours:D2}:{reste.Minutes:D2}:{reste.Seconds:D2}";
            Days_until_Christmas.Text = $"{reste.Days} jour(s) restant(s) avant Noël!";
        }

        /// <summary>
        /// Arrête proprement les ressources (minuterie, audio) à la fermeture de la fenêtre.
        /// </summary>
        protected override void OnClosed(System.EventArgs e)
        {
            countdownTimer.Stop();
            bgm.Stop();
            bgm.Dispose();
            base.OnClosed(e);
        }

        // Gestionnaire de clic: saisie du nom (réservé pour future extension UI).
        private void BtnEnterName_Click(object sender, RoutedEventArgs e)
        {
        }

        // Gestionnaire de clic: choix du genre (réservé pour future extension UI).
        private void BtnChooseGender_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Change le thème en fonction de la sélection du ComboBox des thèmes.
        /// </summary>
        private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox themeSelector && themeSelector.SelectedItem is ComboBoxItem selected)
            {
                string? theme = selected.Content?.ToString();

                // Exemple simple: feedback à l'utilisateur.
                MessageBox.Show($"Thème sélectionné : {theme ?? "Aucun"}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}