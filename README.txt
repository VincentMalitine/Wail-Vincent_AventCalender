# Calendrier de l'Avent - Application WPF

## Description du projet

Cette application est un **calendrier de l'Avent interactif** développé en C# avec WPF (.NET 9). Elle affiche un compte à rebours en temps réel jusqu'à Noël et permet d'ouvrir des cartes quotidiennes en fonction du nombre de jours restants avant le 25 décembre.

## Fonctionnalités principales

### 1. Compte à rebours vers Noël
- Affichage dynamique du temps restant jusqu'au prochain 25 décembre
- Format d'affichage : `Xj HH:mm:ss` (jours, heures, minutes, secondes)
- Mise à jour automatique chaque seconde
- Si Noël de l'année en cours est passé, le compte à rebours cible automatiquement l'année suivante

### 2. Personnalisation de l'expérience utilisateur
Au démarrage de l'application :
- **Saisie du prénom** : Une boîte de dialogue demande votre prénom (facultatif)
- **Saisie du genre** : Une boîte de dialogue demande votre genre (M ou F)
  - La saisie est obligatoire et validée strictement
  - Seules les réponses "M" ou "F" sont acceptées (insensible à la casse)
  - Un message d'erreur s'affiche si la saisie est incorrecte
- **Message de bienvenue** : Un message personnalisé s'affiche avec votre prénom (si fourni)

### 3. Musique de fond
- Deux musiques de fond disponibles (bgm1.wav et bgm2.wav)
- Lecture en boucle automatique au démarrage
- Bouton permettant de basculer entre les deux musiques à tout moment

### 4. Cartes quotidiennes du calendrier de l'Avent
- Chaque jour de décembre correspond à une carte spécifique
- Le bouton "Carte du jour" charge automatiquement la carte correspondant au nombre de jours restants
- Les cartes sont des pages WPF individuelles (Card1.xaml à Card25.xaml)
- Si vous n'êtes pas en décembre ou si la carte n'existe pas, un message d'erreur s'affiche

## Structure du code

### Classe principale : `MainWindow`
Hérite de `Window` (classe WPF pour créer des fenêtres)

#### Variables principales
- `bgm1`, `bgm2` : Lecteurs audio pour les deux musiques de fond
- `NoelTimer` : Texte formaté du compte à rebours
- `countdownTimer` : Minuterie qui se déclenche chaque seconde
- `continuer` : Contrôle la boucle de validation du genre
- `bgmchoice` : Détermine quelle musique est active
- `DaysLeft` : Nombre de jours restants avant Noël (+ 1 pour la logique du calendrier)

#### Méthodes principales

**`MainWindow()` - Constructeur**
- Initialise l'interface graphique
- Configure et démarre le chronomètre
- Gère les dialogues de saisie utilisateur
- Lance la musique de fond

**`CountdownTimer_Tick()`**
- Appelée automatiquement chaque seconde par le chronomètre
- Met à jour l'affichage du compte à rebours

**`MettreAJourNoelTimer()`**
- Calcule le temps restant jusqu'au prochain 25 décembre
- Met à jour les éléments visuels avec les nouvelles valeurs
- Gère automatiquement le passage à l'année suivante après Noël

**`OnClosed()`**
- Nettoie les ressources (chronomètre, lecteurs audio) à la fermeture
- Évite les fuites mémoire

**`Button_TodayCard_Click()`**
- Charge dynamiquement la carte correspondant au jour actuel
- Utilise la réflexion C# pour créer l'instance de la page
- Affiche la carte dans le Frame `MainContent`

**`Button_Click()`**
- Bascule entre les deux musiques de fond
- Arrête la musique actuelle et lance l'autre

## Comment utiliser l'application

### Prérequis
- Windows 10/11
- .NET 9 Runtime installé
- Les fichiers audio doivent être présents dans `Ressources\BGM\`
  - `bgm1.wav`
  - `bgm2.wav`
- Les pages de cartes doivent exister dans `Ressources\Views\`
  - `Card1.xaml` à `Card25.xaml` (ou autant que nécessaire)

### Lancement de l'application

1. **Démarrez l'exécutable** de l'application

2. **Suivez les dialogues de bienvenue** :
   - Entrez votre prénom (ou laissez vide et validez)
   - Entrez votre genre : tapez `M` ou `F` puis validez
   - Cliquez sur "OK" pour le message de bienvenue

3. **Interface principale** :
   - Le compte à rebours vers Noël s'affiche et se met à jour en temps réel
   - La musique de fond démarre automatiquement

4. **Ouvrir la carte du jour** :
   - Cliquez sur le bouton prévu pour afficher la carte du jour
   - La carte correspondant au nombre de jours restants s'affichera dans la fenêtre

5. **Changer de musique** :
   - Cliquez sur le bouton de changement de musique pour alterner entre bgm1 et bgm2

### Période d'utilisation optimale
L'application est conçue pour être utilisée pendant le mois de décembre, jusqu'au 25 décembre. En dehors de cette période, le système de cartes peut afficher un message indiquant que vous n'êtes pas en décembre.

## Structure des fichiers
