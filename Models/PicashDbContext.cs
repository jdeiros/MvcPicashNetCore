using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MvcPicashNetCore.Models
{
    public class PicashDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DebtCollector> DebtCollectors { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<PaymentCommitment> PaymentCommitments { get; set; }
        public DbSet<Route> Routes { get; set; }

        public PicashDbContext(DbContextOptions<PicashDbContext> options): base(options)
        {
        }

        //Siembra de datos:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var debtCollector = new DebtCollector
            {
                Birthdate = Convert.ToDateTime("12/4/1980 12:10:15 PM", new CultureInfo("en-US")),
                DebtCollectorId = Guid.NewGuid().ToString(),
                CellPhone = "+54 9 11 5521 3345",
                Name = "Juan",
                SurName = "Perez",
                OptionalContact = "juanperez@perezcompany.com"
            };

            var route = new Route() {RouteId = Guid.NewGuid().ToString(), DebtCollectorId = debtCollector.DebtCollectorId, Code = "RAM"};
            List<Customer> customers = GenerateRandomCustomers(route, 50);
            List<Address> addresses = LoadAddresses(customers);

            modelBuilder.Entity<DebtCollector>().HasData(debtCollector);
            modelBuilder.Entity<Route>().HasData(route);
            modelBuilder.Entity<Customer>().HasData(customers.ToArray());
            modelBuilder.Entity<Address>().HasData(addresses.ToArray());
        }

        private List<Address> LoadAddresses(List<Customer> customers)
        {
             string[] street = { "Rosales", "Agulleiro", "Beiro", "Cuenca", "Virasoro", "M. T. de Alvear", "Bianco" };
            string[] number = { "1243", "666", "895", "397", "1236", "1789", "2765" };

            Random rnd = new Random();

            List<Address> completeList = new List<Address>();
            foreach (Customer cus in customers)
            {
                int rndIndex1 = rnd.Next(0, 6);
                int rndIndex2 = rnd.Next(0, 6);
                completeList.Add(
                    new Address()
                    {
                        AddressId = Guid.NewGuid().ToString(),
                        IsMain = true,
                        Description = $"{street[rndIndex1]} {number[rndIndex2]}",
                        CustomerId = cus.CustomerId
                    }
                );
            }
            return completeList;
        }

        private List<Customer> GenerateRandomCustomers(Route route, int cant)
        {
            string[] name1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] surname = { "Ruiz", "Sarmiento", "Uribe", "Sosa", "Pérez", "Toledo", "Herrera" };
            string[] name2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };


            var customerList = from n1 in name1
                               from n2 in name2
                               from sn in surname
                               select new Customer
                               {
                                   RouteId = route.RouteId,
                                   Name = $"{n1} {n2}",
                                   SurName = $"{sn}",
                                   CustomerId = Guid.NewGuid().ToString()
                               };

            return customerList.OrderBy((cus) => cus.CustomerId).Take(cant).ToList();
        }

    }
}