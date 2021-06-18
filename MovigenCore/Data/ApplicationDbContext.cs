using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovigenCore.Models;

namespace MovigenCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Empresa> Empresa { get; set; }        
        public DbSet<Prodmae> Prodmae { get; set; }
        public DbSet<Paramet> Paramet { get; set; }
        public DbSet<Cliemae> Cliemae { get; set; }
        public DbSet<Conmae> Conmae { get; set; }

        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Detalle> Detalle { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        
      
        
    }
}
