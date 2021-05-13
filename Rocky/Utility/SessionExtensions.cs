using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Utility
{
    public static class SessionExtensions
    {
        /// <summary>
        /// This class is used to extend the capabilities of sessions to store more advanced data types other than string and Int,
        /// by using a key value pair, the value can represent the serialized Object with the key being used for retrieval
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="session"></param>
        /// <param name="key">The key that was assigned to the serialized data obj</param>
        /// <param name="value">The data object to be serialized/deserialized</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
