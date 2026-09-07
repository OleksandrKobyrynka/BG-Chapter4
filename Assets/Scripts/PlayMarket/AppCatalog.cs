using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;

[CreateAssetMenu(fileName = "AppsCatalog", menuName = "Play Market/App Catalog")]
public class AppCatalog : ScriptableObject
{
    [SerializeField] private List<AppData> _apps = new();

    [CreateProperty]
    public IReadOnlyList<AppData> Apps => _apps;
}