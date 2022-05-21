using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace KozzerTools
{
    /// <summary>
    /// EventArgs for ConsoleRedirect Write and WriteLine events. Exposes the original string, or can get value as a type via Json deserialization
    /// </summary>
    public class ConsoleRedirectEventArgs : EventArgs
    {
        /// <summary>
        /// String value of what would have been displayed in the Console
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Flag indicating whether Event has been handled so all subsquent handlers can ignore
        /// </summary>
        public bool Handled { get; set; } = false;

        /// <summary>
        /// Constructor -- at least 1 EventHandler required 
        /// </summary>
        /// <param name="value">String value to pass from Console to EventHandler</param>
        public ConsoleRedirectEventArgs(string value) => Value = value;

        /// <summary>
        /// Deserialize Json into an object of type <typeparamref name="T" /> and return it. Will return default or null if deserialiazation fails.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the Json to</typeparam>
        /// <exception cref="KozzerException">Thrown when Json deserialization to <typeparamref name="T" /> object throws an Exception</exception>
        /// <returns>Deserialized object of type <typeparamref name="T" /></returns>
        public T GetValueAs<T>()
        {
            try
            {
                // Undo all character escaping
                var jsonValue = Regex.Unescape(Value);

                // De-serialize json into our object, return default if failed
                var obj = JsonSerializer.Deserialize<T>(jsonValue);
                if (obj == null)
                    return default(T);

                // Return reconstructed object
                return obj;
            }
            catch (Exception ex)
            {
                throw new KozzerException($"Failed to Deseralize Json into object: {ex.GetType().Name}: {ex.Message}", ex);
            }
        }
    }
}
