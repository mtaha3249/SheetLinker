using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class EventsSpawn : MonoBehaviour
{
    [SerializeField]
    private string url = "https://script.google.com/macros/s/AKfycbxHLP8yjErEQhU-BLTMJE6i0MllRpEPF2qTcVx0xNoe7Ur34dPqIG8bh4ay3CwUoQL4/exec";

    [Inject]
    private IFetchData<List<EventsData>> _fetchData;

    [SerializeField]
    private List<EventsData> _eventsData;

    [SerializeField] private UIDocument _document;
    [SerializeField] private VisualTreeAsset _eventItem;
    [SerializeField] private VisualTreeAsset _fsLoading;

    private VisualElement _eventUI, _loadingFS;
    private ScrollView _scrollview;

    private Loading _loading;

    private LoadingStatus _loadingStatus;
    private string _message = "Loading Events Data.";

    private void Awake()
    {
        _eventUI = _document.rootVisualElement;
    }

    private void OnEnable()
    {
        _scrollview = _eventUI.Q<ScrollView>("EventsItems");
        _loadingFS = _eventUI.Q<VisualElement>("Loading");

        _loadingFS.style.display = DisplayStyle.None;
        _scrollview.style.display = DisplayStyle.None;
    }

    private void Start()
    {
        _loadingStatus = LoadingStatus.InProgress;

        if (_loading == null)
            _loading = new Loading(_loadingFS);

        StartCoroutine(_fetchData.FetchData(url, OnProgress, OnCompleted));
        RefreshLoadingUI();

        if (_loadingStatus == LoadingStatus.InProgress)
        {
            DOTween.To(() => _message, x => _message = x, _message + "...", 2).SetLoops(10);
        }
    }

    void OnProgress(float progress, string info)
    {
        _loadingStatus = LoadingStatus.InProgress;
        _loading.UpdateLoading(progress, _message);
    }

    void OnCompleted(bool isDone, List<EventsData> data)
    {
        _loadingStatus = isDone ? LoadingStatus.Completed : LoadingStatus.Fail;

        if (!isDone)
            return;

        _eventsData = data;
        _eventsData = _eventsData.OrderBy(x => (int)x.EventType).ToList();
        _eventsData.ForEach(item =>
        {
            item.Init();
            EventItem _item = new EventItem(item, _eventItem);
            _scrollview.Add(_item._item);
            StartCoroutine(_item.FetchIcon());
        });

        RefreshLoadingUI();
    }

    void RefreshLoadingUI()
    {
        if (_loadingStatus == LoadingStatus.InProgress)
        {
            _loadingFS.style.display = DisplayStyle.Flex;
            _scrollview.style.display = DisplayStyle.None;
        }
        else
        {
            _loadingFS.style.display = DisplayStyle.None;
            _scrollview.style.display = DisplayStyle.Flex;
        }
    }
}