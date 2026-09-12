using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppDetailsView : MonoBehaviour
{
    [Header("Main info")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _developerText;
    [SerializeField] private TMP_Text _ratingText;
    [SerializeField] private TMP_Text _descriptionText;

    [Header("Screenshots")]
    [SerializeField] private RectTransform _screenshotsContent;
    [SerializeField] private ScreenshotItemView _screenshotPrefab;
    [SerializeField] private ScrollController _screenshotsScrollController;

    [Header("Panels")]
    [SerializeField] private GameObject _detailsPanel;
    [SerializeField] private ScreenshotPanelView _screenshotPanel;

    public void Show(AppData app)
    {
        _iconImage.sprite = app.Icon;
        _nameText.text = app.AppName;
        _developerText.text = app.Developer;
        _ratingText.text = app.RatingText;
        _descriptionText.text = app.Description;

        ClearScreenshots();
        CreateScreenshots(app);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_screenshotsContent);

        _screenshotsScrollController.ResetToStart();

        _detailsPanel.SetActive(true);
    }

    public void Hide()
    {
        _detailsPanel.SetActive(false);
    }

    private void ClearScreenshots()
    {
        for (int i = _screenshotsContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_screenshotsContent.GetChild(i).gameObject);
        }
    }

    private void CreateScreenshots(AppData app)
    {
        foreach (Sprite screenshot in app.Screenshots)
        {
            ScreenshotItemView item = Instantiate(_screenshotPrefab, _screenshotsContent);

            item.Setup(screenshot, OnScreenshotClicked);
        }
    }

    private void OnScreenshotClicked(Sprite sprite)
    {
        _screenshotPanel.OpenScreenshot(sprite);
    }
}