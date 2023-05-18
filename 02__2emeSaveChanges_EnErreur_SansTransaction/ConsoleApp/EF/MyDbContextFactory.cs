using Demo.Infra.EF;

using Microsoft.EntityFrameworkCore; // Pour DbContextOptionsBuilder
using Microsoft.EntityFrameworkCore.Design; //Pour IDesignTimeDbContextFactory

namespace ConsoleApp.EF;

public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args) //<<<La méthode qui sera automatiquement appelée par : update-database, etc...
    {
        return CreateDbContext();
    }

    public MyDbContext CreateDbContext()
    {
        Console.WriteLine("\n\nPréparation de l'instanciation de MyDbContext\n\n");

        var databaseName = "Essais_EF_TRANSAC_2emeSaveChanges_EnErreur_SansTransaction";
        var connectionString = $"Server=PC-RP;Database={databaseName};Trusted_Connection=true;TrustServerCertificate=true;";

        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

        optionsBuilder.UseSqlServer(connectionString); //Méthode issue du Nuget : Microsoft.EntityFrameworkCore.SqlServer

        return new MyDbContext(optionsBuilder.Options);
    }
}
