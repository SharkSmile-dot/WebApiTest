using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiTest.Models
{
    public class OperationsContext : DbContext
    {
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Analytics> Analytics { get; set; }
        public DbSet<Contragent> Contragents { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Analytics1> Analytics1 { get; set; }
        public OperationsContext(DbContextOptions<OperationsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
