using AppCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace AppCrud.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        
        public DbSet<Empleado> Empleados { get; set; }
        
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Empleado>(tb =>
            {
                tb.HasKey(Col => Col.idEmpleado);
                tb.Property(col => col.idEmpleado)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();
                tb.Property(col => col.Nombre).IsRequired().HasMaxLength(100);
                tb.Property(col => col.correo).IsRequired().HasMaxLength(50);
                tb.Property(col => col.FechaContrato).IsRequired();
                tb.Property(col => col.Clave).IsRequired().HasMaxLength(250);
                tb.Property(col => col.Activo).IsRequired();
            });
            modelBuilder.Entity<Empleado>().ToTable("Empleados");
        }
    }
}