namespace ProcessLib;

public class StringWriteStream : IWriteStream
{
    private string _str;
    public string Val => _str;
    public StringWriteStream(string str)
    {
        _str = str;
    }

    public void Write(string str)
    {
        _str += str;
    }

    public bool IsOpen()
    {
        return true;
    }

    public void Close()
    {
        return;
    }
}