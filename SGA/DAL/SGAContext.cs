using SGA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SGA.DAL
{
    public class SGAContext : DbContext
    {
        public SGAContext() : base("SGAContext")
        {
             Database.SetInitializer<SGAContext>(new DropCreateDatabaseAlways<SGAContext>());
         //  Database.SetInitializer<SGAContext>(new DropCreateDatabaseIfModelChanges<SGAContext>());
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Estudiante> Estudiantes { set; get; }
        public DbSet<Matricula> Matriculas { set; get; }
        public DbSet<Curso> Cursos { set; get; }
        public DbSet<Titulo> Titulos { set; get; }
        public DbSet<Tutor> Tutores { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Curso>().HasMany(c => c.Tutores).WithMany(t => t.Cursos)
                .Map(i => i.MapLeftKey("CursoID")
                .MapRightKey("TutorID").ToTable("CursoTutor"));
        }
        public static SGAContext Create()
        {
            return new SGAContext();
        }

        public System.Data.Entity.DbSet<SGA.Models.Administrador> Administradors { get; set; }

        public System.Data.Entity.DbSet<SGA.Models.Cliente> Clientes { get; set; }

        public System.Data.Entity.DbSet<SGA.Models.Generacion> Generacions { get; set; }

        public System.Data.Entity.DbSet<SGA.Models.EstudianteParaFactura> EstudianteParaFacturas { get; set; }

        public System.Data.Entity.DbSet<SGA.Models.Factura> Facturas { get; set; }
        public System.Data.Entity.DbSet<SGA.Models.Nota> Notas { get; set; }

    }


}