using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollButtonController : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private Button _leftScrollButton;
    [SerializeField] private Button _rightScrollButton;

    [SerializeField] private float _scrollDuration = 0.25f;
    [SerializeField, Range(0.1f, 1f)] private float _scrollStepMultiplier = 0.5f;

    private Coroutine _scrollCoroutine;

    private void OnEnable()
    {
        _leftScrollButton.onClick.AddListener(ScrollLeft);
        _rightScrollButton.onClick.AddListener(ScrollRight);
    }

    private void OnDisable()
    {
        _leftScrollButton.onClick.RemoveListener(ScrollLeft);
        _rightScrollButton.onClick.RemoveListener(ScrollRight);
    }

    private void ScrollLeft() => AnimateScroll(GetScrollAmount());
    private void ScrollRight() => AnimateScroll(-GetScrollAmount());

    private float GetScrollAmount() => _viewport.rect.width * _scrollStepMultiplier;

    private void AnimateScroll(float deltaX)
    {
        float maxX = 0f;
        float minX = Mathf.Min(0, _viewport.rect.width - _content.rect.width);

        float targetX = Mathf.Clamp(_content.anchoredPosition.x + deltaX, minX, maxX);

        if (Mathf.Approximately(_content.anchoredPosition.x, targetX))
        {
            return;
        }

        if (_scrollCoroutine != null) StopCoroutine(_scrollCoroutine);
        {
            _scrollCoroutine = StartCoroutine(SmoothScroll(targetX));
        }
    }

    private IEnumerator SmoothScroll(float targetX)
    {
        float startX = _content.anchoredPosition.x;
        float elapsed = 0f;

        while (elapsed < _scrollDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _scrollDuration);
            float eased = 1f - Mathf.Pow(1f - t, 3f);

            _content.anchoredPosition = new Vector2(
                Mathf.Lerp(startX, targetX, eased),
                _content.anchoredPosition.y
            );
            yield return null;
        }

        _content.anchoredPosition = new Vector2(targetX, _content.anchoredPosition.y);
        _scrollCoroutine = null;
    }

    public void ResetToStart()
    {
        if (_scrollCoroutine != null)
        {
            StopCoroutine(_scrollCoroutine);
            _scrollCoroutine = null;
        }

        _content.anchoredPosition = new Vector2(0f, _content.anchoredPosition.y);
    }
}