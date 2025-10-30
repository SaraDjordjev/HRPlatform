using HRPlatform.Data;
using HRPlatform.Interfaces;
using HRPlatform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Text.RegularExpressions;

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
			var existingCandidate = await _context.Candidates.FirstOrDefaultAsync(c =>
				c.Email == newCandidate.Email ||
				c.ContactNumber == newCandidate.ContactNumber
			);

			if (existingCandidate != null)
			{
				throw new Exception("Candidate with the same information already exists.");
			}

			if (newCandidate.Skills != null && newCandidate.Skills.Any())
			{
				var skillsToAssign = new List<Skill>();

				foreach (var skill in newCandidate.Skills)
				{
					var existingSkill = await _context.Skills
						.FirstOrDefaultAsync(s => s.Name.ToLower() == skill.Name.ToLower());

					if (existingSkill != null)
						skillsToAssign.Add(existingSkill);
					else
						skillsToAssign.Add(skill);
				}
				newCandidate.Skills = skillsToAssign;
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

			candidate.Skills.Clear();
			_context.Candidates.Remove(candidate);

			await _context.SaveChangesAsync();
			return candidate;
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

		public async Task<IEnumerable<Candidate>> SearchCandidatesAsync(string? name, string? skill)
		{
			
			if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(skill))
			{
				return new List<Candidate>();
			}

			var query = _context.Candidates.Include(c => c.Skills).AsQueryable();

			if (!string.IsNullOrWhiteSpace(name))
			{
				query = query.Where(c => c.FullName.ToLower().Contains(name.ToLower()));
			}

			if (!string.IsNullOrWhiteSpace(skill))
			{
				query = query.Where(c => c.Skills.Any(s => s.Name.ToLower().Contains(skill.ToLower())));
			}

			return await query.ToListAsync();
		}

	}
}
