using UnityEngine;
using System.Collections;

public interface Influenceable {

    void ApplyInfluence(influences inf);

    void RemoveInfluence(influences inf);

    void RemoveInfluenceTimer(influences inf, float time);
}
