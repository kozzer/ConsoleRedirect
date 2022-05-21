# ConsoleRedirect

`ConsoleRedirect` intercepts Console.Write() and Console.WriteLine() calls and routes the text to one or more EventHandlers.

## Classes

  #### `ConsoleRedirect`
    Create an instance anywhere in your application, and all Console text will be re-routed to 
    the Event Handler passed into the Constructor. Also contains a static method to serialize an 
    object to Json, then passing the resulting string to Console.WriteLine().

   #### `ConsoleRedirectEventArgs`
    Contains the Console text in the Value property.  If receiving an object that's been 
    serialized to Json, use the GetValueAs<T>() method to retrieve as an instance of T.

## Usage

* All you need to do is instantiate it and make System.Console.WriteLine() calls. The constructor requires an event handler of type `EventHandler<ConsoleRedirectEventArgs>` with the following signature (name of method can be anything, also can be public or private):

      void HandlerMethod(object sender, ConsoleRedirectEventArgs e)
  
* Inside your event handler, using the `ConsoleRedirectEventArgs` class, you'll have access to the string sent from the Console.Write() or Console.WriteLine() call. The event args expose the string value, and the class also has a `GetValueAs<T>()` method that uses Json deserialization to re-build a serialized object passed through the console:

      private void redirectEventHandler(object sender, ConsoleRedirectEventArgs e)
      {
          DateTime theDateTimeObject = e.GetValueAs<DateTime>();
          showNotification("From the Console", theDateTimeObject.ToString("dddd, MMMM d, yyyy"));   // Saturday, May 21, 2022
      }
  
* `ConsoleRedirect` class has a static method `WriteObject<T>()`, which serializes an object into Json and passes the string to System.Console.WriteLine() to be retrieved by your event handler as described above:

      ConsoleRedirect.WriteObject<WidgetManifest>(manifest);   // Serializes manifest to Json, then calls Console.WriteLine()
  
* You can add more event handlers by calling the `ConsoleRedirect.AddEventHandler()` method, which accepts an action with the same method signature as what the constructor accepts:

      var consoleRedirect = new ConsoleRedirect(redirectEventHandler);
      consoleRedirect.AddRedirectHandler(anotherEventHandler);
      consoleRedirect.AddRedirectHandler(theMoreTheMerrierEventHandler);
  
* To restore normal System.Console functionality, keep a reference to the instance and call `Dispose()`.
  
