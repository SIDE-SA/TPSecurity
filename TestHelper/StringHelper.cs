using System.Text;

namespace TestHelper
{
    public static class StringHelper
    {
        public static string GetStringWithLength(int length)
        {
            return Enumerable.Range(1, length)
                             .Select(x => "a")
                             .Aggregate(new StringBuilder(), (prev, current) => prev.Append(current))
                             .ToString();
        }
    }
}