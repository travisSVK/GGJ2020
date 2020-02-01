using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : MonoBehaviour
{
    private enum BuildModes
    {
        NONE,
        FORGE
    }

    [SerializeField] private GameObject m_forge;

    private GameObject m_tmpBuilding = null;

    private BuildModes m_buildMode = BuildModes.NONE;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            EnterForgeMode();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {

        }

        if (m_buildMode != BuildModes.NONE)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_tmpBuilding.transform.position = new Vector3(Mathf.Round(position.x + 0.5f) - 0.5f, Mathf.Round(position.y + 0.5f) - 0.5f, 0.0f);

            if (Input.GetMouseButtonDown(0))
            {
                m_buildMode = BuildModes.NONE;
                m_tmpBuilding = null;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_buildMode = BuildModes.NONE;
                    Destroy(m_tmpBuilding);
                    m_tmpBuilding = null;
                }
            }
        }
    }

    private void EnterForgeMode()
    {
        m_tmpBuilding = Instantiate(m_forge);
        if (m_buildMode == BuildModes.FORGE)
        {
            m_buildMode = BuildModes.NONE;
            Destroy(m_tmpBuilding);
        }
        else
        {
            m_buildMode = BuildModes.FORGE;
        }
    }
}
