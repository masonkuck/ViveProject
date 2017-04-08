using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WireController : MonoBehaviour
{
    public bool IsSource;
    public bool IsElectrified;
    public Material ElectricMaterial;
    public Material NonElectricMaterial;
    private List<WireController> touchedControls;

    // Use this for initialization
    void Start()
    {
        IsElectrified = false;
        touchedControls = new List<WireController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSource)
            IsElectrified = true;
        else
        {
            if (touchedControls.Count > 0)
                CheckIsElectrified();
            else
                IsElectrified = false;
        }

        Renderer rend = GetComponent<Renderer>();
        if (IsElectrified)
        {
            rend.material = ElectricMaterial;
        }
        else
        {
            rend.material = NonElectricMaterial;
        }
    }

    public bool CheckIsElectrified()
    {
        List<WireController> list = new List<WireController>();
        list.Add(this);

        if (IsSource || touchedControls.FirstOrDefault(x => x.CheckIsElectrified(list)) != null)
        {
            return IsElectrified = true;
        }
        else
        {
            return IsElectrified = false;
        }

    }
    public bool CheckIsElectrified(List<WireController> list)
    {
        list.Add(this);
        if (IsSource || touchedControls.FirstOrDefault(x => !list.Contains(x) && x.CheckIsElectrified(list)) != null)
        {
            return IsElectrified = true;
        }
        else
        {
            return IsElectrified = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
            return;
        WireController comp = other.gameObject.GetComponent<WireController>();
        if (comp != null)
        {
            touchedControls.Add(comp);

            CheckIsElectrified();
        }
    }

    void OnTriggerExit(Collider other)
    {
        WireController comp = other.gameObject.GetComponent<WireController>();
        if (comp != null && touchedControls.Contains(comp))
        {
            touchedControls.Remove(comp);
        }
    }
}
