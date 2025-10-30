using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace HRPlatform.Models
{
	public class Skill
	{
		[JsonIgnore]
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		[JsonIgnore]
		public List<Candidate> Candidates { get; set; } = new();
	}

}
