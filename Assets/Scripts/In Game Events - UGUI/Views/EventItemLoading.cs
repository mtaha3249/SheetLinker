using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EventItemLoading : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _progress;

    [SerializeField]
    private Image _fill;

    public void Setup(Transform parent)
    {
        RectTransform transform = GetComponent<RectTransform>();
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.offsetMin = Vector2.zero;
        transform.offsetMax = Vector2.zero;
    }

    public void ShowProgress(float progress, string info)
    {
        _fill.fillAmount = progress;
        _progress.text = string.Format("{0} %", (int)(progress * 100.00));
        _title.text = info;
    }

    public class Pool : MonoMemoryPool<Transform, EventItemLoading>
    {
        protected override void Reinitialize(Transform p1, EventItemLoading item)
        {
            base.Reinitialize(p1, item);
            item.Setup(p1);
        }
    }
}
