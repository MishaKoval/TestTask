using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    [SerializeField] private float desroyTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,desroyTime);
    }
}
