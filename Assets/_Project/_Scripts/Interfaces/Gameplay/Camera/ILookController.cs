namespace _Project._Scripts.Interfaces.Gameplay.Camera
{
    public interface ILookController
    {
        float Pitch { get; }
        float Yaw { get; }

        void Apply(float yawDelta, float pitchDelta);
        void SetYaw(float yaw);
        void SetPitch(float pitch);
    }
}