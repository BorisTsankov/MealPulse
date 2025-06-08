namespace Services.Services.Interfaces
{
    public interface IAiService
    {
        Task<string> AskAsync(List<(string Role, string Content)> chatHistory);
    }
}