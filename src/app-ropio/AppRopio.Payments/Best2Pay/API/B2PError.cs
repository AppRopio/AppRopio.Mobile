namespace AppRopio.Payments.Best2Pay.API
{
    public class B2PError
    {
        public B2PError(int code, string description)
        {
            Code = code;
            Description = description;
        }

        public int Code { get; private set; }
        public string Description { get; private set; }
    }
}
