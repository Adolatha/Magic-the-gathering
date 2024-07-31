using Microsoft.EntityFrameworkCore;
using MagicTheGatheringApp.Models;

public class Service
{
    private readonly MyDbContext _context;

    public Service(MyDbContext context)
    {
        _context = context;
    }
    public async Task<List<Card>> GetCards()
    {
        return await _context.Cards.ToListAsync();
    }
    public async Task<List<Color>> GetColors()
    {
        return await _context.Colors.ToListAsync();
    }

}
 