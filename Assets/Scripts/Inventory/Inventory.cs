namespace SA
{
    [System.Serializable]
    public struct Inventory
    {
        public string weaponID;
        public Weapon curWeapon;

        public void ReloadCurrentWeapon()
        {
            int target = curWeapon.magazineBullets;

            curWeapon.curBullets = target;
        }
    }
}