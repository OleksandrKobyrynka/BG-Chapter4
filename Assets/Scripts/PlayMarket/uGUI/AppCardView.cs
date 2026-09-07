using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppCardView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _ratingText;
    [SerializeField] private Button _button;

    private AppData _app;

    public void Setup(AppData app, Action<AppData> onClick)
    {
        _app = app;

        _iconImage.sprite = app.Icon;
        _nameText.text = app.AppName;
        _ratingText.text = app.RatingText;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            onClick?.Invoke(_app);
        });
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}