using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    #region Inspector

    [SerializeField] private float damageRadius = 1.0f;

    [SerializeField] private float waitTime = 3.0f;

    [SerializeField] private GameObject stoneEffect;

    [SerializeField] private GameObject bombEffect;

    [SerializeField] private float yOffset = 1.0f;

    #endregion

    #region Fields

    private Animator anim;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hero")|| other.CompareTag("Farmer")|| other.CompareTag("Dog"))
        {
            StartCoroutine(WaitFire());
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator WaitFire()
    {
        anim.SetBool("IsExpose", true);
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("IsExpose", false);
        Fire();
    }

    #endregion
    
    private void Fire()
    {
        Instantiate(bombEffect, transform.position, Quaternion.identity);
        var colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var coll in colliders)
        {
            if (coll.CompareTag("Stone"))
            {
                var position = coll.transform.position;
                Vector3 pos = new Vector3(position.x, position.y + yOffset, position.z);
                Instantiate(stoneEffect, pos, Quaternion.identity);
                Destroy(coll.gameObject);
            }

            if (coll.CompareTag("Hero"))
            {
                coll.GetComponent<Pig>()?.onDamaged.Invoke();
            }

            if (coll.CompareTag("Farmer"))
            {
                coll.GetComponent<Farmer>()?.onBombed.Invoke();
            }

            if (coll.CompareTag("Dog"))
            {
                coll.GetComponent<Dog>()?.onBombed.Invoke();
            }

            Destroy(gameObject);
        }
    }
}