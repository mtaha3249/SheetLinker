using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EventItem
{
    private VisualElement _bg, _fg, _icon, _loadingVisual;
    private TextElement _title, _timeleft, _description;
    private Button _playButton;

    public VisualElement _item;
    private EventsData _data;
    private Loading _loading;
    private LoadingStatus _loadingStatus;
    private string _message = "Loading Image.";

    public EventItem(EventsData data, VisualTreeAsset itemUI)
    {
        _data = data;
        _item = itemUI.Instantiate();
        FetchUIElements();
        SetupUIElements();

        if (_loadingStatus == LoadingStatus.InProgress)
        {
            DOTween.To(() => _message, x => _message = x, _message + "...", 2).SetLoops(10);
        }
    }

    void FetchUIElements()
    {
        _bg = _item.Q<VisualElement>("BG");
        _fg = _item.Q<VisualElement>("FG");
        _icon = _item.Q<VisualElement>("Icon");
        _title = _item.Q<TextElement>("Title");
        _timeleft = _item.Q<TextElement>("TimeLeft");
        _description = _item.Q<TextElement>("Description");
        _playButton = _item.Q<Button>("Play");
        _loadingVisual = _item.Q<VisualElement>("Loading");

        if (_loading == null)
            _loading = new Loading(_loadingVisual);
    }

    void SetupUIElements()
    {
        _title.text = _data.EventTitle;
        _fg.style.backgroundColor = _data.TitleColor;
        _timeleft.text = string.Format("TimeLeft: \t {0}d {1}h {2}m {3}s", _data.EventTime.Days, _data.EventTime.Hours, _data.EventTime.Minutes, _data.EventTime.Seconds);
        _bg.style.backgroundColor = _data.BodyColor;
        _description.text = _data.Description;
        _playButton.text = _data.EventType.ToString();

        if (_data.IconSprite != null)
            _icon.style.backgroundImage = new StyleBackground(_data.IconSprite);
        else
        {
            _loadingStatus = LoadingStatus.InProgress;
            RefreshLoadingUI();
        }

        if (_data.EventType == EventType.ComingSoon)
        {
            _timeleft.style.display = DisplayStyle.None;
        }
        else if (_data.EventType == EventType.Expired)
        {
            _playButton.style.display = DisplayStyle.None;
            _timeleft.text = _data.EventType.ToString();
        }
    }

    public IEnumerator FetchIcon() => _data.FetchData(_data.IconUrl, OnProgress, OnCompleted);

    private void OnProgress(float progress, string message)
    {
        _loadingStatus = LoadingStatus.InProgress;
        _loading.UpdateLoading(progress, _message);
    }

    private void OnCompleted(bool isDone, Sprite icon)
    {
        _loadingStatus = isDone ? LoadingStatus.Completed : LoadingStatus.Fail;

        if (!isDone)
            return;

        SetupUIElements();
        RefreshLoadingUI();
    }

    void RefreshLoadingUI()
    {
        if (_loadingStatus == LoadingStatus.InProgress)
        {
            _loadingVisual.style.display = DisplayStyle.Flex;
            _icon.style.display = DisplayStyle.None;
        }
        else if (_loadingStatus == LoadingStatus.Completed)
        {
            _loadingVisual.style.display = DisplayStyle.None;
            _icon.style.display = DisplayStyle.Flex;
        }
        else
        {
            Debug.Log("Error In Downloading");
        }
    }
}
