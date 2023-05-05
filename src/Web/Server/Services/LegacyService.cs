namespace Podcast.Server.Services
{
    public class LegacyService
    {
        public async Task<string> RetrieveData(int delay)
        {
            var random = new Random();

            await Task.Delay(delay * 1000);

            return Guid.NewGuid().ToString();
        }
    }
}
