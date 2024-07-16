namespace FoodOrderingWeb.Extensions
{
    public static class RandomShuffleExtension
    {
        private static Random rng = new Random();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(item => rng.Next());
        }
    }
}
