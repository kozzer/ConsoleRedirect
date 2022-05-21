using System;

namespace KozzerTools
{
    /// <summary>
    /// Wrapper Exception class to indicate failure happened in Kozzer code
    /// </summary>
    public class KozzerException : Exception
    {
        public KozzerException() 
            : base() { }

        public KozzerException(string message) 
            : base(message) { }

        public KozzerException (string message, Exception inner) 
            : base(message, inner) { }

        public KozzerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) 
            : base(info, context) { }
    }
}
