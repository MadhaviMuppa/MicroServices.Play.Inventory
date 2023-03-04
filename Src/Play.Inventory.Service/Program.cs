using play.common.mongodb;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//var configuration = builder.Configuration;//builder.Services.AddSingleton(provider => { provider.GetService(IConfiguration)});

builder.Services.AddMongo().AddMongoRepository<InventoryItem>("Inventoryitems");
builder.Services.AddHttpClient<CatalogClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7223");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(provider =>
            {
                provider.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader().AllowAnyMethod();
            });
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
