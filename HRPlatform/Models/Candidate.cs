using System.ComponentModel.DataAnnotations;

namespace HRPlatform.Models
{
	public class Candidate
	{
		public Candidate(string FullName, DateTime DateOfBirth, string Email, string ContactNumber, List<Skill> Skills)
		{
			this.FullName = FullName;
			this.DateOfBirth = DateOfBirth;
			this.Email = Email;
			this.ContactNumber = ContactNumber;
			this.Skills = Skills;
		}

		public Candidate() { }

		public int Id { get; set; }
		//[Required]
		public string FullName { get; set; } //= string.Empty;
		public DateTime DateOfBirth { get; set; }
		[EmailAddress]
		public string Email { get; set; } //= string.Empty;
		[Phone]
		public string ContactNumber { get; set; } //= string.Empty;
		public List<Skill> Skills { get; set; } = new List<Skill>();
	}
}
