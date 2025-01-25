using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpriteChanger : MonoBehaviour
{
    [SerializeField] Sprite[] danceSprites;
    SpriteRenderer spriteRenderer;
    int currentSprite = 0;

    [SerializeField] KeyCode[] keycode;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.anyKeyDown && danceSprites != null) 
        {
            NextSprite();
        }*/

        for (int i = 0; i < keycode.Length; i++) 
        {
            if (Input.GetKeyDown(keycode[i]))
            {
                NextSprite();
            }
        }
    }

    void NextSprite() 
    {
        ++currentSprite;

        if (currentSprite > danceSprites.Length - 1) 
        {
            currentSprite = 0;
        }

        spriteRenderer.sprite = danceSprites[currentSprite];
    }
}
