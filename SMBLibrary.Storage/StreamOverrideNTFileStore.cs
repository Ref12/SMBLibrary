namespace SMBLibrary.Storage;

public class OverrideNTStreamStore<TOuter, TInner>(INTFileStore<TInner> inner)
    : NTStreamStore<TOuter, TInner>(inner)
{
    protected override TInner Unwrap(TOuter handle)
    {
        throw new System.NotImplementedException();
    }

    protected override TOuter Wrap(TInner handle)
    {
        throw new System.NotImplementedException();
    }

    public override NTStatus WriteFile(out int numberOfBytesWritten, TOuter handle, long offset, byte[] data)
    {
        return base.WriteFile(out numberOfBytesWritten, handle, offset, data);
    }

    public override NTStatus ReadFile(out byte[] data, TOuter handle, long offset, int maxCount)
    {
        return base.ReadFile(out data, handle, offset, maxCount);
    }
}