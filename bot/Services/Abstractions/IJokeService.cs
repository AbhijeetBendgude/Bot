using System.Threading.Tasks;

namespace bot.Services.Abstractions
{
    public interface IJokeService
    {
        Task<string> GetJoke();
    }
}
