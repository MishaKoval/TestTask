using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Farmer : MonoBehaviour
{
    private enum FarmerState
    {
        Default = 0,
        Dirty = 1,
        Angry = 2
    }

    #region Inspector

    [SerializeField] private List<Sprite> sprites;

    [SerializeField] private List<Sprite> angrySprites;

    [SerializeField] private List<Sprite> dirtySprites;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed = 2.0f;

    [SerializeField] private float searchYOffset = 2.0f;

    [SerializeField] private float dirChangeTime = 3.0f;
    
    [SerializeField] private float waitBombTime = 1.5f;
    
    [SerializeField] public UnityEvent onBombed;
    
    #endregion

    #region Fields

    private Animator anim;

    private Rigidbody rb;

    private AudioSource audioSource;

    private Vector3 movement;

    private MoveState moveState;

    private FarmerState farmerState;
    
    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        onBombed.AddListener(() => StartCoroutine(WaitBombAnim()));
        onBombed.AddListener(()=> audioSource.Play());
    }

    void Start()
    {
        farmerState = FarmerState.Default;
        moveState = MoveState.Right;
        movement = transform.right * -1;
        StartCoroutine(ChangeDirection());
        StartCoroutine(FindPig());
    }


    void Update()
    {
        CheckDirection();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    private void LateUpdate()
    {
        if (CheckBorders())
        {
            movement *= -1;
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator FindPig()
    {
        while (gameObject)
        {
            if (SearchPig())
            {
                farmerState = FarmerState.Angry;
                speed *= 2;
            }

            yield return new WaitForSeconds(3.0f);
            speed = 2.0f;
            farmerState = FarmerState.Default;
        }
    }

    private IEnumerator WaitBombAnim()
    {
        speed /= 2;
        anim.SetBool("IsBombed", true);
        yield return new WaitForSeconds(waitBombTime);
        farmerState = FarmerState.Dirty;
        anim.SetBool("IsBombed", false);
        yield return new WaitForSeconds(waitBombTime * 2);
        farmerState = FarmerState.Default;
        speed = 2.0f;
    }

    private IEnumerator ChangeDirection()
    {
        while (gameObject)
        {
            yield return new WaitForSeconds(dirChangeTime);
            var rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    movement = Vector3.right.normalized;
                    break;
                case 1:
                    movement = Vector3.left.normalized;
                    break;
                case 2:
                    movement = Vector3.forward.normalized;
                    break;
                case 3:
                    movement = Vector3.back.normalized;
                    break;
            }
        }
    }

    #endregion

    private void CheckDirection()
    {
        int spriteId;
        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.z))
        {
            if (rb.velocity.x > 0)
            {
                spriteId = (int) MoveState.Right;
                moveState = MoveState.Right;
            }
            else
            {
                spriteId = (int) MoveState.Left;
                moveState = MoveState.Left;
            }
        }
        else
        {
            if (rb.velocity.z > 0)
            {
                spriteId = (int) MoveState.Up;
                moveState = MoveState.Up;
            }
            else
            {
                spriteId = (int) MoveState.Down;
                moveState = MoveState.Down;
            }
        }

        switch (farmerState)
        {
            case FarmerState.Default:
                spriteRenderer.sprite = sprites[spriteId];
                break;
            case FarmerState.Angry:
                spriteRenderer.sprite = angrySprites[spriteId];
                break;
            case FarmerState.Dirty:
                spriteRenderer.sprite = dirtySprites[spriteId];
                break;
        }
    }

    private bool CheckBorders()
    {
        Vector3 dir = new Vector3();
        switch (moveState)
        {
            case MoveState.Left:
            case MoveState.Right:
                dir = Vector3.right * Mathf.Sign(rb.velocity.x);
                break;
            case MoveState.Up:
            case MoveState.Down:
                dir = Vector3.forward * Mathf.Sign(rb.velocity.z);
                break;
        }

        var position = transform.position;
        //Debug.DrawRay(position, dir, Color.blue);
        Ray ray = new Ray(position, dir);
        if (Physics.Raycast(ray, out var hit, 0.5f))
        {
            if (hit.collider.CompareTag("Stone"))
            {
                if (moveState == MoveState.Left || moveState == MoveState.Right)
                {
                    movement = Vector3.down.normalized;
                }
                else
                {
                    movement = Vector3.left.normalized;
                }
            }

            if (hit.collider.CompareTag("Border"))
            {
                return true;
            }
        }

        return false;
    }

    private bool SearchPig()
    {
        Vector3 dir = new Vector3();
        switch (moveState)
        {
            case MoveState.Left:
            case MoveState.Right:
                dir = Vector3.right * Mathf.Sign(rb.velocity.x);
                break;
            case MoveState.Up:
            case MoveState.Down:
                dir = Vector3.forward * Mathf.Sign(rb.velocity.z);
                break;
        }

        var position = transform.position;
        position.y += searchYOffset;
        Ray ray = new Ray(position, dir);
        Debug.DrawRay(position, dir);
        //Debug.Break();
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider.CompareTag("Hero"))
            {
                return true;
            }
        }

        return false;
    }
}