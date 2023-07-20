using Microsoft.EntityFrameworkCore;

class ProjectsDB : DbContext
{
    public ProjectsDB(DbContextOptions<ProjectsDB> options)
        : base(options) { }

    public DbSet<Projects> Projectss => Set<Projects>();
}