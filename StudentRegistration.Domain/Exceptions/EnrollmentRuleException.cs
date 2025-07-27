namespace StudentRegistration.Domain.Exceptions
{
	public class EnrollmentRuleException : DomainException
	{
		public EnrollmentRuleException(string message) : base(message)
		{
		}
	}
}