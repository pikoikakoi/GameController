using UnityEngine;

public class Boundaries : MonoBehaviour
{
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2; 
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;   //menyimpan posisi pertama
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 - objectWidth, screenBounds.x + objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 - objectHeight, screenBounds.y + objectHeight);
        // viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1, screenBounds.x);
        // viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1, screenBounds.y);
        transform.position = viewPos;   //menyimpan nilai terakhir
    }
}
