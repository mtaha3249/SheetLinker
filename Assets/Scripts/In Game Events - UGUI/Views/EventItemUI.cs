using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Attached to event Item prefab
/// </summary>
public class EventItemUI : MonoBehaviour
{
    /// <summary>
    /// Fetch the loading pool of image
    /// </summary>
    [Inject]
    private EventItemLoadingPool _loaderPool;

    /// <summary>
    /// Data to show on this item
    /// </summary>
    [SerializeField]
    private EventsData _data;
    /// <summary>
    /// Loading parent of image
    /// </summary>
    [SerializeField]
    private Transform _loaderParent;
    /// <summary>
    /// UI Title BG
    /// </summary>
    [Header("UI")]
    [SerializeField]
    private Image _titleBg;
    /// <summary>
    /// UI Background BG
    /// </summary>
    [SerializeField]
    private Image _backgroundBg;
    /// <summary>
    /// UI Icon
    /// </summary>
    [SerializeField]
    private Image _icon;
    /// <summary>
    /// Title Text
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _title;
    /// <summary>
    /// Time left
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _time;
    /// <summary>
    /// Description of the item
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _description;
    /// <summary>
    /// Button text which shows Active, Coming Soon
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _buttonText;

    /// <summary>
    /// Reference of image loader
    /// </summary>
    private EventItemLoading _loader;
    /// <summary>
    /// Status of image loading
    /// </summary>
    private LoadingStatus _loadStatus;

    /// <summary>
    /// Calls when item is spawned
    /// </summary>
    /// <param name="parent">Parent to be of this parent</param>
    /// <param name="data">data to show on this item</param>
    public void Setup(Transform parent, EventsData data)
    {
        _data = data;

        _loadStatus = LoadingStatus.InProgress;
        // load icon image
        StartCoroutine(_data.FetchData(_data.IconUrl, OnProgress, OnCompleted));
        ImageLoadingProgress();

        RectTransform transform = GetComponent<RectTransform>();
        transform.parent = parent;

        SetupUI();
    }

    /// <summary>
    /// Setup UI on the item
    /// </summary>
    void SetupUI()
    {
        _titleBg.color = _data.TitleColor;
        _backgroundBg.color = _data.BodyColor;
        _title.text = _data.EventTitle;
        _description.text = _data.Description;
        _time.text = string.Format("TimeLeft: \t {0}d {1}h {2}m {3}s", _data.EventTime.Days, _data.EventTime.Hours, _data.EventTime.Minutes, _data.EventTime.Seconds);
        _buttonText.text = _data.EventType.ToString();
        _icon.sprite = _data.IconSprite != null ? _data.IconSprite : null;

        if(_data.EventType == EventType.ComingSoon)
        {
            _time.gameObject.SetActive(false);
        }
        else if (_data.EventType == EventType.Expired)
        {
            _buttonText.transform.parent.gameObject.SetActive(false);
            _time.text = _data.EventType.ToString();
        }
    }

    /// <summary>
    /// Loading Progress of Image
    /// </summary>
    void ImageLoadingProgress()
    {
        if (!_loader && _loadStatus == LoadingStatus.InProgress)
            _loader = _loaderPool.Spawn(_loaderParent);
        else if (_loader && _loadStatus != LoadingStatus.InProgress)
            _loaderPool.Remove(_loader);
    }

    /// <summary>
    /// Calls when progress is on the way
    /// </summary>
    /// <param name="progress">float value from 0 - 1</param>
    /// <param name="info">text of the laoding</param>
    void OnProgress(float progress, string info)
    {
        if (_loader)
            _loader.ShowProgress(progress, "Loading Image...");
    }

    /// <summary>
    /// Calls when the downloading is done
    /// </summary>
    /// <param name="isDone">is download successfull</param>
    /// <param name="data">data to download in this case the sprite</param>
    void OnCompleted(bool isDone, Sprite data)
    {
        _loadStatus = isDone ? LoadingStatus.Completed : LoadingStatus.Fail;

        if (!isDone)
            return;

        SetupUI();

        ImageLoadingProgress();
    }

    /// <summary>
    /// Item Pool
    /// </summary>
    public class Pool : MonoMemoryPool<Transform, EventsData, EventItemUI>
    {
        protected override void Reinitialize(Transform p1, EventsData p2, EventItemUI item)
        {
            base.Reinitialize(p1, p2, item);
            item.Setup(p1, p2);
        }
    }
}
