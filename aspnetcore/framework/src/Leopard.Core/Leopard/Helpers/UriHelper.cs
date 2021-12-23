﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    public static class UrlHelper
    {
        /// <summary>
        /// 判断一个 uri 是本机还是远程
        /// 参考：https://mp.weixin.qq.com/s/EdeCDNqQZStJk_kDew31Bg
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns></returns>
        public static bool IsLocal(Uri uri)
        {
            IPAddress[] host;
            IPAddress[] local;
            bool isLocal = false;

            host = Dns.GetHostAddresses(uri.Host);
            local = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress hostAddress in host)
            {
                if (IPAddress.IsLoopback(hostAddress))
                {
                    isLocal = true;
                    break;
                }
                else
                {
                    foreach (IPAddress localAddress in local)
                    {
                        if (hostAddress.Equals(localAddress))
                        {
                            isLocal = true;
                            break;
                        }
                    }

                    if (isLocal)
                    {
                        break;
                    }
                }
            }

            return isLocal;
        }
    }
}
