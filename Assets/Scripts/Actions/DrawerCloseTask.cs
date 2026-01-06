using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DrawerCloseTask : MonoBehaviour
{
    [Header("Configuración del Cajón")]
    public Transform drawerRoot;       // El objeto con el Rigidbody/Joint
    public Vector3 closedLocalPos;     // La posición local (0,0,0) donde el cajón está cerrado
    public float closeThreshold = 0.02f; // Margen de error (2cm)
    public XRGrabInteractable handleGrab; // La manija para desactivarla

    [Header("Referencias de Tarea")]
    public TensionController tensionController;
    public TMP_Text taskTextToStrike;
    public AudioSource successAudio;

    private bool taskCompleted = false;

    void Update()
    {
        if (!taskCompleted && drawerRoot != null)
        {
            // Calculamos qué tan lejos está el cajón de su meta "Cerrado"
            float distance = Vector3.Distance(drawerRoot.localPosition, closedLocalPos);

            // Si el jugador lo empujó hasta la meta
            if (distance <= closeThreshold)
            {
                CompleteTask();
            }
        }
    }

    void CompleteTask()
    {
        taskCompleted = true;

        // 1. Ajuste perfecto: Forzamos la posición final para que no quede un poco abierto
        drawerRoot.localPosition = closedLocalPos;

        // 2. Bloqueo físico
        Rigidbody rb = drawerRoot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Ya no se mueve más
        }

        // 3. Desactivar agarre
        if (handleGrab != null)
        {
            handleGrab.enabled = false;
        }

        // --- Lógica de éxito ---
        if (tensionController != null) tensionController.TaskCompleted();

        if (taskTextToStrike != null)
            taskTextToStrike.text = $"<s>{taskTextToStrike.text}</s>";

        if (successAudio != null) successAudio.Play();

        Debug.Log("¡Cajón cerrado con éxito!");
    }
}