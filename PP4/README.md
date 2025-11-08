# Práctica Programada 4 - Programación Avanzada Web 

**Estudiante:** Lineth Leiva Vargas 
**Carné:** FI18009940 

## Comandos utilizados 

dotnet new console -n PP4 
dotnet new sln -n PP4
dotnet sln PP4.sln add PP4.csproj 
mkdir data
mkdir Models
dotnet build
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run 

## Ayuda de IA 
 
Copilot y ChatGPT

## Respuestas a las preguntas 

**¿Cómo cree que resultaría el uso de la estrategia de Code First para crear y actualizar una base de datos de tipo NoSQL (como por ejemplo MongoDB)? ¿Y con Database First? ¿Cree que habría complicaciones con las Foreign Keys?**

Tal vez no funcionaría de la mejor manera, porque Code First está diseñado para bases de datos relacionales. En las bases de datos NoSQL no existen llaves foráneas, por lo que las migraciones no funcionarían correctamente. 

**¿Cuál carácter, además de la coma (,) y el Tab (\t), se podría usar para separar valores en un archivo de texto con el objetivo de ser interpretado como una tabla (matriz)? ¿Qué extensión le pondría y por qué? Por ejemplo: Pipe (|) con extensión .pipe.**

Otro carácter que se puede usar es el punto y coma ( ; ), ya que evita conflictos cuando los datos contienen comas.

