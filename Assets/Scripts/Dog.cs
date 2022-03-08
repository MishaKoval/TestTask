using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dog : MonoBehaviour
{
    private enum DogType
    {
        Vertical = 0,
        Horizontal = 1
    }

    private enum DogSpriteType
    {
        Default = 0,
        Dirty = 1,
        Angry = 2
    }

    #region Inspector

    [SerializeField] private float speed = 1.5f;

    [SerializeField] private float angrySpeed = 2.5f;
    
    [SerializeField] private float waitBombTime = 1.0f;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private List<Sprite> moveSprites;

    [SerializeField] private List<Sprite> angrySprites;

    [SerializeField] private List<Sprite> dirtySprites;
    
    [SerializeField] private Vector3 startTarget;

    [SerializeField] private Rigidbody rb;
    
    [SerializeField] public UnityEvent onBombed;
    

    #endregion

    #region Fields

    private Vector3 startpos;

    private Vector3 targetpos;

    private Vector3 dir;

    private DogType dogType;

    private MoveState moveState;

    private MoveState startState;

    private DogSpriteType dogSpriteType;

    private Animator anim;

    private AudioSource audioSource;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        startpos = transform.position;
        dir = startTarget - startpos;
        targetpos = startTarget;
        dogType = Mathf.Abs(dir.z) > 0 ? DogType.Vertical : DogType.Horizontal;
        dogSpriteType = DogSpriteType.Default;
        onBombed.AddListener(() => StartCoroutine(WaitBombAnim()));
        onBombed.AddListener(()=> audioSource.Play());
    }

    void Start()
    {
        CheckDirection();
        startState = moveState;
        InvokeRepeating("CheckTargetPos", 0, 0.1f);
    }
    
    private void Update()
    {
        CheckDirection();
    }

    private void FixedUpdate()
    {
        rb.velocity = dir.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hero"))
        {
            other.gameObject.GetComponent<Pig>().onDamaged.Invoke();
        }
    }
    
    

    #endregion

    #region Coroutines

    private IEnumerator WaitBombAnim()
    {
        if (dogSpriteType == DogSpriteType.Dirty)
        {
            StartCoroutine(WaitDestroy());
        }

        if (dogSpriteType == DogSpriteType.Angry)
        {
            dogSpriteType = DogSpriteType.Dirty;
        }

        if (dogSpriteType == DogSpriteType.Default)
        {
            dogSpriteType = DogSpriteType.Angry;
            speed /= 2;
        }

        anim.SetBool("IsBombed", true);
        yield return new WaitForSeconds(waitBombTime);
        anim.SetBool("IsBombed", false);
        speed = angrySpeed;
    }

    private IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    #endregion
    
    private void CheckDirection()
    {
        int spriteId = 0;
        switch (dogType)
        {
            case DogType.Horizontal:
                if (dir.x > 0)
                {
                    //spriteRenderer.sprite = moveSprites[(int) MoveState.Right];
                    spriteId = (int) MoveState.Right;
                    moveState = MoveState.Right;
                }
                else
                {
                    //spriteRenderer.sprite = moveSprites[(int) MoveState.Left];
                    spriteId = (int) MoveState.Left;
                    moveState = MoveState.Left;
                }

                break;
            case DogType.Vertical:
                if (dir.z > 0)
                {
                    //spriteRenderer.sprite = moveSprites[(int) MoveState.Up];
                    spriteId = (int) MoveState.Up;
                    moveState = MoveState.Up;
                }
                else
                {
                    //spriteRenderer.sprite = moveSprites[(int) MoveState.Down];
                    spriteId = (int) MoveState.Down;
                    moveState = MoveState.Down;
                }

                break;
        }

        switch (dogSpriteType)
        {
            case DogSpriteType.Default:
                spriteRenderer.sprite = moveSprites[spriteId];
                break;
            case DogSpriteType.Angry:
                spriteRenderer.sprite = angrySprites[spriteId];
                break;
            case DogSpriteType.Dirty:
                spriteRenderer.sprite = dirtySprites[spriteId];
                break;
        }
    }

    private void CheckTargetPos()
    {
        if ((targetpos - transform.position).sqrMagnitude < 0.1f)
        {
            dir *= -1.0f;
            targetpos = startState == moveState ? startpos : startTarget;
            //CheckDirection();
        }
    }
}