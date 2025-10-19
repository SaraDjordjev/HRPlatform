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
			//postojanje istog kandidata
			var existingCandidate = await _context.Candidates.FirstOrDefaultAsync(c =>
				//c.FullName == newCandidate.FullName &&
				//c.DateOfBirth == newCandidate.DateOfBirth &&
				c.Email == newCandidate.Email ||
				c.ContactNumber == newCandidate.ContactNumber
			);

			if (existingCandidate != null)
			{
				throw new Exception("Candidate with the same information already exists.");
			}

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
			// Ako nisu uneti ni name ni skill, ne vraćaj sve
			if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(skill))
			{
				return new List<Candidate>(); // prazna lista
			}

			var query = _context.Candidates.Include(c => c.Skills).AsQueryable(); //uslovi za dinamicko dodavanje

			if (!string.IsNullOrWhiteSpace(name))
			{
				query = query.Where(c => c.FullName.ToLower().Contains(name.ToLower()));
			}

			if (!string.IsNullOrWhiteSpace(skill))
			{
				query = query.Where(c => c.Skills.Any(s => s.Name.ToLower().Contains(skill.ToLower()))); //Java->JavaScript
				
			}

			return await query.ToListAsync();
		}

	}
}
