using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using Kontur.ImageTransformer;

namespace ImageTransformerTests
{
    [TestFixture]
    public class UrlParserTests
    {

        [Test]
        public void ShouldThrow_BadUrl()
        {
            try
            {
                UrlParser.ParseUrl("/hello/world");
                Assert.Fail();
            }
            catch (InvalidOperationException e)
            {

            }
        }

        [Test]
        public void ShouldThrow_BadCoords()
        {
            try
            {
                UrlParser.ParseUrl("/process/grayscale/bla,bla,bla");
                Assert.Fail();
            }
            catch (InvalidOperationException e)
            {

            }
        }

        [Test]
        public void ShouldThrow_BadFilter()
        {
            try
            {
                UrlParser.ParseUrl("/process/filterfilter/1,2,3,4");
                Assert.Fail();
            }
            catch (InvalidOperationException e)
            {

            }
        }

        [Test]
        public void ShouldParse_GoodUrl()
        {
            var requestParams = UrlParser.ParseUrl("/process/grayscale/1,1,2,2");

            Assert.That(requestParams.Filter.FilterType, Is.EqualTo(FilterType.Grayscale));
        }

        [Test]
        public void ShouldParse_GoodUrlThreshold()
        {
            var requestParams = UrlParser.ParseUrl("/process/threshold(20)/1,1,2,2");

            Assert.That(requestParams.Filter.FilterType, Is.EqualTo(FilterType.Threshold));
        }

        [Test]
        public void ShouldNotParse_BadParamThreshold()
        {
            try
            {
                UrlParser.ParseUrl("/process/threshold(-20)/1,1,2,2");
                Assert.Fail();
            }
            catch (InvalidOperationException e)
            {

            }
        }

        [Test]
        public void ShouldParseCorrectly_ThresholdParam()
        {
            var requestParams = UrlParser.ParseUrl("/process/threshold(20)/1,1,2,2");

            Assert.That(requestParams.Filter.FilterParam, Is.EqualTo(20));
        }

    }
}