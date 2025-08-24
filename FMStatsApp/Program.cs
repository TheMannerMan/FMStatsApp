using FMStatsApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDistributedMemoryCache(); // +++ Viktigt för session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(180);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();

builder.Services.AddScoped<IPlayerScoringService, PlayerScoringService>();
builder.Services.AddScoped<IHtmlParserService, HtmlParserService>();
builder.Services.AddScoped<IPlayerSessionService, PlayerSessionService>();
builder.Services.AddScoped<IStartingXIOptimizerService, StartingXIOptimizerService>();
builder.Services.AddTransient<HtmlParser>();
builder.Services.AddTransient<ScoringCalculator>();
builder.Services.AddHttpContextAccessor();

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
app.UseSession();          // Flyttad efter UseRouting
app.UseAuthorization();

app.MapRazorPages();

app.Run();
