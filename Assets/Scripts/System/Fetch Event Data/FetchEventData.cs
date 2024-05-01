using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FetchEventData : IFetchData<List<EventsData>>
{
    [SerializeField]
    private List<EventsData> _eventsData = new List<EventsData>();

    public List<EventsData> _downloadedData
    {
        get => _eventsData;
    }

    public IEnumerator FetchData(string url, Action<float, string> OnProgress, Action<bool, List<EventsData>> OnCompleted)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SendWebRequest();

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
                    _eventsData = JsonConvert.DeserializeObject<List<EventsData>>(webRequest.downloadHandler.text);
                    OnCompleted?.Invoke(true, _eventsData);
                    break;
            }
        }
    }
}
