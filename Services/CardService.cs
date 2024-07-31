using Microsoft.EntityFrameworkCore;
using MagicTheGatheringApp.Models;
using System.Drawing;

namespace MagicTheGatheringApp.Services;

public class CardService
{
    private readonly IDbContextFactory<MyDbContext> _gallery;

    public CardService(IDbContextFactory<MyDbContext> gallery)
    {
        _gallery = gallery;
    }
    public List<Card> GetCards()
    {
        using MyDbContext context = _gallery.CreateDbContext();
        return context.Cards.ToList();
    }

    public List<Card> GetCardsFromSet(string setCode)
    {
        using MyDbContext context = _gallery.CreateDbContext();
        return context.Cards.Where(c => c.SetCode == setCode).ToList();
    }

    public List<Models.Color> GetColors()
    {
        using MyDbContext context = _gallery.CreateDbContext();
        return context.Colors.ToList();
    }

    public async Task<List<Set>> GetSets(string query)
    {
        using MyDbContext context = _gallery.CreateDbContext();
        return await context.Sets
            .AsNoTracking()
            .Where(s => EF.Functions.ILike(s.Name, $"%{query}%")).ToListAsync();
    }

    public async Task<List<Card>> SearchCards(string query, string lastName, int pageSize)
    {
        using MyDbContext context = _gallery.CreateDbContext();
        IQueryable<Card> cardsQuery = context.Cards
            .Where(card => !string.IsNullOrEmpty(card.OriginalImageUrl))
            .OrderBy(card => card.Name);
        
        if (!string.IsNullOrEmpty(query))
        {
            cardsQuery = ApplySearch(cardsQuery, query, context);
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            cardsQuery = cardsQuery.Where(card => string.Compare(card.Name, lastName) > 0);
        }

        var CardsOnPage = await cardsQuery
            .Take(pageSize)
            .ToListAsync();

        return CardsOnPage;
    }

    public IQueryable<Card> ApplySearch(IQueryable<Card> cardsQuery, string query, MyDbContext gallery)
    {
        return from card in cardsQuery
               join set in gallery.Sets on card.SetCode equals set.Code
               where EF.Functions.Like(card.Name, $"%{query}%") || EF.Functions.Like(set.Name, $"%{query}%") || EF.Functions.Like(set.Code, $"%{query}%")
               select card;
    }

    public async Task<List<Card>> GetCards(string lastname, int pageSize)
    { 
        using MyDbContext context = _gallery.CreateDbContext();
        IQueryable<Card> cardsQuery = context.Cards
            .Where(card => !string.IsNullOrEmpty(card.OriginalImageUrl))
            .OrderBy(card => card.Name);

        if (!string.IsNullOrEmpty(lastname))
        {
            cardsQuery = cardsQuery.Where(card => string.Compare(card.Name, lastname) > 0);
        }

        var CardsOnPage = await cardsQuery
            .Take(pageSize)
            .ToListAsync();

        return CardsOnPage;
    }

    public async Task<Card> GetCardById(long id)
    {
        using MyDbContext context = _gallery.CreateDbContext();
        Card? card = await context.Cards.FirstOrDefaultAsync(c => c.Id == id);
        return card!; 
    }

    public async Task<List<Card>> FilterCards(string query, string lastName, int pageSize, int? convertedManaCost = null, string? type = null, string? rarityCode = null, string? color = null)
    {
        using MyDbContext context = _gallery.CreateDbContext();
        IQueryable<Card> cardsQuery = context.Cards
            .AsNoTracking()
            .Where(card => !string.IsNullOrEmpty(card.OriginalImageUrl));

        if (convertedManaCost.HasValue)
        {
            cardsQuery = cardsQuery.Where(card => card.ConvertedManaCost == convertedManaCost.ToString());
        }

        if (!string.IsNullOrEmpty(type))
        {
            cardsQuery = cardsQuery.Where(card => card.CardTypes.Any(ct => ct.Type.Name == type));
        }

        if (!string.IsNullOrEmpty(rarityCode))
        {
            cardsQuery = cardsQuery.Where(card => card.RarityCode == rarityCode);
        }

        if (!string.IsNullOrEmpty(color))
        {
            cardsQuery = cardsQuery.Where(card => card.CardColors.Any(cc => cc.Color.Name == color));
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            cardsQuery = cardsQuery.Where(card => string.Compare(card.Name, lastName) > 0);
        }

        if (!string.IsNullOrEmpty(query))
        {
            cardsQuery = ApplySearch(cardsQuery, query, context);
        }

        var CardsOnPage = await cardsQuery
            .OrderBy(card => card.Name)
            .Take(pageSize)
            .ToListAsync();

        return CardsOnPage;
    }

}
