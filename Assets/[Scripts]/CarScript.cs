using UnityEngine;

public class CarScript : MonoBehaviour
{
    public GameObject particle;

    // Update is called once per frame
    private void Update()
    {
        if (gameObject.transform.position.y <= 5 && particle)
        {
            particle.SetActive(true);
        }
    }
}