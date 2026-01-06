using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OpenDoorTask : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    public HingeJoint doorHinge;
    public float openThreshold = 45f;
    public XRGrabInteractable grabInteractable; // Arrastra la manija aquí

    [Header("Referencias de Tarea")]
    public TensionController tensionController;
    public TMP_Text taskTextToStrike;
    public AudioSource successAudio;

    private bool taskCompleted = false;

    void Update()
    {
        if (!taskCompleted && doorHinge != null)
        {
            float currentAngle = Mathf.Abs(doorHinge.angle);

            if (currentAngle >= openThreshold)
            {
                CompleteTask();
            }
        }
    }

    void CompleteTask()
    {
        taskCompleted = true;

        // 1. Bloquear la puerta físicamente
        Rigidbody rb = doorHinge.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Hace que la puerta ya no responda a empujones
        }

        // 2. Evitar que el jugador la vuelva a agarrar
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        // --- Lógica original de tu script ---
        if (tensionController != null)
            tensionController.TaskCompleted();

        if (taskTextToStrike != null)
            taskTextToStrike.text = $"<s>{taskTextToStrike.text}</s>";

        if (successAudio != null)
            successAudio.Play();

        Debug.Log("¡Puerta abierta y bloqueada!");
    }
}