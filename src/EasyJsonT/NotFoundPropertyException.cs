namespace EasyJsonT
{
    using System;

    public class NotFoundPropertyException : Exception
    {
        public NotFoundPropertyException(string message) : base(message)
        {
        }
    }
}
