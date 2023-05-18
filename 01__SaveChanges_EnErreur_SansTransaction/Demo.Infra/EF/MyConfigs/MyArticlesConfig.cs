using Microsoft.EntityFrameworkCore; //Pour IEntityTypeConfiguration
using Microsoft.EntityFrameworkCore.Metadata.Builders; //Pour EntityTypeBuilder

using Demo.Domain;

namespace Demo.Infra.EF.MyConfigs;

internal class MyArticlesConfig : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> articlesBuilder)
    {
        articlesBuilder
            .HasKey(article => article.Id);

        articlesBuilder.Property(article => article.Label)
            .IsRequired()
            .HasMaxLength(75);

        articlesBuilder
            .HasOne<CategorieArticle>().WithMany() // Article *<------>1 CategorieArticle
            .HasForeignKey(article => article.CategorieId);
    }
}
