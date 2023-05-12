namespace Podcast.Updater.Worker;

public interface IPodcastUpdateHandler
{
    Task HandleUpdateAsync(CancellationToken cancellationToken);
}

public class PodcastUpdateHandler : IPodcastUpdateHandler
{
    private readonly ILogger<PodcastUpdateHandler> _logger;
    private readonly IFeedClient _feedClient;
    private readonly PodcastDbContext _podcastDbContext;
    private static readonly Guid DEFAULT_SHOW_ID = new Guid("936DA01F-9ABD-4d9d-80C7-02AF85C822A8");

    public PodcastUpdateHandler(PodcastDbContext podcastDbContext,
        ILogger<PodcastUpdateHandler> logger, IFeedClient feedClient)
    {
        _podcastDbContext = podcastDbContext;
        _logger = logger;
        _feedClient = feedClient;
    }

    public async Task HandleUpdateAsync(CancellationToken cancellationToken)
    {
        var feeds = await _podcastDbContext.Feeds.Include(x => x.Show!.Episodes).ToListAsync(cancellationToken);

        foreach (var feed in feeds)
        {
            _logger.LogInformation("Updating feed: {url}", feed.Url);

            var show = await _feedClient.GetShowAsync(feed, cancellationToken);

            if (!show.Episodes.Any()) continue;

            if (feed.Show == null)
            {
                feed.Show = show;
            }
            else
            {
                var newEpisodes = GetNewEpisodes(feed.Show.Episodes, show.Episodes);
            }

            await _podcastDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static IEnumerable<Episode> GetNewEpisodes(IEnumerable<Episode> existingEpisodes,
        IEnumerable<Episode> allEpisodes)
    {
        var newEpisodes = allEpisodes.Where(newEpisode =>
            existingEpisodes.All(existingEpisode => existingEpisode.Url != newEpisode.Url));
        return newEpisodes;
    }
}