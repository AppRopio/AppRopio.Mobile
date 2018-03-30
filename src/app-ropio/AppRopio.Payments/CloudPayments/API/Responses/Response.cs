using System;
namespace AppRopio.Payments.CloudPayments.API.Responses
{
    public class Response<T>
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public T Model { get; set; }
    }
}
