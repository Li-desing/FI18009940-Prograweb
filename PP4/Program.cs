using PP4.Data;
using PP4.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

using var context = new BooksContext();
context.Database.EnsureCreated();

if (!context.Authors.Any())                 
{
    Console.WriteLine("La base de datos está vacía, se llenará con los datos del archivo CSV");
    Console.WriteLine("Cargando");

    var csvPath = Path.Combine("data", "books.csv");
    var lines = File.ReadAllLines(csvPath).Skip(1);

    foreach (var line in lines)
    {
        var parts = SplitCsvLine(line);
        if (parts.Length < 3) continue;

        var authorName = parts[0].Trim();
        var titleName = parts[1].Trim();
        var tags = parts[2].Split('|',StringSplitOptions.RemoveEmptyEntries)
                           .Select(t => t.Trim());

        var author = context.Authors
                            .FirstOrDefault(a => a.AuthorName == authorName)  ?? new Author { AuthorName = authorName };

        if (author.AuthorId == 0)
        {
            context.Authors.Add(author);
        }

        var title = new Title
        {
            TitleName = titleName,
            Author = author
        };
        context.Titles.Add(title);

        foreach (var tagName in tags)
        {
            var tag = context.Tags
                             .FirstOrDefault(t => t.TagName == tagName) ?? new Tag { TagName = tagName };

            if (tag.TagId == 0)
            {
                context.Tags.Add(tag);
            }

            context.TitleTags.Add(new TitleTag
            {
                Title = title,
                Tag = tag
            });

        }

    }

    context.SaveChanges();
    Console.WriteLine("Listo la base de datos se ha llenado con los datos del archivo CSV");
}
else 

{
    Console.WriteLine("La base de datos ya contiene datos, se generarán los archivos TSV.");
    Console.WriteLine("Procesando");

    var query = context.Titles
                       .Include(t => t.Author)
                       .Include(t => t.TitleTags)
                       .ThenInclude(tt => tt.Tag)
                       .ToList();

    var grouped = query.GroupBy(t => t.Author.AuthorName[0].ToString().ToUpperInvariant());

    foreach (var group in grouped)
    {
        var tsvPath = Path.Combine("data", $"{group.Key}.tsv");
        using var writer = new StreamWriter(tsvPath);
        writer.WriteLine("AuthorName\tTitleName\tTagsName");

        var ordered = group 
          .OrderByDescending(t => t.Author.AuthorName)
          .ThenByDescending(t => t.TitleName);

         foreach(var title in ordered)
         {
            foreach (var tag in title.TitleTags.Select(tt => tt.Tag).OrderByDescending(t => t.TagName))
            {
                writer.WriteLine($"{title.Author.AuthorName}\t{title.TitleName}\t{tag.TagName}");
            }
         } 
    }

    Console.WriteLine("Listo los archivos TSV se han generado en la carpeta data.");                           
}


static string[] SplitCsvLine(string line)
{
    var parts = new List<string>();
    bool inQuotes = false;
    var current = new System.Text.StringBuilder();

    foreach (char c in line)
    {
        if (c == '"')
        {
            inQuotes = !inQuotes;
        }
        else if (c == ',' && !inQuotes)
        {
            parts.Add(current.ToString());
            current.Clear();
        }
        else
        {
            current.Append(c);
        }
    }

    parts.Add(current.ToString());
    return parts.ToArray();
}

using (var bd = new BooksContext())
{
    if (bd.Authors.Any())
    {
        Console.WriteLine("La base de datos se está leyendo para crear los archivos TSV.");
        Console.WriteLine("Procesando");

        var query = from a in bd.Authors
                    from t in bd.Titles
                    where t.AuthorId == a.AuthorId
                    from tt in bd.TitleTags
                    where tt.TitleId == t.TitleId   
                    from tag in bd.Tags
                    where tag.TagId == tt.TagId
                    orderby a.AuthorName descending, t.TitleName , tag.TagName
                    select new
                    {
                        a.AuthorName,
                        t.TitleName,
                        tag.TagName
                    };

        var groups = query.AsEnumerable().GroupBy(q => q.AuthorName[0]);
        
        foreach (var group in groups)
        {
            string filePath = Path.Combine("data", $"{group.Key}.tsv");
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("AuthorName\tTitleName\tTagName");
                foreach (var item in group)
                {
                    writer.WriteLine($"{item.AuthorName}\t{item.TitleName}\t{item.TagName}");
                }
            }
        }

        Console .WriteLine("Listo los archivos TSV se han generado en la carpeta data.");
    }
}