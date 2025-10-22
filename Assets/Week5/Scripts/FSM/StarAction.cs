using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/Star")]
public class StarAction : Action
{
    public float duration = 5f;
    public AudioClip invincibilityStart;

    public override void Act(StateController controller)
    {
        // Start the coroutine on the controller (which IS a MonoBehaviour)
        controller.StartCoroutine(Invincibility(controller));
        BuffStateController b = (BuffStateController)controller;
        b.gameObject.GetComponent<AudioSource>().time = 0.2f; // start playback 0.8s into the clip
        b.gameObject.GetComponent<AudioSource>().PlayOneShot(invincibilityStart);
    }

    private IEnumerator Invincibility(StateController controller)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemies");
        int playerLayer = controller.gameObject.layer;

        // Disable collision between Mario and enemies
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        SpriteRenderer sr = controller.GetComponent<SpriteRenderer>();
        Color[] colors = new Color[]
        {
            Color.yellow, Color.cyan, Color.magenta, Color.white
        };

        for (float t = 0; t < duration;)
        {
            sr.color = colors[Random.Range(0, colors.Length)];

            float normalized = t / duration;
            float currentRate = Mathf.Lerp(0.15f, 0.01f, normalized); // faster near end

            yield return new WaitForSeconds(currentRate);
            t += currentRate;
        }

        // Reset to normal
        sr.color = Color.white;

        // Re-enable collisions after time ends
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }
}
