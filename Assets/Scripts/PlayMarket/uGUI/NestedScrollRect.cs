using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestedScrollRect : ScrollRect
{
    private bool _routeToParent;
    private ScrollRect _parentScrollRect;

    protected override void Awake()
    {
        base.Awake();

        Transform parent = transform.parent;

        while (parent != null)
        {
            ScrollRect candidate = parent.GetComponent<ScrollRect>();

            if (candidate != null)
            {
                _parentScrollRect = candidate;
                break;
            }

            parent = parent.parent;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        _routeToParent = ShouldRouteToParent(eventData);

        if (_routeToParent && _parentScrollRect != null)
        {
            _parentScrollRect.OnBeginDrag(eventData);
        }
        else
        {
            base.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (_routeToParent && _parentScrollRect != null)
        {
            _parentScrollRect.OnDrag(eventData);
        }
        else
        {
            base.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (_routeToParent && _parentScrollRect != null)
        {
            _parentScrollRect.OnEndDrag(eventData);
        }
        else
        {
            base.OnEndDrag(eventData);
        }

        _routeToParent = false;
    }

    public override void OnScroll(PointerEventData eventData)
    {
        bool horizontalDominant =
            Mathf.Abs(eventData.scrollDelta.x) > Mathf.Abs(eventData.scrollDelta.y);

        bool shouldRoute =
            (!horizontal && horizontalDominant) ||
            (!vertical && !horizontalDominant);

        if (shouldRoute && _parentScrollRect != null)
        {
            _parentScrollRect.OnScroll(eventData);
        }
        else
        {
            base.OnScroll(eventData);
        }
    }

    private bool ShouldRouteToParent(PointerEventData eventData)
    {
        float absX = Mathf.Abs(eventData.delta.x);
        float absY = Mathf.Abs(eventData.delta.y);

        if (!vertical && absY > absX)
        {
            return true;
        }

        if (!horizontal && absX > absY)
        {
            return true;
        }

        return false;
    }
}