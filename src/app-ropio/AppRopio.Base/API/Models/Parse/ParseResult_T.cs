namespace AppRopio.Base.API.Models
{
    public class ParseResult<T>: ParseResult  where T : class
    {
        public T ParsedObject { get; set; }
    }
}
