namespace TheBattleCard
{
    [System.Serializable]
    public class KyNang
    {
        public string ten;
        public string moTa;
        public float tiLeKichHoat;
        public LoaiKyNang loaiKyNang;
        public float effectValue;
    }

    public enum LoaiKyNang
    {
        BuffAtk = 0,
        BuffDef = 1,
        GiamAtk = 2,
        GiamDef = 3,
        ChanSkill = 4,
        HoiSinh = 5
    }
}
