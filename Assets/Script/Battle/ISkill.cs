using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ISkill
{
    int Cost { get; }
    void Skill();
}
