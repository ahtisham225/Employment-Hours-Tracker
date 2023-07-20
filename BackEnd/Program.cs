using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProjectsDB>(opt => opt.UseInMemoryDatabase("ProjectsList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.MapGet("/projectitems", async (ProjectsDB db) =>
    await db.Projectss.ToListAsync());


app.MapGet("/projectitems/{id}", async (int id, ProjectsDB db) =>
    await db.Projectss.FindAsync(id)
        is Projects project
            ? Results.Ok(project)
            : Results.NotFound());

app.MapPost("/projectitems", async (Projects project, ProjectsDB db) =>
{
    db.Projectss.Add(project);
    await db.SaveChangesAsync();

    return Results.Created($"/projectitems/{project.Id}", project);
});

app.Run();