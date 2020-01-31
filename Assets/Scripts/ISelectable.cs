using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    void OnBeginHover();

    void OnEndHover();

    void OnSelect();

    void OnDeselect();
}
