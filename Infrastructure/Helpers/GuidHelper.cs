namespace Infrastructure.Helpers
{
    public static class GuidHelper
    {
        public static Guid GetGuidFromString(string guidString)
        {
            if (Guid.TryParse(guidString, out Guid guid))
            {
                return guid;
            }
            else
            {
                throw new ArgumentException("Invalid guid string");
            }
        }
    }
}
