using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainScreen;
    [SerializeField] private AppDetailsView _detailsView;

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
}