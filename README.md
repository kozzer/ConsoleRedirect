# ConsoleRedirect

`ConsoleRedirect` intercepts Console.Write() and Console.WriteLine() calls and routes the text to one or more EventHandlers.

## Benefits

`ConsoleRedirect` gives us is a way to report back to the UI, or anywhere, in a given app without having to define thread-unsafe global objects or have to pass around a progress reporter through all of our methods. This way, you instantiate `ConsoleRedirect` where you want to receive the text or deserialized objects, like in a WPF Window, Windows Form, User Control, etc.  Then to use it, you just write to the Console via Console.WriteLine without having to pass any instances around or maintain global state. 

All it takes is 1 line of code (call to constructor) + the event handler, and it'll work anywhere in your application -- including library projects that know nothing about the UI. Anything can call Console.WriteLine in .net. 

The static method `ConsoleRedirect.WriteObject<T>()` uses System.Console.WriteLine() under the hood, using the passed-in object's serialized Json string.  Since it's static, there's no need to pass around instances.

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
  
* Inside your event handler, using the `ConsoleRedirectEventArgs` class, you'll have access to the string sent from the Console.Write() or Console.WriteLine() call. The event args expose the string value:

      private void redirectEventHandler(object sender, ConsoleRedirectEventArgs e)
      {
          MessageBox.Show(e.Value, "From the Console");   
      }
 
* `ConsoleRedirectEventArgs` also has a `GetValueAs<T>()` method that uses Json deserialization to re-build a serialized object passed through the console:

      private void redirectEventHandler(object sender, ConsoleRedirectEventArgs e)
      {
          DateTime theDateTimeObject = e.GetValueAs<DateTime>();
          MessageBox.Show(theDateTimeObject.ToString("dddd, MMMM d, yyyy"), "From the Console");   
      }
  
* `ConsoleRedirect` class has a static method `WriteObject<T>()`, which serializes an object into Json and passes the string to System.Console.WriteLine() to be retrieved by your event handler as described above:

      // Serialize manifestObject to Json, then call Console.WriteLine()
      ConsoleRedirect.WriteObject<WidgetManifest>(manifestObject);   
  
* You can add more event handlers by calling the `ConsoleRedirect.AddEventHandler()` method, which accepts an action with the same method signature as what the constructor accepts:

      var consoleRedirect = new ConsoleRedirect(redirectEventHandler);
      consoleRedirect.AddRedirectHandler(anotherEventHandler);
      consoleRedirect.AddRedirectHandler(theMoreTheMerrierEventHandler);
  
* To restore normal System.Console functionality, keep a reference to the instance and call `Dispose()`.
