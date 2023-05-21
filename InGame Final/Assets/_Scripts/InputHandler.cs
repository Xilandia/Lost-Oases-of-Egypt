using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    private RaycastHit hit;
    
    public List<PlayerUnit> selectedUnits;
    public PlayerTrainer selectedStructure;

    public bool isDragging;
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

    public void HandlePlayerInput()
    {
	    HandleMouseClickAndDrag();
	    HandleUnitOrders();
	    HandleStructurePrototype();
	    HandleHotkeys();
    }

    private void HandleMouseClickAndDrag()
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
				    if (AddedUnit(hit.transform.gameObject.GetComponent<PlayerUnit>(), Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
				    {
					    DeselectStructure();
					    // do unit stuff
				    }

				    if (AddedTrainer(hit.transform.gameObject.GetComponent<PlayerTrainer>()))
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
			    // rework for object pools later - to get rid of GetComponent
			    if (IsWithinSelectionBounds(child))
			    {
				    AddedUnit(child.gameObject.GetComponent<PlayerUnit>(), true);
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
    }

    private void HandleUnitOrders()
    {
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
					    foreach (PlayerUnit unit in selectedUnits)
					    {
						    unit.MoveUnit(hit.point);
					    }
					    break;
			    }
		    }
	    }
    }

    private void HandleStructurePrototype()
    {
	    if (selectedStructure)
	    {
		    if (selectedStructure.isPrototype)
		    {
			    selectedStructure.UpdatePrototypePosition();

			    if (Input.GetKeyDown(KeyCode.Space))
			    {
				    if (BuildingHandler.instance.TryToPlace())
				    {
					    selectedStructure.isPrototype = false;
					    selectedStructure.StartConstruction();
				    }
			    }
			    if (Input.GetKeyDown(KeyCode.T))
			    {
				    selectedStructure.trainerPlacable.Rotate();
			    }
				
			    if (Input.GetKeyDown(KeyCode.Escape))
			    {
				    DeselectStructure();
			    }
		    }
	    }
    }

    private void HandleHotkeys()
    {
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
    
    public void FirstSelectStructure(PlayerTrainer pU)
	{
	    AddedTrainer(pU);
	}

    private void DeselectUnits()
    {
	    DeselectStructure();
	    
        foreach (PlayerUnit selectedUnit in selectedUnits)
        {
	        selectedUnit.interactable.OnInteractExit();
        }
        selectedUnits.Clear();
    }

    private void DeselectStructure()
    {
	    if (selectedStructure)
	    {
		    selectedStructure.interactable.OnInteractExit();
		    if (selectedStructure.isPrototype)
		    {
			    Destroy(selectedStructure.gameObject);
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

	private IUnit AddedUnit(PlayerUnit pU, bool canMultiSelect = false)
	{
		if (pU == null)
		{
			return null;
		}
		
		IUnit iUnit = pU.interactable;
		if (iUnit)
		{
			if (!canMultiSelect)
			{
				DeselectUnits();
			}
			
			selectedUnits.Add(pU);
			iUnit.OnInteractEnter();
			
			return iUnit;
		}
		return null;
	}
	
	private ITrainer AddedTrainer(PlayerTrainer pT)
	{
		if (pT == null)
		{
			return null;
		}
		
		ITrainer iTrainer = pT.interactable;
		if (iTrainer)
		{
			DeselectUnits();
			
			selectedStructure = pT;
			iTrainer.OnInteractEnter();
			
			return iTrainer;
		}
		return null;
	}
	
	
}
