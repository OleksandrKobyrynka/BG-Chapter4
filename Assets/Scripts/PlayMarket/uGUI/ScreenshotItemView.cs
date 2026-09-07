using UnityEngine;
using UnityEngine.UI;

public class ScreenshotItemView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;

    public void Setup(Sprite sprite)
    {
        _image.sprite = sprite;
        _image.preserveAspect = true;

        if (sprite != null)
        {
            float aspectRatio = sprite.rect.width / sprite.rect.height;

            _aspectRatioFitter.aspectRatio = aspectRatio;
        }
    }
}