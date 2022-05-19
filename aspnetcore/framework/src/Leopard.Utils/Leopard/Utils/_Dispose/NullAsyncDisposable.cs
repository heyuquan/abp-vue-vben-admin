using System;
using System.Threading.Tasks;

namespace Leopard.Utils;

/// <summary>
/// 场景：方法需要返回null的Dispose时
/// </summary>
public sealed class NullAsyncDisposable : IAsyncDisposable
{
    public static NullAsyncDisposable Instance { get; } = new NullAsyncDisposable();

    private NullAsyncDisposable()
    {

    }

    public ValueTask DisposeAsync()
    {
        return default;
    }
}
