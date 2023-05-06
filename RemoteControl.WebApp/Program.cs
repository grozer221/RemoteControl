using RemoteControl.MsSql.Extensions;
using RemoteControl.WebApp.Extensions;
using RemoteControl.WebApp.GraphApi;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();

// Add services to the container.
builder.Services.AddInjectableServices();
builder.Services.AddMsSql(configuration.GetConnectionString("RemoteControlDatabase"));
builder.Services.AddGraphQLApi();
builder.Services.AddJwtAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.UseWebSockets();
app.UseGraphQLUpload<AppSchema>()
    .UseGraphQL<AppSchema>();
app.UseGraphQLAltair();

app.MapFallbackToFile("index.html");

app.Run();
