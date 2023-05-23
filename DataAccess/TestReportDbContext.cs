using Microsoft.EntityFrameworkCore;
using IPR_BE.Models.TestReport;

namespace IPR_BE.DataAccess;


public class TestReportDbContext : DbContext {
    public TestReportDbContext() : base() { }

    public TestReportDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Candidate> Candidates { get; set; }

    public DbSet<TestAttempt> TestAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestAttempt>(entity => {
            entity.HasKey(e => e.attemptId)
                .HasName("attemptId");

            entity.Property(e => e.testId)
                .HasColumnName("testId")
                .HasColumnType("bigint")
                .IsRequired();

            entity.Property(e => e.status)
                .HasColumnType("varchar(20)");

            entity.HasOne<Candidate>()
                .WithMany()
                .HasForeignKey(p => p.candidateId);
    
            entity.ToTable("TestAttempts");
        });

        modelBuilder.Entity<Candidate>(entity => {
            entity.HasKey(e => e.id)
                .HasName("id");

            entity.Property(e => e.name)
                .HasColumnName("candidateName")
                .HasColumnType("varchar(255)");
            
            entity.Property(e => e.email)
                .HasColumnName("candidateEmail")
                .HasColumnType("varchar(255)");

            entity.ToTable("Candidates");
        });
    }
}