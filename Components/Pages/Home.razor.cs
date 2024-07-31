using Microsoft.AspNetCore.Components;

namespace MagicTheGatheringApp.Components.Pages
{
    public partial class Home
    {
        [Inject] public NavigationManager? Navigation { get; set; }
        private async Task Redirect() => Navigation!.NavigateTo("/cards");
    }
}
