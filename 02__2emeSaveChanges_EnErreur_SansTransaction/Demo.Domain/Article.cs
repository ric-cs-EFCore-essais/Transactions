namespace Demo.Domain;

public class Article
{
    public int Id { get; set; }
    public string Label { get; set; } = null!;
    public int CategorieId { get; set; }

}