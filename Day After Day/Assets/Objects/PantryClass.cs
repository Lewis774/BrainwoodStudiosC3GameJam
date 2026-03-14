using UnityEngine;

public class PantryClass : MonoBehaviour
{
    public int foodQuantity;
    public Vector2 playerPosition; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var pos = transform.position;
        foodQuantity = Random.Range(10, 100);
        //Position = new Vector2(pos.x, pos.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null) {
                var positions = hit.collider.gameObject.transform.position;
                Debug.Log ("CLICKED " + hit.collider.name + " At " + new Vector2(positions.x, positions.y));
                //int pantryFood = hit.collider.gameObject.GetComponent<Script>()
            }
        }
    }
}
