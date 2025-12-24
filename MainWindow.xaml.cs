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
using Wail_Vincent_AventCalender.Ressources.Views;

namespace Wail_Vincent_AventCalender
{
    public partial class MainWindow : Window
    {
        // Musique de fond.
        readonly SoundPlayer bgm = new SoundPlayer(@"Ressources\BGM\bgm.wav");
        // Texte du décompte vers Noël au format "Dj HH:mm:ss".
        string NoelTimer { get; set; } = string.Empty;
        // Minuterie pour mettre à jour le décompte chaque seconde.
        readonly DispatcherTimer countdownTimer = new DispatcherTimer();
        // Contrôle la boucle de saisie du genre (M/F).
        bool continuer = true;

        // Nombre de jours restants avant Noël.
        int DaysLeft { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Configuration et démarrage de la minuterie.
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
            MettreAJourNoelTimer();
            CountDownTextBlock.Text = NoelTimer;
            countdownTimer.Start();

            // Saisie du prénom (facultative).
            var prenom = Interaction.InputBox(
                "Entrez votre prénom :",
                "Bienvenue",
                ""
            ).Trim();

            // Saisie du genre avec validation stricte sur M/F.
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

            // Message de bienvenue personnalisé.
            if (string.IsNullOrWhiteSpace(prenom))
            {
                MessageBox.Show("Bonjour !");
            }
            else
            {
                MessageBox.Show($"Bonjour {prenom} !");
            }

            // Démarrage de la musique de fond en boucle.
            bgm.PlayLooping();

            Debug.WriteLine($"Décompte initial Noël: {NoelTimer}");
        }

        // Appelé toutes les secondes pour rafraîchir le décompte affiché.
        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            MettreAJourNoelTimer();
            CountDownTextBlock.Text = NoelTimer;
        }

        // Calcule le temps restant jusqu'au prochain 25 décembre (heure locale Windows)
        // et met à jour le texte du décompte.
        public void MettreAJourNoelTimer()
        {
            DateTime maintenant = DateTime.Now;

            int anneeNoel = maintenant.Year;
            DateTime prochainNoel = new DateTime(anneeNoel, 12, 25, 0, 0, 0);

            // Si Noël de cette année est déjà passé, viser l'année suivante.
            if (maintenant >= prochainNoel)
            {
                anneeNoel++;
                prochainNoel = new DateTime(anneeNoel, 12, 25, 0, 0, 0);
            }

            TimeSpan reste = prochainNoel - maintenant;

            NoelTimer = $"{reste.Days}j {reste.Hours:D2}:{reste.Minutes:D2}:{reste.Seconds:D2}";
            Days_until_Christmas.Text = $"{reste.Days} jour(s) restant(s) avant Noël!";
            DaysLeft = reste.Days + 1;
        }

        // Libère la minuterie et la musique à la fermeture de la fenêtre.
        protected override void OnClosed(EventArgs e)
        {
            countdownTimer.Stop();
            bgm.Stop();
            bgm.Dispose();
            base.OnClosed(e);
        }

        // Affiche la carte correspondant au nombre de jours restants.
        private void Button_TodayCard_Click(object sender, RoutedEventArgs e)
        {
            // Exemple : reste 5 jours -> Card5 (Page).
            var typeName = $"Wail_Vincent_AventCalender.Ressources.Views.Card{DaysLeft}";
            var type = Type.GetType(typeName);

            if (type == null)
            {
                MessageBox.Show($"Vous n'êtes pas/plus en décembre.");
                return;
            }

            if (Activator.CreateInstance(type) is Page page)
            {
                // MainContent doit être un Frame dans MainWindow.xaml.
                MainContent.Content = page;
            }
            else
            {
                MessageBox.Show("Impossible de charger la page de carte correspondante.");
            }
        }
    }
}