using Microsoft.EntityFrameworkCore;
using IPR_BE.Models;

namespace IPR_BE.DataAccess;

public class TestReportDbContext : DbContext {
    public TestReportDbContext() : base() { } 

    public TestReportDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Candidate> Candidates { get; set; }

    public DbSet<TestAttempt> TestAttempts { get; set; }

    public DbSet<InterviewBotLog> InterviewBotLogs { get; set; }

    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InterviewBotLog>((entity) => {
            entity.Property<int>("logId")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");
                
            entity.HasKey("logId")
                .HasName("logId");

            entity.ToTable("InterviewBotLogs");
        });

        modelBuilder.Entity<TestAttempt>(entity => {
            entity.Property("attemptId")
                .ValueGeneratedNever()
                .HasColumnType("bigint");

            entity.Property(e => e.testId)
                .HasColumnName("testId")
                .HasColumnType("bigint")
                .IsRequired();

            entity.Property(e => e.status)
                .HasColumnType("varchar(20)");

            entity.HasKey("attemptId")
                .HasName("attemptId");

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
                .HasColumnType("varchar(255)")
                .IsRequired(); 

            entity.Property(e => e.email)
                .HasColumnName("candidateEmail")
                .HasColumnType("varchar(255)")
                .IsRequired();

            entity.Property(e => e.currentRole)
                .HasColumnName("currentRole")
                .HasColumnType("varchar(255)");

            entity.Property(e => e.yearsExperience)
                .HasColumnName("yearsExperience")
                .HasColumnType("integer")
                .HasDefaultValue(0);

            entity.ToTable("Candidates");
        });

        modelBuilder.Entity<Skill>(entity => {
            entity.HasKey(e => e.id);

            entity.Property(e => e.name);
        });
    }
}