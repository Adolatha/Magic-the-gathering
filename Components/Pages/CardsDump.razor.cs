using MagicTheGatheringApp.Controllers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MagicTheGatheringApp.Components.Pages
{
    public partial class CardsDump : ComponentBase
    {
        [Inject]

        protected CardsDumpController? Controller { get; set; }

        protected string NoMatchingCardsError => "No matching cards found.";
        protected string NoMatchingCardsMessage => "";


        protected override async Task OnInitializedAsync()
        {
            await Controller!.LoadCardsAsync();
        }

        protected string GetNoMatchingCardsMessage()
        {
            return NoMatchingCardsMessage;
        }

        protected string GetNoMatchingCardsError()
        {
            return NoMatchingCardsError;
        }

        protected async Task LoadCardsAsync(string noMatchingCardsMessage, string noMatchingCardsError)
        {
            vard moreCards = await Controller!.LoadCardsAsync();

            if (!moreCards.Any())
            {
                NoMatchingCardsMessage = "No Matching Cards";
                NoMatchingCardsError = "No Matching Cards found";
            }
        }

        protected async Task OnFilter(CardFilterParameters parameters)
        {
            var cards = await Controller!.OnFilter(parameters);
            
            if (!cards.Any())
            {
            
                NoMatchingCardsError = "No Matching Cards found";
                NoMatchingCardsMessage = "No Matching Cards";
            }
        }

        protected async Task OnReset()
            {
            await Controller!.OnReset();
        }
    }
}
