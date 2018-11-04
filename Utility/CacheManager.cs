using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class CacheManager
    {
        
        private static bool _useMemcache = ConfigurationManager.AppSettings["UseMemcache"] == "true" ? true : false;

        public static T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> callback)
        {
            T t = default(T);

            try
            {
                if (_useMemcache)
                {
                    using (MemcachedClient _client = new MemcachedClient())
                    {
                        t = (T)_client.Get(key);
                        if (t == null)
                        {
                            lock (key)
                            {
                                t = (T)_client.Get(key);
                                if (t == null)
                                {
                                    t = callback();
                                    if (t != null)
                                    {
                                        _client.Store(Enyim.Caching.Memcached.StoreMode.Add, key, t, cacheDuration);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return callback();
                }
            }
            catch (Exception )
            {
                
            }

            return t;
        }

        public static bool ExpireCache(string key)
        {
            try
            {
                if (_useMemcache)
                {
                    lock (key)
                    {
                        using (MemcachedClient _client = new MemcachedClient())
                        {
                            return _client.Remove(key);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
    
}
