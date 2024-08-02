using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicTheGatheringApp.Components;
using MagicTheGatheringApp.Models;
using MagicTheGatheringApp.Services;
using Microsoft.AspNetCore.Components;

namespace MagicTheGatheringApp.Controllers
{
    public class DeckManagerController
    {
        private readonly DeckService _deckService;
        public List<Card>? Cards { get; private set; }
        public CardFilterParameters CurrentFilterParameters { get; private set; }

        public DeckManagerController(DeckService deckService)
        {
            _deckService = deckService;
            Cards = new List<Card>();
            CurrentFilterParameters = new CardFilterParameters();
        }

        public List<Card?> LoadDeckCards(int deckId)
        {
            List<Card?> cards;

            if (!string.IsNullOrEmpty(CurrentFilterParameters.SearchQuery))
            {
                cards = _deckService.SearchCardInDeck(deckId, CurrentFilterParameters.SearchQuery);
            }
            else if (HasActiveFilters())
            {
                cards = _deckService.FilterCardsInDeck(deckId, CurrentFilterParameters.SearchQuery,
                                                       CurrentFilterParameters.ConvertedManaCostFilter,
                                                       CurrentFilterParameters.TypeFilter,
                                                       CurrentFilterParameters.RarityCodeFilter,
                                                       CurrentFilterParameters.ColorFilter);
            }
            else
            {
                cards = _deckService.GetCards(deckId);
            }

            UpdateCardList(cards);
            return cards;
        }

        public List<Card?> OnSearchDeck(int deckId, CardFilterParameters parameters)
        {
            UpdateFilterParameters(parameters);
            var cards = _deckService.SearchCardInDeck(deckId, parameters.SearchQuery);
            UpdateCardList(cards);
            return cards;
        }

        public List<Card?> OnFilterDeck(int deckId, CardFilterParameters parameters)
        {
            UpdateFilterParameters(parameters);
            var cards = _deckService.FilterCardsInDeck(deckId, parameters.SearchQuery,
                                                       parameters.ConvertedManaCostFilter,
                                                       parameters.TypeFilter,
                                                       parameters.RarityCodeFilter,
                                                       parameters.ColorFilter);
            UpdateCardList(cards);
            return cards;
        }

        public void OnDeckReset(int deckId)
        {
            ResetFilterParameters();
            LoadDeckCards(deckId);
        }

        private bool HasActiveFilters()
        {
            return CurrentFilterParameters.ConvertedManaCostFilter != null ||
                   CurrentFilterParameters.TypeFilter != null ||
                   CurrentFilterParameters.RarityCodeFilter != null ||
                   CurrentFilterParameters.ColorFilter != null;
        }

        private void UpdateFilterParameters(CardFilterParameters parameters)
        {
            CurrentFilterParameters.SearchQuery = parameters.SearchQuery;
            CurrentFilterParameters.ConvertedManaCostFilter = parameters.ConvertedManaCostFilter;
            CurrentFilterParameters.TypeFilter = parameters.TypeFilter;
            CurrentFilterParameters.RarityCodeFilter = parameters.RarityCodeFilter;
            CurrentFilterParameters.ColorFilter = parameters.ColorFilter;
        }

        private void UpdateCardList(List<Card?> cards)
        {
            Cards!.Clear();
            Cards!.AddRange(cards!);
        }

        private void ResetFilterParameters()
        {
            CurrentFilterParameters = new CardFilterParameters();
            Cards = new List<Card>();
        }
    }
}
