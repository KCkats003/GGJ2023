using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
    const int MAX_SEEDS = 10;

    [SerializeField]
    private GameObject root;

    [SerializeField]
    private SeedSpawner seedSpawner;
    public void setSeedSpawner(SeedSpawner seedSpawner)
    {
        this.seedSpawner = seedSpawner;
    }


    public float distance = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sprout(int seedCount)
    {
        float angle = 2*Mathf.PI / seedCount;
        float initialAngle = Random.Range(0, 2*Mathf.PI);

        // Creates new attached seeds
        for (int i = 0; i < seedCount; i++)
        {
            Vector3 offset = new Vector3(Mathf.Cos(angle * i + initialAngle), Mathf.Sin(angle * i + initialAngle), 0) * distance;
            GameObject newSeed = seedSpawner.CreateSeed(transform.position + offset);

            newSeed.GetComponent<SeedScript>().distance = distance * 0.6f;
            GameObject newRoot = Instantiate(root, transform.position, Quaternion.identity);
            newRoot.transform.parent = transform;
            newRoot.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
            newRoot.GetComponent<LineRenderer>().SetPosition(1, offset);

        }

    }


}
