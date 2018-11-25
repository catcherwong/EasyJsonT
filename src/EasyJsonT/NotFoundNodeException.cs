namespace EasyJsonT
{
    using System;

    /// <summary>
    /// Not found property exception.
    /// </summary>
    public class NotFoundNodeException : Exception
    {
        public NotFoundNodeException(string message) : base(message)
        {
        }
    }
}
