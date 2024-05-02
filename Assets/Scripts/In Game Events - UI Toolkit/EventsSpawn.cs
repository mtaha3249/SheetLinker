using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

/// <summary>
/// Fetch and Spawn Events and attached to the UI Document
/// </summary>
public class EventsSpawn : MonoBehaviour
{
    /// <summary>
    /// Download Data URL
    /// </summary>
    [SerializeField]
    private string url = "https://script.google.com/macros/s/AKfycbxHLP8yjErEQhU-BLTMJE6i0MllRpEPF2qTcVx0xNoe7Ur34dPqIG8bh4ay3CwUoQL4/exec";

    /// <summary>
    /// Injected downloaded data
    /// </summary>
    [Inject]
    private IFetchData<List<EventsData>> _fetchData;

    /// <summary>
    /// cached event data
    /// </summary>
    [SerializeField]
    private List<EventsData> _eventsData;

    /// <summary>
    /// Main UI Document
    /// Events UI
    /// </summary>
    [SerializeField] private UIDocument _document;
    /// <summary>
    /// Event Item Document
    /// </summary>
    [SerializeField] private VisualTreeAsset _eventItem;
    /// <summary>
    /// Full Screen Loading Document
    /// </summary>
    [SerializeField] private VisualTreeAsset _fsLoading;

    /// <summary>
    /// Cache UI elements
    /// </summary>
    private VisualElement _eventUI, _loadingFS;
    private ScrollView _scrollview;

    /// <summary>
    /// Loading to show
    /// </summary>
    private Loading _loading;

    /// <summary>
    /// Loading Status
    /// </summary>
    private LoadingStatus _loadingStatus;
    private string _message = "Loading Events Data.";

    private void Awake()
    {
        _eventUI = _document.rootVisualElement;
    }

    private void OnEnable()
    {
        // fetch data
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
        
        // loading data from sheet
        StartCoroutine(_fetchData.FetchData(url, OnProgress, OnCompleted));
        RefreshLoadingUI();

        if (_loadingStatus == LoadingStatus.InProgress)
        {
            DOTween.To(() => _message, x => _message = x, _message + "...", 2).SetLoops(10);
        }
    }

    /// <summary>
    /// Calls when data is downloading from sheet
    /// </summary>
    /// <param name="progress">progress from 0 - 1</param>
    /// <param name="info">message to show</param>
    void OnProgress(float progress, string info)
    {
        _loadingStatus = LoadingStatus.InProgress;
        _loading.UpdateLoading(progress, _message);
    }

    /// <summary>
    /// Calls when data is downloaded
    /// </summary>
    /// <param name="isDone">is completed or fail</param>
    /// <param name="data">loaded data</param>
    void OnCompleted(bool isDone, List<EventsData> data)
    {
        _loadingStatus = isDone ? LoadingStatus.Completed : LoadingStatus.Fail;

        if (!isDone)
            return;

        _eventsData = data;
        // order by the event type, Active on Top, Coming soon and Expired at last
        _eventsData = _eventsData.OrderBy(x => (int)x.EventType).ToList();
        // iterate each item
        _eventsData.ForEach(item =>
        {
            // initialize data
            item.Init();
            // create item
            EventItem _item = new EventItem(item, _eventItem);
            _scrollview.Add(_item._item);
            // download image of the data
            StartCoroutine(_item.FetchIcon());
        });

        RefreshLoadingUI();
    }

    /// <summary>
    /// Update loading status
    /// </summary>
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