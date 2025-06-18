// https://discussions.unity.com/t/how-to-make-a-readonly-property-in-inspector/75448/5
using System;
using UnityEngine;

/// <summary>
/// Make field readonly
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ReadOnlyAttribute : PropertyAttribute {
    public bool WritableOnPlay { get; set; }
    public bool WritableOnEditor { get; set; }
    public bool WritableOnInstance { get; set; }
}