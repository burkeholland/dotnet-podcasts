using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Podcast.Shared;
using Podcast.Server.Services;

namespace Podcast.Server.Pages
{
    public class Landing : PageModel
    {
        private readonly PodcastService _podcastService;
        private readonly LegacyService _legacyService;
        private const int DATA_ID = 2;

        public Show[]? FeaturedShows { get; set; }

        public Landing(PodcastService podcastService, LegacyService legacyService)
        {
            _podcastService = podcastService;
            _legacyService = legacyService;
        }

        public async Task OnGet()
        {
            var shows = await _podcastService.GetShows(50, null);
            FeaturedShows = shows?.Where(s => s.IsFeatured).ToArray();

            RetrieveSupportInfo();
        }

        public async void RetrieveSupportInfo()
        {
            var response = await _legacyService.RetrieveData(DATA_ID);

            await Response.WriteAsync(response);
        }
    }
}
