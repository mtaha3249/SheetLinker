using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Class component attach to the prefab of loading
/// </summary>
public class FSLoading : MonoBehaviour
{
    /// <summary>
    /// Text to show on the loading
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _title;
    /// <summary>
    /// % of loading done in this process
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _progress;

    /// <summary>
    /// The image fillamount on the loading
    /// </summary>
    [SerializeField]
    private Image _fill;

    /// <summary>
    /// Calls when the loading prefab is spawned
    /// </summary>
    /// <param name="parent">parent to be of this object</param>
    public void Setup(Transform parent)
    {
        RectTransform transform = GetComponent<RectTransform>();
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.offsetMin = Vector2.zero;
        transform.offsetMax = Vector2.zero;
    }

    /// <summary>
    /// Calls when need to update the loading
    /// </summary>
    /// <param name="progress">value of the progress from 0 - 1</param>
    /// <param name="info">text information of the loading / like what is loading can be different text</param>
    public void ShowProgress(float progress, string info)
    {
        _fill.fillAmount = progress;
        _progress.text = string.Format("{0} %", (int)(progress * 100.00));
        _title.text = info;
    }

    /// <summary>
    /// Pool of the Loading
    /// </summary>
    public class Pool : MonoMemoryPool<Transform, FSLoading>
    {
        /// <summary>
        /// Calls on the initialize of the item initialize
        /// </summary>
        /// <param name="p1">parent to be</param>
        /// <param name="item">item spawned</param>
        protected override void Reinitialize(Transform p1, FSLoading item)
        {
            base.Reinitialize(p1, item);
            item.Setup(p1);
        }
    }
}
