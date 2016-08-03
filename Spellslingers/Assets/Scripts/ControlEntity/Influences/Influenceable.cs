using UnityEngine;
using System.Collections;

public interface Influenceable {

    object ApplyInfluence(influences inf);

    object RemoveInfluence(influences inf);

    object RemoveInfluenceTimer(influences inf, float time);
}
