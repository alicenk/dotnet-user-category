using LibraryApi.Creator;
using LibraryApi.Data;
using LibraryApi.Extentions;
using Microsoft.EntityFrameworkCore;
using Nest;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Redis bağlantısı
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(configuration);
});

//Elasticsearch Bağlantısı
var settings = new ConnectionSettings(new Uri(builder.Configuration["Elasticsearch:Uri"]));
var client = new ElasticClient(settings);
// Elasticsearch servisini ekleyin
builder.Services.AddSingleton<IElasticClient>(client);
// Elasticsearch indexlerini oluşturan servis
var indexCreator = new ElasticsearchIndexCreator(client);
indexCreator.CreateIndices();

// Servisler
builder.Services.AddApplicationServices();

// Add DbContext and specify connection string
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();