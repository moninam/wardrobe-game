using UnityEngine;

public class TensionController : MonoBehaviour
{
    [Header("Control de Tensión")]
    public float tension = 0f;
    public float maxTension = 100f;
    public int unresolvedTasks = 3;

    [Header("Vibración del Armario")]
    public Transform armarioVisual;
    public float vibrationAmplitude = 0.02f;
    public float vibrationSpeed = 10f;

    [Header("Sonido Interno")]
    public AudioSource source;
    public float maxVolume = 1f;

    [Header("Explosión")]
    public GameObject modeloArmario;          // Objeto visual que desaparecerá
    public GameObject explosionEffect;        // Prefab de partículas
    public AudioClip explosionSound;

    [Header("Pantalla de Game Over")]
    public GameObject gameOverCanvas;         // UI desactivada al inicio

    private bool hasExploded = false;
    private float originalY;

    void Start()
    {
        if (armarioVisual != null)
            originalY = armarioVisual.localPosition.y;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);
    }

    void Update()
    {
        if (hasExploded) return;

        // Solo sube tensión si hay tareas pendientes
        if (unresolvedTasks > 0)
        {
            tension += (100f / 60f) * Time.deltaTime;
            tension = Mathf.Clamp(tension, 0, maxTension);
        }

        float intensity = tension / maxTension;

        // Vibración creciente
        if (armarioVisual != null)
        {
            float offset = Mathf.Sin(Time.time * vibrationSpeed * (1 + intensity * 2)) * vibrationAmplitude * intensity;
            Vector3 pos = armarioVisual.localPosition;
            pos.y = originalY + offset;
            armarioVisual.localPosition = pos;
        }

        // Sonido progresivo
        if (source != null)
        {
            source.volume = Mathf.Lerp(0f, maxVolume, intensity);
        }

        // Si la tensión llegó al máximo, detonar explosión
        if (tension >= maxTension && !hasExploded)
        {
            ExplodeArmario();
        }
    }

    void ExplodeArmario()
    {
        hasExploded = true;

        // Detener sonido
        if (source != null)
            source.Stop();

        // Reproducir sonido de explosión
        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        // Instanciar partículas
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Desactivar el modelo visual del armario
        if (modeloArmario != null)
            modeloArmario.SetActive(false);

        // Activar la pantalla de Game Over
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        Debug.Log("Armario explotó, GAME OVER");
    }

    public void AddTension(float amount)
    {
        tension = Mathf.Clamp(tension + amount, 0, maxTension);
    }

    public void ResetTension()
    {
        tension = 0f;
        hasExploded = false;
        unresolvedTasks = 3;

        if (source != null) source.Play();
        if (modeloArmario != null) modeloArmario.SetActive(true);
        if (gameOverCanvas != null) gameOverCanvas.SetActive(false);
    }

    public void TaskCompleted()
    {
        unresolvedTasks = Mathf.Max(0, unresolvedTasks - 1);
    }
}
