using System;
using System.Collections.Generic;
using System.IO;

namespace ShaellLang;
using Mono.Unix;

public class UnixPathFinder : IPathFinder
{
    
    private uint uid;
    private uint gid;
		
    public UnixPathFinder() {
        uid = Mono.Unix.Native.Syscall.geteuid();
        gid = Mono.Unix.Native.Syscall.getegid();
    }
    
    public string GetAbsolutePath(string path)
    {
        return GetFirstExisting(path);
    }
    
    public bool CanBeExecutable(string path) {
        var thing = new Mono.Unix.UnixFileInfo(path);
        if (!thing.Exists) {
            return false;
        }
        var perms = thing.FileAccessPermissions;
        return (
            ((perms & FileAccessPermissions.OtherExecute) != 0) ||
            ((perms & FileAccessPermissions.UserExecute) != 0 && thing.OwnerUserId == uid) ||
            ((perms & FileAccessPermissions.GroupExecute) != 0 && thing.OwnerGroupId == gid)
        );
    }
    
    private string GetFirstExisting(string path)
    {
        if (File.Exists(path) && CanBeExecutable(path))
            return path;
        var paths = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator);
        foreach (var envPath in paths)
        {
            
            var absolutePath = Path.Join(envPath, path);
            if (CanBeExecutable(absolutePath))
                return absolutePath;
            
        }

        return null;
    }
}