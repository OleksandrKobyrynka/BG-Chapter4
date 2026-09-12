using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayMarketController : MonoBehaviour
{
    [Header("Documents and data")]
    [SerializeField] private UIDocument _document;
    [SerializeField] private AppCatalog _catalog;

    [Header("Templates")]
    [SerializeField] private VisualTreeAsset _appCardTemplate;
    [SerializeField] private VisualTreeAsset _screenshotItemTemplate;

    private readonly Dictionary<AppCategory, VisualElement> _tabsByCategory = new();

    private VisualElement _mainScreen;
    private VisualElement _detailsScreen;

    private Button _backButton;
    private ScrollView _detailsScroll;
    private ScrollView _screenshotsView;
    private VisualElement _screenshotsContent;

    private VisualElement _screenshotOverlay;
    private VisualElement _screenshotOverlayBackground;
    private Image _fullscreenScreenshotImage;
    private Button _closeScreenshotButton;

    private void OnEnable()
    {
        VisualElement root = _document.rootVisualElement;

        CacheStaticElements(root);
        CacheTabs(root);
        BuildAllTabs();

        _backButton.clicked += ReturnToMain;

        _closeScreenshotButton.clicked += CloseScreenshotOverlay;
    }

    private void OnDisable()
    {
        if (_backButton != null)
        {
            _backButton.clicked -= ReturnToMain;
        }

        if (_closeScreenshotButton != null)
        {
            _closeScreenshotButton.clicked -= CloseScreenshotOverlay;
        }
    }

    private void CacheStaticElements(VisualElement root)
    {
        _mainScreen = root.Q<VisualElement>("main-screen");
        _detailsScreen = root.Q<VisualElement>("details-screen");

        _backButton = root.Q<Button>("back-button");
        _detailsScroll = root.Q<ScrollView>("details-scroll");
        _screenshotsView = root.Q<ScrollView>("screenshots-view");
        _screenshotsContent = root.Q<VisualElement>("screenshots-content");

        _screenshotOverlay = root.Q<VisualElement>("screenshot-overlay");
        _screenshotOverlayBackground = root.Q<VisualElement>("screenshot-overlay-background");
        _fullscreenScreenshotImage = root.Q<Image>("fullscreen-screenshot-image");
        _closeScreenshotButton = root.Q<Button>("close-screenshot-button");

        if (_mainScreen == null)
        {
            Debug.LogError("Element 'main-screen' was not found.", this);
        }

        if (_detailsScreen == null)
        {
            Debug.LogError("Element 'details-screen' was not found.", this);
        }

        if (_backButton == null)
        {
            Debug.LogError("Button 'back-button' was not found.", this);
        }

        if (_screenshotsContent == null)
        {
            Debug.LogError("Element 'screenshots-content' was not found.", this);
        }

        if (_screenshotOverlay == null)
        {
            Debug.LogError("Element 'screenshot-overlay' was not found.", this);
        }

        if (_fullscreenScreenshotImage == null)
        {
            Debug.LogError("Image 'fullscreen-screenshot-image' was not found.", this);
        }

        if (_closeScreenshotButton == null)
        {
            Debug.LogError("Button 'close-screenshot-button' was not found.", this);
        }
    }

    private void CacheTabs(VisualElement root)
    {
        _tabsByCategory.Clear();

        foreach (AppCategory category in Enum.GetValues(typeof(AppCategory)))
        {
            string tabName = $"{category.ToString().ToLowerInvariant()}-tab-container";

            VisualElement tab = root.Q<VisualElement>(tabName);

            if (tab == null)
            {
                Debug.LogWarning(
                    $"No tab found with name '{tabName}' for category '{category}'.",
                    this
                );

                continue;
            }

            VisualElement grid = tab.Q<VisualElement>(className: "apps-container");

            if (grid == null)
            {
                Debug.LogWarning(
                    $"No element with class 'apps-container' found inside '{tabName}'.",
                    this
                );

                continue;
            }

            _tabsByCategory[category] = grid;
        }
    }

    private void BuildAllTabs()
    {
        foreach (VisualElement tab in _tabsByCategory.Values)
        {
            tab.Clear();
        }

        foreach (AppData app in _catalog.Apps)
        {
            if (!_tabsByCategory.TryGetValue(app.Category, out VisualElement tab))
            {
                Debug.LogWarning(
                    $"No tab exists for category '{app.Category}'. " +
                    $"App '{app.AppName}' was skipped.",
                    this
                );

                continue;
            }

            VisualElement card = _appCardTemplate.Instantiate();

            card.dataSource = app;

            card.RegisterCallback<ClickEvent>(_ => OpenDetails(app));

            tab.Add(card);
        }
    }

    private void OpenDetails(AppData app)
    {
        _detailsScreen.dataSource = app;

        BuildScreenshots(app);
        ResetDetailsScrollPosition();

        _mainScreen.AddToClassList("main-screen--closed");

        _detailsScreen.RemoveFromClassList("details-screen--open");
        _detailsScreen.AddToClassList("details-screen--open");

        _mainScreen.pickingMode = PickingMode.Ignore;
        _detailsScreen.pickingMode = PickingMode.Position;
    }

    private void ReturnToMain()
    {
        _mainScreen.RemoveFromClassList("main-screen--closed");
        _detailsScreen.RemoveFromClassList("details-screen--open");

        _mainScreen.pickingMode = PickingMode.Position;
        _detailsScreen.pickingMode = PickingMode.Ignore;
    }

    private void BuildScreenshots(AppData app)
    {
        _screenshotsContent.Clear();

        foreach (Sprite screenshot in app.Screenshots)
        {
            VisualElement screenshotItem = _screenshotItemTemplate.Instantiate();

            Image image = screenshotItem.Q<Image>("screenshot-image");

            if (image != null)
            {
                image.sprite = screenshot;
                image.scaleMode = ScaleMode.ScaleToFit;
            }

            screenshotItem.RegisterCallback<ClickEvent>(_ => OpenScreenshotOverlay(screenshot));

            _screenshotsContent.Add(screenshotItem);
        }
    }

    private void ResetDetailsScrollPosition()
    {
        if (_detailsScroll != null)
        {
            _detailsScroll.scrollOffset = Vector2.zero;
        }

        if (_screenshotsView != null)
        {
            _screenshotsView.scrollOffset = Vector2.zero;
        }
    }

    private void OpenScreenshotOverlay(Sprite screenshot)
    {
        if (screenshot == null || _screenshotOverlay == null || _fullscreenScreenshotImage == null)
        {
            return;
        }

        _fullscreenScreenshotImage.sprite = screenshot;
        _fullscreenScreenshotImage.scaleMode = ScaleMode.ScaleToFit;

        _screenshotOverlay.AddToClassList("screenshot-overlay--open");

        _screenshotOverlay.pickingMode = PickingMode.Position;
    }

    private void CloseScreenshotOverlay()
    {
        if (_screenshotOverlay == null)
        {
            return;
        }

        _screenshotOverlay.RemoveFromClassList("screenshot-overlay--open");

        _screenshotOverlay.pickingMode = PickingMode.Ignore;

        if (_fullscreenScreenshotImage != null)
        {
            _fullscreenScreenshotImage.sprite = null;
        }
    }
}