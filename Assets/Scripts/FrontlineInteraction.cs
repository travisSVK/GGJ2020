﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontlineInteraction : MonoBehaviour, ISelectable
{
    [SerializeField] private SpriteRenderer m_inactive = null;
    [SerializeField] private SpriteRenderer m_active = null;

    public void Awake()
    {
        m_inactive.enabled = true;
        m_active.enabled = false;
    }

    public void OnBeginHover()
    {
        m_inactive.enabled = false;
        m_active.enabled = true;
    }

    public void OnEndHover()
    {
        m_inactive.enabled = true;
        m_active.enabled = false;
    }

    public void OnSelect()
    {

    }

    public void OnDeselect()
    {
    }
}
