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

    [SerializeField] private GameObject m_forgePrefab;
    [SerializeField] private GameObject m_tmpForgePrefab;

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
                switch (m_buildMode)
                {
                    case BuildModes.NONE:
                        break;
                    case BuildModes.FORGE:
                        GameObject forge = Instantiate(m_forgePrefab);
                        forge.name = "Forge";
                        forge.transform.position = m_tmpBuilding.transform.position;
                        break;
                    default:
                        break;
                }

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
        if (m_buildMode == BuildModes.FORGE)
        {
            m_buildMode = BuildModes.NONE;
            Destroy(m_tmpBuilding);
            m_tmpBuilding = null;
        }
        else
        {
            m_buildMode = BuildModes.FORGE;
            m_tmpBuilding = Instantiate(m_tmpForgePrefab);
        }
    }
}
