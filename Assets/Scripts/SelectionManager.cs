using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public ISelectable currentHover = null;

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);

        if (hit)
        {
            ISelectable hoverObject = hit.collider.GetComponent<ISelectable>();
            if (currentHover != null && currentHover != hoverObject)
            {
                currentHover.OnEndHover();
                currentHover = hoverObject;
                if (currentHover != null)
                {
                    currentHover.OnBeginHover();
                }
            }
        }
    }
}
