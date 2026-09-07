using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppTabsController : MonoBehaviour
{
    [SerializeField] private AppCatalog _catalog;
    [SerializeField] private AppCardView _cardPrefab;
    [SerializeField] private ScreenManager _screenManager;

    [SerializeField] private Transform _tabsRoot;

    private Dictionary<AppCategory, AppTabView> _tabsByCategory;

    private void Awake()
    {
        AppTabView[] tabs = _tabsRoot.GetComponentsInChildren<AppTabView>(true);

        _tabsByCategory = tabs
            .GroupBy(tab => tab.Category)
            .ToDictionary(group => group.Key, group => group.First());
    }

    private void Start()
    {
        BuildTabs();
    }

    private void BuildTabs()
    {
        foreach (AppTabView tab in _tabsByCategory.Values)
        {
            ClearContent(tab.Content);
        }

        foreach (AppData app in _catalog.Apps)
        {
            if (!_tabsByCategory.TryGetValue(app.Category, out AppTabView tab))
            {
                Debug.LogWarning(
                    $"No tab exists for category {app.Category}. " +
                    $"App '{app.AppName}' was skipped."
                );

                continue;
            }

            AppCardView card = Instantiate(_cardPrefab, tab.Content);

            card.Setup(app, OnAppClicked);
        }
    }

    private void ClearContent(Transform content)
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    private void OnAppClicked(AppData app)
    {
        _screenManager.OpenDetails(app);
    }
}