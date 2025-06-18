using System;

[AttributeUsage(AttributeTargets.Class)]
public class ManagerAttribute : Attribute
{
    public string Region { get; }
    public string AccessName { get; }
    
    public ManagerAttribute(string accessName = null,string region = "Content")
    {
        Region = region;
        AccessName = accessName;
    }
}