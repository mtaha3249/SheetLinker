using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EventItemUI : MonoBehaviour
{
    [Inject]
    private EventItemLoadingPool _loaderPool;

    [SerializeField]
    private EventsData _data;
    [SerializeField]
    private Transform _loaderParent;
    [Header("UI")]
    [SerializeField]
    private Image _titleBg;
    [SerializeField]
    private Image _backgroundBg;
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _time;
    [SerializeField]
    private TextMeshProUGUI _description;
    [SerializeField]
    private TextMeshProUGUI _buttonText;

    private EventItemLoading _loader;
    private LoadingStatus _loadStatus;

    public void Setup(Transform parent, EventsData data)
    {
        _data = data;

        _loadStatus = LoadingStatus.InProgress;
        StartCoroutine(_data.FetchData(_data.IconUrl, OnProgress, OnCompleted));
        ImageLoadingProgress();

        RectTransform transform = GetComponent<RectTransform>();
        transform.parent = parent;

        SetupUI();
    }

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

    void ImageLoadingProgress()
    {
        if (!_loader && _loadStatus == LoadingStatus.InProgress)
            _loader = _loaderPool.Spawn(_loaderParent);
        else if (_loader && _loadStatus != LoadingStatus.InProgress)
            _loaderPool.Remove(_loader);
    }

    void OnProgress(float progress, string info)
    {
        if (_loader)
            _loader.ShowProgress(progress, "Loading Image...");
    }

    void OnCompleted(bool isDone, Sprite data)
    {
        _loadStatus = isDone ? LoadingStatus.Completed : LoadingStatus.Fail;

        if (!isDone)
            return;

        SetupUI();

        ImageLoadingProgress();
    }

    public class Pool : MonoMemoryPool<Transform, EventsData, EventItemUI>
    {
        protected override void Reinitialize(Transform p1, EventsData p2, EventItemUI item)
        {
            base.Reinitialize(p1, p2, item);
            item.Setup(p1, p2);
        }
    }
}
