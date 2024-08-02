using MagicTheGatheringApp.Components;
using MagicTheGatheringApp.Controllers;
using MagicTheGatheringApp.Models;
using MagicTheGatheringApp.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<Service>();
builder.Services.AddScoped<CardService>();
builder.Services.AddScoped<SetService>();
builder.Services.AddScoped<DraftService>();
builder.Services.AddScoped<DeckService>();
builder.Services.AddScoped<CardsDumpController>();
builder.Services.AddScoped<SetController>();
builder.Services.AddScoped<DraftManagerController>();
builder.Services.AddScoped<DecksController>();
builder.Services.AddScoped<DeckManagerController>();


builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyDbContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
