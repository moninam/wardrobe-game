using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class KeySocketTask : MonoBehaviour
{
    public XRSocketInteractor socket;
    public TensionController tensionController;
    public TMP_Text taskTextToStrike;
    public AudioSource successAudio;

    private bool taskCompleted = false;

    void Update()
    {
        if (!taskCompleted && socket.hasSelection)
        {
            taskCompleted = true;

            // Marca tarea como resuelta
            tensionController.TaskCompleted();

            // Tachar texto
            if (taskTextToStrike != null)
                taskTextToStrike.text = $"<s>{taskTextToStrike.text}</s>";

            // Sonido de éxito
            if (successAudio != null)
                successAudio.Play();
        }
    }
}
