using Leopard.Utils;
using System;

namespace Microsoft.Extensions.ObjectPool
{
    public static class ObjectPoolExtensions
    {
        public static IDisposable Get<T>(this ObjectPool<T> pool, out T pooledObject)
            where T : class
        {
            var rented = pool.Get();
            pooledObject = rented;
            return new ActionDisposable(() => pool.Return(rented));
        }
    }
}
