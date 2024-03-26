namespace CredECard.Common.BusinessService
{
    using System;

    /// <author>Keyur</author>
    /// <created>20-Jun-2017</created>
    /// <summary>
    /// Implement this interface to support Caching
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICacheable<T>
    {
        string CacheName {get;}
        T CacheValue{get;}
    }
}
