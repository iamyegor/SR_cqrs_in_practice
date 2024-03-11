namespace Logic.Utils;

public class ExceptionIncrementor
{
    public int Value { get; private set; }

    public void Increment()
    {
        Value++;
    }
}
