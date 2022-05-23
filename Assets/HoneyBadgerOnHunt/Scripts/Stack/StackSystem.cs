using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StackSystem : MonoBehaviour
{
    #region Serializable Field
    [Header("Stack")]
    [SerializeField] private CollectablePart collectablePartPrefab;
    [SerializeField] private Transform stackParent;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Sound")]
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip winSound;
    #endregion

    #region Public Field
    [HideInInspector]
    public bool final = false;

    [Header("Partical")]
    public ParticleSystem winPartical;

    [Header("Final")]
    public float finalSpeed;

    #endregion

    #region Private Field
    private float finalOffsetZ = 1.72f;
    private float finalYPos = 0.1f;
    private float partOffset = 0.125f;
    private List<CollectablePart> collectableParts = new List<CollectablePart>();
    private Animator anim;
    private PlayerController playerController;
    private AudioSource sound;
    #endregion

    #region Unity
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
        sound = GetComponent<AudioSource>();
        final = false;
        levelText.text = SceneManager.GetActiveScene().name;
        coinText.text = PlayerPrefs.GetInt("Coin", 0).ToString() + " Honeycomb";
    }

    private void Update()
    {
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
    #endregion

    #region Private Functions
    private void Collect()
    {
        CollectablePart collectablePart = Instantiate(collectablePartPrefab);
        collectablePart.transform.parent = stackParent;
        collectablePart.transform.localPosition = new Vector3(0, collectableParts.Count * partOffset, 0);
        collectableParts.Add(collectablePart);
        sound.PlayOneShot(coinSound);
        collectablePart.GetComponent<BoxCollider>().enabled = false;
    }

    private void FailGame()
    {
        for (int i = 0; i < collectableParts.Count; i++)
        {
            Destroy(collectableParts[i].gameObject);
        }

        sound.PlayOneShot(failSound);
        collectableParts.Clear();
        anim.SetTrigger("FallBack");
        playerController.SetGameState(true);
        GameManager.current.SetPanel(false, false, false, true);
    }

    private void FinishGame()
    {
        sound.PlayOneShot(winSound, 4f);
        int finalCoin = collectableParts.Count * collectableParts.Count;
        PlayerPrefs.SetInt("Coin", finalCoin + PlayerPrefs.GetInt("Coin"));
        winText.text = "You collect " + finalCoin.ToString() + " honeycomb!";
        coinText.text = finalCoin.ToString() + "Honeycomb";
        final = true;
        winPartical.Play();
        anim.SetTrigger("Win");
        playerController.SetGameState(true);
        GameManager.current.SetPanel(false, true, false, false);
    }
    #endregion
}
