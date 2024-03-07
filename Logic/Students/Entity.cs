namespace Logic.Students;

public abstract class Entity
{
    public long Id { get; }

    public override bool Equals(object? obj)
    {
        if (obj is Entity entity)
        {
            return entity.Id.Equals(Id);
        }

        return false;
    }

    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity? a, Entity? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
