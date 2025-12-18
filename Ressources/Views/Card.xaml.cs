using System.Windows.Controls;

namespace Wail_Vincent_AventCalender
{
    /// <summary>
    /// Représente une carte affichable dans l'interface utilisateur.
    /// </summary>
    public class Card : UserControl
    {
        public Card()
        {
            // Exemple de contenu minimal pour la carte.
            Content = new TextBlock
            {
                Text = "Carte du jour",
                FontSize = 24,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
        }
    }
}