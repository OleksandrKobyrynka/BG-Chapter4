using UnityEngine;
using UnityEngine.UI;

public class ScreenshotPanelView : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private Image _image;
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    public void OpenScreenshot(Sprite sprite)
    {
        _image.sprite = sprite;
        _image.preserveAspect = true;

        if (sprite != null)
        {
            float aspectRatio = sprite.rect.width / sprite.rect.height;

            _aspectRatioFitter.aspectRatio = aspectRatio;
        }

        _panel.SetActive(true);
    }

    public void Hide()
    {
        _image.sprite = null;
        _panel.SetActive(false);
    }
}