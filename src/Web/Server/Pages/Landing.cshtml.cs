using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Podcast.Shared;

namespace Podcast.Server.Pages
{
    public class Landing : PageModel
    {
        private readonly PodcastService _podcastService;

        public Show[]? FeaturedShows { get; set; }

        public Landing(PodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        public async Task OnGet()
        {
            var shows = await _podcastService.GetShows(50, null);
            FeaturedShows = shows?.Where(s => s.IsFeatured).ToArray();

            RetrieveSupportInfo();
        }

        public async void RetrieveSupportInfo()
        {
            var shows = await _podcastService.GetShows(50, null);
            FeaturedShows = shows?.Where(s => s.IsFeatured).ToArray();

            var leadauthor = FeaturedShows.FirstOrDefault().Author;

            await Response.WriteAsync(string.Format($"Logging the number featured shows {0}", leadauthor));
        }
    }
}
