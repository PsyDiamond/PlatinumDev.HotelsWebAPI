public static class XmlResultExtension
{
    public static IResult Xml<T>(this IResultExtensions _, T result) => 
        new XmlResult<T>(result);
}