﻿namespace S2Cognition.Integrations.Core.Data;

public interface IConfiguration
{
    IServiceProvider IoC { get; }
}

public class Configuration : IConfiguration
{
    public const int DefaultPageSize = 25;

    public IServiceProvider IoC { get; private set; }

    protected Configuration(IServiceProvider ioc)
    {
        IoC = ioc;
    }
}

public static class Search
{
    public static PartialSearchTerm Key(string key)
    {
        return new PartialSearchTerm(key);
    }
}

public class PartialSearchTerm
{
    private readonly string _key;

    internal PartialSearchTerm(string key)
    {
        _key = key;
    }

    public SearchTerm HasValue<T>()
    {
        return new SearchTerm(typeof(T), _key);
    }
}

public class SearchTerm
{
    private readonly Type _datatype;
    public Type DataType => _datatype;

    private readonly string _key;
    public string Key => _key;

    internal SearchTerm(Type datatype, string key)
    {
        _datatype = datatype;
        _key = key;
    }
}
