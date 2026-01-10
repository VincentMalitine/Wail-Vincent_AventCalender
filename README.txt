
## Technologies utilisées

- **Langage** : C# 13.0
- **Framework** : .NET 9
- **Interface graphique** : WPF (Windows Presentation Foundation)
- **Audio** : System.Media.SoundPlayer
- **Dialogues** : Microsoft.VisualBasic.Interaction

## Notes techniques

### Calcul du compte à rebours
- L'application utilise `DateTime.Now` pour obtenir l'heure système
- Le prochain Noël est calculé comme le 25 décembre à 00:00:00
- Si la date actuelle est >= 25 décembre, l'année est incrémentée

### Chargement dynamique des cartes
- Les cartes sont chargées dynamiquement via la réflexion (`Type.GetType()`)
- Le nom de la classe est construit selon le pattern : `Wail_Vincent_AventCalender.Ressources.Views.Card{X}`
- Où `{X}` est le nombre de jours restants + 1

### Gestion de la mémoire
- Les ressources audio sont correctement libérées avec `Dispose()` à la fermeture
- Le chronomètre est arrêté pour éviter les fuites mémoire

## Auteurs

Projet réalisé par Wail et Vincent dans le cadre d'un calendrier de l'Avent interactif.

## Licence

Ce projet est un projet éducatif.