using System.Media;              // Pour utiliser SoundPlayer (lecture audio simple).
using System.Text;               // Outils pour manipulation avancée du texte (non utilisé ici).
using System.Windows;            // Composants WPF (Window, Application…).
using System.Windows.Controls;   // Contrôles WPF (Button, TextBlock…).
using System.Windows.Data;       // Binding et data context.
using System.Windows.Documents;  // Documents WPF (FlowDocument, etc.).
using System.Windows.Input;      // Gestion du clavier/souris.
using System.Windows.Media;      // Couleurs, Brushes, effets visuels.
using System.Windows.Media.Imaging;     // Pour charger des images (BitmapImage).
using System.Windows.Navigation; // Navigation dans les pages.
using System.Windows.Shapes;     // Formes (Line, Rectangle…).
using System.Windows.Threading;  // DispatcherTimer pour exécuter du code périodiquement.
using Microsoft.VisualBasic;     
using System;                   
using System.Diagnostics;       

namespace Wail_Vincent_AventCalender
{
    /// <summary>
    /// Fenêtre principale WPF de ton application Calendrier de l’Avent.
    /// Gère : décompte vers Noël, musique de fond, interactions utilisateur.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Crée et prépare un lecteur audio pour la musique en boucle.
        private readonly SoundPlayer bgm = new SoundPlayer(@"Ressources\BGM\jingle bells funk wav");

        /// <summary>
        /// Texte contenant le décompte formaté vers Noël.
        /// Exemple : "24j 12:33:54".
        /// </summary>
        public string NoelTimer { get; private set; } = string.Empty;

        // Timer qui s'exécute toutes les secondes pour mettre à jour le décompte.
        private readonly DispatcherTimer countdownTimer = new DispatcherTimer();

        // Variable permettant de contrôler la boucle demandant le genre (M/F).
        private bool continuer = true;

        /// <summary>
        /// Constructeur de la fenêtre.
        /// Configure la minuterie, demande prénom et genre, lance la musique.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();  // Charge le XAML et initialise les composants.

            // Configuration de la minuterie (1 seconde).
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;  // Appelle la méthode chaque seconde.

            // Calcul initial du décompte.
            MettreAJourNoelTimer();

            // Affiche le texte du décompte dans l'interface.
            CountDownTextBlock.Text = NoelTimer;

            // Démarre le timer.
            countdownTimer.Start();

            // Première boîte de dialogue : demande le prénom.
            var prenom = Interaction.InputBox(
                "Entrez votre prénom :",
                "Bienvenue",
                ""
            ).Trim();

            // Demande du genre : boucle tant que la saisie n’est pas correcte.
            while (continuer)
            {
                var genre = Interaction.InputBox(
                    "Entrez votre genre (M pour Masculin ou F pour Féminin) :",
                    "Genre",
                    ""
                ).Trim().ToUpperInvariant(); // Met en majuscule pour simplifier

                // Vérifie si saisie valide
                if (genre == "M" || genre == "F")
                {
                    continuer = false;  // Sortir de la boucle
                }
                else
                {
                    MessageBox.Show("Veuillez entrer M ou F");  // Message d’erreur
                }
            }

            // Salue l’utilisateur selon qu’il a fourni un prénom ou pas.
            if (string.IsNullOrWhiteSpace(prenom))
            {
                MessageBox.Show("Bonjour !");
            }
            else
            {
                MessageBox.Show($"Bonjour {prenom} !");
            }

            // Lance la musique en boucle.
            bgm.PlayLooping();

            // Imprime dans la console debug le décompte initial.
            Debug.WriteLine($"Décompte initial Noël: {NoelTimer}");
        }

        /// <summary>
        /// Appelé à chaque tick du timer (toutes les secondes).
        /// Met à jour le décompte et rafraîchit l'affichage.
        /// </summary>
        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            MettreAJourNoelTimer();            // Recalcul du temps restant
            CountDownTextBlock.Text = NoelTimer; // Mise à jour dans l'interface
        }

        /// <summary>
        /// Calcule combien de temps il reste avant le 25 décembre (prochain Noël).
        /// Met à jour les TextBlocks de l’UI.
        /// </summary>
        public void MettreAJourNoelTimer()
        {
            var maintenant = DateTime.Now;    // Date et heure actuelles

            // Détermine l'année du prochain Noël.
            int annee = (maintenant.Month == 12 && maintenant.Day > 25)
                ? maintenant.Year + 1 : maintenant.Year;

            var prochainNoel = new DateTime(annee, 12, 25, 0, 0, 0);

            // Si on est déjà le 25, bascule automatiquement vers Noël de l'année suivante.
            if (maintenant >= prochainNoel)
            {
                prochainNoel = new DateTime(annee + 1, 12, 25, 0, 0, 0);
            }

            // Calcule le TimeSpan restant.
            var reste = prochainNoel - maintenant;

            // Formate le texte du décompte.
            NoelTimer = $"{reste.Days}j {reste.Hours:D2}:{reste.Minutes:D2}:{reste.Seconds:D2}";

            // Texte affiché dans l'interface.
            Days_until_Christmas.Text = $"{reste.Days} jour(s) restant(s) avant Noël!";
        }

        /// <summary>
        /// Appelé automatiquement lorsque la fenêtre est fermée.
        /// Arrête proprement la musique et la minuterie.
        /// </summary>
        protected override void OnClosed(System.EventArgs e)
        {
            countdownTimer.Stop(); // Arrête le timer
            bgm.Stop();            // Arrête la musique
            bgm.Dispose();         // Libère les ressources audio
            base.OnClosed(e);      // Appelle la version parent
        }

        // Gestion du bouton "Reveal Daily Card".
        private void Button_TodayCard_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
