using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StonesManager : MonoBehaviour
{

    [SerializeField] private Slider stoneSlider;
    
    private int stonesCount;

    public UnityEvent onStoneDestroy;

    private void Awake()
    {
        stonesCount = transform.childCount;
        stoneSlider.maxValue = stonesCount;
        stoneSlider.wholeNumbers = true;
        stoneSlider.value = stoneSlider.maxValue - stonesCount;
        onStoneDestroy.AddListener(RefreshStonesCount);
        Debug.Log($"Камней: {stonesCount}");
    }
    
    private void RefreshStonesCount()
    {
        stonesCount--;
        stoneSlider.value = stoneSlider.maxValue - stonesCount;
        if (stonesCount == 0)
        {
            SceneManager.LoadScene("WinGame");
        }
    }
    

    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
