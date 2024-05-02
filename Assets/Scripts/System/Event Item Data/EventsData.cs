using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Data class which will be fetched from the server
/// </summary>
[Serializable]
public class EventsData : IFetchData<Sprite>
{
    /// <summary>
    /// Event Title
    /// </summary>
    [JsonProperty("Event Title")]
    public string EventTitle;
    /// <summary>
    /// Icon url where icon will be downloaded
    /// </summary>
    [JsonProperty("Icon")]
    public string IconUrl;
    /// <summary>
    /// Icon sprite will be placed here after downloading
    /// </summary>
    public Sprite IconSprite;

    /// <summary>
    /// Type of reward
    /// </summary>
    [JsonProperty("Reward Type")]
    public RewardType RewardType;
    /// <summary>
    /// Amount given in the reward
    /// </summary>
    [JsonProperty("Reward Amount")]
    public int RewardAmount;
    /// <summary>
    /// Description of the event
    /// </summary>
    public string Description;

    /// <summary>
    /// Hexcode of the title color
    /// </summary>
    [JsonProperty("Title Color"), HideInInspector]
    public string TitleColorHex;

    /// <summary>
    /// Hexcode of the body color
    /// </summary>
    [JsonProperty("Body Color"), HideInInspector]
    public string BodyColorHex;
    /// <summary>
    /// Time of the event
    /// </summary>
    [JsonProperty("Event Time")]
    public TimeSpan EventTime;

    /// <summary>
    /// Event Type
    /// </summary>
    [JsonProperty("Event Type")]
    public EventType EventType;

    /// <summary>
    /// Color type of the title
    /// </summary>
    public Color TitleColor;
    /// <summary>
    /// Color type of the body
    /// </summary>
    public Color BodyColor;

    /// <summary>
    /// Downaloaded data which is sprite
    /// </summary>
    public Sprite _downloadedData
    {
        get => IconSprite;
    }
    
    /// <summary>
    /// Calls when initialize
    /// </summary>
    public void Init()
    {
        TitleColor = ParseColor(TitleColorHex);
        BodyColor = ParseColor(BodyColorHex);
    }

    /// <summary>
    /// Parse Hexcode to the color type
    /// </summary>
    /// <param name="HexCode">Hexcode to convert</param>
    /// <returns>Color Type of the given hexcode</returns>
    Color ParseColor(string HexCode)
    {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(HexCode, out myColor);
        return myColor;
    }

    /// <summary>
    /// Fetch Icon from the given url
    /// </summary>
    /// <param name="url">icon url</param>
    /// <param name="OnProgress">Raise when downloading is in progress</param>
    /// <param name="OnCompleted">Raise when downlaoding is completed</param>
    /// <returns></returns>
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
                // texture downloaded
                Texture2D myTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                // creating sprite from the downloaded texture
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