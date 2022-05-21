using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace KozzerTools
{
    /// <summary>
    /// <para>Intercepts text sent to console via Console.Write and Console.WriteLine from anywhere within a running application and redirects it to an event handler</para>Nuget Package:<list type="bullet"><item>System.Text.Json v6.0.4</item></list>
    /// </summary>
    public class ConsoleRedirect : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        private       Progress<ConsoleRedirectEventArgs>     consoleReporter;
        private event EventHandler<ConsoleRedirectEventArgs> writeEvent;
        private event EventHandler<ConsoleRedirectEventArgs> writeLineEvent;

        #region Constructor

        /// <summary>
        /// Create new instance and set it as the default receiver of Console.WriteLine calls, and return instance.  <list type="bullet"><item>NOTE: Call this with a using statement, ex. using var redirect = new ConsoleRedirect(handler);</item></list>
        /// </summary>
        /// <param name="redirectHandler">Event handler that will accept the Console message strings in the EventArgs object</param>
        /// <returns>A new instance of ConsoleRedirect set to redirect Console.WriteLine messages </returns>
        public ConsoleRedirect(EventHandler<ConsoleRedirectEventArgs> redirectHandler)
        {
            Console.SetOut(this);
            setRedirectHandler(redirectHandler);
        }

        #endregion

        #region Event Handler methods

        /// <summary>
        /// Add an addtional EventHandler redirect target to receive Console.WriteLine text
        /// </summary>
        /// <param name="redirectHandler">EventHandler to add to list of endpoints for Console.WriteLine text</param>
        public void AddRedirectHandler(EventHandler<ConsoleRedirectEventArgs> redirectHandler)
        {
            consoleReporter.ProgressChanged += redirectHandler;
        }

        /// <summary>
        /// Accepts a ConsoleRedirect instance (made in a using statement), and an event handler to accept strings coming from Console.Write/Line calls
        /// </summary>
        /// <param name="redirectHandler"></param>
        private void setRedirectHandler(EventHandler<ConsoleRedirectEventArgs> redirectHandler)
        {
            consoleReporter = new Progress<ConsoleRedirectEventArgs>();
            consoleReporter.ProgressChanged += redirectHandler;
            writeLineEvent += (s, e) => reportToEventHandler(e);
            writeEvent     += (s, e) => reportToEventHandler(e);
        }

        private void reportToEventHandler(ConsoleRedirectEventArgs e)
        {
            // If not handled yet, go ahead and pass string to the EventHandler, then set the flag
            if (!e.Handled)
            {
                ((IProgress<ConsoleRedirectEventArgs>)consoleReporter).Report(e);
                e.Handled = true;
            }
        }

        #endregion

        #region Write

        /// <summary>
        /// Fire all Write event handlers and pass the value
        /// </summary>
        /// <param name="value">String value to send to Write event handlers</param>
        public override void Write(string value)
        {
            callWriteEventHandlers(value);
        }

        /// <summary>
        /// Serialize <typeparamref name="T" /> as Json and send to Write event handlers
        /// </summary>
        /// <typeparam name="T">Type to serialize into Json</typeparam>
        /// <param name="value">Value or object of type <typeparamref name="T" /></param>
        public void Write<T>(T value)
        {
            var json = JsonSerializer.Serialize(value);
            callWriteEventHandlers(json);
        }

        private void callWriteEventHandlers(string value)
        {
            if (writeEvent != null)
                writeEvent(this, new ConsoleRedirectEventArgs(value));
        }

        #endregion

        #region WriteLine

        /// <summary>
        /// Fire all WriteLine event handlers and pass the value
        /// </summary>
        /// <param name="value">String value to send to WriteLine event handlers</param>
        public override void WriteLine(string value)
        {
            callWriteLineEventHandlers(value);
        }

        /// <summary>
        /// Serialize <typeparamref name="T" /> as Json and send to WriteLine event handlers
        /// </summary>
        /// <typeparam name="T">Type to serialize into Json</typeparam>
        /// <param name="value">Value or object of type <typeparamref name="T" /></param>
        public void WriteLine<T>(T value)
        {
            var json = JsonSerializer.Serialize(value);
            callWriteLineEventHandlers(json);
        }

        private void callWriteLineEventHandlers(string value)
        {
            if (writeLineEvent != null)
                writeLineEvent(this, new ConsoleRedirectEventArgs(value));
        }

        #endregion

        #region Misc - static method WriteObject<T>(), Dispose()

        /// <summary>
        /// Serialize object of type <typeparamref name="T" /> to Json and then pass result to Console.WriteLine()
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="value">Object of type</param>
        public static void WriteObject<T>(T value)
        {
            var json = JsonSerializer.Serialize(value);
            Console.WriteLine(json);
        }

        /// <summary>
        /// Disposes this ConsoleRedirect instance
        /// </summary>
        public new void Dispose()
        {
            try
            {
                base.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // It's OK if already disposed, so just eat the error
            }
        }

        #endregion
    }

}
