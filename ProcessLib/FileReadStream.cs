namespace ProcessLib;

public class FileReadStream : IReadStream
{
    public FileReadStream()
    {
        
    }
    
    public void StartPiping()
    {
        throw new NotImplementedException();
    }

    public void Pipe(IWriteStream writeStream)
    {
        throw new NotImplementedException();
    }

    public bool ReachedEnd()
    {
        throw new NotImplementedException();
    }

    public event ReadStreamFinished? ReadStreamFinished;
}