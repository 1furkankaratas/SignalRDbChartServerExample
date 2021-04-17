using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using SignalRDbChartServerExample.Hubs;
using SignalRDbChartServerExample.Models;
using TableDependency.SqlClient;

namespace SignalRDbChartServerExample.Subscription
{
    public interface IDatabaseSubscription
    {
        void Configure(string tableName);
    }

    public class DatabaseSubscription<T> : IDatabaseSubscription where T : class, new()
    {
        private readonly IConfiguration _configuration;
        private IHubContext<SaleHub> _hubContext;

        SqlTableDependency<T> _tableDependency;

        public DatabaseSubscription(IConfiguration configuration, IHubContext<SaleHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;
        }

        public void Configure(string tableName)
        {
            _tableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("Sql"),tableName);
            _tableDependency.OnChanged += async (o, e) =>
            {
                

                SignalRDbChartExampleContext context = new SignalRDbChartExampleContext();

                var data = (from Person in context.Persons
                    join sale in context.Sales on Person.Id equals sale.PersonId
                    select new { Person, sale }).ToList();

                List<object> datas = new List<object>();

                var personsName = data.Select(d => d.Person.Name  +" "+ d.Person.Surname).Distinct().ToList();

                foreach (var p in personsName)
                {
                    datas.Add(new
                    {
                        name = p,
                        data = data.Where(x => x.Person.Name + " " +x.Person.Surname == p).Select(s => s.sale.Price).ToList()
                    });
                }

                await _hubContext.Clients.All.SendAsync("receiveMessage", datas);
            };

            _tableDependency.OnError += (o, e) =>
            {

            };

            _tableDependency.Start();
        }

        ~DatabaseSubscription()
        {
            _tableDependency.Stop();
        }
    }
}