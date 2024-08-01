using MagicTheGatheringApp.Models;
using Microsoft.AspNetCore.Components;

namespace MagicTheGatheringApp.Components
{
    public partial class CardComponent : IDisposable
    {
        [Parameter]
        public Card? card { get; set; }
        private CardDetails? cardDetails;

        private bool _isDisposed = false;

        public void Dispose()
            { 
            _isDisposed = true;
            }

       private void showCard()
        {
            if (!_isDisposed)
            {
               cardDetails!.Show(Card!);
            }
        }
    }
}
