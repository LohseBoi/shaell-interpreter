namespace ProcessLib;

public class TextWriteStream : IWriteStream
{
    private readonly TextWriter _streamWriter;

    public TextWriteStream(TextWriter streamWriter)
    {
        _streamWriter = streamWriter;
    }
    public void Write(string text)
    {
        _streamWriter.Write(text);
    }

    public bool IsOpen()
    {
        return true;
    }

    public void Close()
    {
        _streamWriter.Close();
    }
}