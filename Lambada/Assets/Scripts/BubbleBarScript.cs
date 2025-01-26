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
        //slider.maxValue = gameManager.GetComponent<GameManager>().amountToTwerk;
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameManager.GetComponent<GameManager>().spaceKeyJustPressed)
        //{
            setBubbleButt(gameManager.GetComponent<GameManager>().twerkCount);
        Debug.Log("bar: |" + gameManager.GetComponent<GameManager>().twerkCount);
        //}
    }

    private void setBubbleButt(int twerks)
    {
        slider.value = twerks;
    }

}
