using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Slider slider;
    [SerializeField] private float lerpSmoothness;

    private bool IsGameOver = false;

    private float xLimit = 4.3f;

    private void Update()
    {
        if (!IsGameOver)
        {
            float x = Mathf.Lerp(transform.position.x, slider.value * xLimit, lerpSmoothness * Time.deltaTime);
            float y = transform.position.y;
            float z = transform.position.z + speed * Time.deltaTime;

            transform.position = new Vector3(x, y, z);
        }    
    }

    public void SetGameState(bool isGameOver)
    {
        this.IsGameOver = isGameOver;
    }

}
