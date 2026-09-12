using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollController : MonoBehaviour
{
    [SerializeField] private RectTransform _content;

    private void OnEnable()
    {
        ResetToStart();
    }

    public void ResetToStart()
    {
        _content.anchoredPosition = new Vector2(0f, _content.anchoredPosition.y);
    }
}