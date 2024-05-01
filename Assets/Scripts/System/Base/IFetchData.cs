using System;
using System.Collections;

public enum LoadingStatus
{
    InProgress,
    Completed,
    Fail
}

public interface IFetchData<T>
{
    public T _downloadedData { get; }

    public IEnumerator FetchData(string url,Action<float, string> OnProgress, Action<bool, T> OnCompleted);
}
