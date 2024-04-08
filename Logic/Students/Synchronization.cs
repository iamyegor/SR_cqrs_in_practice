namespace Logic.Students;

public class Synchronization
{
    public string Name { get; private set; }
    public bool IsSyncRequired { get; set; }
    public int RowVersion { get; set; }
}