namespace Webhooks.Core;

public record PayloadPredicate(string Selector, string ExpectedValue)
{
	public string Key => Selector;
	public string Value => ExpectedValue;
}

public record PayloadFilter(string Key, string Value) : PayloadPredicate(Key, Value)
{
	public new string Selector => Key;
	public new string ExpectedValue => Value;
}