using HRPlatform.Models;

namespace HRPlatform.Interfaces
{
	public interface ICandidateService
	{
		Task<IEnumerable<Candidate>> GetAllAsync();
		Task<Candidate> AddCandidateAsync(Candidate newCandidate);

		Task<Candidate?> UpdateCandidateAsync(int id, Candidate updateCandidate);

		Task<Candidate?> DeleteCandidateAsync(int id);

		Task<IEnumerable<Candidate>> SearchCandidatesAsync(string? name, string? skill);
	}
}
