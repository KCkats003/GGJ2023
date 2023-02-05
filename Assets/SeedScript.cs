using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
    const int MAX_SEEDS = 6;
    private int maxSeedsDefault = 3;
    private int maxSeeds = 3;
    private int numSeeds = 0;

    [SerializeField]
    private GameObject root;

    [SerializeField]
    private SeedSpawner seedSpawner;

    private SeedScript outputSeed = null;
    public SeedScript getOutputSeed() { return outputSeed; }

    public void setSeedSpawner(SeedSpawner seedSpawner)
    {
        this.seedSpawner = seedSpawner;
    }
    private bool full = false;
    public bool isFull(){return full;}
    public void setFull(bool full){this.full = full;}

    private SeedType seedType;
    public SeedType getSeedType(){return seedType;}
    public void setSeedType(SeedType seedType)
    {
        this.seedType = seedType;
        if (seedType.type == SeedType.Types.Input)
        {
            maxSeeds = 0;
        }
        else if (seedType.type == SeedType.Types.Base)
        {
            maxSeeds = MAX_SEEDS;
        }
        else if (seedType.type == SeedType.Types.Transport)
        {
            maxSeeds = 1;
        }
        else if (seedType.type == SeedType.Types.Synthesis)
        {
            maxSeeds = 1;
        }
        else
        {
            maxSeeds = maxSeedsDefault;
        }
    }
    private SeedScript parentSeed = null;
    public void setParentSeed(SeedScript parentSeed)
    {
        this.parentSeed = parentSeed;
        AddOutputSeed(parentSeed);
    }
    public SeedScript getParentSeed(){return parentSeed;}

    public float distance = 2.0f;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem ps;
    [SerializeField]
    private ArrayList inputResources = new ArrayList();
    public ArrayList getInputResources() { return inputResources; }
    [SerializeField]
    public ArrayList outputResources = new ArrayList();

    private float popupspeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (seedType.type != SeedType.Types.Input){
            spriteRenderer.color = seedType.color;
        }
        if (transform.localScale.x < 0.5f)
        {
            transform.localScale += new Vector3(popupspeed, popupspeed, popupspeed) * Time.deltaTime;
        }

        
    }

    public Vector3 GetSeedPosition(Vector3 position)
    {
        position = position - transform.position;
        Vector3 initialOffset = Vector3.ClampMagnitude((new Vector3(position.x, position.y, 0)), distance);

        return initialOffset;
    } 

    public void Sprout(SeedType st, Vector3 position)
    {
        numSeeds++;
        if (numSeeds >= maxSeeds){
            full = true;
        }

        float initialAngle = Vector2.Angle(Vector2.right, position - transform.position);

        Vector3 offset = GetSeedPosition(position);

        // Creates new attached seeds

        GameObject newSeed = seedSpawner.CreateSeed(transform.position + offset, st);

        //newSeed.GetComponent<SeedScript>().distance = distance * 0.6f;

        CreateRoot(offset, st.color);

        newSeed.GetComponent<SeedScript>().OnSprout(this);

    }
    GameObject CreateRoot(Vector3 offset, Color color)
    {
        GameObject newRoot = Instantiate(root, transform.position, Quaternion.identity);
        newRoot.transform.parent = transform;
        newRoot.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
        newRoot.GetComponent<LineRenderer>().SetPosition(1, offset);
        Gradient gradient = new Gradient();
        GradientColorKey colorKey = new GradientColorKey(color, 0.0f);
        gradient.SetKeys(new GradientColorKey[] { colorKey }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        newRoot.GetComponent<LineRenderer>().colorGradient = gradient;
        return newRoot;
    }
    public void OnSprout(SeedScript parentSeed)
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();

        setParentSeed(parentSeed);

        full = false;
        
        GameObject nearestCrystal = SeedGenerator.GetNearestCrystal(transform.position);
        if (nearestCrystal != null && seedType.type == SeedType.Types.Default){
            seedType = new SeedType(nearestCrystal.GetComponent<Crystal>().resourceType.color, SeedType.Types.Input);
            Color color = nearestCrystal.GetComponent<Crystal>().resourceType.color;
            CreateRoot(nearestCrystal.transform.position - transform.position, color);
            GetComponent<SpriteRenderer>().color = color;
            AddInputResource(nearestCrystal.GetComponent<Crystal>().resourceType.Clone());
            nearestCrystal.GetComponent<Crystal>().AddTapper();
            full = true;
        }

        
        // Rotate the particle system to face the parent seed
        if (parentSeed != null)
        {
            
            // Create a discrete gradient with every output resource color
            UpdateOutputResource();

            
        } else {
            ps.Stop();
        }
    }
    private void UpdateOutputResource(){
        if (!ps) { return; }
        var main = ps.main;
        // Create a discrete gradient with every output resource color
        ParticleSystem.MinMaxGradient gradient = new ParticleSystem.MinMaxGradient();
        gradient.mode = ParticleSystemGradientMode.RandomColor;
        gradient.gradient = new Gradient();
        gradient.gradient.mode = GradientMode.Fixed;

        GradientColorKey[] colorKeys = new GradientColorKey[outputResources.Count];
        for (int i = 0; i < outputResources.Count; i++){
            var or = outputResources[i] as ResourceType;
            colorKeys[i] = new GradientColorKey(or.color, (float)(i+1) / (outputResources.Count));
        }
        gradient.gradient.SetKeys(colorKeys, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });
        main.startColor = gradient;
        if (outputResources.Count > 0 && parentSeed != null){
            ps.Play();
        } else {
            ps.Stop();
        }
    }

    public void AddInputResource(ResourceType rt, int depth = 50)
    {
        if (depth <= 0) { return; }
        depth--;
        Debug.Log("Adding input resource: " + rt.name + " " + rt.value);
        bool found = false;
        foreach (ResourceType ir in inputResources.ToArray())
        {
            if (ir.name == rt.name)
            {
                found = true;
                ir.value += rt.value;
                break;
            }
        }
        if (!found)
        {
            inputResources.Add(rt);
        }

        outputResources.Clear();
        foreach (ResourceType ir in inputResources.ToArray())
        {
            outputResources.Add(ir.Clone());
        }

        if (seedType.type == SeedType.Types.Synthesis){
            CheckSynthesis();
        }

        UpdateOutputResource();
        if (outputSeed != null)
        {
            outputSeed.AddInputResource(rt.Clone(), depth);
        }
    }

    public void RemoveInputResource(ResourceType rt, int depth = 50)
    {
        if (depth <= 0) { Debug.LogWarning("Avoiding Stack Overflow!"); return; }
        depth--;
        foreach (ResourceType ir in inputResources.ToArray())
        {
            if (ir.name == rt.name)
            {
                ir.value -= rt.value;
                if (ir.value <= 0)
                {
                    inputResources.Remove(ir);
                    break;
                }
            }
        }
        outputResources.Clear();
        foreach (ResourceType ir in inputResources.ToArray())
        {
            outputResources.Add(ir.Clone());
        }

        UpdateOutputResource();
        if (outputSeed != null)
        {
            outputSeed.RemoveInputResource(rt.Clone(), depth);
        }
    }

    public void RemoveInputResourceByString(string name, int amount)
    {
        foreach (ResourceType ir in inputResources.ToArray())
        {
            if (ir.name == name)
            {
                var res = ir.Clone();
                res.value = amount;
                RemoveInputResource(res);
            }
        }
    }

    public void AddOutputSeed(SeedScript ss)
    {
        Debug.Log("Adding output seed");
        if (outputSeed == ss){ return; }
        if (ss == null)
        {
            outputSeed = null;
            return;
        }
        foreach (ResourceType rt in outputResources.ToArray())
        {
            if (outputSeed != null){
                outputSeed.RemoveInputResource(rt.Clone());
            }
            ss.AddInputResource(rt.Clone());
        }
        outputSeed = ss;
        RotateParticleToOutputSeed();

    }

    void RotateParticleToOutputSeed()
    {
        if (outputSeed != null)
        {
            ps.transform.rotation = Quaternion.LookRotation(outputSeed.transform.position - transform.position);
            var main = ps.main;
            main.startLifetime = Vector3.Distance(outputSeed.transform.position, transform.position) / 4.0f - 0.1f;
        }
    }

    void CheckSynthesis(){
        // Checks if certain combinations of input resources are present and synthesizes them into a new resource
        string[] combo1 = new string[] { "Water", "Leaf" };
        string[] combo2 = new string[] { "Flower", "Light" };

        // Check for combo 1
        bool combo1Found = true;
        foreach (string s in combo1){
            bool found = false;
            foreach (ResourceType rt in inputResources){
                if (rt.name == s){
                    found = true;
                    break;
                }
            }
            if (!found){
                combo1Found = false;
                break;
            }
        }
        if (combo1Found){
            foreach (string s in combo1){
                RemoveInputResourceByString(s, 1);
            }
            AddInputResource(new ResourceType(Color.white, "Sugar", 1));
        }
        // Check for combo 2
        bool combo2Found = true;
        foreach (string s in combo2){
            bool found = false;
            foreach (ResourceType rt in inputResources){
                if (rt.name == s){
                    found = true;
                    break;
                }
            }
            if (!found){
                combo2Found = false;
                break;
            }
        }
        if (combo2Found){
            foreach (string s in combo2){
                RemoveInputResourceByString(s, 1);
            }
            AddInputResource(new ResourceType(new Color((63f/255f), (53f/255f), (204f/255f)), "Fruit", 1));
        }


    }

}
