using MagicTheGatheringApp.Services;
using MagicTheGatheringApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagicTheGatheringApp.Controllers
{
    public class DecksController
    {
        private readonly DeckService _deckService;
        private List<Deck> _decks;
        private bool _isLoading;

        public DecksController(DeckService deckService)
        {
            _deckService = deckService;
            _decks = new List<Deck>();
            _isLoading = false;
        }

        public bool IsLoading => _isLoading;

        public async Task InitializeAsync()
        {
            _isLoading = true;
            await LoadDecksAsync();
            _isLoading = false;
        }

        private async Task LoadDecksAsync()
        {
            _decks = await _deckService.GetDecksAsync();
        }

        public IReadOnlyList<Deck> Decks => _decks.AsReadOnly();
    }
}
