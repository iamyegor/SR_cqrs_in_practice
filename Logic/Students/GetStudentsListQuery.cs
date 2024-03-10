using Api.DTOs;
using AutoMapper;
using Logic.DAL;

namespace Logic.Students;

public interface IQuery<TResult>;

public class GetStudentsListQuery : IQuery<IEnumerable<StudentDto>>
{
    public string? EnrolledIn { get; }
    public int? NumberOfCourses { get; }

    public GetStudentsListQuery(string? enrolledIn, int? numberOfCourses)
    {
        EnrolledIn = enrolledIn;
        NumberOfCourses = numberOfCourses;
    }
}

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public TResult Handle(TQuery query);
}

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
