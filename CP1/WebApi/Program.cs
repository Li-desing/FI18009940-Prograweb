using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var list = new List<object>();
var random = new Random();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapPost("/", ([FromHeader] bool xml = false) =>
{
    if (xml)
    {
        var xmlSerializer = new XmlSerializer(typeof(List<object>));
        using var stringWriter = new StringWriter();
        xmlSerializer.Serialize(stringWriter, list);
        return Results.Content(stringWriter.ToString(), "application/xml");
    }
    else
    {
        return Results.Ok(list);
    }
});

app.MapPut("/", ([FromForm] int quantity, [FromForm] string type) =>
{
    if (quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    if (type != "int" && type != "float")
        return Results.BadRequest(new { error = "'type' must be 'int' or 'float'" });

    for (int i = 0; i < quantity; i++)
    {
        if (type == "int")
            list.Add(random.Next(0, 101));
        else
            list.Add(Math.Round(random.NextDouble() * 100, 2));
    }

    return Results.Ok(new { message = $"{quantity} {type} numbers added" });
}).DisableAntiforgery();


app.MapDelete("/", ([FromForm] int quantity) =>
{
    if (quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    if (list.Count < quantity)
        return Results.BadRequest(new { error = $"List contains only {list.Count} elements" });

    list.RemoveRange(0, quantity);
    return Results.Ok(new { message = $"{quantity} elements removed" });
}).DisableAntiforgery();


app.MapMethods("/", new[] { "PATCH" }, () =>
{
    list.Clear();
    return Results.Ok(new { message = "List cleared" });
}).DisableAntiforgery();

app.Run();

//Me guíe con Chatgpt y con Copilot 
