using System.Collections.Generic;
using _Scripts.GameFlow.Menu;
using _Scripts.GameFlow.Transitions;
using UnityEngine;
using UnityEngine.EventSystems;
using _Scripts.Interaction.Action;
using _Scripts.Interaction.Interactable;
using _Scripts.Player.Management;
using _Scripts.Player.Structure;
using _Scripts.Player.Unit;

namespace _Scripts.Interaction.Management
{
	public class InputHandler : MonoBehaviour
	{
		public static InputHandler instance;

		private RaycastHit hit;

		public List<PlayerUnit> selectedUnits;
		public List<PlayerWorker> selectedWorkers;
		public PlayerBarracks selectedBarracks;
		public PlayerTower selectedTower;

		public bool isDragging;
		private Vector3 mousePosition;

		private Camera cam;

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
						if (AddedUnit(hit.transform.gameObject.GetComponent<PlayerUnit>(),
							    Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
						{
							DeselectBarracks();
							DeselectTower();
							DeselectWorkers();
							// do unit stuff
						}
						
						if (AddedWorker(hit.transform.gameObject.GetComponent<PlayerWorker>(),
							    Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
						{
							DeselectBarracks();
							DeselectTower();
							DeselectUnits();
							// do unit stuff
						}

						if (AddedBarracks(hit.transform.gameObject.GetComponent<PlayerBarracks>()))
						{
							// do structure stuff
							DeselectTower();
							DeselectUnits();
							DeselectWorkers();
						}
					}
					else
					{
						isDragging = true;
						DeselectBarracks();
						DeselectTower();
						DeselectUnits();
						DeselectWorkers();
					}
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				foreach (Transform child in PlayerManager.instance.playerUnits)
				{
					// rework for object pools later - to get rid of GetComponent
					if (IsWithinSelectionBounds(child))
					{
						if (child.CompareTag("Unit"))
						{
							AddedUnit(child.gameObject.GetComponent<PlayerUnit>(), true);
						}
						else if (child.CompareTag("Worker"))
						{
							AddedWorker(child.gameObject.GetComponent<PlayerWorker>(), true);
						}
					}
				}

				isDragging = false;
			}
		}

		private void HandleUnitOrders()
		{
			if (Input.GetMouseButtonDown(1) && (HaveSelectedUnits() || HaveSelectedWorkers()))
			{
				mousePosition = Input.mousePosition;

				Ray ray = cam.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit))
				{
					foreach (PlayerUnit unit in selectedUnits)
					{
						unit.MoveUnit(hit.point);
					}

					foreach (PlayerWorker worker in selectedWorkers)
					{
						worker.MoveWorker(hit.point);
					}
				}
			}
		}

		private void HandleStructurePrototype()
		{
			if (selectedBarracks)
			{
				if (selectedBarracks.isPrototype)
				{
					selectedBarracks.UpdatePrototypePosition();

					if (Input.GetKeyDown(KeyCode.Space))
					{
						if (BuildingHandler.instance.TryToPlace())
						{
							selectedBarracks.isPrototype = false;
							selectedBarracks.StartConstruction();
						}
					}

					if (Input.GetKeyDown(KeyCode.T))
					{
						selectedBarracks.barracksPlacable.Rotate();
					}
				}
			}
			
			if (selectedTower)
			{
				if (selectedTower.isPrototype)
				{
					selectedTower.UpdatePrototypePosition();

					if (Input.GetKeyDown(KeyCode.Space))
					{
						if (BuildingHandler.instance.TryToPlace())
						{
							selectedTower.isPrototype = false;
							selectedTower.StartConstruction();
						}
					}

					if (Input.GetKeyDown(KeyCode.T))
					{
						selectedTower.towerPlacable.Rotate();
					}
				}
			}
		}

