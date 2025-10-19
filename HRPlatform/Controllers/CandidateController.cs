using HRPlatform.Interfaces;
using HRPlatform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRPlatform.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CandidateController : ControllerBase
	{
		private readonly ICandidateService _candidateService;
		private readonly ISkillService _skillService;

		public CandidateController(ICandidateService candidateService, ISkillService skillService)
		{
			_candidateService = candidateService;
			_skillService = skillService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateCandidate([FromBody] Candidate newCandidate)
		{
			if (newCandidate == null || !ModelState.IsValid) return BadRequest();

			var createdCandidate = new Candidate
			(
				FullName: newCandidate.FullName,
				DateOfBirth: newCandidate.DateOfBirth,
				Email: newCandidate.Email,
				ContactNumber: newCandidate.ContactNumber,
				Skills: newCandidate.Skills
			);
			createdCandidate = await _candidateService.AddCandidateAsync(newCandidate);
			return CreatedAtAction(nameof(CreateCandidate), new { id = createdCandidate.Id }, createdCandidate);
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var candidates = await _candidateService.GetAllAsync();
			if (!candidates.Any()) return NotFound();

			return Ok(candidates);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCandidate(int id)
		{
			var deletedCandidate = await _candidateService.DeleteCandidateAsync(id);
			if (deletedCandidate == null) return NotFound();
			return Ok(deletedCandidate);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCandidate(int id, [FromBody] Candidate updateCandidate)
		{
			if (updateCandidate == null || !ModelState.IsValid) return BadRequest();

			var updatedCandidate = await _candidateService.UpdateCandidateAsync(id, new Candidate
			{
				FullName = updateCandidate.FullName,
				DateOfBirth = updateCandidate.DateOfBirth,
				Email = updateCandidate.Email,
				ContactNumber = updateCandidate.ContactNumber
			});
			if (updatedCandidate == null) return NotFound();

			return Ok(updatedCandidate);
		}

		[HttpPost("{candidateId}/skills")]
		public async Task<IActionResult> AddSkill(int candidateId, [FromBody] string skillName)
		{
			try
			{
				var skill = await _skillService.AddSkillToCandidateAsync(candidateId, skillName);
				return Ok(skill);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpDelete("{candidateId}/skills/{skillName}")]
		public async Task<IActionResult> RemoveSkill(int candidateId, string skillName)
		{
			var removeSkill = await _skillService.RemoveSkillFromCandidateAsync(candidateId, skillName);
			if (removeSkill == null) return NotFound();
			return Ok(removeSkill);
		}

		[HttpPatch("{candidateId}/skills/update")]
		public async Task<IActionResult> UpdateSkillName(int candidateId, [FromQuery] string oldName, [FromQuery] string newName)
		{
			try
			{
				var updatedSkill = await _skillService.UpdateSkillAsync(candidateId, oldName, newName);
				return Ok(updatedSkill);
			}
			catch (Exception ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}


	}
}
