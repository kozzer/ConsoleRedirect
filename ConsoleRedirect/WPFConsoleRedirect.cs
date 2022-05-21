using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace KozzerTools
{
    /// <summary>
    /// <para>WPF Implementation: Redirects Console.Write() and Console.WriteLine() text to EventHandler(s) passed to this object.  At least 1 EventHandler is required, provided into Constructor. Can add additional redirect endpoints via the AddRedirectHandler method.</para>Nuget Package:<list type="bullet"><item>System.Text.Json v6.0.4</item></list>
    /// </summary>
    public class WPFConsoleRedirect : ConsoleRedirect
    {
        /// <summary>
        /// Creates new instance without auto-calling Dispose(). Will redirect Console.WriteLine text to passed-in EventHandler<list type="bullet"><item>NOTE: Implements IDisposable -- You should make sure you either call this constructor in a using statement, or call Dispose() explicitly in your client code.</item></list>
        /// </summary>
        /// <param name="redirectHandler">EventHandler to receive Console.WriteLine text</param>
        public WPFConsoleRedirect(EventHandler<ConsoleRedirectEventArgs> redirectHandler) : base(redirectHandler) { }

        /// <summary>
        /// Creates a new instance, also automatically setting the Dispose() call tied to the passed-in FrameworkElement.  Will redirect Console.WriteLine text to passed-in EventHandler
        /// </summary>
        /// <param name="redirectHandler">EventHandler to receive Console.WriteLine text</param>
        /// <param name="wpfElement">WPF element/UserControl/Window that this object will use to determine when to call Dispose()</param>
        public WPFConsoleRedirect(EventHandler<ConsoleRedirectEventArgs> redirectHandler, FrameworkElement wpfElement) : base(redirectHandler)
        {
            // FrameworkElement is a base class for most of WPF, includes Window class, all types of controls, and even things like Storyboards
            //  --> we'll change what we do here based on what the passed-in object actually is

            if (wpfElement is Window)
                // Element is a Window
                setWindowClosingHandler(wpfElement as Window);

            else if (wpfElement is Control)
            {
                // Element is not a window itself but actually a Control, so try to get the parent window
                var controlWindow = Window.GetWindow(wpfElement);
                if (controlWindow != null)
                    // We were able to get the window so set the window handler
                    setWindowClosingHandler(controlWindow);
            }

            // No matter the type for wpfElement, assign the Dispatcher.ShutdownStarted handler so DisposeRedirect() is called when app is closing
            setDispatcherShutdownHandler(wpfElement);
        }

        private void setWindowClosingHandler(Window window) => window.Closing += (s, e) => Dispose();
        private void setDispatcherShutdownHandler(FrameworkElement wpfElement) => wpfElement.Dispatcher.ShutdownStarted += (s, e) => Dispose();

    }
}
