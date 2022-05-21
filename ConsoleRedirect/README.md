# ConsoleRedirect

ConsoleRedirect intercepts Console.Write() and Console.WriteLine() calls and routes the text to one or more EventHandlers.

## Classes

* ConsoleRedirect -- Create an instance anywhere in your application, and all Console text will be re-routed to the Event Handler passed into the Constructor. Also contains a static method serialize an object using Json, then passing the resulting string to Console.WriteLine().
* ConsoleRedirectEventArgs -- Contains the Console text in the Value property.  If receiving an object that's been serialized to Json, use the GetValueAs<T> method.