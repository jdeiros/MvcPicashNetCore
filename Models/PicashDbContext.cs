using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MvcPicashNetCore.Models;

namespace MvcPicashNetCore.Models
{
    public class PicashDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<DebtCollector> DebtCollectors { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<CollectionWeek> CollectionWeeks { get; set; }
        public DbSet<Holyday> Holydays { get; set; }

        public PicashDbContext(DbContextOptions<PicashDbContext> options): base(options)
        {
        }

        //Siembra de datos:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<Zone> zones = new List<Zone>();
            zones.Add(
             new Zone{ 
                ZoneId = Guid.NewGuid().ToString(),
                Code = "TOR",
                Name = "Tortuguitas, Malvinas Argentinas, Bs. As",
             });
            zones.Add(
             new Zone{ 
                ZoneId = Guid.NewGuid().ToString(),
                Code = "GAR",
                Name = "Garin, Escobar, Bs. As",
             });
            List<DebtCollector> debtCollectors = new List<DebtCollector>();
            debtCollectors.Add(
                new DebtCollector{
                                    Birthdate = Convert.ToDateTime("12/4/1980 12:10:15 PM", new CultureInfo("en-US")),
                                    DebtCollectorId = Guid.NewGuid().ToString(),
                                    CellPhone = "115521-3345",
                                    Name = "Martín",
                                    SurName = "Gomez",
                                    OptionalContact = "martin@outlook.com"
                                    
                                }
            );
            debtCollectors.Add(
                new DebtCollector{
                                    Birthdate = Convert.ToDateTime("12/12/1987 12:10:15 PM", new CultureInfo("en-US")),
                                    DebtCollectorId = Guid.NewGuid().ToString(),
                                    CellPhone = "115521-2555",
                                    Name = "Gabriel",
                                    SurName = "Rojo",
                                    OptionalContact = "Gabriel@rojo.com"
                                }
            );
            

            var collectionWeek = new CollectionWeek()
            {
                CollectionWeekId = Guid.NewGuid().ToString(),
                Code = "LuSa",
                Description = "Lunes a Sabados, sin feriados",
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Saturday = true,
                Sunday = false,
                Holiday = false
            };
            
            var holiday = new Holyday()
            {
                HolydayId = Guid.NewGuid().ToString(),
                Description = "Día del Trabajador",
                Date = Convert.ToDateTime("5/1/2019", new CultureInfo("en-US"))
            };

            List<LoanType> loanTypes = LoadLoanTypes(collectionWeek);
            var route = new Route() { 
                                        RouteId = Guid.NewGuid().ToString(), 
                                        DebtCollectorId = debtCollectors.FirstOrDefault().DebtCollectorId, 
                                        Code = "ClientesA", 
                                        Name = "Lista Clientes A" 
                                    };
            
            List<Customer> customers = GenerateRandomCustomers(route, 50);
            List<Address> addresses = LoadAddresses(customers);



            modelBuilder.Entity<CollectionWeek>().HasData(collectionWeek);
            modelBuilder.Entity<Holyday>().HasData(holiday);

            modelBuilder.Entity<Zone>().HasData(zones.ToArray());
            modelBuilder.Entity<DebtCollector>().HasData(debtCollectors.ToArray());

            modelBuilder.Entity<Route>().HasData(route);
            modelBuilder.Entity<Customer>().HasData(customers.ToArray());
            modelBuilder.Entity<Address>().HasData(addresses.ToArray());
            modelBuilder.Entity<LoanType>().HasData(loanTypes.ToArray());
        }

        private static List<LoanType> LoadLoanTypes(CollectionWeek collectionWeek)
        {
            List<LoanType> loanTypes = new List<LoanType>();
            loanTypes.Add(
               new LoanType()
               {
                   LoanTypeId = Guid.NewGuid().ToString(),
                   Code = "26D40%",
                   Description = "26 Cuotas diarias, 40% de interés total",
                   InstallmentsAmount = 26,
                   InterestPercentage = 40,
                   CollectionWeekId = collectionWeek.CollectionWeekId
               });
            loanTypes.Add(
                new LoanType()
                {
                    LoanTypeId = Guid.NewGuid().ToString(),
                    Code = "20D30%",
                    Description = "20 Cuotas diarias, 30% de interés total",
                    InstallmentsAmount = 20,
                    InterestPercentage = 30,
                    CollectionWeekId = collectionWeek.CollectionWeekId
                });
            return loanTypes;
        }

        private List<Address> LoadAddresses(List<Customer> customers)
        {
            string[] street = { "Directorio", "Moreno", "Av. Olivos", "Venezuela", "Misiones", "Luis María Drago", "Albert Einstein" };
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
            
            Random rnd = new Random();
            
            var customerList = from n1 in name1
                               from n2 in name2
                               from sn in surname
                               select new Customer
                               {
                                   RouteId = route.RouteId,
                                   Name = $"{n1} {n2}",
                                   SurName = $"{sn}",
                                   CustomerId = Guid.NewGuid().ToString(),
                                   Birthdate = DateTime.Now.AddYears(-rnd.Next(21, 75)),
                                   CellPhone = "+54 9 11 5521 "+ rnd.Next(0,9999),
                                   OptionalContact = $"{n1}.{n2}"+ "@gmail.com"
                               };

            return customerList.OrderBy((cus) => cus.CustomerId).Take(cant).ToList();
        }
    }
}