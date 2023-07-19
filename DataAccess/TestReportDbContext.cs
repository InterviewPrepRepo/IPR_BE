using Microsoft.EntityFrameworkCore;
using IPR_BE.Models;

namespace IPR_BE.DataAccess;

public class TestReportDbContext : DbContext {
    public TestReportDbContext() : base() { } 

    public TestReportDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<TestAttemptSection> TestAttemptSections {get; set;} //in progress
    public DbSet<TestAttemptQuestionSection> TestAttemptQuestionSections {get; set;} //in progress
    // public DbSet<TestSectionQuestion> TestSectionQuestions {get; set;} //in progress
    // public DbSet<TestTag> TestTags {get; set;} //in progress
    public DbSet<Candidate> Candidates { get; set; } //done
    public DbSet<TestAttempt> TestAttempts { get; set; } //in progress
    public DbSet<InterviewBotLog> InterviewBotLogs { get; set; } //done
    public DbSet<TestSection> TestSections { get; set; } //in progress
    public DbSet<Test> Tests { get; set; } //in progress
    public DbSet<Tag> Tags { get; set; } //in progress
    public DbSet<QuestionType> QuestionTypes { get; set; } //in progress
    public DbSet<Question> Questions { get; set; } //in progress
    public DbSet<Skill> Skills { get; set; } //in progress

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        modelBuilder.Entity<TestAttemptSection>((entity) => {
            
            



        });

        modelBuilder.Entity<TestAttemptQuestionSection>((entity) => {




        });

        modelBuilder.Entity<TestSectionQuestion>((entity) => {




        });

        modelBuilder.Entity<TestAttempt>((entity) => {




        });

        modelBuilder.Entity<TestSection>((entity) => {




        });
        
        modelBuilder.Entity<Test>((entity) => {




        });

        modelBuilder.Entity<Tag>((entity) => {




        });

        modelBuilder.Entity<QuestionType>((entity) => {



        });

        modelBuilder.Entity<Question>((entity) => {
        });

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

            // entity.Property(e => e.testId)
            //     .HasColumnName("testId")
            //     .HasColumnType("bigint")
            //     .IsRequired();

            entity.Property(e => e.status)
                .HasColumnType("varchar(20)");

            entity.HasKey("attemptId")
                .HasName("attemptId");

            // entity.HasOne<Candidate>()
            //     .WithMany()
            //     .HasForeignKey(p => p.candidateId);
    
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

            entity.HasIndex(e => e.name).IsUnique();
        });
        

        //Lets try this

         var eTypes = modelBuilder.Model.GetEntityTypes();
            foreach(var type in eTypes)
            {
                var foreignKeys = type.GetForeignKeys();
                foreach(var foreignKey in foreignKeys)
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }


    }
}