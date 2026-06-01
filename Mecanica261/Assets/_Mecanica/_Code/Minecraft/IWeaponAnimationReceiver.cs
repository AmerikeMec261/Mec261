namespace Minecraft
{
    public interface IWeaponAnimationReceiver
    {
        void PlayPunch();
        void PlaySwing();
        void PlayBowRecoil();
        void SetAiming(bool isAiming);
    }
}
