using System.Xml.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => Results.Redirect("/swagger")).WithName("RootRedirect");


static bool TryParseHeaderBool(HttpRequest req, string headerName, out bool value)
{
    value = false;
    if (!req.Headers.TryGetValue(headerName, out var vals) || vals.Count == 0)
        return false; // no presente -> caller debe usar default false
    return bool.TryParse(vals.First(), out value);
}

static IResult Error(string msg)
{
    return Results.Json(new { error = msg }, statusCode: 400);
}

static string SerializeToXml<T>(T obj)
{
    var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("Result"));
    using var ms = new MemoryStream();
    var settings = new System.Xml.XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = true };
    using var writer = System.Xml.XmlWriter.Create(ms, settings);
    serializer.Serialize(writer, obj);
    return Encoding.UTF8.GetString(ms.ToArray());
}

record ResultDto(string Ori, string New);


app.MapPost("/include/{position:int}", async (HttpRequest req, int position) =>
{

    var value = req.Query["value"].ToString();

    var form = await req.ReadFormAsync();
    var text = form["text"].ToString();


    bool xml = false;
    if (req.Headers.TryGetValue("xml", out var hv) && hv.Count > 0)
        bool.TryParse(hv.First(), out xml);

    if (position < 0) return Error("'position' must be 0 or higher");
    if (string.IsNullOrWhiteSpace(value)) return Error("'value' cannot be empty");
    if (string.IsNullOrWhiteSpace(text)) return Error("'text' cannot be empty");

    var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

    var insertIndex = Math.Min(position, words.Count);
    words.Insert(insertIndex, value);

    var newText = string.Join(' ', words);

    if (xml)
    {
        var dto = new ResultDto(text, newText);
        var xmlStr = SerializeToXml(dto);
        return Results.Text(xmlStr, "application/xml");
    }
    return Results.Json(new { ori = text, @new = newText });
});


app.MapPut("/replace/{length:int}", async (HttpRequest req, int length) =>
{
    if (length <= 0) return Error("'length' must be greater than 0");

    var value = req.Query["value"].ToString();
    var form = await req.ReadFormAsync();
    var text = form["text"].ToString();

    bool xml = false;
    if (req.Headers.TryGetValue("xml", out var hv) && hv.Count > 0)
        bool.TryParse(hv.First(), out xml);

    if (string.IsNullOrWhiteSpace(value)) return Error("'value' cannot be empty");
    if (string.IsNullOrWhiteSpace(text)) return Error("'text' cannot be empty");

    var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    for (int i = 0; i < words.Length; i++)
    {
        if (words[i].Length == length)
            words[i] = value;
    }

    var newText = string.Join(' ', words);

    if (xml)
    {
        var dto = new ResultDto(text, newText);
        var xmlStr = SerializeToXml(dto);
        return Results.Text(xmlStr, "application/xml");
    }
    return Results.Json(new { ori = text, @new = newText });
});


app.MapDelete("/erase/{length:int}", async (HttpRequest req, int length) =>
{
    if (length <= 0) return Error("'length' must be greater than 0");

    var form = await req.ReadFormAsync();
    var text = form["text"].ToString();

    bool xml = false;
    if (req.Headers.TryGetValue("xml", out var hv) && hv.Count > 0)
        bool.TryParse(hv.First(), out xml);

    if (string.IsNullOrWhiteSpace(text)) return Error("'text' cannot be empty");

    var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

    words = words.Where(w => w.Length != length).ToList();

    var newText = string.Join(' ', words);

    if (xml)
    {
        var dto = new ResultDto(text, newText);
        var xmlStr = SerializeToXml(dto);
        return Results.Text(xmlStr, "application/xml");
    }
    return Results.Json(new { ori = text, @new = newText });
});

app.Run();