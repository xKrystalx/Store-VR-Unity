using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : Singleton<User>
{
    public string userName;
    public string token;

    protected User() {}
}
