using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicTheGatheringApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicTheGatheringApp.Services
{
    public class DraftService
    {
        public static readonly int PackSize = 16;

        private readonly CardService _cardService;
        private readonly IDbContextFactory<MyDbContext> _dbContextFactory;
        private readonly Dictionary<Guid, Card> _currentTable = new();
        private readonly Dictionary<Guid, bool> _currentDeck = new();
        private string? _currentSetCode = null;
        private bool _isDrafting = false;

        public DraftService(CardService cardService, IDbContextFactory<MyDbContext> dbContextFactory)
        {
            _cardService = cardService;
            _dbContextFactory = dbContextFactory;
        }

        public void StartDraft(string setCode)
        {
            _currentTable.Clear();
            _currentDeck.Clear();
            _currentSetCode = setCode;
            _isDrafting = true;
        }

        public async Task FinishDraftAsync(string deckName)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var deck = new Deck
            {
                Name = deckName
            };

            context.Decks.Add(deck);
            await context.SaveChangesAsync();

            var deckCards = GetDeck().Select(card => new DeckCard
            {
                DeckId = deck.Id,
                CardId = (int?)card.Item.Id
            }).ToList();

            context.DeckCards.AddRange(deckCards);
            await context.SaveChangesAsync();

            ResetDraftState();
        }

        public bool IsDrafting => _isDrafting;

        public void OpenPack()
        {
            if (_currentSetCode == null)
            {
                throw new InvalidOperationException("Draft not started");
            }

            var availableCards = _cardService.GetCardsFromSet(_currentSetCode);
            var pack = Random.Shared.GetItems(availableCards.ToArray(), PackSize);

            foreach (var card in pack)
            {
                var cardId = Guid.NewGuid();
                _currentTable[cardId] = card;
                _currentDeck[cardId] = false;
            }
        }

        public void AddCardToDeck(Guid cardId)
        {
            if (!_currentTable.ContainsKey(cardId))
            {
                throw new KeyNotFoundException("Card not found");
            }

            _currentDeck[cardId] = true;
        }

        public void RemoveCardFromDeck(Guid cardId)
        {
            if (!_currentTable.ContainsKey(cardId))
            {
                throw new KeyNotFoundException("Card not found");
            }

            _currentDeck[cardId] = false;
        }

        public List<IdentifiedItem<Card>> GetTable()
        {
            return _currentTable
                .Where(pair => !_currentDeck[pair.Key])
                .Select(IdentifiedItem<Card>.FromPair)
                .ToList();
        }

        public List<IdentifiedItem<Card>> GetDeck()
        {
            return _currentTable
                .Where(pair => _currentDeck[pair.Key])
                .Select(IdentifiedItem<Card>.FromPair)
                .ToList();
        }

        private void ResetDraftState()
        {
            _currentTable.Clear();
            _currentDeck.Clear();
            _currentSetCode = null;
            _isDrafting = false;
        }
    }
}
