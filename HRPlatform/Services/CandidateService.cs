using HRPlatform.Data;
using HRPlatform.Interfaces;
using HRPlatform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace HRPlatform.Services
{
	public class CandidateService : ICandidateService
	{
		private readonly HRPlatformDbContext _context;

		public CandidateService(HRPlatformDbContext context)
		{
			_context = context;
		}
		public async Task<Candidate> AddCandidateAsync(Candidate newCandidate)
		{
			if (newCandidate.Skills != null && newCandidate.Skills.Any())
			{
				foreach (var skill in newCandidate.Skills)
				{
					skill.Candidate = newCandidate; // povezuje EF reference
				}
			}

			await _context.Candidates.AddAsync(newCandidate);
			await _context.SaveChangesAsync();
			return newCandidate;
		}

		public async Task<IEnumerable<Candidate>> GetAllAsync()
		{
			return await _context.Candidates.Include(c => c.Skills).ToListAsync();
		}

		public async Task<Candidate?> DeleteCandidateAsync(int id)
		{
			var candidate = await _context.Candidates.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == id); ;
			if(candidate == null) return null;

			//delete skills
			// Obriši sve skill-ove
			_context.Skills.RemoveRange(candidate.Skills);
			_context.Candidates.Remove(candidate);
			await _context.SaveChangesAsync();
			return candidate;
		}

		public Task<Candidate> SearchCandidatesAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<Candidate?> UpdateCandidateAsync(int id, Candidate updateCandidate)
		{
			var existingCandidate = await _context.Candidates.FindAsync(id);
			if (existingCandidate == null) return null;

			existingCandidate.FullName = updateCandidate.FullName;
			existingCandidate.DateOfBirth = updateCandidate.DateOfBirth;
			existingCandidate.Email = updateCandidate.Email;
			existingCandidate.ContactNumber = updateCandidate.ContactNumber;

			await _context.SaveChangesAsync();
			return existingCandidate;
		}

		



		/*public async Task<Skill> AddSkillToCandidateAsync(int id, string skillName)
		{ 
			var candidate = await _context.Candidates.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == id);
			if (candidate == null) throw new Exception("Candidate not found");

			var newSkill = new Skill
			{
				Name = skillName,
				CandidateId = candidate.Id
			};

			await _context.Skills.AddAsync(newSkill);
			await _context.SaveChangesAsync();
			return newSkill;
		}
		*/
	}
}
