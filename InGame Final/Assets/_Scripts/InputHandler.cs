using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    private RaycastHit hit;
    
    private List<Transform> selectedUnits = new List<Transform>();

	private bool isDragging = false;
	private Vector3 mousePosition;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

	private void OnGUI()
	{
		if (isDragging)
		{
			Rect rect = MultiSelect.GetScreenRectangle(mousePosition, Input.mousePosition);
			MultiSelect.DrawScreenRectangle(rect, new Color(0f, 0f, 0f, 0.25f)); 
			MultiSelect.DrawScreenRectangleBorder(rect, 3, Color.blue);
		}
	}

    public void HandleUnitMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {

			mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                LayerMask layerHit = hit.transform.gameObject.layer;

                switch (layerHit.value)
                {
                    case 8:
                        SelectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
                        break;
                    default:
						isDragging = true;
                        DeselectUnits();
                        break;
                }
            }
        }

		if (Input.GetMouseButtonUp(0))
		{
			foreach (Transform child in PlayerManager.instance.playerUnits)
	        {
				// for single category of units
				/*if (isWithinSelectionBounds(child))
                {
                   	SelectUnit(child, true);
               	}*/


				// for when we add multiple categories of units
				foreach (Transform unit in child)
				{	
                	if (isWithinSelectionBounds(unit))
                	{
                    	SelectUnit(unit, true);
                	}
            	}
			}
			isDragging = false;
		}

		if (Input.GetMouseButtonDown(1) && haveSelectedUnits())
		{
			mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                LayerMask layerHit = hit.transform.gameObject.layer;

                switch (layerHit.value)
                {
                    case 8:
						// do something? (player unit layer)
                        break;
    				case 9:                
						break;
					default:
						foreach (Transform unit in selectedUnits)
						{
							PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
							pU.MoveUnit(hit.point);
						}
                        break;
                }
            }
		}

    }

    private void SelectUnit(Transform unit, bool multiSelect = false)
    {
        if (!multiSelect)
        {
            DeselectUnits();
        }
        selectedUnits.Add(unit);
        unit.Find("Highlight").gameObject.SetActive(true);
    }
    
    private void DeselectUnits()
    {
        foreach (Transform selectedUnit in selectedUnits)
        {
            selectedUnit.Find("Highlight").gameObject.SetActive(false);
        }
        selectedUnits.Clear();
    }

	private bool isWithinSelectionBounds(Transform tf)
    {
        if (!isDragging)
        {
            return false;
        }

        Camera cam = Camera.main;
        Bounds viewportBounds = MultiSelect.GetViewportBounds(cam, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(cam.WorldToViewportPoint(tf.position));
    }

	private bool haveSelectedUnits()
    {
        return selectedUnits.Count > 0;
    }

}
