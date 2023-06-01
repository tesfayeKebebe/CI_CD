namespace Health.Mobile.Server.Models;

public class KeyValueOf<TKey, TValue>
{
    public TKey Key { get; }
    public TValue Value { get; }

    public KeyValueOf(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}