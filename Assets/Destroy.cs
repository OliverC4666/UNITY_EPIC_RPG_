using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject GROUND;
    private Transform thisObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float GROUND_WIDTH = GROUND.transform.localScale.x;
        thisObj = gameObject.transform;
        thisObj.localScale =new Vector3(GROUND_WIDTH,0,0) ;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
