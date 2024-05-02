using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

/// <summary>
/// Attach on the parent of the event
/// </summary>
public class EventDataUI : MonoBehaviour
{
    /// <summary>
    /// Data to load from the URL
    /// </summary>
    [SerializeField]
    private string url = "https://script.google.com/macros/s/AKfycbxHLP8yjErEQhU-BLTMJE6i0MllRpEPF2qTcVx0xNoe7Ur34dPqIG8bh4ay3CwUoQL4/exec";

    /// <summary>
    /// Fetch Data of the Event Injected object
    /// </summary>
    [Inject]
    private IFetchData<List<EventsData>> _fetchData;
    /// <summary>
    /// Pool of the loading
    /// </summary>
    [Inject]
    private FSLoadingPool _loaderPool;
    /// <summary>
    /// Pool of the item spawn in the pool
    /// </summary>
    [Inject]
    private EventItemPool _eventItemPool;

    /// <summary>
    /// Cache the event data in this variable
    /// </summary>
    [SerializeField]
    private List<EventsData> _eventsData;

    /// <summary>
    /// FS loading parent
    /// </summary>
    [SerializeField]
    private Transform loaderParent;
    /// <summary>
    /// Event item to spawn parent
    /// </summary>
    [SerializeField]
    private Transform eventItemParent;

    /// <summary>
    /// Spawned FSLoader reference
    /// </summary>
    private FSLoading loader;
    /// <summary>
    /// Loading status of the data
    /// </summary>
    private LoadingStatus _loadStatus;
    /// <summary>
    /// Message to show on the FS Loading
    /// </summary>
    private string _message = "Loading Events Data.";

    private void Awake()
    {
        // Fetch Data and wait till it's downloaded
        StartCoroutine(_fetchData.FetchData(url, OnProgress, OnCompleted));
    }

    private void OnEnable()
    {
        // Update load status
        if (_loadStatus == LoadingStatus.InProgress)
        {
            loader = _loaderPool.Spawn(loaderParent);
            // Update message while loading
            DOTween.To(() => _message, x => _message = x, _message + "...", 2).SetLoops(10);
        }
    }

    /// <summary>
    /// Calls when the data loading is in progress
    /// </summary>
    /// <param name="progress">progress of the laoding 0 - 1</param>
    /// <param name="info">message to display on the progress</param>
    void OnProgress(float progress, string info)
    {
        _loadStatus = LoadingStatus.InProgress;
        if (loader)
            loader.ShowProgress(progress, _message);
    }

    /// <summary>
    /// Calls when data is laoded
    /// </summary>
    /// <param name="isDone">is it completed or fail</param>
    /// <param name="data">output data</param>
    void OnCompleted(bool isDone, List<EventsData> data)
    {
        _loadStatus = isDone ? LoadingStatus.Completed : LoadingStatus.Fail;

        if (!isDone)
            return;

        _eventsData = data;
        // order downloaded data by the event type
        // active at top, coming soon at second and expired at below
        _eventsData = _eventsData.OrderBy(x => (int)x.EventType).ToList();
        // run for every item
        _eventsData.ForEach(item =>
        {
            // initialize item
            item.Init();
            // spawn item UI from the pool
            _eventItemPool.Spawn(eventItemParent, item);
        });

        if (loader)
            _loaderPool.Remove(loader);
    }
}
