using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MagicTheGatheringApp.Models;

namespace MagicTheGatheringApp.Services
{
    public class DeckService
    {
        private readonly IDbContextFactory<MyDbContext> _gallery;

        public DeckService(IDbContextFactory<MyDbContext> gallery)
        {
            _gallery = gallery;
        }

        public List<Card?> GetCards(int deckId)
        {
            using var context = _gallery.CreateDbContext();
            return (
                from deckCard in context.DeckCards
                where deckCard.DeckId == deckId
                select (
                    from card in context.Cards
                    where card.Id == deckCard.CardId
                    select card
                ).FirstOrDefault()
            ).ToList();
        }

        public async Task<List<Deck>> GetDecksAsync()
        {
            using var context = _gallery.CreateDbContext();
            return await context.Decks
                .Include(d => d.DeckCards)
                    .ThenInclude(dc => dc.Card)
                .ToListAsync();
        }

        public async Task<bool> DeleteDeckAsync(int deckId)
        {
            using var context = _gallery.CreateDbContext();
            var deck = await context.Decks
                .Include(d => d.DeckCards)
                .ThenInclude(dc => dc.Card)
                .FirstOrDefaultAsync(d => d.Id == deckId);

            if (deck == null) return false;

            context.Cards.RemoveRange(deck.DeckCards.Select(dc => dc.Card));
            context.Decks.Remove(deck);
            await context.SaveChangesAsync();
            return true;
        }

        public List<Card?> SearchCardInDeck(int deckId, string query)
        {
            using var context = _gallery.CreateDbContext();
            var cards = GetCards(deckId);

            if (!string.IsNullOrEmpty(query))
            {
                cards = (from card in cards
                         join set in context.Sets on card!.SetCode equals set.Code
                         where card.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                               set.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                               set.Code.Contains(query, StringComparison.OrdinalIgnoreCase)
                         select card).ToList();
            }

            return cards ?? new List<Card?>();
        }

        public List<Card?> FilterCardsInDeck(int deckId, string query, int? convertedManaCost = null, string? type = null, string? rarityCode = null, string? color = null)
        {
            using var context = _gallery.CreateDbContext();
            var cards = GetCards(deckId);

            if (convertedManaCost.HasValue)
            {
                cards = cards?.Where(card => card!.ConvertedManaCost == convertedManaCost.ToString()).ToList();
            }

            if (!string.IsNullOrEmpty(type))
            {
                cards = cards?.Where(card => card!.CardTypes.Any(ct => ct.Type.Name == type)).ToList();
            }

            if (!string.IsNullOrEmpty(rarityCode))
            {
                cards = cards?.Where(card => card!.RarityCode == rarityCode).ToList();
            }

            if (!string.IsNullOrEmpty(color))
            {
                cards = cards?.Where(card => card!.CardColors.Any(cc => cc.Color.Name == color)).ToList();
            }

            return cards?.OrderBy(card => card!.Name).ToList() ?? new List<Card?>();
        }
    }
}
