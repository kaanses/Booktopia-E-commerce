namespace App.WebApi.Controllers;

public class book
{
    
    public int Id { get; set; }
    
    public string Title { get; set; }
    public string Author { get; set; }
    public int PublicationYear { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    private string FormatName(string name)
    {
        string[] words = name.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (!string.IsNullOrEmpty(words[i]))
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }
        return string.Join(" ", words);
    }
}