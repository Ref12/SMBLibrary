using System.Collections.Generic;

namespace SMBLibrary;

public readonly record struct NTResult<T>(NTStatus Status, T Result = default)
{
    public static implicit operator NTStatus(NTResult<T> r) => r.Status;

    public static implicit operator T(NTResult<T> r) => r.Result;
}

public readonly record struct FileCreateResult(object Handle, FileStatus FileStatus);

public readonly record struct QueryDirectoryResult(List<QueryDirectoryFileInformation> Entries);