using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainScreen;
    [SerializeField] private AppDetailsView _detailsView;
    [SerializeField] private ScreenshotPanelView _screenshotView;

    public void OpenDetails(AppData app)
    {
        _mainScreen.SetActive(false);
        _detailsView.Show(app);
    }

    public void CloseDetails()
    {
        _detailsView.Hide();
        _mainScreen.SetActive(true);
    }

    public void CloseScreenshot()
    {
        _screenshotView.Hide();
    }
}