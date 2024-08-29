using Microsoft.Azure.Cosmos;

namespace be_ip_repository.Helpers
{
    public static class FeedIteratorExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this FeedIterator<T> feedIterator)
        {
            if (feedIterator == null) throw new ArgumentNullException(nameof(feedIterator));

            List<T> items = new List<T>();

            while (feedIterator.HasMoreResults)
            {
                FeedResponse<T> response = await feedIterator.ReadNextAsync();
                items.AddRange(response);
            }

            return items;
        }
    }
}
