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
    private bool full = false;
    public bool isFull(){return full;}
    public void setFull(bool full){this.full = full;}

    private SeedType seedType;
    public void setSeedType(SeedType seedType)
    {
        this.seedType = seedType;
    }
    private SeedScript parentSeed = null;
    public void setParentSeed(SeedScript parentSeed)
    {
        this.parentSeed = parentSeed;
    }

    public float distance = 3.0f;

    public ResourceType outputResource;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem ps;
    private ArrayList inputResources = new ArrayList();
    public ArrayList getInputResources() { return inputResources; }


    // Start is called before the first frame update
    void Start()
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = seedType.color;
    }

    public void Sprout(int seedCount, SeedType st)
    {
        full = true;
        float angle = 2*Mathf.PI / seedCount;
        float initialAngle = Random.Range(0, 2*Mathf.PI);

        // Creates new attached seeds
        for (int i = 0; i < seedCount; i++)
        {
            Vector3 offset = new Vector3(Mathf.Cos(angle * i + initialAngle), Mathf.Sin(angle * i + initialAngle), 0) * distance;
            GameObject newSeed = seedSpawner.CreateSeed(transform.position + offset, st);

            newSeed.GetComponent<SeedScript>().distance = distance * 0.6f;

            CreateRoot(offset, st);

            newSeed.GetComponent<SeedScript>().OnSprout(this);
        }

    }
    GameObject CreateRoot(Vector3 offset, SeedType st)
    {
        GameObject newRoot = Instantiate(root, transform.position, Quaternion.identity);
        newRoot.transform.parent = transform;
        newRoot.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
        newRoot.GetComponent<LineRenderer>().SetPosition(1, offset);
        Gradient gradient = new Gradient();
        GradientColorKey colorKey = new GradientColorKey(st.color, 0.0f);
        gradient.SetKeys(new GradientColorKey[] { colorKey }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        newRoot.GetComponent<LineRenderer>().colorGradient = gradient;
        return newRoot;
    }
    public void OnSprout(SeedScript parentSeed)
    {
        this.parentSeed = parentSeed;
        full = false;

        // Initialize output resource
        outputResource = new ResourceType(new Color(0,0,0,0), "None", 0);

        if (seedType.type == SeedType.Types.Input)
        {
            GameObject nearestCrystal = SeedGenerator.GetNearestCrystal(transform.position);
            if (nearestCrystal != null)
            {
                CreateRoot(nearestCrystal.transform.position - transform.position, seedType);
                AddInputResource(nearestCrystal.GetComponent<Crystal>().resourceType);
            }
            full = true;
        }

        

        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        // Rotate the particle system to face the parent seed
        if (parentSeed != null)
        {
            ps.transform.rotation = Quaternion.LookRotation(parentSeed.transform.position - transform.position);
            var main = ps.main;
            main.startLifetime = Vector3.Distance(parentSeed.transform.position, transform.position) / 4.0f - 0.1f;
            main.startColor = new ParticleSystem.MinMaxGradient(outputResource.color);
            ps.Play();
        } else {
            ps.Stop();
        }
    }
    private void UpdateOutputResource(){
        Debug.Log(inputResources[0]);
        outputResource = inputResources[0] as ResourceType;
        if (!ps) { return; }
        var main = ps.main;
        main.startColor = new ParticleSystem.MinMaxGradient(outputResource.color);

    }
    public void AddInputResource(ResourceType rt)
    {
        inputResources.Add(rt);
        UpdateOutputResource();
        if (parentSeed != null)
        {
            parentSeed.AddInputResource(outputResource);
        }
    }

}
