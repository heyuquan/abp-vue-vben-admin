using System;
using System.Threading;
using System.Threading.Tasks;

namespace Leopard.Threading;

/// <summary>
/// 确保 action 只执行一次。
/// AsyncOneTimeRunner 通常作为类的 私有、静态、只读 对象实例，确保在类的声明周期中，某个操作只执行一次
/// private static readonly AsyncOneTimeRunner asyncOneTimeRunner = new AsyncOneTimeRunner();
/// </summary>
public class AsyncOneTimeRunner
{
    private volatile bool _runBefore;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public async Task RunAsync(Func<Task> action)
    {
        if (_runBefore)
        {
            return;
        }

        using (await _semaphore.LockAsync())
        {
            if (_runBefore)
            {
                return;
            }

            await action();

            _runBefore = true;
        }
    }
}
