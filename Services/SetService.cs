using System.Threading.Tasks;
using MagicTheGatheringApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicTheGatheringApp.Services
{
    public class SetService
    {
        private readonly IDbContextFactory<MyDbContext> _gallery;

        public SetService(IDbContextFactory<MyDbContext> gallery)
        {
            _gallery = gallery;
        }

        public Set? GetSetByCode(string code)
        {
            using var context = _gallery.CreateDbContext();
            return context.Sets.SingleOrDefault(set => set.Code == code);
        }

        public string GetSetName(string? setCode)
        {
            using var context = _gallery.CreateDbContext();
            return context.Sets.FirstOrDefault(set => set.Code == setCode)?.Name ?? string.Empty;
        }

        public string GetRarityName(string? rarityCode)
        {
            using var context = _gallery.CreateDbContext();
            return context.Rarities.FirstOrDefault(rarity => rarity.Code == rarityCode)?.Name ?? string.Empty;
        }

        public Rarity? GetRarityByCode(string code)
        {
            using var context = _gallery.CreateDbContext();
            return context.Rarities.SingleOrDefault(rarity => rarity.Code == code);
        }
    }
}
