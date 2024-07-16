namespace FoodOrderingWeb.Extensions
{
    public class FoodNotFoundException : Exception
    {
        public FoodNotFoundException(string message ) : base(message) { }
    }
}
