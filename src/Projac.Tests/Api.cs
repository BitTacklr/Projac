
using System.IO;
using NUnit.Framework;
using PublicApiGenerator;

namespace Projac.Tests
{
    [TestFixture]
    public class Api
    {
        [Test]
        public void WriteLatestVersion()
        {
            var assembly = typeof(Resolve).Assembly;
            var report = ApiGenerator.GeneratePublicApi(assembly);
            var path = 
                ".." + Path.DirectorySeparatorChar + 
                ".." + Path.DirectorySeparatorChar + 
                ".." + Path.DirectorySeparatorChar + 
                ".." + Path.DirectorySeparatorChar + 
                "Versions" + Path.DirectorySeparatorChar +
                "latest.txt";
            File.WriteAllText(path, report);
        }
    }
}