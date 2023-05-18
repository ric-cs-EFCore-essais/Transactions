using Microsoft.EntityFrameworkCore;

using Demo.Domain;


namespace Demo.Infra.EF;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        Console.WriteLine("\n\n - Instanciation de MyDbContext -\n\n");
    }


    public DbSet<Article> Articles { get; set; }
    public DbSet<CategorieArticle> CategorieArticles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
    }

    // Configuration de la connexion à la base de données
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("chaine_de_connexion");
    //}
}