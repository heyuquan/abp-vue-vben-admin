using Leopard.Helpers.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    /// <summary>
    /// 系统相关
    /// </summary>
    public static class SystemHelper
    {
        /// <summary>
        /// 判断应用程序是否以管理员身份运行（支持windows、linux）
        /// </summary>
        public static bool IsAdministrator()
        {
            // 需要引用nuget包Mono.Posix.NETStandard
            // 参考：https://mp.weixin.qq.com/s/iFT43Kzuys5euHWL1swKsA
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                  new WindowsPrincipal(WindowsIdentity.GetCurrent())
                      .IsInRole(WindowsBuiltInRole.Administrator) :
                  Mono.Unix.Native.Syscall.geteuid() == 0;
        }

        /// <summary>
        /// 是否 linux 运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsLinuxRunTime()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            return isUnix;
        }

        /// <summary>
        /// 是否 windows 运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowRunTime()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        /// <summary>
        /// 电脑指标
        /// </summary>
        public class ComputerMetrics
        {
            public static ComputerMetricsInfo GetMetrics()
            {
                ComputerMetricsInfo computerInfo = new ComputerMetricsInfo();
                try
                {
                    MemoryMetrics client = new MemoryMetrics();
                    MemoryMetricsInfo memoryMetrics = client.GetMetrics();
                    computerInfo.TotalRAM = Math.Ceiling(memoryMetrics.Total / 1024).ToString() + " GB";
                    computerInfo.RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total).ToString() + " %";
                    computerInfo.CPURate = Math.Ceiling(GetCPURate().CastTo<double>()) + " %";
                    computerInfo.RunTime = GetRunTime();
                }
                catch (Exception ex)
                {
                    ex.ReThrow2();
                }
                return computerInfo;
            }

            public static string GetCPURate()
            {
                string cpuRate = string.Empty;
                if (IsLinuxRunTime())
                {
                    string output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                    cpuRate = output.Trim();
                }
                else
                {
                    string output = ShellHelper.Cmd("wmic", "cpu get LoadPercentage");
                    cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
                }
                return cpuRate;
            }

            public static string GetRunTime()
            {
                string runTime = string.Empty;
                try
                {
                    if (IsLinuxRunTime())
                    {
                        string output = ShellHelper.Bash("uptime -s");
                        output = output.Trim();
                        runTime = TimeHelper.FormatTime((DateTime.Now - output.CastTo<DateTime>()).TotalMilliseconds.ToString().Split('.')[0].CastTo<long>());
                    }
                    else
                    {
                        string output = ShellHelper.Cmd("wmic", "OS get LastBootUpTime/Value");
                        string[] outputArr = output.Split("=", StringSplitOptions.RemoveEmptyEntries);
                        if (outputArr.Length == 2)
                        {
                            runTime = TimeHelper.FormatTime((DateTime.Now - outputArr[1].Split('.')[0].CastTo<DateTime>()).TotalMilliseconds.ToString().Split('.')[0].CastTo<long>());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ReThrow2();
                }
                return runTime;
            }
        }

        /// <summary>
        /// 内存指标
        /// </summary>
        public class MemoryMetrics
        {
            public MemoryMetricsInfo GetMetrics()
            {
                if (IsLinuxRunTime())
                {
                    return GetUnixMetrics();
                }
                return GetWindowsMetrics();
            }

            private MemoryMetricsInfo GetWindowsMetrics()
            {
                string output = ShellHelper.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");

                var lines = output.Trim().Split("\n");
                var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
                var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

                var metrics = new MemoryMetricsInfo();
                metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
                metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
                metrics.Used = metrics.Total - metrics.Free;

                return metrics;
            }

            private MemoryMetricsInfo GetUnixMetrics()
            {
                string output = ShellHelper.Bash("free -m");

                var lines = output.Split("\n");
                var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var metrics = new MemoryMetricsInfo();
                metrics.Total = double.Parse(memory[1]);
                metrics.Used = double.Parse(memory[2]);
                metrics.Free = double.Parse(memory[3]);

                return metrics;
            }
        }
    }

    /// <summary>
    /// 内存指标信息
    /// </summary>
    public class MemoryMetricsInfo
    {
        /// <summary>
        /// 总内存
        /// </summary>
        public double Total { get; set; }
        /// <summary>
        /// 已使用内存
        /// </summary>
        public double Used { get; set; }
        /// <summary>
        /// 空闲内存
        /// </summary>
        public double Free { get; set; }
    }

    /// <summary>
    /// 电脑指标信息
    /// </summary>
    public class ComputerMetricsInfo
    {
        /// <summary>
        /// CPU使用率
        /// </summary>
        public string CPURate { get; set; }
        /// <summary>
        /// 总内存
        /// </summary>
        public string TotalRAM { get; set; }
        /// <summary>
        /// 内存使用率
        /// </summary>
        public string RAMRate { get; set; }
        /// <summary>
        /// 系统运行时间
        /// </summary>
        public string RunTime { get; set; }
    }
}

