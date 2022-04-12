using System.Diagnostics;

namespace ProcessLib;

public class ExternalProgram
{
    private ProcessReadStream _out;
    private ProcessReadStream _err;
    private WriteStream _in;
    private Process _process;
    private bool _started;
    public ExternalProgram(string fileName, IEnumerable<string> arguments)
    {
        _process = new Process();
        _process.StartInfo.FileName = fileName;
        foreach (string argument in arguments)
        {
            _process.StartInfo.ArgumentList.Add(argument);
        }
        _process.StartInfo.UseShellExecute = false;
        _process.StartInfo.RedirectStandardOutput = true;
        _process.StartInfo.RedirectStandardError = true;
        _process.StartInfo.RedirectStandardInput = true;
        _process.StartInfo.CreateNoWindow = true;
        
        _out = new ProcessReadStream();
        _err = new ProcessReadStream();
        _in = new WriteStream();

        _started = false;
    }

    public void Start()
    {
        if (_started)
        {
            throw new Exception("JobObject already started");
        }
        _started = true;
        _process.Start();
        _out.Bind(_process, _process.StandardOutput);
        _out.StartPiping();
        _err.Bind(_process, _process.StandardError);
        _err.StartPiping();
        _in.Bind(_process.StandardInput);
    }

    public async Task Wait()
    {
        await _process.WaitForExitAsync();
    }
    
    public IReadStream Out => _out;
    public IReadStream Err => _err;
    public IWriteStream In => _in;
}