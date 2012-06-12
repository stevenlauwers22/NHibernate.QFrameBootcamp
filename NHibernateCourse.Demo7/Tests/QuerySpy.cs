using System;
using System.Linq;
using NUnit.Framework;

namespace NHibernateCourse.Demo7.Tests
{
    public static class QuerySpyExtension
    {
        public static QuerySpy Queries(this int expectedNumber)
        {
            return new QuerySpy(expectedNumber);
        }
    }

    public class QuerySpy : IDisposable
    {
        private readonly int _expectedNumber;
        private readonly LogSpyForNHibernateSql _spy;

        public QuerySpy(int expectedNumber)
        {
            _expectedNumber = expectedNumber;
            _spy = new LogSpyForNHibernateSql();
        }

        public void Dispose()
        {
            Assert.AreEqual(_expectedNumber, _spy.Appender.GetEvents().Count(), string.Format("Too many queries, expected {0} but was  {1}: ", _expectedNumber, _spy.Appender.GetEvents().Count()) + Environment.NewLine + _spy.GetWholeLog());
        }
    }
}