using MagicTheGatheringApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Threading.Tasks;

namespace MagicTheGatheringApp.Components
{
    public partial class Filter : ComponentBase
    {
        [Inject]
        protected CardService? service { get; set; }

        [Parameter]
        public CardFilterParameters FilterParameters { get; set; } = new CardFilterParameters();

        [Parameter]
        public EventCallback<CardFilterParameters> OnSearch { get; set; }

        [Parameter]
        public EventCallback<CardFilterParameters> OnFilter { get; set; }

        [Parameter]
        public EventCallback OnReset { get; set; }

        protected async Task SearchCards()
        {
            await OnSearch.InvokeAsync(FilterParameters);
        }

        protected async Task FilterCards()
        {
            await OnFilter.InvokeAsync(FilterParameters);
        }

        protected async Task ResetSearch()
        {
            await OnReset.InvokeAsync();
        }
    }

    public class CardFilterParameters
    {
        public string SearchQuery { get; set; } = "";
        public int? ConvertedManaCostFilter { get; set; } = null;
        public string? TypeFilter { get; set; } = null;
        public string? RarityCodeFilter { get; set; } = null;
        public string? ColorFilter { get; set; } = null;
    }
}