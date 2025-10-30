using HRPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace HRPlatform.Data
{
	public class HRPlatformDbContext : DbContext
	{
		public HRPlatformDbContext(DbContextOptions<HRPlatformDbContext> options) : base(options)
		{
		}
		public DbSet<Models.Candidate> Candidates { get; set; }
		public DbSet<Models.Skill> Skills { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		
			modelBuilder.Entity<Candidate>()
								.HasMany(c => c.Skills)
								.WithMany(s => s.Candidates)
								.UsingEntity<Dictionary<string, object>>(
									"CandidateSkills",
									j => j.HasOne<Skill>().WithMany().HasForeignKey("SkillId"),
									j => j.HasOne<Candidate>().WithMany().HasForeignKey("CandidateId"),
									j =>
									{
										j.HasKey("CandidateId", "SkillId");
									});


			modelBuilder.Entity<Candidate>().HasData(
				new Candidate
				{
					Id = 1,
					FullName = "Lu Salome",
					DateOfBirth = new DateTime(1998, 3, 15),
					Email = "lusalome@example.com",
					ContactNumber = "+381601234567"
				},
				new Candidate
				{
					Id = 2,
					FullName = "Irvin Jalom",
					DateOfBirth = new DateTime(1995, 11, 5),
					Email = "marko@example.com",
					ContactNumber = "+381641112233"
				}
			);

			modelBuilder.Entity<Skill>().HasData(

				new Skill { Id = 1, Name = "C#" },
				new Skill { Id = 2, Name = "Unity" },
				new Skill { Id = 3, Name = "SQL" },
				new Skill { Id = 4, Name = "JavaScript" },
				new Skill { Id = 5, Name = "React" },
				new Skill { Id = 6, Name = "Node.js" },
				new Skill { Id = 7, Name = "Java" }
			);

			modelBuilder.Entity("CandidateSkills").HasData(
				new { CandidateId = 1, SkillId = 1 },
				new { CandidateId = 1, SkillId = 2 },
				new { CandidateId = 1, SkillId = 3 },
				new { CandidateId = 2, SkillId = 4 },
				new { CandidateId = 2, SkillId = 5 },
				new { CandidateId = 2, SkillId = 6 }
			);
		}
	}
}
