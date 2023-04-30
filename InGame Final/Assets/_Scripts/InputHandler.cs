using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    private RaycastHit hit;
    
    public List<Transform> selectedUnits = new List<Transform>();
    public Transform selectedStructure = null;

    public bool isDragging = false;
	private Vector3 mousePosition;

	private Camera cam;
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        cam = Camera.main;
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
	        if (EventSystem.current.IsPointerOverGameObject())
	        {
		        return;
	        }
	        
	        mousePosition = Input.mousePosition;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                LayerMask layerHit = hit.transform.gameObject.layer;
                
                if (layerHit.value == 8)
                {
	                if (AddedUnit(hit.transform, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
	                {
		                DeselectStructure();
		                // do unit stuff
	                }

	                if (AddedTrainer(hit.transform))
	                {
		                // do trainer stuff
	                }
                }
                else
                {
	                isDragging = true;
	                DeselectUnits();
                }
            }
            // for selection over void (probably unwanted behavior)
            /*else
            {
	            isDragging = true;
	            DeselectUnits();
            }*/
        }

		if (Input.GetMouseButtonUp(0))
		{
			foreach (Transform child in PlayerManager.instance.playerUnits)
	        {
				// for single category of units
				/*if (isWithinSelectionBounds(child))
                {
                   	AddedUnit(child, true);
               	}*/


				// for when we add multiple categories of units
				foreach (Transform unit in child)
				{
					if (isWithinSelectionBounds(unit))
                    {
	                    AddedUnit(unit, true);
                    }
            	}
			}
			isDragging = false;
		}

		if (Input.GetMouseButtonDown(1) && haveSelectedUnits())
		{
			mousePosition = Input.mousePosition;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                LayerMask layerHit = hit.transform.gameObject.layer;

                switch (layerHit.value)
                {
                    case 8:
						// do something? (player unit layer)
                        break;
    				case 16:                
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

    private void DeselectUnits()
    {
	    DeselectStructure();
	    
        foreach (Transform selectedUnit in selectedUnits)
        {
	        selectedUnit.gameObject.GetComponent<IUnit>().OnInteractExit();
        }
        selectedUnits.Clear();
    }

    private void DeselectStructure()
    {
	    if (selectedStructure)
	    {
		    selectedStructure.gameObject.GetComponent<ITrainer>().OnInteractExit();
		    selectedStructure = null;
	    }
    }
    
	private bool isWithinSelectionBounds(Transform tf)
    {
        if (!isDragging)
        {
            return false;
        }
        
        Bounds viewportBounds = MultiSelect.GetViewportBounds(cam, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(cam.WorldToViewportPoint(tf.position));
    }

	private bool haveSelectedUnits()
    {
        return selectedUnits.Count > 0;
    }

	private IUnit AddedUnit(Transform tf, bool canMultiSelect = false)
	{
		IUnit iUnit = tf.GetComponent<IUnit>();
		if (iUnit)
		{
			if (!canMultiSelect)
			{
				DeselectUnits();
			}
			
			selectedUnits.Add(iUnit.gameObject.transform);
			iUnit.OnInteractEnter();
			
			return iUnit;
		}
		return null;
	}
	
	private ITrainer AddedTrainer(Transform tf)
	{
		ITrainer iTrainer = tf.GetComponent<ITrainer>();
		if (iTrainer)
		{
			DeselectUnits();
			
			selectedStructure = iTrainer.gameObject.transform;
			iTrainer.OnInteractEnter();
			
			return iTrainer;
		}
		return null;
	}
	
	
}
