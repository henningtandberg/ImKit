using System;
using System.IO;

namespace ImKit.Components.FileSystem;

public enum FileType
{
    Regular,
    Directory,
    DirectoryHidden,
    Link,
    Special,
    Hidden
}

public static class FileTypeExtensions
{
    public static FileType GetFileType(this string path)
    {
        var fileInfo = new FileInfo(path);

        if (fileInfo.LinkTarget != null)
            return FileType.Link;

        var isDirectory = fileInfo.Attributes.HasFlag(FileAttributes.Directory);

        if (OperatingSystem.IsWindows())
        {
            var isHidden = fileInfo.Attributes.HasFlag(FileAttributes.Hidden);

            if (isDirectory && isHidden)
                return FileType.DirectoryHidden;

            if (isDirectory)
                return FileType.Directory;

            if (isHidden)
                return FileType.Hidden;

            return fileInfo.Attributes.HasFlag(FileAttributes.Device)
                ? FileType.Special
                : FileType.Regular;
        }

        var isHiddenUnix = fileInfo.Name.StartsWith('.');

        if (isDirectory && isHiddenUnix)
            return FileType.DirectoryHidden;

        if (isDirectory)
            return FileType.Directory;

        if (isHiddenUnix)
            return FileType.Hidden;

        var mode = File.GetUnixFileMode(path);
        const UnixFileMode typeMask = (UnixFileMode)0xF000;
        var fileType = mode & typeMask;

        return fileType switch
        {
            (UnixFileMode)0xC000 => FileType.Special, // Socket
            (UnixFileMode)0x6000 => FileType.Special, // Block device
            (UnixFileMode)0x2000 => FileType.Special, // Character device
            (UnixFileMode)0x1000 => FileType.Special, // FIFO/pipe
            _ => FileType.Regular
        };
    }
}
