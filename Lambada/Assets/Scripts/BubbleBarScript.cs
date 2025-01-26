using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleBarScript : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (gameManager.GetComponent<GameManager>().
    }

    private void setBubbleButt(int twerks)
    {
        slider.value = twerks;
    }
}
