using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class EventDataUI : MonoBehaviour
{
    [SerializeField]
    private string url = "https://script.google.com/macros/s/AKfycbxHLP8yjErEQhU-BLTMJE6i0MllRpEPF2qTcVx0xNoe7Ur34dPqIG8bh4ay3CwUoQL4/exec";

    [Inject]
    private IFetchData<List<EventsData>> _fetchData;
    [Inject]
    private FSLoadingPool _loaderPool;
    [Inject]
    private EventItemPool _eventItemPool;

    [SerializeField]
    private List<EventsData> _eventsData;

    [SerializeField]
    private Transform loaderParent;
    [SerializeField]
    private Transform eventItemParent;

    private FSLoading loader;
    private LoadingStatus _loadStatus;
    private string _message = "Loading Events Data.";

    private void Awake()
    {
        StartCoroutine(_fetchData.FetchData(url, OnProgress, OnCompleted));
    }

    private void OnEnable()
    {
        if (_loadStatus == LoadingStatus.InProgress)
        {
            loader = _loaderPool.Spawn(loaderParent);
            DOTween.To(() => _message, x => _message = x, _message + "...", 2).SetLoops(10);
        }
    }

    void OnProgress(float progress, string info)
    {
        _loadStatus = LoadingStatus.InProgress;
        if (loader)
            loader.ShowProgress(progress, _message);
    }

    void OnCompleted(bool isDone, List<EventsData> data)
    {
        _loadStatus = isDone ? LoadingStatus.Completed : LoadingStatus.Fail;

        if (!isDone)
            return;

        _eventsData = data;
        _eventsData = _eventsData.OrderBy(x => (int)x.EventType).ToList();
        _eventsData.ForEach(item =>
        {
            item.Init();
            _eventItemPool.Spawn(eventItemParent, item);
        });

        if (loader)
            _loaderPool.Remove(loader);
    }
}
