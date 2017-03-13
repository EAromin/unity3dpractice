using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
    public Mesh og;
    public float spawnProbability;
    public Material mat;
    public int maxDepth;
    private int depth;
    public float childScale;
    private Material[,] mats;
    public Mesh[] meshes;

    private static Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };
    // Use this for initialization
    void Start () {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-twist,twist),0f,0f);

        if (mats == null)
        {
            InitializeMaterials();
        }
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = mats[depth,Random.Range(0,2)];
      
        if (depth < maxDepth)
        {
            StartCoroutine(Breed());
                    }
	}

    private void InitializeMaterials()
    {
        mats = new Material[maxDepth + 1,2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            mats[i,0] = new Material(mat);
            mats[i,0].color =
                Color.Lerp(Color.black, Color.red, t);
            mats[i, 1] = new Material(mat);
            mats[i, 1].color =
                Color.Lerp(Color.blue, Color.cyan, t);
        }
        mats[maxDepth,0].color = Color.magenta;
        mats[maxDepth, 1].color = Color.green;
    }

    private IEnumerator Breed()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            if (Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(.5f, 2f));
                new GameObject("Fractal Child").AddComponent<Fractal>().
                    Initialize(this, i);
            }
        }
    }
    private void Initialize(Fractal f, int i)
    {
        twist = f.twist;
        maxRotationSpeed = f.maxRotationSpeed;
        spawnProbability = f.spawnProbability;
        meshes = f.meshes;
        mats = f.mats;
        og = f.og;
        mat = f.mat;
        maxDepth = f.maxDepth;
        depth = f.depth + 1;
        transform.parent = f.transform;
        childScale = f.childScale;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[i] * (.5f + .5f * childScale);
        transform.localRotation = childOrientations[i];
    }

    // Update is called once per frame
    public float maxRotationSpeed;

    private float rotationSpeed;
    public float twist;

    void Update () {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
