using UnityEngine;

[CreateAssetMenu]
public class SkillSystemCategory : SkillSystemIdentifiedObjectSO
{
    #region 함수
    public override bool Equals(object other)
        => base.Equals(other);

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(SkillSystemCategory lhs, string rhs)
    {
        if (rhs is null)
            return rhs is null;
        return lhs.CodeName == rhs;
    }

    public static bool operator !=(SkillSystemCategory lhs, string rhs) => !(lhs == rhs);
    #endregion // 함수
}
