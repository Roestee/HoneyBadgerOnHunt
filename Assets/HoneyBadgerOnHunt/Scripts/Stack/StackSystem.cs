using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSystem : MonoBehaviour
{
    [SerializeField] private CollectablePart collectablePartPrefab;
    [SerializeField] private Transform stackParent;

    [HideInInspector]
    public bool final = false;

    public float finalSpeed;
    public ParticleSystem winPartical;

    private float finalOffsetZ = 1.72f;
    private float finalYPos = 0.1f;
    private float partOffset = 0.125f;
    private List<CollectablePart> collectableParts = new List<CollectablePart>();
    private Animator anim;
    private PlayerController playerController;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
        final = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collect();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Remove();
        }
        if (final)
        {
            for (int i = 0; i < collectableParts.Count; i++)
            {
                Transform collectablePartTransform = collectableParts[collectableParts.Count - 1 - i].transform;

                float x = collectablePartTransform.position.x;
                float y = collectablePartTransform.position.y;
                float z = collectablePartTransform.position.z;

                if (z < transform.position.z + (i + 1f) * finalOffsetZ + 0.25f)
                {
                    z += Time.deltaTime * finalSpeed;
                }
                else
                {
                    y = finalYPos;
                    x = 0f;
                }
                collectablePartTransform.position = new Vector3(x, y, z);

            }           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            Destroy(other.gameObject);

            CollectablePart collectablePart = other.GetComponent<CollectablePart>();

            if (!collectablePart.collected)
            {
                Collect();
            }

            collectablePart.collected = true;
        }
        else if (other.CompareTag("Obstacle"))
        {
            FailGame();
        }
        else if (other.CompareTag("Finish"))
        {
            FinishGame();
        }
    }

    private void Collect()
    {
        CollectablePart collectablePart = Instantiate(collectablePartPrefab);
        collectablePart.transform.parent = stackParent;
        collectablePart.transform.localPosition = new Vector3(0, collectableParts.Count * partOffset, 0);
        collectableParts.Add(collectablePart);
        collectablePart.GetComponent<BoxCollider>().enabled = false;
    }

    private void Remove()
    {
        int index = collectableParts.Count - 1;

        if (index < 0)
        {
            return;
        }

        Destroy(collectableParts[index].gameObject);
        collectableParts.RemoveAt(index);
    }

    private void FailGame()
    {
        for (int i = 0; i < collectableParts.Count; i++)
        {
            Destroy(collectableParts[i].gameObject);
        }

        collectableParts.Clear();
        anim.SetTrigger("FallBack");
        playerController.SetGameState(true);
        GameManager.current.SetPanel(false, false, false, true);
    }

    private void FinishGame()
    {
        final = true;
        winPartical.Play();
        anim.SetTrigger("Win");
        playerController.SetGameState(true);
        GameManager.current.SetPanel(false, true, false, false);
    }
}
