using HRPlatform.Data;
using HRPlatform.Interfaces;
using HRPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace HRPlatform.Services
{
	public class SkillService : ISkillService
	{
		private readonly HRPlatformDbContext _context;
		private object candidate;

		public SkillService(HRPlatformDbContext context)
		{
			_context = context;
		}

		public async Task<Skill> AddSkillToCandidateAsync(int candidateId, string skillName)
		{
			var candidate = await _context.Candidates.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == candidateId);

			if (candidate == null)
				throw new Exception("Candidate not found");

			var skill = await _context.Skills
				.FirstOrDefaultAsync(s => s.Name.ToLower() == skillName.ToLower());

			if (skill == null)
			{
				skill = new Skill { Name = skillName };
				_context.Skills.Add(skill);
			}

			if (candidate.Skills.Any(s => s.Name.ToLower() == skillName.ToLower()))
				throw new Exception($"Candidate already has the skill '{skillName}'.");

			candidate.Skills.Add(skill);
			await _context.SaveChangesAsync();

			return skill;
		}

		public async Task<Skill?> RemoveSkillFromCandidateAsync(int candidateId, string skillName)
		{
			var candidate = await _context.Candidates
				.Include(c => c.Skills)
				.FirstOrDefaultAsync(c => c.Id == candidateId);

			if (candidate == null)
				throw new Exception("Candidate not found");

			var skill = candidate.Skills
				.FirstOrDefault(s => s.Name.ToLower() == skillName.ToLower());

			if (skill == null)
				throw new Exception("Skill not found for this candidate");

			candidate.Skills.Remove(skill);
			await _context.SaveChangesAsync();

			return skill;
		}

		public async Task<Skill?> UpdateSkillAsync(int candidateId, string oldSkillName, string newSkillName)
		{
			var candidate = await _context.Candidates.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == candidateId);

			if (candidate == null)
				throw new Exception("Candidate not found");

			var oldSkill = candidate.Skills.FirstOrDefault(s => s.Name.ToLower() == oldSkillName.ToLower());

			if (oldSkill == null)
				throw new Exception("Skill not found for this candidate");

			var newSkill = await _context.Skills
				.FirstOrDefaultAsync(s => s.Name.ToLower() == newSkillName.ToLower());

			if (newSkill == null)
			{
				newSkill = new Skill { Name = newSkillName };
				_context.Skills.Add(newSkill);
				await _context.SaveChangesAsync();
			}

			candidate.Skills.Remove(oldSkill);
			candidate.Skills.Add(newSkill);

			await _context.SaveChangesAsync();

			return newSkill;
		}
	}
}
