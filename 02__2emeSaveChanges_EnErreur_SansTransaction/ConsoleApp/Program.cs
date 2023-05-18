using Demo.Domain;

using ConsoleApp.EF;
using Microsoft.EntityFrameworkCore;

using (var myDbContext = (new MyDbContextFactory()).CreateDbContext())
{
    
    //-- PHASE SANS ERREUR A VENIR, et avec un SaveChanges() --
    var categorie1 = new CategorieArticle()
    {
        Label = "Categorie 1"
    };
    myDbContext.CategorieArticles.Add(categorie1); //Opération à appliquer, tout à fait valide.
    myDbContext.SaveChanges();   //<< REM. : sans cette sauvegarde intermédiaire là,
                                 //          il ne sera pas possible d'avoir categorie1.Id renseigné suite à l'insertion demandée ci-dessus !
                                 //          MÊME SI en effet on fait un SaveChanges() après toutes les opérations.


    //-- PHASE AVEC ERREUR à venir, lors du SaveChanges() --
    var article1 = new Article()
    {
        Label = "Article 1",
        //CategorieId = categorie1.Id  //FK obligatoire valide à préciser, omise volontairement donc
    };
    myDbContext.Articles.Add(article1); //<<< Opération à appliquer, *NON valide* car le champ FK CategorieId de l' Article, n'est pas renseigné !



    //-- TENTATIVE D'ECRITURE EN BASE --
    try
    {
        myDbContext.SaveChanges(); //Génèrera une DbUpdateException

    } catch(DbUpdateException ex)
    {
        Console.WriteLine($"*!! EF DbUpdateException : {ex.Message}\n");
    }
    finally
    {
        Console.WriteLine("CONCLUSION : " +
                          "\n             lorsqu'un myDbContext.SaveChanges() se déroule sans erreur, " + 
                          "\n             alors la base de données est effectivement mise à jour," +
                          "\n             même si un myDbContext.SaveChanges() qui suit plante !" +
                          "\n             Sachant que ces appels à SaveChanges() sont tout deux dans un même :  using(myDbContext) {...}" +
                          "\n              (Bien entendu, les modifs demandées qui font planter le 2ème SaveChanges(), ne seront, ELLES, " +
                          "\n               pas appliquées sur la base !)"
                          );

        Console.ReadKey();
    }

}

