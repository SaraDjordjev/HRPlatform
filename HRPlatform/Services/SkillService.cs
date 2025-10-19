using HRPlatform.Data;
using HRPlatform.Interfaces;
using HRPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRPlatform.Services
{
	public class SkillService : ISkillService
	{
		private readonly HRPlatformDbContext _context;

		public SkillService(HRPlatformDbContext context)
		{
			_context = context;
		}

		public async Task<Skill> AddSkillToCandidateAsync(int candidateId, string skillName)
		{
			var candidate = await _context.Candidates.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == candidateId);

			if (candidate == null)
				throw new Exception("Candidate not found");

			var skill = new Skill
			{
				Name = skillName,
				CandidateId = candidateId
			};

			candidate.Skills.Add(skill);
			await _context.SaveChangesAsync();

			return skill;
		}

		public Task<List<Skill>> GetSkillsByCandidateAsync(int candidateId)
		{
			throw new NotImplementedException();
		}

		public async Task<Skill?> RemoveSkillFromCandidateAsync(int candidateId, string skillName)
		{
			var candidate = await _context.Candidates
	   .Include(c => c.Skills)
	   .FirstOrDefaultAsync(c => c.Id == candidateId);

			if (candidate == null)
				throw new Exception("Candidate not found");

			var skill = candidate.Skills
				.FirstOrDefault(s => s.Name.ToLower() == skillName.ToLower()); // case-insensitive

			if (skill == null)
				throw new Exception("Skill not found for this candidate");

			_context.Skills.Remove(skill);
			await _context.SaveChangesAsync();

			return skill;
		}

		public async Task<Skill?> UpdateSkillAsync(int candidateId, string oldSkillName, string newSkillName)
		{
			var candidate = await _context.Candidates
				.Include(c => c.Skills)
				.FirstOrDefaultAsync(c => c.Id == candidateId);

			if (candidate == null)
				throw new Exception("Candidate not found");

			var skill = candidate.Skills.FirstOrDefault(s => s.Name == oldSkillName);

			if (skill == null)
				throw new Exception("Skill not found for this candidate");

			skill.Name = newSkillName;

			await _context.SaveChangesAsync();
			return skill;
		}
	}
}
