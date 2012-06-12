using System;
using System.Linq;
using Castle.DynamicProxy;

namespace NHibernateCourse.Demo7.Infrastructure.Transactions
{
    public class TransactionInterceptor : IInterceptor
    {
        private readonly ISessionProvider _sessionProvider;

        public TransactionInterceptor(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            // Get the transaction settings
            var transactionSettings = invocation.TargetType
                .GetCustomAttributes(typeof(TransactionAttribute), true)
                .OfType<TransactionAttribute>()
                .SingleOrDefault();

            // Check if transaction settings were defined
            if (transactionSettings == null)
            {
                invocation.Proceed();
                return;
            }

            // A transaction is required
            using (var session = _sessionProvider.GetSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    invocation.Proceed();

                    // Check if we need to commit the transaction
                    if (transactionSettings.AutoCommit)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }
    }
}