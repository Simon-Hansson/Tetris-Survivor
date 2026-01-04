using UnityEngine;

public class UICollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(
        gameObject.GetComponent<Transform>().localScale.x,
        gameObject.GetComponent<Transform>().localScale.y
        );
    }
}
