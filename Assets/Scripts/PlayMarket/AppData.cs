using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public enum AppCategory
{
    Apps,
    Games
}

[CreateAssetMenu(fileName = "NewApp", menuName = "Play Market/App Data")]
public class AppData : ScriptableObject
{
    [Header("ID")]
    [SerializeField] private string _id;

    [Header("Main card")]
    [SerializeField] private string _appName;
    [SerializeField] private string _developer;
    [SerializeField, Range(0f, 5f)] private float _rating = 4.5f;
    [SerializeField] private AppCategory _category;
    [SerializeField] private Sprite _icon;

    [Header("Details")]
    [SerializeField, TextArea(4, 12)]
    private string _description;

    [SerializeField] private Sprite[] _screenshots;

    [CreateProperty]
    public string AppName => _appName;
    [CreateProperty]
    public string Developer => _developer;
    [CreateProperty]
    public string RatingText => _rating.ToString("0.0");
    [CreateProperty]
    public AppCategory Category => _category;
    [CreateProperty]
    public Sprite Icon => _icon;
    [CreateProperty]
    public string Description => _description;
    [CreateProperty]
    public IReadOnlyList<Sprite> Screenshots => _screenshots;
}