		private void HandleHotkeys()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				ActionFrame.instance.ActivateButton(0);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				ActionFrame.instance.ActivateButton(1);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				ActionFrame.instance.ActivateButton(2);
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				ActionFrame.instance.ActivateButton(3);
			}
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				ActionFrame.instance.ActivateButton(4);
			}
			if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				ActionFrame.instance.ActivateButton(5);
			}
			if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				ActionFrame.instance.ActivateButton(6);
			}
			if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				ActionFrame.instance.ActivateButton(7);
			}
			if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				ActionFrame.instance.ActivateButton(8);
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (PauseMenuHandler.instance.isGamePaused)
				{
					if (ControlViewHandler.instance.isControlViewActive)
					{
						ControlViewHandler.instance.Resume();
					}
					else
					{
						PauseMenuHandler.instance.Resume();
					}
				}
				else
				{
					DeselectAll();
					DeselectWorkers();
				}
			}
			if (Input.GetKeyDown(KeyCode.G))
			{
				StageTransitionHandler.instance.LoadStage();
			}
			if (Input.GetKeyDown(KeyCode.F1))
			{
				if (ControlViewHandler.instance.isControlViewActive)
				{
					ControlViewHandler.instance.Resume();
				}
				else
				{
					ControlViewHandler.instance.Pause();
				}
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				if (PauseMenuHandler.instance.isGamePaused)
				{
					PauseMenuHandler.instance.Resume();
				}
				else
				{
					PauseMenuHandler.instance.Pause();
				}
			}
		}

		public void FirstSelectBarracks(PlayerBarracks pB)
		{
			DeselectTower();
			DeselectUnits();
			AddedBarracks(pB);
		}
		
		public void FirstSelectTower(PlayerTower pT)
		{
			DeselectBarracks();
			DeselectUnits();
			AddedTower(pT);
		}

		public void DeselectAll()
		{
			DeselectBarracks();
			DeselectTower();
			DeselectUnits();
		}

		private void DeselectUnits()
		{
			foreach (PlayerUnit selectedUnit in selectedUnits)
			{
				selectedUnit.interactable.OnInteractExit();
			}

			selectedUnits.Clear();
		}
		
		private void DeselectWorkers()
		{
			foreach (PlayerWorker worker in selectedWorkers)
			{
				worker.interactable.OnInteractExit();
			}

			selectedWorkers.Clear();
		}

		private void DeselectBarracks()
		{
			if (selectedBarracks)
			{
				selectedBarracks.interactable.OnInteractExit();
				
				if (selectedBarracks.isPrototype)
				{
					Destroy(selectedBarracks.gameObject);
				}

				selectedBarracks = null;
			}
		}
		
		private void DeselectTower()
		{
			if (selectedTower)
			{
				selectedTower.interactable.OnInteractExit();
				
				if (selectedTower.isPrototype)
				{
					Destroy(selectedTower.gameObject);
				}

				selectedTower = null;
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

		public bool HaveSelectedUnits()
		{
			return selectedUnits.Count > 0;
		}
		
		public bool HaveSelectedWorkers()
		{
			return selectedWorkers.Count > 0;
		}

		private InteractableUnit AddedUnit(PlayerUnit pU, bool canMultiSelect = false)
		{
			if (pU == null)
			{
				return null;
			}

			InteractableUnit iUnit = pU.interactable;
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
		
		private InteractableWorker AddedWorker(PlayerWorker pW, bool canMultiSelect = false)
		{
			if (pW == null)
			{
				return null;
			}

			InteractableWorker iUnit = pW.interactable;
			if (iUnit)
			{
				if (!canMultiSelect)
				{
					DeselectWorkers();
				}

				selectedWorkers.Add(pW);
				iUnit.OnInteractEnter();

				return iUnit;
			}

			return null;
		}

		private InteractableBarracks AddedBarracks(PlayerBarracks pB)
		{
			if (pB == null)
			{
				return null;
			}

			InteractableBarracks iBarracks = pB.interactable;
			if (iBarracks)
			{
				selectedBarracks = pB;
				iBarracks.OnInteractEnter();

				return iBarracks;
			}

			return null;
		}
		
		private InteractableTower AddedTower(PlayerTower pT)
		{
			if (pT == null)
			{
				return null;
			}

			InteractableTower iTower = pT.interactable;
			if (iTower)
			{
				selectedTower = pT;
				iTower.OnInteractEnter();

				return iTower;
			}

			return null;
		}
	}
}