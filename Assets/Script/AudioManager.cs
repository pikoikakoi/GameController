using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource bubbleSFX;
    public AudioSource coinSFX;
    public AudioSource collideSFX;

    private void OnEnable()
    {
        Bubble.CollectBubble += PlayBubble;
        Coin.CollectCoin += PlayStar;
        Obstacle.CollectObstacle += PlayObstacle;
    }

    private void OnDisable() {
        Bubble.CollectBubble -= PlayBubble;
        Coin.CollectCoin -= PlayStar;
        Obstacle.CollectObstacle -= PlayObstacle;
        
    }

    private void PlayBubble(float value)
    {
        bubbleSFX.PlayOneShot(bubbleSFX.clip);
    }

    private void PlayStar(int value)
    {
        coinSFX.PlayOneShot(coinSFX.clip);
    }

    private void PlayObstacle(bool value)
    {
        collideSFX.Play();
    }
}
