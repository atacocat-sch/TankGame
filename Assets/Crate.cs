using UnityEngine;

public abstract class Crate : MonoBehaviour
{
    public GameObject pickupFX;

    private void Start()
    {
        pickupFX.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (TryApplyEffect(collision.gameObject))
            {
                pickupFX.SetActive(true);
                pickupFX.transform.SetParent(null);

                Destroy(gameObject);
            }
        }
    }

    protected abstract bool TryApplyEffect(GameObject target);
}
