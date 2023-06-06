namespace TimMovie.Web.Extensions;

public static class KeyValuePairExtension
{
    public static bool IsDefault<TKey, TValue>(this KeyValuePair<TKey, TValue> pair)
    {
        return default(KeyValuePair<TKey, TValue>).Equals(pair);
    }
    
    public static bool IsNotDefault<TKey, TValue>(this KeyValuePair<TKey, TValue> pair)
    {
        return !pair.IsDefault();
    }
}