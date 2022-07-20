using WebApiVersion.Services;
using WebApiVersion.Services.Serializers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDocumentService, DocumentService>();

builder.Services.Scan(sc => sc
    .FromExecutingAssembly()
        .AddClasses(classes => classes.AssignableTo<IDocumentSerializer>())
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());


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
