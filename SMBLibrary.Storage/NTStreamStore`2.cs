using System.Collections.Generic;
using System.Reflection.Metadata;

namespace SMBLibrary.Storage;

public abstract class NTStreamStore<TOuter, TInner>(INTStreamStore<TInner> inner) : INTStreamStore<TOuter>
{
    protected abstract TInner Unwrap(TOuter handle);

    protected abstract TOuter Wrap(TInner handle);

    public virtual NTStatus CloseFile(TOuter handle)
    {
        return inner.CloseFile(Unwrap(handle));
    }

    public virtual NTStatus CreateFile(out TOuter handle, out FileStatus fileStatus, string path, AccessMask desiredAccess, FileAttributes fileAttributes, ShareAccess shareAccess, CreateDisposition createDisposition, CreateOptions createOptions, SecurityContext securityContext)
    {
        var result = inner.CreateFile(out var typedHandle, out fileStatus, path, desiredAccess, fileAttributes, shareAccess, createDisposition, createOptions, securityContext);
        handle = Wrap(typedHandle);
        return result;
    }

    public virtual NTStatus FlushFileBuffers(TOuter handle)
    {
        return inner.FlushFileBuffers(Unwrap(handle));
    }

    public virtual NTStatus ReadFile(out byte[] data, TOuter handle, long offset, int maxCount)
    {
        return inner.ReadFile(out data, Unwrap(handle), offset, maxCount);
    }

    public virtual NTStatus WriteFile(out int numberOfBytesWritten, TOuter handle, long offset, byte[] data)
    {
        return inner.WriteFile(out numberOfBytesWritten, Unwrap(handle), offset, data);
    }
}