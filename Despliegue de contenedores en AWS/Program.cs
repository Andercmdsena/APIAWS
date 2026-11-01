using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Despliegue de contenedores en AWS - API",
        Version = "v1",
        Description = "API sencilla de ejemplo para gestionar una lista de nombres en memoria."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = string.Empty; // Swagger UI en la raíz "/"
});

// En Elastic Beanstalk, nginx recibe el tráfico por HTTP (puerto 80) y lo pasa a la app
// por el puerto 5000, así que la redirección a HTTPS solo se usa en desarrollo.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// Endpoint de salud: sirve para comprobar rápido qué versión está desplegada.
// Para probar el pipeline, cambia la versión, haz commit y push.
app.MapGet("/health", () => Results.Ok(new
{
    status = "OK",
    version = "2.0",
    fechaUtc = DateTime.UtcNow
}));

app.Run();
