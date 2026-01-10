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
    // Cette classe représente la fenêtre principale de l'application.
    // "partial" signifie qu'une autre partie de cette classe existe dans un autre fichier (le fichier XAML).
    // "Window" signifie que cette classe hérite de la classe Window fournie par WPF.
    public partial class MainWindow : Window
    {
        // ========== SECTION DES VARIABLES (appelées "champs" en C#) ==========
        
        // SoundPlayer est un objet qui permet de jouer des fichiers audio WAV.
        // "readonly" signifie qu'on ne pourra jamais changer cette variable après l'avoir créée.
        // "@" avant la chaîne permet d'écrire le chemin sans avoir à doubler les backslashes (ex: \\ devient juste \).
        readonly SoundPlayer bgm1 = new SoundPlayer(@"Ressources\BGM\bgm1.wav");
        readonly SoundPlayer bgm2 = new SoundPlayer(@"Ressources\BGM\bgm2.wav");
        
        // Cette variable stocke le texte du compte à rebours vers Noël.
        // "string" = chaîne de caractères (du texte).
        // "get; set;" = on peut lire et modifier cette variable depuis n'importe où dans la classe.
        // "string.Empty" = chaîne vide (équivalent à "").
        string NoelTimer { get; set; } = string.Empty;
        
        // DispatcherTimer est un chronomètre spécial pour les applications WPF.
        // Il permet d'exécuter du code à intervalles réguliers (ici, chaque seconde).
        readonly DispatcherTimer countdownTimer = new DispatcherTimer();
        
        // Ces variables sont des booléens (vrai/faux, true/false).
        // "continuer" contrôle la boucle qui demande le genre de l'utilisateur.
        bool continuer = true;
        // "bgmchoice" détermine quelle musique de fond jouer (true = bgm1, false = bgm2).
        bool bgmchoice = true;

        // Cette variable stocke le nombre de jours restants avant Noël.
        // "int" = nombre entier (1, 2, 3, etc.).
        int DaysLeft { get; set; }

        // ========== CONSTRUCTEUR (code qui s'exécute quand on crée la fenêtre) ==========
        public MainWindow()
        {
            // Cette ligne initialise tous les éléments visuels définis dans MainWindow.xaml.
            // Sans cette ligne, rien ne s'affiche !
            InitializeComponent();

            // ===== Configuration du chronomètre qui met à jour le compte à rebours =====
            
            // On dit au chronomètre de se déclencher toutes les 1 seconde.
            // "TimeSpan.FromSeconds(1)" crée un intervalle de temps de 1 seconde.
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            
            // "+=" permet d'attacher une fonction à un événement.
            // Ici, on dit : "Quand le chronomètre sonne, exécute la fonction CountdownTimer_Tick".
            countdownTimer.Tick += CountdownTimer_Tick;
            
            // On calcule le temps restant jusqu'à Noël pour la première fois.
            MettreAJourNoelTimer();
            
            // On affiche ce temps dans l'élément visuel "CountDownTextBlock" (défini dans le XAML).
            CountDownTextBlock.Text = NoelTimer;
            
            // On démarre le chronomètre (maintenant il va sonner chaque seconde).
            countdownTimer.Start();

            // ===== Demande du prénom à l'utilisateur =====
            
            // "Interaction.InputBox" affiche une petite fenêtre où l'utilisateur peut taper du texte.
            // Les 3 paramètres sont : le message, le titre de la fenêtre, et la valeur par défaut.
            // ".Trim()" enlève les espaces au début et à la fin de ce que l'utilisateur a tapé.
            var prenom = Interaction.InputBox(
                "Entrez votre prénom :",
                "Bienvenue",
                ""
            ).Trim();

            // ===== Demande du genre à l'utilisateur avec vérification =====
            
            // "while" = tant que (ici, tant que "continuer" est vrai, on répète le code).
            while (continuer)
            {
                // On demande à l'utilisateur de taper M ou F.
                // ".ToUpperInvariant()" transforme tout en majuscules (m devient M, f devient F).
                var genre = Interaction.InputBox(
                    "Entrez votre genre (M pour Masculin ou F pour Féminin) :",
                    "Genre",
                    ""
                ).Trim().ToUpperInvariant();

                // "if" = si. On vérifie si l'utilisateur a bien tapé M ou F.
                // "||" signifie "OU" (l'un ou l'autre).
                if (genre == "M" || genre == "F")
                {
                    // Si c'est bon, on met "continuer" à false pour sortir de la boucle.
                    continuer = false;
                }
                else
                {
                    // Sinon, on affiche un message d'erreur et la boucle recommence.
                    MessageBox.Show("Veuillez entrer M ou F");
                }
            }

            // ===== Message de bienvenue personnalisé =====
            
            // "string.IsNullOrWhiteSpace" vérifie si le prénom est vide ou contient seulement des espaces.
            if (string.IsNullOrWhiteSpace(prenom))
            {
                // Si l'utilisateur n'a rien tapé, on dit juste "Bonjour !".
                MessageBox.Show("Bonjour !");
            }
            else
            {
                // Sinon, on dit bonjour avec son prénom.
                // "$" permet d'insérer des variables dans une chaîne avec {nomVariable}.
                MessageBox.Show($"Bonjour {prenom} !");
            }

            // ===== Démarrage de la musique de fond =====
            
            // On vérifie quelle musique doit jouer selon la valeur de "bgmchoice".
            // "== true" n'est pas vraiment nécessaire, on pourrait juste écrire "if (bgmchoice)".
            if (bgmchoice == true)
            {
                // ".PlayLooping()" joue la musique en boucle (elle recommence quand elle finit).
                bgm1.PlayLooping();
            }
            else
            {
                bgm2.PlayLooping();
            }

            // Cette ligne écrit un message dans la console de débogage de Visual Studio.
            // C'est utile pour vérifier que tout fonctionne pendant le développement.
            Debug.WriteLine($"Décompte initial Noël: {NoelTimer}");
        }

        // ========== FONCTION EXÉCUTÉE CHAQUE SECONDE PAR LE CHRONOMÈTRE ==========
        
        // "private" = cette fonction n'est utilisable que dans cette classe.
        // "void" = cette fonction ne renvoie aucune valeur.
        // "object? sender" = l'objet qui a déclenché l'événement (ici, le chronomètre). Le "?" signifie qu'il peut être null.
        // "EventArgs e" = informations supplémentaires sur l'événement (on ne les utilise pas ici).
        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            // On recalcule le temps restant jusqu'à Noël.
            MettreAJourNoelTimer();
            
            // On met à jour l'affichage avec le nouveau temps.
            CountDownTextBlock.Text = NoelTimer;
        }

        // ========== FONCTION QUI CALCULE LE TEMPS RESTANT JUSQU'À NOËL ==========
        
        // "public" = cette fonction peut être utilisée depuis n'importe où (pas juste dans cette classe).
        public void MettreAJourNoelTimer()
        {
            // "DateTime.Now" donne la date et l'heure actuelles de l'ordinateur.
            DateTime maintenant = DateTime.Now;

            // On récupère l'année actuelle (exemple : 2026).
            int anneeNoel = maintenant.Year;
            
            // On crée une date qui représente Noël de cette année : 25 décembre à minuit.
            // Les 6 nombres sont : année, mois, jour, heure, minute, seconde.
            DateTime prochainNoel = new DateTime(anneeNoel, 12, 25, 0, 0, 0);

            // On vérifie si Noël de cette année est déjà passé.
            // ">=" signifie "supérieur ou égal à".
            if (maintenant >= prochainNoel)
            {
                // Si oui, on ajoute 1 à l'année pour cibler le Noël de l'année prochaine.
                anneeNoel++;
                prochainNoel = new DateTime(anneeNoel, 12, 25, 0, 0, 0);
            }

            // On calcule la différence entre Noël et maintenant.
            // "TimeSpan" est un type qui représente une durée (jours, heures, minutes, secondes).
            TimeSpan reste = prochainNoel - maintenant;

            // On construit le texte du compte à rebours.
            // "{reste.Days}j" = nombre de jours suivi de "j".
            // "{reste.Hours:D2}" = heures sur 2 chiffres (ex: 05 au lieu de 5).
            // "D2" signifie "affiche au moins 2 chiffres".
            NoelTimer = $"{reste.Days}j {reste.Hours:D2}:{reste.Minutes:D2}:{reste.Seconds:D2}";
            
            // On met à jour un autre élément visuel avec le nombre de jours.
            Days_until_Christmas.Text = $"{reste.Days} jour(s) restant(s) avant Noël!";
            
            // On stocke le nombre de jours + 1 dans la variable "DaysLeft".
            // Le +1 est probablement là pour gérer un décalage dans la logique du calendrier.
            DaysLeft = reste.Days + 1;
        }

        // ========== FONCTION EXÉCUTÉE QUAND ON FERME LA FENÊTRE ==========
        
        // "protected" = accessible uniquement dans cette classe et ses classes dérivées.
        // "override" = on remplace la fonction "OnClosed" qui existe déjà dans la classe parente "Window".
        protected override void OnClosed(EventArgs e)
        {
            // Il faut toujours nettoyer les ressources quand on ferme l'application.
            
            // On arrête le chronomètre.
            countdownTimer.Stop();
            
            // On arrête la première musique.
            bgm1.Stop();
            // ".Dispose()" libère la mémoire utilisée par l'objet. Très important !
            bgm1.Dispose();
            
            // Pareil pour la deuxième musique.
            bgm2.Stop();
            bgm2.Dispose();
            
            // On appelle la fonction originale "OnClosed" de la classe parente.
            // Sans cette ligne, des bugs peuvent apparaître.
            base.OnClosed(e);
        }

        // ========== FONCTION EXÉCUTÉE QUAND ON CLIQUE SUR LE BOUTON "CARTE DU JOUR" ==========
        
        // Dans WPF, quand on clique sur un bouton, une fonction comme celle-ci est appelée.
        private void Button_TodayCard_Click(object sender, RoutedEventArgs e)
        {
            // ===== Construction dynamique du nom de la page à afficher =====
            
            // On construit une chaîne de caractères qui contient le nom complet d'une classe.
            // Exemple : si DaysLeft = 5, typeName sera "Wail_Vincent_AventCalender.Ressources.Views.Card5".
            // "$" permet d'insérer la valeur de DaysLeft dans la chaîne.
            var typeName = $"Wail_Vincent_AventCalender.Ressources.Views.Card{DaysLeft}";
            
            // "Type.GetType" essaie de trouver une classe qui porte ce nom dans le projet.
            // Si la classe n'existe pas, "type" sera null (vide).
            var type = Type.GetType(typeName);

            // On vérifie si la classe a été trouvée.
            if (type == null)
            {
                // Si non, on affiche un message d'erreur.
                MessageBox.Show($"Vous n'êtes pas/plus en décembre.");
                // "return" arrête l'exécution de la fonction ici.
                return;
            }

            // ===== Création d'une instance de la page et affichage =====
            
            // "Activator.CreateInstance" crée un nouvel objet du type trouvé.
            // "is Page page" vérifie si cet objet est bien une Page, et si oui, le stocke dans "page".
            if (Activator.CreateInstance(type) is Page page)
            {
                // "MainContent" est un élément Frame dans le XAML.
                // Un Frame est comme un cadre qui peut afficher différentes pages.
                // Ici, on charge la page de la carte dans ce cadre.
                MainContent.Content = page;
            }
            else
            {
                // Si l'objet créé n'est pas une Page, on affiche une erreur.
                MessageBox.Show("Impossible de charger la page de carte correspondante.");
            }
        }

        // ========== FONCTION EXÉCUTÉE QUAND ON CLIQUE SUR LE BOUTON DE CHANGEMENT DE MUSIQUE ==========
        
        // Cette fonction permet de basculer entre les deux musiques de fond.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // On vérifie quelle musique joue actuellement.
            if (bgmchoice == true)
            {
                // Si c'est la musique 1, on passe à la musique 2.
                bgmchoice = false;
                bgm1.Stop();
                bgm2.PlayLooping();
            }
            else
            {
                // Si c'est la musique 2, on repasse à la musique 1.
                bgmchoice = true;
                bgm2.Stop();
                bgm1.PlayLooping();
            }
        }
    }
}