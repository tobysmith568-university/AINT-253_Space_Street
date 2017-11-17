using UnityEngine;
public class Mapping
{
    string name;
    KeyCode primaryInput;
    KeyCode? secondryInput;
    public string Name { get { return name; } set { name = value; } }
    public KeyCode PrimaryInput { get { return primaryInput; } set { primaryInput = value; } }
    public KeyCode? SecondryInput { get { return secondryInput; } set { secondryInput = value; } }

    public Mapping(string name, KeyCode primaryInput, KeyCode? secondryInput = null)
    {
        Name = name;
        PrimaryInput = primaryInput;
        SecondryInput = secondryInput;
    }
}
