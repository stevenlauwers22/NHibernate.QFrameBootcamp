using System;

namespace NHibernateCourse.Demo7.Infrastructure.Transactions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TransactionAttribute : Attribute
    {
        private readonly bool _autoCommit;

        public TransactionAttribute(bool autoCommit)
        {
            _autoCommit = autoCommit;
        }

        public bool AutoCommit
        {
            get { return _autoCommit; }
        }
    }
}