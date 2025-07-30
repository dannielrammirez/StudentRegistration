namespace StudentRegistration.Application.Common
{
	public class GenericResponse<T>
	{
		public T Data { get; set; }
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
		public GenericResponse(T data, bool isSuccess, string message)
		{
			Data = data;
			IsSuccess = isSuccess;
			Message = message;
		}

		public GenericResponse() { }
	}
}