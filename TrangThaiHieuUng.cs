namespace TheBattleCard
{
    [System.Serializable]
    public enum LoaiHieuUng
    {
        Burn = 0,
        Freeze = 1
    }

    [System.Serializable]
    public class TrangThaiHieuUng
    {
        public LoaiHieuUng loaiHieuUng;
        public int soRoundConLai;
        public float giaTriHieuUng;
    }
}
