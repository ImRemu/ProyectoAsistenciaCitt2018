﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp_MVC.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class satcEntities : DbContext
    {
        public satcEntities()
            : base("name=satcEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<asistencia> asistencia { get; set; }
        public DbSet<det_asist> det_asist { get; set; }
        public DbSet<horario> horario { get; set; }
        public DbSet<log_acciones> log_acciones { get; set; }
        public DbSet<taller> taller { get; set; }
        public DbSet<usuario> usuario { get; set; }
    }
}