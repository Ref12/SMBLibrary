using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codex.Utilities;
using DiskAccessLibrary.FileSystems.Abstractions;
using LibGit2Sharp;

namespace SMBLibrary.Storage;

public class GitFileSystem : FileSystem
{
    public Repository Repo { get; }

    public Tree Tree { get; }

    public GitFileSystem(string path, string treeish = null)
    {
        Repo = new Repository(path);

        Name = Repo.Network.Remotes.FirstOrDefault()?.Url is string url
            ? url.Substring(url.IndexOf('/') + 1)
            : Path.GetFileName(path);

        Tree = GetTree(Repo.Lookup(treeish ?? "HEAD"));
    }

    private Tree GetTree(GitObject obj)
    {
        if (obj is Tree t) return t;
        else if (obj.Peel<Commit>() is Commit c) return c.Tree;

        throw new NotImplementedException();
    }

    public override string Name { get; }

    public override long Size => 100L << 40;

    public override long FreeSpace => 0;

    public override bool SupportsNamedStreams => false;


    public override FileSystemEntry CreateDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public override FileSystemEntry CreateFile(string path)
    {
        throw new NotImplementedException();
    }

    public override void Delete(string path)
    {
        throw new NotImplementedException();
    }

    public override FileSystemEntry GetEntry(string path)
    {
        return HandleEntry(path, GetFsEntry, handleDirectories: true);
    }

    private static FileSystemEntry GetFsEntry(TreeEntry entry, Blob b)
    {
        return new FileSystemEntry(
            fullName: entry.Path,
            name: entry.Name,
            isDirectory: false,
            size: (ulong)b.Size,
            DateTime.UtcNow,
            DateTime.UtcNow,
            DateTime.UtcNow,
            isArchived: false,
            isReadonly: true,
            isHidden: false);
    }

    private T HandleEntry<T>(TreeEntry entry, Func<TreeEntry, Blob, T> getResult, bool handleDirectories)
    {
        var obj = entry?.Target;
        if (Out.VarIf(entry?.TargetType == TreeEntryTargetType.Blob, obj as Blob, out var b) || handleDirectories)
        {
            return getResult(entry, b);
        }

        throw new FileNotFoundException(entry.Path);
    }

    private T HandleEntry<T>(string path, Func<TreeEntry, Blob, T> getResult, bool handleDirectories)
    {
        var entry = GetTreeEntry(path);
        return HandleEntry(entry, getResult, handleDirectories);
    }

    private TreeEntry GetTreeEntry(string path)
    {
        return Tree[path];
    }

    public override List<FileSystemEntry> ListEntriesInDirectory(string path)
    {
        var entry = GetTreeEntry(path);
        if (entry?.TargetType != TreeEntryTargetType.Tree) throw new DirectoryNotFoundException(path);

        var tree = (Tree)entry.Target;
        return tree.Select(e => HandleEntry(e, GetFsEntry, handleDirectories: true)).ToList();
    }

    public override void Move(string source, string destination)
    {
        throw new NotImplementedException();
    }

    public override Stream OpenFile(string path, FileMode mode, FileAccess access, FileShare share, FileOptions options)
    {
        return HandleEntry(path, (e, b) => b.GetContentStream(), handleDirectories: false);
    }

    public override void SetAttributes(string path, bool? isHidden, bool? isReadonly, bool? isArchived)
    {
        throw new NotImplementedException();
    }

    public override void SetDates(string path, DateTime? creationDT, DateTime? lastWriteDT, DateTime? lastAccessDT)
    {
        throw new NotImplementedException();
    }
}