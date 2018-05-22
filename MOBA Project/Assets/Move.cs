using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float turnSpeed = 50f;


    bool isTouchDevice;

    //Speed
    float arrowMouseSpeed = 10.0f;

    // Use this for initialization
    void Start()
    {

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            isTouchDevice = true;
        }
        else
        {
            isTouchDevice = false;
        }

        //local de rotação
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

    }


    void Update()
    {
        var posicao = transform.position;


        //matriz da posicao
        Vector3 localRight = new Vector3();
        localRight.x = transform.worldToLocalMatrix.m00;
        localRight.y = transform.worldToLocalMatrix.m01;
        localRight.z = transform.worldToLocalMatrix.m02;

        Vector3 localFront = new Vector3();
        localFront.x = transform.worldToLocalMatrix.m20;
        localFront.y = transform.worldToLocalMatrix.m21;
        localFront.z = transform.worldToLocalMatrix.m22;

        //movendo a camera
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += localRight * (-0.5f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += localFront * (0.5f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += localFront * (-0.5f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += localRight * (0.5f);
        }



        if (isTouchDevice)
        {

            cameraRotationAccelerometer();
        }
        else
        {
            //rotação camera e do mouse
            moveWithArrowAndMouse();
        }
    }


    float xValue;
    float xValueMinMax = 50.0f;

    float yValue;
    float yValueMinMax = 50.0f;

    float cameraSpeed = 20.0f;
    Vector3 accelometerSmoothValue;

    void cameraRotationAccelerometer()
    {
        //Set X Min Max
        if (xValue < -xValueMinMax)
            xValue = -xValueMinMax;

        if (xValue > xValueMinMax)
            xValue = xValueMinMax;

        //Set Y Min Max
        if (yValue < -yValueMinMax)
            yValue = -yValueMinMax;

        if (yValue > yValueMinMax)
            yValue = yValueMinMax;

        accelometerSmoothValue = lowpass();

        xValue += accelometerSmoothValue.x;
        yValue += accelometerSmoothValue.y;

        transform.rotation = new Quaternion(yValue, xValue, 0, cameraSpeed);
    }

    //Smooth Accelerometer
    public float AccelerometerUpdateInterval = 1.0f / 100.0f;
    public float LowPassKernelWidthInSeconds = 0.001f;
    public Vector3 lowPassValue = Vector3.zero;
    Vector3 lowpass()
    {
        float LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;//tweakable
        lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
        return lowPassValue;
    }

    //Movimento teclado e mouse
    void moveWithArrowAndMouse()
    {


        //Keyboard Mouse
        moveCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), arrowMouseSpeed);
    }

    //Movimentos paramentos
    float mouseX;
    float mouseY;
    Quaternion localRotation;
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void moveCamera(float horizontal, float verticle, float moveSpeed)
    {
        mouseX = horizontal;
        mouseY = -verticle;

        rotY += mouseX * moveSpeed;
        rotX += mouseY * moveSpeed;

        localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }
}