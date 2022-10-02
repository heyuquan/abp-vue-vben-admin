using System;

namespace Leopard.Utils;

/// <summary>
/// 场景：方法需要返回null的Dispose时
/// </summary>
public sealed class NullDisposable : IDisposable
{
    public static NullDisposable Instance { get; } = new NullDisposable();

    private NullDisposable()
    {

    }

    public void Dispose()
    {

    }
}
