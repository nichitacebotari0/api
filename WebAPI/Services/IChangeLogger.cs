namespace WebAPI.Services
{
    public interface IChangeLogger
    {
        Task Log(string modelName, string discordId, string discordNick, string previous, string next);

    }
}