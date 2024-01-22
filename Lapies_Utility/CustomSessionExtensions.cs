using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Lapis_Utility
{
    public static class CustomSessionExtensions
    {
        public static void Set<T> (this ISession session , string key , T val)
        {
            session.SetString(key, JsonSerializer.Serialize(val));
        }
        
        public static T Get<T> (this ISession session , string key)
        {
            var serializedValue= session.GetString(key);
            if (serializedValue == null)
            {
                return default;
            }
            else
            {
                return JsonSerializer.Deserialize<T>(serializedValue);
            }
        }
    }
}
