using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using estacionamento.Models;

namespace estacionamento.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<estacionamento.Models.SaidaVeiculoEstacionado> VeiculoEstacionado { get; set; }
    }
}
