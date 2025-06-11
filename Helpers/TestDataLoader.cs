using Newtonsoft.Json;
using Models;

namespace Helpers
{
    public static class TestDataLoader
    {
        public static IEnumerable<UserTestData> LoadUsers(string relativePath)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);
            var json = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<List<UserTestData>>(json);
        }
    }
}