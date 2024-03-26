using System.Threading.Tasks;
using Kontur.BigLibrary.Tests.Core.Helpers;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Integration
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public async Task Setup()
        {
            await DbHelper.CreateDbAsync();
        }
    }
}