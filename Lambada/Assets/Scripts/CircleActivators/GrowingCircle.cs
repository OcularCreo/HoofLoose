using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCircle : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject activator;

    public bool grow;
    
    [SerializeField] private float maxScale;
    [SerializeField] private float initialGrowSpeed;
    private float curGrowSpeed;

    // Start is called before the first frame update
    void Start()
    {
        grow = true;
        curGrowSpeed = initialGrowSpeed;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(grow)
        {
            if(transform.localScale.x < maxScale)
            {
                curGrowSpeed = initialGrowSpeed + (0.15f * (gameManager.combo / 4));

                curGrowSpeed = Mathf.Clamp(curGrowSpeed, 0.1f, 1.85f);

                transform.localScale += Vector3.one * curGrowSpeed * Time.deltaTime;
            } else
            {
                transform.localScale = Vector3.zero;
                gameManager.combo = 0;
                gameManager.failCounter++;

                activator.GetComponent<CircleActivator>().missed();

                activator.SetActive(false);
            }
        }
    }

}
