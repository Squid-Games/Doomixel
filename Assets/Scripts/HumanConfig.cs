using UnityEngine;


public class HumanConfig : MonoBehaviour
{
    private Transform Container;
    public Mesh quad;

    void Awake()
    {
       
        this.gameObject.transform.localPosition = new Vector3(0f, 1f, 0f);
        this.gameObject.transform.localScale = new Vector3(0.721f, 1.8746f, 1.8746f);
        this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        this.gameObject.AddComponent<MeshFilter>();
        this.gameObject.GetComponent<MeshFilter>().mesh = quad;

        this.gameObject.AddComponent<MeshRenderer>();
        this.gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/choomba4"); ;

        this.gameObject.AddComponent<MeshCollider>();
        this.gameObject.GetComponent<MeshCollider>().convex = true;
        this.gameObject.GetComponent<MeshCollider>().sharedMesh = quad;
        this.gameObject.AddComponent<Killable>();

        this.gameObject.AddComponent<BilboardedObject>();

        Container = GameObject.FindGameObjectWithTag("Enemies").transform;

        this.transform.SetParent(Container);

    }

    
}
