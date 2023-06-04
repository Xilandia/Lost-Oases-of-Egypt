using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    private RaycastHit hit;
    
    public List<Transform> selectedUnits;
    public Transform selectedStructure;

    public bool isDragging;
	private Vector3 mousePosition;
	private bool structureIsPrototype;

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

    public void HandlePlayerInput()
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
		                // do structure stuff
	                }
                }
                else
                {
	                isDragging = true;
	                DeselectUnits();
                }
            }
        }

		if (Input.GetMouseButtonUp(0))
		{
			foreach (Transform child in PlayerManager.instance.playerUnits)
	        {
				// for single category of units
				if (IsWithinSelectionBounds(child))
                {
                   	AddedUnit(child, true);
               	}


				// for when there are nested categories of units
				/*foreach (Transform unit in child)
				{
					if (IsWithinSelectionBounds(unit))
                    {
	                    AddedUnit(unit, true);
                    }
            	}*/
			}
			isDragging = false;
		}

		if (Input.GetMouseButtonDown(1) && HaveSelectedUnits())
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
	                    // do something? (enemy unit layer)
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

		if (selectedStructure)
		{
			if (structureIsPrototype)
			{
				PlayerTrainer pT = selectedStructure.gameObject.GetComponent<PlayerTrainer>();
				pT.UpdatePrototypePosition();

				if (Input.GetKeyDown(KeyCode.Space))
				{
					if (BuildingHandler.instance.TryToPlace())
					{
						pT.isPrototype = false;
						structureIsPrototype = false;
						pT.StartConstruction();
					}
				}
				if (Input.GetKeyDown(KeyCode.T))
				{
					selectedStructure.gameObject.GetComponent<PlacableObject>().Rotate();
				}
				
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					DeselectStructure();
				}
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Z))
		{
			BuildingHandler.instance.InitializeWithObject(0);
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			BuildingHandler.instance.InitializeWithObject(1);
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			BuildingHandler.instance.InitializeWithObject(2);
		}
		
    }
    
    public void FirstSelectStructure(Transform tf)
	{
	    AddedTrainer(tf);
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
		    if (structureIsPrototype)
		    {
			    Destroy(selectedStructure.gameObject);
			    structureIsPrototype = false;
		    }
		    selectedStructure = null;
	    }
    }
    
	private bool IsWithinSelectionBounds(Transform tf)
    {
        if (!isDragging)
        {
            return false;
        }
        
        Bounds viewportBounds = MultiSelect.GetViewportBounds(cam, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(cam.WorldToViewportPoint(tf.position));
    }

	private bool HaveSelectedUnits()
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
			
			PlayerTrainer structure = iTrainer.gameObject.GetComponent<PlayerTrainer>();
			structureIsPrototype = structure.isPrototype;
			
			return iTrainer;
		}
		return null;
	}
	
	
}
