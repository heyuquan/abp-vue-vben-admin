﻿using Leopard.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard
{
    public static class App
    {
        public static void Init(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 私有设置，避免重复解析
        /// </summary>
        internal static AppSettingsOptions _settings;

        /// <summary>
        /// 应用全局配置
        /// </summary>
        public static AppSettingsOptions Settings => _settings ??= Configuration.GetSection(AppSettingsOptions.SectionName).Get<AppSettingsOptions>();

        /// <summary>
        /// 全局配置选项
        /// </summary>
        public static IConfiguration Configuration { get; private set; }
    }
}
