using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;
using NHibernateCourse.Demo7.Infrastructure;
using NHibernateCourse.Demo7.Infrastructure.Transactions;

namespace NHibernateCourse.Demo7.Domain.Handlers
{
    public interface IGetCustomersFromBelgiumWithTotalOrderPrice : IQueryList<GetCustomersFromBelgiumWithTotalOrderPriceResult>
    {
    }

    public class GetCustomersFromBelgiumWithTotalOrderPriceResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal TotalOrderPrice { get; set; }
    }

    [Transaction(false)]
    public class GetCustomersFromBelgiumWithTotalOrderPrice : IGetCustomersFromBelgiumWithTotalOrderPrice
    {
        private readonly ISession _session;

        public GetCustomersFromBelgiumWithTotalOrderPrice(ISession session)
        {
            _session = session;
        }

        public IEnumerable<GetCustomersFromBelgiumWithTotalOrderPriceResult> List()
        {
            GetCustomersFromBelgiumWithTotalOrderPriceResult resultAlias = null;
            Order orderAlias = null;

            var query = _session.QueryOver<Customer>();
            query.Where(c => c.BillingAddress.Country == "Belgie");
            query.Left.JoinAlias(c => c.Orders, () => orderAlias);
            query.SelectList(list => list
                    .SelectGroup(c => c.Id).WithAlias(() => resultAlias.Id)
                    .SelectGroup(c => c.Name).WithAlias(() => resultAlias.Name)
                    .SelectSum(() => orderAlias.Price).WithAlias(() => resultAlias.TotalOrderPrice));
            query.TransformUsing(Transformers.AliasToBean<GetCustomersFromBelgiumWithTotalOrderPriceResult>());

            return query.List<GetCustomersFromBelgiumWithTotalOrderPriceResult>();
        }

    }
}