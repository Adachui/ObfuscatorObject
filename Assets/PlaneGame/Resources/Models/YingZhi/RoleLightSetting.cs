using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleLightSetting : MonoBehaviour {

    public Color roleShadowColor = new Color(0.2f, 0.2f, 0.2f, 1);
    public bool isUpdate = false;

    private Light roleLight;


	void Start () {
        roleLight = GetComponent<Light>();

        SetRoleDirectionalLight();
    }


    void Update()
    {
        if (isUpdate) {
            SetRoleDirectionalLight();
        }
        
    }


    private void SetRoleDirectionalLight()
    {
        Shader.SetGlobalColor("_RoleDirectionalLightColor", roleLight.color);
        Shader.SetGlobalVector("_RoleDirectionalLightDir", new Vector4(roleLight.transform.forward.x, roleLight.transform.forward.y, roleLight.transform.forward.z, roleLight.intensity));
        Shader.SetGlobalColor("_ShadowColor", roleShadowColor);
    }

}
