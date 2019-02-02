using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Selectable
{
    //DANG YOU NO MULTIPLE INHERITANCE!!!//
    //public static readonly string NO_OPTIONAL_DATA_C = "NO_OPTIONAL_DATA";
    string OnObjectSelect( string optionalData );
}
