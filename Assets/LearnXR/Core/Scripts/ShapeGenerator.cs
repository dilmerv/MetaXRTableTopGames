using LearnXR.Core;
using UnityEngine;

public class ShapeGenerator : Singleton<ShapeGenerator>
{
    [SerializeField]
    private float maxScale = 1;

    [SerializeField] private Material defaultMaterial;

    [SerializeField] private PhysicMaterial physicMaterial;
    public void CreateShape() => CreateShape(transform.position, true);

    public void CreateShape(Vector3 position, bool randomizeColor = false)
    {
        var randomScale = Random.Range(0, maxScale);
        Vector3 scale = new Vector3(randomScale, randomScale, randomScale);

        GameObject shape = GameObject.CreatePrimitive(GetRandomPrimitiveType());
        shape.transform.localScale = scale;
        shape.transform.position = position;
        var shapePhysics = shape.AddComponent<Rigidbody>();
        shapePhysics.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        if (physicMaterial) shape.GetComponent<Collider>().material = physicMaterial;
        var shapeRenderer = shape.GetComponent<Renderer>();
        if (randomizeColor)
        {
            var newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"))
            {
                color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f)
            };
            shapeRenderer.material = newMaterial;
        }
        else
            shapeRenderer.material = defaultMaterial;
    }
    
    private PrimitiveType GetRandomPrimitiveType()
    { 
        PrimitiveType[] primitiveTypes = { PrimitiveType.Cube, PrimitiveType.Sphere, PrimitiveType.Capsule };
        int randomIndex = Random.Range(0, primitiveTypes.Length);
        return primitiveTypes[randomIndex];
    }
}
