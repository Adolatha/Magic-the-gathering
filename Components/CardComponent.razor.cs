using MagicTheGatheringApp.Models;
using Microsoft.AspNetCore.Components;

namespace MagicTheGatheringApp.Components
{
    public partial class CardComponent : IDisposable
    {
        [Parameter]
        public Card? Card { get; set; }
        private CardDetails? _cardDetails;

        private bool _isDisposed = false;

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _cardDetails?.Dispose();
            }
        }
    }
}
