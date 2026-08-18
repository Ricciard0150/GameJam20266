using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] float _interactionRange = 3f;
    private Camera _mainCam;
    private IInteractable _hit;

    void Start()
    {
        _mainCam = Camera.main;
        Debug.Log("✅ Interaction inicializado");
    }

    void Update()
    {
        if (_mainCam == null)
        {
            Debug.LogError("❌ _mainCam é NULL!");
            return;
        }

        if (!Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out RaycastHit hit, _interactionRange))
        {
            _hit?.HideOutline();
            _hit = null;
            return;
        }

        if (hit.collider.TryGetComponent(out IInteractable interactable))
        {
            if (_hit == interactable)
                return;

            _hit?.HideOutline();
            _hit = interactable;
            _hit.ShowOutline();
        }
        else
        {
            _hit?.HideOutline();
            _hit = null;
        }

        // Interação com tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_hit != null)
            {
                _hit.Interact();
            }
        }
    }
}