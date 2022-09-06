using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ghost : MonoBehaviour
{
    public float GhostDelay;
    private float GhostDelaySeconds;
    public GameObject Ghost;
    public bool makeGhost = false;

    void Start()
    {
        GhostDelaySeconds = GhostDelay;
    }

    void Update()
    {
        if(makeGhost)
        {
            if (GhostDelaySeconds > 0)
            {
                GhostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(Ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.transform.localScale = this.transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                GhostDelaySeconds = GhostDelay;
                Destroy(currentGhost, 0.15f);
            }
        }
    }
}
