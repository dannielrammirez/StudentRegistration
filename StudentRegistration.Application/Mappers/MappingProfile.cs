using AutoMapper;
using StudentRegistration.Application.Dtos;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Mappers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Student, StudentDto>();

			CreateMap<Professor, ProfessorDto>()
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

			CreateMap<Course, CourseDto>();

			CreateMap<Enrollment, EnrolledCourseDto>()
				.ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
				.ForMember(dest => dest.ProfessorName, opt => opt.MapFrom(src => $"{src.Course.Professor.FirstName} {src.Course.Professor.LastName}"));
		}
	}
}