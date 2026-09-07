using UnityEngine;
using UnityEngine.UI;

public class AppTabView : MonoBehaviour
{
    [SerializeField] private AppCategory _category;

    [SerializeField] private Transform _content;

    public AppCategory Category => _category;
    public Transform Content => _content;

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}