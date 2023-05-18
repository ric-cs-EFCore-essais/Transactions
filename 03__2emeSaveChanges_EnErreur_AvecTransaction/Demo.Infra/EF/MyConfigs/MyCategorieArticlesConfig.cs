using Microsoft.EntityFrameworkCore; //Pour IEntityTypeConfiguration
using Microsoft.EntityFrameworkCore.Metadata.Builders; //Pour EntityTypeBuilder

using Demo.Domain;

namespace Demo.Infra.EF.MyConfigs;

internal class MyCategorieArticlesConfig : IEntityTypeConfiguration<CategorieArticle>
{
    public void Configure(EntityTypeBuilder<CategorieArticle> categorieArticlesBuilder)
    {
        categorieArticlesBuilder
            .HasKey(categorieArticle => categorieArticle.Id);

        categorieArticlesBuilder.Property(categorieArticle => categorieArticle.Label)
            .IsRequired()
            .HasMaxLength(75);
    }
}
