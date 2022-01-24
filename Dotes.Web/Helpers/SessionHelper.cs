using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Ext.Extensions
{
    public static class SessionHelper
    {
        public static string AuthSessionKey = "AASKKND_AUTHINFO";
        public static string UserSessionKey = "AASKKND_USERINFO";

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            try
            {
                var value = session.GetString(key);
                return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
