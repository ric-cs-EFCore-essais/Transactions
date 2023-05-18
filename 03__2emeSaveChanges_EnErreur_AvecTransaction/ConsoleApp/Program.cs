using Microsoft.EntityFrameworkCore;

using Demo.Domain;

using ConsoleApp.EF;

using (var myDbContext = (new MyDbContextFactory()).CreateDbContext())
{
    using (var transaction = myDbContext.Database.BeginTransaction())
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
            //transaction.Commit(); //<< SI je décommente cette ligne-ci, alors le premier SaveChanges() (uniquement), sera bien écrit en base.
                                    //   car c'est bien un SaveChanges() qui ne générera pas d'erreur, contrairement à celui ci-dessous.

            myDbContext.SaveChanges(); //Génèrera une DbUpdateException

            transaction.Commit(); //Ligne non atteinte car le SaveChanges() ci-dessus générera une Exception !
                                  //DONC : comme le transaction.Commit() du dessus est en commentaire, rien
                                  //         (pas même le premier SaveChanges() qui n'aurait pas fait planter)
                                  //       ne sera écrit en base ! OK : c'est bien le principe attendu pour une Transaction !

        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"*!! EF DbUpdateException : {ex.Message}\n");
            
            //transaction.Rollback(); //<< Pas indispensable, car le simple fait de ne PAS avoir pu atteindre l'instruction transaction.Commit(),
                                    //   fait déjà que rien ne sera écrit en base ! Donc rien à Rollbacker en ce sens ! c'est CERTAIN !
                                    // Cependant, c'est une bonne pratique, et il paraîtrait que cet appel mettrait d'autres choses au propre (libération de ressources, etc... ?),
                                    // bien que le Dispose() devrait suffire pour ça, je pense (appel auto. au Dispose() de la transaction, en sortie de son using(){})
        }
        finally
        {
            Console.WriteLine("CONCLUSION : " +
                              "\n             lorsqu'une tentative d'écriture en base, est faite dans le cadre d'une Transaction explicite" +
                              "\n                using(var transaction = myDbContext.Database.BeginTransaction()) {...}" +
                              "\n             alors tant qu'un transaction.Commit() n'y est pas fait : RIEN n'est écrit en base !" +
                              "\n              (même pas les appels à SaveChanges() sans erreur, qu'on a pu effectuer avant" +
                              "\n               ce transaction.Commit() !)." +
                              "\n             PAR CONTRE, les compteurs auto-increment Éventuellement impliqués seront quand même incrémentés !!");

            Console.ReadKey();
        }
    }
}

