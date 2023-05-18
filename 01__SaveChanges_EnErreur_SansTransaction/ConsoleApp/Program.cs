using Demo.Domain;

using ConsoleApp.EF;
using Microsoft.EntityFrameworkCore;

using (var myDbContext = (new MyDbContextFactory()).CreateDbContext())
{

    //-- PHASE SANS ERREUR A VENIR, et sans SaveChanges() --
    var categorie1 = new CategorieArticle()
    {
        Label = "Categorie 1"
    };
    myDbContext.CategorieArticles.Add(categorie1); //Opération à appliquer, tout à fait valide.
    //myDbContext.SaveChanges();                   //<< REM. : sans cette sauvegarde intermédiaire là,
                                                   //          il ne sera pas possible d'avoir categorie1.Id renseigné suite à l'insertion demandée ci-dessus !
                                                   //          MÊME SI en effet on fait un SaveChanges() après toutes les opérations.


    //-- PHASE AVEC ERREUR à venir, lors du SaveChanges() --
    var article1 = new Article()
    {
        Label = "Article 1",
        CategorieId = categorie1.Id  //FK obligatoire à préciser (not null), MAIS à ce stade categorie1.Id (auto-increment) est non renseigné (=0),
                                     // car on n'a pas fait un myDbContext.SaveChanges() suite au Add de categorie1 plus haut !
                                     // ET MÊME si la valeur affectée ici à CategorieId était juste (par ex. en dur),
                                     //  (c-à-d bien égale à l'Id auto-incr. théorique de la catégorie dont on a demandé un Add plus haut), eh bien,
                                     // le fait de ne pas avoir fait le SaveChanges plus haut suite au Add, fait que l'insertion de l'Article ici, fera planter
                                     // ci-dessous lors du SaveChanges final !
    };
    myDbContext.Articles.Add(article1); //<<< Opération à appliquer, *NON valide* car le champ CategorieId de l' Article n'est pas une FK valide



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
                          "\n             lors d'un myDbContext.SaveChanges(), si au moins UNE des opérations à appliquer sur la base n'est pas valide," +
                          "\n              ALORS : AUCUNE d'entre elles n'est appliquée, ceci sans passer par une Transaction explicitement !" +
                          "\n              PAR CONTRE, les compteurs auto-increment Éventuellement impliqués seront quand même incrémentés !!");

        Console.ReadKey();
    }

}

