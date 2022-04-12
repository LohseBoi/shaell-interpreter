namespace ProcessLib;

public interface IWriteStream
{
    public void Write(string text);

    public bool IsOpen();
    
    public void Close();
}