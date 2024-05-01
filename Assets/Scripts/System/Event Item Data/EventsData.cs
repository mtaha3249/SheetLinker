using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class EventsData : IFetchData<Sprite>
{
    [JsonProperty("Event Title")]
    public string EventTitle;
    [JsonProperty("Icon")]
    public string IconUrl;
    public Sprite IconSprite;

    [JsonProperty("Reward Type")]
    public RewardType RewardType;
    [JsonProperty("Reward Amount")]
    public int RewardAmount;
    public string Description;

    [JsonProperty("Title Color"), HideInInspector]
    public string TitleColorHex;

    [JsonProperty("Body Color"), HideInInspector]
    public string BodyColorHex;
    [JsonProperty("Event Time")]
    public TimeSpan EventTime;

    [JsonProperty("Event Type")]
    public EventType EventType;

    public Color TitleColor;
    public Color BodyColor;

    public Sprite _downloadedData
    {
        get => IconSprite;
    }

    public void Init()
    {
        TitleColor = ParseColor(TitleColorHex);
        BodyColor = ParseColor(BodyColorHex);
    }

    Color ParseColor(string HexCode)
    {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(HexCode, out myColor);
        return myColor;
    }

    public IEnumerator FetchData(string url, Action<float, string> OnProgress, Action<bool, Sprite> OnCompleted)
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);

        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            OnProgress?.Invoke(webRequest.downloadProgress, EventTitle);
            yield return null;
        }

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                OnCompleted?.Invoke(false, null);
                Debug.LogError("Error Downloading Image: " + webRequest.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                OnCompleted?.Invoke(false, null);
                Debug.LogError("Error Downloading Image: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                OnCompleted?.Invoke(false, null);
                Debug.LogError("Error Downloading Image: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                Texture2D myTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                IconSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
                OnCompleted?.Invoke(true, IconSprite);
                break;
        }
    }
}

public enum EventType
{
    Active,
    ComingSoon,
    Expired
}