using AutoMapper;
using DTOs;
using Logic.DAL;
using Logic.Students.Queries.Common;

namespace Logic.Students.Queries.GetStudentsList;

public class GetStudentsListQueryHandler
    : IQueryHandler<GetStudentsListQuery, IEnumerable<StudentDto>>
{
    private readonly StudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public GetStudentsListQueryHandler(IMapper mapper, StudentRepository studentRepository)
    {
        _mapper = mapper;
        _studentRepository = studentRepository;
    }

    public IEnumerable<StudentDto> Handle(GetStudentsListQuery query)
    {
        IReadOnlyList<Student> studentsFromDb = _studentRepository.GetList(
            query.EnrolledIn,
            query.NumberOfCourses
        );

        return _mapper.Map<IEnumerable<StudentDto>>(studentsFromDb);
    }
}