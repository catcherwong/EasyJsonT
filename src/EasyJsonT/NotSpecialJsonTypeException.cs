namespace EasyJsonT
{
    using System;

    /// <summary>
    /// Not special json type exception.
    /// </summary>
    public class NotSpecialJsonTypeException : Exception
    {
        public NotSpecialJsonTypeException(string message) : base(message)
        {
        }
    }
}
