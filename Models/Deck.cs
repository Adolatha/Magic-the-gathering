namespace MagicTheGatheringApp.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<DeckCard> DeckCards { get; set; } = new List<DeckCard>();
    }
}
