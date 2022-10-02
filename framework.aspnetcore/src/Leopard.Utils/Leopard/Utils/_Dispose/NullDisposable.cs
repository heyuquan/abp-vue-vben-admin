using System;

namespace Leopard.Utils;

/// <summary>
/// ������������Ҫ����null��Disposeʱ
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
