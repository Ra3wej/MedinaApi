using Microsoft.EntityFrameworkCore;
using MedinaApi.Models;

namespace MedinaApi.Data
{
    public class Medina_Api_DbContext : DbContext

    {
        public Medina_Api_DbContext()
        {

        }
        public Medina_Api_DbContext(DbContextOptions<Medina_Api_DbContext> options) : base(options)
        {

        }
        public virtual DbSet<LanguageSet> LanguageSets { get; set; }
        //public virtual DbSet<ProductTags> ProductTags { get; set; }
        //public DbSet<FirebaseId> FirebaseId { get; set; }
        public virtual DbSet<DashboardUser> DashboardUsers { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Doctors> Doctors { get; set; }
        public DbSet<MedinaApi.Models.ChronicDiases> ChronicDiases { get; set; } = default!;
        public DbSet<MedinaApi.Models.Hospital> Hospital { get; set; } = default!;
        public DbSet<MedinaApi.Models.PatientDoctorVisits> PatientDoctorVisits { get; set; } = default!;
        public DbSet<MedinaApi.Models.PatientChronicDiases> PatientChronicDiases { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LanguageSet>(op =>
            {
                op.HasIndex(p => p.English);
                op.HasIndex(p => p.Arabic);
                op.HasIndex(p => p.Kurdish);
            });
            modelBuilder.Entity<Patient>(op =>
            {
                op.HasIndex(c => c.GuidKey).IsUnique();
                op.HasIndex(c => c.NationalCardId).IsUnique();
                op.HasIndex(c => c.PassportId).IsUnique();
            });
            modelBuilder.Entity<Doctors>(op =>
            {
                op.HasIndex(c => c.GuidKey).IsUnique();
                op.HasIndex(c => c.NationalCardId).IsUnique();
                op.HasIndex(c => c.PassportId).IsUnique();
            });
            ///
            /// 
            /// Temporal Tables
            /// 
            ///

            modelBuilder.Entity<LanguageSet>().ToTable(op =>
            {
                op.IsTemporal();
            });
            
            modelBuilder.Entity<Patient>().ToTable(op =>
            {
                op.IsTemporal();
            });

            modelBuilder.Entity<Doctors>().ToTable(op =>
            {
                op.IsTemporal();
            });
        }
        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var currentTime = DateTime.Now;

        //    foreach (var entry in ChangeTracker.Entries())
        //    {
        //        // Check if the entity has "CreatedAt" and "UpdatedAt" properties
        //        var createdAtProperty = entry.Metadata.FindProperty("CreatedAt");
        //        var updatedAtProperty = entry.Metadata.FindProperty("UpdatedAt");

        //        if (entry.State == EntityState.Added)
        //        {
        //            if (createdAtProperty != null && updatedAtProperty != null)
        //            {
        //                entry.Property(createdAtProperty.Name).CurrentValue = currentTime;
        //                entry.Property(updatedAtProperty.Name).CurrentValue = currentTime;
        //            }
        //        }
        //        else if (entry.State == EntityState.Modified)
        //        {
        //            if (updatedAtProperty != null)
        //            {
        //                entry.Property(updatedAtProperty.Name).CurrentValue = currentTime;
        //            }
        //        }
        //    }
        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }
}
