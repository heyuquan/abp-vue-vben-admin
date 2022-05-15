using System;

namespace Leopard.Threading;

/// <summary>
/// 确保 action 只执行一次。
/// OneTimeRunner通常作为类的 私有、静态、只读 对象实例，确保在类的声明周期中，某个操作只执行一次
/// private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();
/// </summary>
public class OneTimeRunner
{
    private volatile bool _runBefore;

    public void Run(Action action)
    {
        if (_runBefore)
        {
            return;
        }

        lock (this)
        {
            if (_runBefore)
            {
                return;
            }

            action();

            _runBefore = true;
        }
    }
}
