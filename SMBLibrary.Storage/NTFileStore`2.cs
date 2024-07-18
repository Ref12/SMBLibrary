using System.Collections.Generic;
using System.Reflection.Metadata;

namespace SMBLibrary.Storage;

public abstract class NTFileStore<TOuter, TInner>(INTControlStore<TInner> inner, INTStreamStore<TInner> streamInner) 
    : NTStreamStore<TOuter, TInner>(streamInner), INTFileStore<TOuter>
{
    public virtual NTStatus Cancel(object ioRequest)
    {
        return inner.Cancel(ioRequest);
    }

    public virtual NTStatus DeviceIOControl(TOuter handle, uint ctlCode, byte[] input, out byte[] output, int maxOutputLength)
    {
        return inner.DeviceIOControl(Unwrap(handle), ctlCode, input, out output, maxOutputLength);
    }

    public virtual NTStatus GetFileInformation(out FileInformation result, TOuter handle, FileInformationClass informationClass)
    {
        return inner.GetFileInformation(out result, Unwrap(handle), informationClass);
    }

    public virtual NTStatus GetFileSystemInformation(out FileSystemInformation result, FileSystemInformationClass informationClass)
    {
        return inner.GetFileSystemInformation(out result, informationClass);
    }

    public virtual NTStatus GetSecurityInformation(out SecurityDescriptor result, TOuter handle, SecurityInformation securityInformation)
    {
        return inner.GetSecurityInformation(out result, Unwrap(handle), securityInformation);
    }

    public virtual NTStatus LockFile(TOuter handle, long byteOffset, long length, bool exclusiveLock)
    {
        return inner.LockFile(Unwrap(handle), byteOffset, length, exclusiveLock);
    }

    public virtual NTStatus NotifyChange(out object ioRequest, TOuter handle, NotifyChangeFilter completionFilter, bool watchTree, int outputBufferSize, OnNotifyChangeCompleted onNotifyChangeCompleted, object context)
    {
        return inner.NotifyChange(out ioRequest, Unwrap(handle), completionFilter, watchTree, outputBufferSize, onNotifyChangeCompleted, context);
    }

    public virtual NTStatus QueryDirectory(out List<QueryDirectoryFileInformation> result, TOuter handle, string fileName, FileInformationClass informationClass)
    {
        return inner.QueryDirectory(out result, Unwrap(handle), fileName, informationClass);
    }

    public virtual NTStatus SetFileInformation(TOuter handle, FileInformation information)
    {
        return inner.SetFileInformation(Unwrap(handle), information);
    }

    public virtual NTStatus SetFileSystemInformation(FileSystemInformation information)
    {
        return inner.SetFileSystemInformation(information);
    }

    public virtual NTStatus SetSecurityInformation(TOuter handle, SecurityInformation securityInformation, SecurityDescriptor securityDescriptor)
    {
        return inner.SetSecurityInformation(Unwrap(handle), securityInformation, securityDescriptor);
    }

    public virtual NTStatus UnlockFile(TOuter handle, long byteOffset, long length)
    {
        return inner.UnlockFile(Unwrap(handle), byteOffset, length);
    }
}