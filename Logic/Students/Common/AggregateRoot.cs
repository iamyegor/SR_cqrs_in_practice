namespace Logic.Students.Common;

public class AggregateRoot : Entity
{
    public bool IsSyncNeeded { get; set; }

    public AggregateRoot(int id)
        : base(id) { }
}
