using System;

namespace Affecto.Testing.UI.Selenium
{
    public class MultipleMatchingElementsException : Exception
    {
        public MultipleMatchingElementsException(string message)
            : base(message)
        {
        }
    }
}