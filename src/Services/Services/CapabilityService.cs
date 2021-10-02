using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace Presentation.Services
{
    internal class CapabilityService
    {
        [ImportMany]
        public IEnumerable<ICapability> Capabilities { get; set; }

        private void Compose()
        {
            CompositionHost container;
            var executableLocation = Assembly.GetEntryAssembly().Location;
            var path = Path.Combine(Path.GetDirectoryName(executableLocation), "Plugins");

            var configuration = new ContainerConfiguration();
            var files = Directory.EnumerateFiles(path, "*Service.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    var asm = Assembly.LoadFrom(file);
                    configuration.WithAssembly(asm);
                }
                catch (ReflectionTypeLoadException)
                {
                }
                catch (BadImageFormatException)
                {
                }
            }

            using (container = configuration.CreateContainer())
            {
                Capabilities = container.GetExports<ICapability>();
            }
        }

        public void Run()
        {
            Compose();
        }
    }
}