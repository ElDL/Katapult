using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraRotationScript : MonoBehaviour {

    public GameObject target;//the target object
    public float speedMod = 20.0f;//a speed modifier
    private Vector3 point;//the coord to the point where the camera looks at
    public float forceToApply = 50f;
    private bool flagForce = true;
    private Vector3 initialPosition;
    public Vector3 spectatorPosition;
    public GameObject spectatorTarget;


    public float explosionRadius = 10;
    public float explosionForce = 50;

    void Start()
    {//Set up things on the start method
        point = target.transform.position;//get target's coords
        initialPosition = this.transform.position;
        transform.LookAt(point);//makes the camera look to it
    }

    void Update()
    {//makes the camera rotate around "point" coords, rotating around its Y axis given the mouse move events values
        Vector3 forwardDirection = this.transform.forward.normalized;
        Rigidbody rb = target.GetComponent<Rigidbody>();

        if (Input.GetMouseButtonDown(0) && flagForce)
        { 
            Vector3 force = new Vector3(forwardDirection.x * forceToApply, forwardDirection.y * forceToApply, forwardDirection.z * forceToApply);
            rb.AddForce(force);
            flagForce = false;
            setSpectatorCamera();
        }

        if (flagForce)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                resetCameraPosition();
            }

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            transform.RotateAround(point, Vector3.up, mouseX * Time.deltaTime * speedMod);
            transform.RotateAround(point, Vector3.Cross(forwardDirection, Vector3.up), mouseY * Time.deltaTime * speedMod);
        } else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                reloadScene();
            }
            if (Input.GetMouseButtonDown(1))
            {
                rb.AddExplosionForce(explosionForce, target.transform.position, explosionRadius);
                //Destroy(target);
            }
        }
    }

    private void reloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void setSpectatorCamera()
    {
        transform.position = spectatorPosition;
        transform.LookAt(spectatorTarget.transform.position);
    }

    private void resetCameraPosition()
    {
        transform.position = initialPosition;
        transform.LookAt(point);
    }
}
