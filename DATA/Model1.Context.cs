﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DATA
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SafePlaceDbContext : DbContext
    {
        public SafePlaceDbContext()
            : base("name=SafePlaceDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TblFile> TblFile { get; set; }
        public virtual DbSet<TblFileType> TblFileType { get; set; }
        public virtual DbSet<TblPatient> TblPatient { get; set; }
        public virtual DbSet<TblRoom> TblRoom { get; set; }
        public virtual DbSet<TblSummary> TblSummary { get; set; }
        public virtual DbSet<TblTherapist> TblTherapist { get; set; }
        public virtual DbSet<TblTreatment> TblTreatment { get; set; }
        public virtual DbSet<TblTreats> TblTreats { get; set; }
        public virtual DbSet<TblType> TblType { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }
    }
}
