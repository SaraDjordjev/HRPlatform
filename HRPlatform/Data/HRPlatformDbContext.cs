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
			modelBuilder.Entity<Skill>().HasIndex(s => s.Name).IsUnique();

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

				new Skill { Id = 1, Name = "C#", CandidateId = 1 },
				new Skill { Id = 2, Name = "Unity", CandidateId = 1 },
				new Skill { Id = 3, Name = "SQL", CandidateId = 1 },

				new Skill { Id = 4, Name = "JavaScript", CandidateId = 2 },
				new Skill { Id = 5, Name = "React", CandidateId = 2 },
				new Skill { Id = 6, Name = "Node.js", CandidateId = 2 }
			);
		}
	}
}
