using BusinessLogic;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Frontend.Data;
using Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<UserMemory>();
builder.Services.AddSingleton<UserLogic>();
builder.Services.AddSingleton<UserDto>();
builder.Services.AddSingleton<TeamMemory>();
builder.Services.AddSingleton<TeamLogic>();
builder.Services.AddSingleton<PanelLogic>();
builder.Services.AddSingleton<PanelMemory>();
builder.Services.AddSingleton<TaskLogic>();
builder.Services.AddSingleton<RecycleBinLogic>();
builder.Services.AddSingleton<RecycleBinMemory>();
builder.Services.AddSingleton<CommentLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();