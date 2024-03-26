using System.IO;
using System.Reflection;

namespace Kontur.BigLibrary.Tests.Integration.Extensions
{
    public static class ResourceExtentions
    {
        public static string ReadResource(this Assembly resourceAssembly, string resourceName)
        {
            using (var stream = resourceAssembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}