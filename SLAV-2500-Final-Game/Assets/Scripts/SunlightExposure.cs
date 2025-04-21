using UnityEngine;

public class CoverDetector2D : MonoBehaviour
{
    [Tooltip("Which layers count as blocking cover")]
    public LayerMask coverLayer;

    private Transform player;
    public PlayerMovement player_movement;

    void Start()
    {
        // If this script lives on the Light object (child of Player), grab the parent
        player = transform.parent;
        if (player == null)
            Debug.LogWarning("CoverDetector2D: couldn't find parent as player!");
    }

    void Update()
    {
        // 1) Determine origin (light) and target (player) positions
        Vector2 origin = transform.position;
        Vector2 target = player.position;
        Vector2 direction = (target - origin).normalized;
        float distance = Vector2.Distance(origin, target);

        // 2) Cast a ray, looking only for colliders on the Cover layer
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, coverLayer);

        // 3) If nothing was hit, the player is exposed
        if (hit.collider == null)
        {
            // Debug.Log(playerObj.isDead);
            player_movement.Die();
        }

        // (Optional) Draw the ray in the Scene view: green if blocked, red if exposed
        Debug.DrawLine(origin, target, hit.collider != null ? Color.green : Color.red);
    }
}
