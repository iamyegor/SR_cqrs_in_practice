namespace Logic.Students;

public class Course : Entity
{
    public string Name { get; }
    public int Credits { get; }

    public Course(string name, int credits)
    {
        Name = name;
        Credits = credits;
    }
}
