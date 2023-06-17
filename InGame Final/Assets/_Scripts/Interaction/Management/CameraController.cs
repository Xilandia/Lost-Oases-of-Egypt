using UnityEngine;

namespace _Scripts.Interaction.Management
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        public Transform cameraTransform;

        public float normalSpeed;
        public float fastSpeed;
        public float movementSpeed;
        public float movementTime;
        public float rotationAmount;
        public Vector3 zoomAmount;
        public Vector3 zoomMin;
        public Vector3 zoomMax;

        public Vector3 newPosition;
        public Quaternion newRotation;
        public Vector3 newZoom;
        
        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        // Update is called once per frame
        // Called after standard update methods
        void LateUpdate()
        {
            HandleMovementInput();
        }

        void HandleMovementInput()
        {
            // Checks if shift keys are held down to determine movement speed
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            // Handles movement
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -movementSpeed);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -movementSpeed);
            }

            // Handles rotation
            if (Input.GetKey(KeyCode.Q))
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }

            if (Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            // Handles zoom
            if (Input.GetKey(KeyCode.R))
            {
                newZoom += zoomAmount;

                if (newZoom.y < 100) // add indicator (sound?)
                {
                    newZoom = zoomMin;
                }
            }

            if (Input.GetKey(KeyCode.F))
            {
                newZoom -= zoomAmount;
                
                if (newZoom.y > 400) // add indicator (sound?)
                {
                    newZoom = zoomMax;
                }
            }

            // Applies camera changes (movement, rotation, zoom)
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition =
                Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

        }
    }
}
