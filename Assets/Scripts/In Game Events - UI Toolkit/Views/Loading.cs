using UnityEngine.UIElements;

public class Loading
{
    private ProgressBar _loadingSlider;
    private TextElement _loadingText;

    public Loading(VisualElement _loading)
    {
        _loadingSlider = _loading.Q<ProgressBar>("LoadingProgress");
        _loadingText = _loading.Q<TextElement>("LoadingText");
    }

    public void UpdateLoading(float progress, string _message)
    {
        _loadingText.text = _message;
        _loadingSlider.lowValue = (int)(progress * 100);
        _loadingSlider.title = string.Format("{0} %", (int)(progress * 100));
    }
}