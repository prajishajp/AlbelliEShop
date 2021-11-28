using AlbelliEShop.Core;
using AlbelliEShop.Persistence;
using AlbelliEShop.Persistence.Contract;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbClient, DbClient>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.Configure<DbConfig>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "AlbelliEShop",
        Description = "Order 1 or multiple items and get the required bin width for your package. Products available: photobook, calendar, canvas, cards and mug.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Prajisha Jayaprakasan",
            Email = "prajishajayaprakasan@gmail.com"
        }
    });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "Swagger.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddTransient<IOrderService, OrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
