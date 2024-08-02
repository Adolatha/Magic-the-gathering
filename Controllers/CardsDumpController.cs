using MagicTheGatheringApp.Components;
using MagicTheGatheringApp.Models;
using MagicTheGatheringApp.Services;

namespace MagicTheGatheringApp.Controllers
{
    public class CardsDumpController
    {
        private readonly CardService _cardService;
        private const int PageSize = 30;

        public List<Card>? Cards { get; private set; }

        public CardFilterParameters FilterParameters { get; private set; }

        public bool IsLoading { get; private set; }

        public CardsDumpController(CardService cardService)
        {
            _cardService = cardService;
            Cards = new List<Card>();
            FilterParameters = new CardFilterParameters();
            IsLoading = false;
        }

        public async Task<List<Card>> LoadMoreCards()
        {
            IsLoading = true;

            string lastName = Cards!.Any() ? Cards!.Last().Name : string.Empty;
            List<Card> moreCards;

            if (!string.IsNullOrEmpty(FilterParameters.SearchQuery))
            {
                moreCards = await _cardService.SearchCards(FilterParameters.SearchQuery, lastName, PageSize);
            }
            else if (FilterParameters.HasFilters())
            {
                moreCards = await _cardService.FilterCards(
                    FilterParameters.SearchQuery,
                    lastName,
                    PageSize,
                    FilterParameters.ConvertedManaCostFilter,
                    FilterParameters.TypeFilter,
                    FilterParameters.RarityCodeFilter,
                    FilterParameters.ColorFilter);
            }
            else
            {
                moreCards = await _cardService.GetCards(lastName, PageSize);
            }

            Cards!.AddRange(moreCards);
            IsLoading = false;

            return moreCards;
        }

        public async Task<List<Card>> OnSearch(CardFilterParameters parameters)
        {
            IsLoading = true;

            FilterParameters.SearchQuery = parameters.SearchQuery;
            Cards = await _cardService.SearchCards(parameters.SearchQuery, string.Empty, PageSize);

            IsLoading = false;

            return Cards;
        }

        public async Task<List<Card>> OnFilter(CardFilterParameters parameters)
        {
            IsLoading = true;

            FilterParameters = parameters;
            Cards = await _cardService.FilterCards(
                parameters.SearchQuery,
                string.Empty,
                PageSize,
                parameters.ConvertedManaCostFilter,
                parameters.TypeFilter,
                parameters.RarityCodeFilter,
                parameters.ColorFilter);

            IsLoading = false;

            return Cards;
        }

        public async Task OnReset()
        {
            IsLoading = true;

            FilterParameters = new CardFilterParameters();
            Cards!.Clear();
            await LoadMoreCards();

            IsLoading = false;
        }
    }

    public static class CardFilterExtensions
    {
        public static bool HasFilters(this CardFilterParameters filterParameters)
        {
            return filterParameters.ConvertedManaCostFilter != null ||
                   filterParameters.TypeFilter != null ||
                   filterParameters.RarityCodeFilter != null ||
                   filterParameters.ColorFilter != null;
        }



    }
}
