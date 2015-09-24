using NUnit.Framework;
using Bloop.Core.Exception;
using Bloop.Core.Plugin;

namespace Bloop.Test.Plugins
{

    [TestFixture]
    public class PluginInitTest
    {
        [Test]
        public void PublicAPIIsNullTest()
        {
            Assert.Throws(typeof(BloopCritialException), () => PluginManager.Init(null));
        }
    }
}
