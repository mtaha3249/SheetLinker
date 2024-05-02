using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class to download list of events from the sheet
/// </summary>
public class FetchEventData : IFetchData<List<EventsData>>
{
    /// <summary>
    /// Event data loaded from the url
    /// </summary>
    [SerializeField]
    private List<EventsData> _eventsData = new List<EventsData>();

    /// <summary>
    /// downloaded data
    /// </summary>
    public List<EventsData> _downloadedData
    {
        get => _eventsData;
    }

    /// <summary>
    /// Fetch data from the given url
    /// </summary>
    /// <param name="url">url which is used to download</param>
    /// <param name="OnProgress">Raise when downloading is in progress</param>
    /// <param name="OnCompleted">Raise when Completed or Fail</param>
    /// <returns></returns>
    public IEnumerator FetchData(string url, Action<float, string> OnProgress, Action<bool, List<EventsData>> OnCompleted)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // send request
            webRequest.SendWebRequest();

            // when data is loading
            while(!webRequest.isDone)
            {
                OnProgress?.Invoke(webRequest.downloadProgress, "Events Data");
                yield return null;
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    OnCompleted?.Invoke(false, null);
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    OnCompleted?.Invoke(false, null);
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    OnCompleted?.Invoke(false, null);
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    // Parse data using Newtonsoft
                    _eventsData = JsonConvert.DeserializeObject<List<EventsData>>(webRequest.downloadHandler.text);
                    OnCompleted?.Invoke(true, _eventsData);
                    break;
            }
        }
    }
}