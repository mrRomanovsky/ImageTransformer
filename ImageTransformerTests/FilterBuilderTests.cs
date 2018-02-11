using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Drawing;
using System.Reflection;
using Kontur.ImageTransformer;

namespace ImageTransformerTests
{
    [TestFixture]
    class FilterBuilderTests
    {
        private Bitmap bitMap = new Bitmap(50, 50);

        [Test]
        public void ShouldTruncate_DownRight()
        {
            var res = FilterBuilder.TruncateBitmap(bitMap, 1, 1, 20, 20);
            
            Assert.That(res.Width == 20 && res.Height == 20);
        }

        [Test]
        public void ShouldTruncate_DownLeft()
        {
            var res = FilterBuilder.TruncateBitmap(bitMap, 15, 15, -5, 20);

            Assert.That(res.Width == 5 && res.Height == 20);
        }

        [Test]
        public void ShouldTruncate_UpRight()
        {
            var res = FilterBuilder.TruncateBitmap(bitMap, 15, 15, 20, -5);

            Assert.That(res.Width == 20 && res.Height == 5);
        }

        [Test]
        public void ShouldTruncate_UpLeft()
        {
            var res = FilterBuilder.TruncateBitmap(bitMap, 15, 15, -7, -5);

            Assert.That(res.Width == 7 && res.Height == 5);
        }

        [Test]
        public void ShouldThrow_BadCoords()
        {
            try
            {
                var res = FilterBuilder.TruncateBitmap(bitMap, -1, -1, -7, -5);
                Assert.Fail();
            }
            catch (Exception e)
            {
                
            }
        }

        [Test]
        public void ShouldTruncate_BigCoords()
        {
            var res = FilterBuilder.TruncateBitmap(bitMap, 0, 0, 500, 500);
            Assert.That(res.Width == 50 && res.Height == 50);
        }
    }
}
