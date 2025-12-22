using System.Reflection;
using Server;
using Server.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// Register// Build Root Module
AppModule.Register(builder.Services, builder.Configuration);

// Register Custom Authentication
builder.Services.AddAuthentication("Stateful")
    .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, Server.Modules.Auth.StatefulAuthHandler>("Stateful", null);

// Configure Swagger doc generation
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();

app.MapControllers();

// Map Module Endpoints (e.g. WebSockets)
AppModule.MapEndpoints(app);

app.Run();
