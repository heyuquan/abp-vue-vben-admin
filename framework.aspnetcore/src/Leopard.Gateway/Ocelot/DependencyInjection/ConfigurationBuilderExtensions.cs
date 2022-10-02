namespace Ocelot.DependencyInjection
{
    using Ocelot.Configuration.File;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Memory;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddOcelot(this IConfigurationBuilder builder, string environmentName)
        {
            return builder.AddOcelot(".", environmentName);
        }

        public static IConfigurationBuilder AddOcelot(this IConfigurationBuilder builder, string folder, string environmentName)
        {
            const string primaryConfigFile = "ocelot.json";

            const string globalConfigFile = "ocelot.global.json";

            const string subConfigPattern = @"^ocelot\.(.*?)\.json$";

            string excludeConfigName = !string.IsNullOrEmpty(environmentName) ? $"ocelot.{environmentName}.json" : string.Empty;

            var reg = new Regex(subConfigPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var files = new DirectoryInfo(folder)
                .EnumerateFiles()
                .Where(fi => reg.IsMatch(fi.Name) && (fi.Name != excludeConfigName))
                .ToList();

            var fileConfiguration = new FileConfiguration();

            foreach (var file in files)
            {
                if (files.Count > 1 && file.Name.Equals(primaryConfigFile, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var lines = File.ReadAllText(file.FullName);

                var config = JsonConvert.DeserializeObject<FileConfiguration>(lines);

                if (file.Name.Equals(globalConfigFile, StringComparison.OrdinalIgnoreCase))
                {
                    fileConfiguration.GlobalConfiguration = config.GlobalConfiguration;
                }

                fileConfiguration.Aggregates.AddRange(config.Aggregates);
                fileConfiguration.Routes.AddRange(config.Routes);
            }

            var json = JsonConvert.SerializeObject(fileConfiguration);

            File.WriteAllText(primaryConfigFile, json);

            builder.AddJsonFile(primaryConfigFile, false, false);

            return builder;
        }
    }
}
