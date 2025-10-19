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
			var candidate = await _context.Candidates
		.Include(c => c.Skills)
		.FirstOrDefaultAsync(c => c.Id == candidateId);

			if (candidate == null)
				throw new Exception("Candidate not found");

			// Da li već postoji taj skill u bazi
			var skill = await _context.Skills
				.FirstOrDefaultAsync(s => s.Name.ToLower() == skillName.ToLower());

			// Ako ne postoji — kreiraj novi skill
			if (skill == null)
			{
				skill = new Skill { Name = skillName };
				_context.Skills.Add(skill);
			}

			// Ako kandidat već ima taj skill — preskoči
			if (candidate.Skills.Any(s => s.Name.ToLower() == skillName.ToLower()))
				throw new Exception($"Candidate already has the skill '{skillName}'.");

			// Dodaj skill kandidatu
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
