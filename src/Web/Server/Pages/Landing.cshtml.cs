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
        private readonly AudioSpatialSignalProcess _audioService;
        private const int DATA_ID = 2;

        public Show[]? FeaturedShows { get; set; }

        public Landing(PodcastService podcastService, LegacyService legacyService, AudioSpatialSignalProcess audioService)
        {
            _podcastService = podcastService;
            _legacyService = legacyService;
            _audioService = audioService;
        }

        public async Task OnGet()
        {
            var shows = await _podcastService.GetShows(50, null);
            FeaturedShows = shows?.Where(s => s.IsFeatured).ToArray();

            /// RetrieveSupportInfo();

            var ratio = await AudioCompressionRatio();
          }

        public async void RetrieveSupportInfo()
        {
            var response = await _legacyService.RetrieveData(DATA_ID);

            await Response.WriteAsync(response);
        }

        public async Task<QuadraticRoots> AudioCompressionRatio()
        {
            var response = await _audioService.RetrieveQuadraticRoots(3, 4, 5);

            return response;
        }
    }
}
