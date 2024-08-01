using MagicTheGatheringApp.Models;
using MagicTheGatheringApp.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagicTheGatheringApp.Controllers
{
    public class SetController
    {
        private readonly CardService _cardService;
        private List<Set> _sets;

        public SetController(CardService cardService)
        {
            _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
            _sets = new List<Set>();
        }

        public async Task FilterSetsAsync(SetQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            _sets = await _cardService.GetSets(query.Text);
        }

        public IReadOnlyList<Set> GetSets()
        {
            return _sets.AsReadOnly();
        }
    }
}
