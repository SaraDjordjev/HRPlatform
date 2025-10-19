using HRPlatform.Models;

namespace HRPlatform.Interfaces
{
	public interface ISkillService
	{
		Task<Skill> AddSkillToCandidateAsync(int candidateId, string skillName);
		Task<Skill?> RemoveSkillFromCandidateAsync(int candidateId, string skillName);
		Task<List<Skill>> GetSkillsByCandidateAsync(int candidateId);
		Task<Skill?> UpdateSkillAsync(int candidateId, string oldSkillName, string newSkillName);
	}
}
