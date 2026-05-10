using UnityEngine;

namespace TheBattleCard
{
    public class HeThongChienDau
    {
        private readonly XuLyKyNang xuLyKyNang = new XuLyKyNang();

        public SpeedPhaseKetQua XuLySpeedPhase(Tuong benTanCong, Tuong benPhongThu)
        {
            SpeedPhaseKetQua ketQua = new SpeedPhaseKetQua
            {
                benDiTruoc = benTanCong,
                benDiSau = benPhongThu,
                benTanCong = benTanCong,
                benPhongThu = benPhongThu,
                log = string.Empty
            };

            if (benTanCong == null || benPhongThu == null)
            {
                ketQua.log = "[SpeedPhase] Loi: Du lieu Tuong khong hop le.";
                Debug.LogWarning(ketQua.log);
                return ketQua;
            }

            if (benTanCong.speed > benPhongThu.speed)
            {
                ketQua.benDiTruoc = benTanCong;
                ketQua.benDiSau = benPhongThu;
                ketQua.log = "[SpeedPhase] Ben tan cong co speed cao hon nen hanh dong truoc.";
            }
            else if (benTanCong.speed < benPhongThu.speed)
            {
                ketQua.benDiTruoc = benPhongThu;
                ketQua.benDiSau = benTanCong;
                ketQua.log = "[SpeedPhase] Ben phong thu co speed cao hon nen hanh dong truoc.";
            }
            else
            {
                ketQua.benDiTruoc = benTanCong;
                ketQua.benDiSau = benPhongThu;
                ketQua.log = "[SpeedPhase] Hai ben bang speed, ben tan cong duoc uu tien di truoc.";
            }

            Debug.Log($"{ketQua.log} (TanCong: {benTanCong.ten} - {benTanCong.speed}, PhongThu: {benPhongThu.ten} - {benPhongThu.speed})");
            return ketQua;
        }

        public SkillPhaseKetQua XuLySkillPhase(SpeedPhaseKetQua speedPhase)
        {
            SkillPhaseKetQua ketQua = new SkillPhaseKetQua();

            if (speedPhase == null || speedPhase.benDiTruoc == null || speedPhase.benDiSau == null)
            {
                ketQua.log = "[SkillPhase] Loi: Du lieu speed phase khong hop le.";
                Debug.LogWarning(ketQua.log);
                return ketQua;
            }

            Tuong nguoiDiTruoc = speedPhase.benDiTruoc;
            Tuong nguoiDiSau = speedPhase.benDiSau;

            ketQua.log += xuLyKyNang.DungSkillTheoUuTien(speedPhase, nguoiDiTruoc, nguoiDiSau, ketQua, "NguoiDiTruoc");

            if (ketQua.skipExchangeDamage)
            {
                Debug.Log(ketQua.log);
                return ketQua;
            }

            ketQua.log += xuLyKyNang.DungSkillTheoUuTien(speedPhase, nguoiDiSau, nguoiDiTruoc, ketQua, "NguoiDiSau");
            Debug.Log(ketQua.log);
            return ketQua;
        }

        public DamagePhaseKetQua XuLyDamagePhase(SpeedPhaseKetQua speedPhase, SkillPhaseKetQua skillPhase)
        {
            DamagePhaseKetQua ketQua = new DamagePhaseKetQua();

            if (skillPhase != null && skillPhase.skipExchangeDamage)
            {
                ketQua.boQuaDamagePhase = true;
                ketQua.log = "[DamagePhase] Bo qua vi SkillPhase da kich hoat skipExchangeDamage.";
                Debug.Log(ketQua.log);
                return ketQua;
            }

            if (speedPhase == null || speedPhase.benDiTruoc == null || speedPhase.benDiSau == null)
            {
                ketQua.log = "[DamagePhase] Loi: Du lieu speed phase khong hop le.";
                Debug.LogWarning(ketQua.log);
                return ketQua;
            }

            Tuong benTanCong = speedPhase.benTanCong;
            Tuong benPhongThu = speedPhase.benPhongThu;

            float giaTriTanCongTuBenTanCong = benTanCong.atk;
            float giaTriPhongThuTuBenPhongThu = benPhongThu.def;

            float heSoHeTanCong = TinhHeSoHeKhacChe(benTanCong.he, benPhongThu.he);
            float heSoHePhongThu = TinhHeSoHeKhacChe(benPhongThu.he, benTanCong.he);
            giaTriTanCongTuBenTanCong *= heSoHeTanCong;
            giaTriPhongThuTuBenPhongThu *= heSoHePhongThu;

            bool chiMangBenTanCong = Random.value <= benTanCong.tiLeChiMang;
            if (chiMangBenTanCong)
            {
                giaTriTanCongTuBenTanCong *= 1.15f;
            }

            bool chiMangBenPhongThu = Random.value <= benPhongThu.tiLeChiMang;
            if (chiMangBenPhongThu)
            {
                giaTriPhongThuTuBenPhongThu *= 1.15f;
            }

            float satThuongLenBenPhongThu = Mathf.Max(0f, giaTriTanCongTuBenTanCong);
            float satThuongLenBenTanCong = Mathf.Max(0f, giaTriPhongThuTuBenPhongThu);

            benTanCong.atk = Mathf.Max(0f, benTanCong.atk - satThuongLenBenTanCong);
            benPhongThu.def = Mathf.Max(0f, benPhongThu.def - satThuongLenBenPhongThu);

            bool benTanCongDiTruoc = speedPhase.benDiTruoc == benTanCong;
            ketQua.satThuongGayRaBoiBenDiTruoc = benTanCongDiTruoc ? satThuongLenBenPhongThu : satThuongLenBenTanCong;
            ketQua.satThuongGayRaBoiBenDiSau = benTanCongDiTruoc ? satThuongLenBenTanCong : satThuongLenBenPhongThu;
            ketQua.benDiTruocChiMang = benTanCongDiTruoc ? chiMangBenTanCong : chiMangBenPhongThu;
            ketQua.benDiSauChiMang = benTanCongDiTruoc ? chiMangBenPhongThu : chiMangBenTanCong;
            ketQua.heSoHeBenDiTruoc = benTanCongDiTruoc ? heSoHeTanCong : heSoHePhongThu;
            ketQua.heSoHeBenDiSau = benTanCongDiTruoc ? heSoHePhongThu : heSoHeTanCong;
            ketQua.benDiTruocChet = benTanCongDiTruoc
                ? KiemTraTuongChetTheoVaiTroCombat(benTanCong, true)
                : KiemTraTuongChetTheoVaiTroCombat(benPhongThu, false);
            ketQua.benDiSauChet = benTanCongDiTruoc
                ? KiemTraTuongChetTheoVaiTroCombat(benPhongThu, false)
                : KiemTraTuongChetTheoVaiTroCombat(benTanCong, true);

            ketQua.log =
                $"[DamagePhase] HeSoHe(TanCong:{heSoHeTanCong:0.00}, PhongThu:{heSoHePhongThu:0.00}) | " +
                 $"ChiMang(TanCong:{chiMangBenTanCong}) | " +
                $"ChiMang(PhongThu:{chiMangBenPhongThu}) | " +
                $"Exchange(ATK->DEF:{satThuongLenBenPhongThu:0.00}, DEF->ATK:{satThuongLenBenTanCong:0.00}) | " +
                $"Stat Con Lai(TanCong.ATK:{benTanCong.atk:0.00}, PhongThu.DEF:{benPhongThu.def:0.00}) | " +
                $"Chet(A:{ketQua.benDiTruocChet}, B:{ketQua.benDiSauChet})";

            Debug.Log(ketQua.log);
            return ketQua;
        }

        public PassivePhaseKetQua XuLyPassivePhase(SpeedPhaseKetQua speedPhase, DamagePhaseKetQua damagePhase)
        {
            PassivePhaseKetQua ketQua = new PassivePhaseKetQua();

            if (speedPhase == null || speedPhase.benDiTruoc == null || speedPhase.benDiSau == null)
            {
                ketQua.log = "[PassivePhase] Loi: Du lieu speed phase khong hop le.";
                Debug.LogWarning(ketQua.log);
                return ketQua;
            }

            Tuong benTanCong = speedPhase.benTanCong;
            Tuong benPhongThu = speedPhase.benPhongThu;

            bool benTanCongConSong = !KiemTraTuongChetTheoVaiTroCombat(benTanCong, true);
            bool benPhongThuConSong = !KiemTraTuongChetTheoVaiTroCombat(benPhongThu, false);

            if (benTanCongConSong)
            {
                float tangThem = benTanCong.atk * 0.05f;
                benTanCong.atk += tangThem;
                ketQua.log += $"[PassivePhase] {benTanCong.ten} con song -> kich hoat noi tai: +5% ATK (+{tangThem:0.00}).\n";
            }
            else
            {
                ketQua.log += $"[PassivePhase] {benTanCong.ten} da chet -> khong kich hoat noi tai.\n";
            }

            if (benPhongThuConSong)
            {
                float tangThem = benPhongThu.def * 0.05f;
                benPhongThu.def += tangThem;
                ketQua.log += $"[PassivePhase] {benPhongThu.ten} con song -> kich hoat noi tai: +5% DEF (+{tangThem:0.00}).\n";
            }
            else
            {
                ketQua.log += $"[PassivePhase] {benPhongThu.ten} da chet -> khong kich hoat noi tai.\n";
            }

            ketQua.benTanCongConSong = benTanCongConSong;
            ketQua.benPhongThuConSong = benPhongThuConSong;
            Debug.Log(ketQua.log);
            return ketQua;
        }

        private float TinhHeSoHeKhacChe(He heTanCong, He hePhongThu)
        {
            if ((heTanCong == He.Lua && hePhongThu == He.Dat) ||
                (heTanCong == He.Dat && hePhongThu == He.Nuoc) ||
                (heTanCong == He.Nuoc && hePhongThu == He.Lua) ||
                (heTanCong == He.Gio && hePhongThu == He.Bang) ||
                (heTanCong == He.Bang && hePhongThu == He.Set) ||
                (heTanCong == He.Set && hePhongThu == He.Gio))
            {
                return 1.2f;
            }

            if ((hePhongThu == He.Lua && heTanCong == He.Dat) ||
                (hePhongThu == He.Dat && heTanCong == He.Nuoc) ||
                (hePhongThu == He.Nuoc && heTanCong == He.Lua) ||
                (hePhongThu == He.Gio && heTanCong == He.Bang) ||
                (hePhongThu == He.Bang && heTanCong == He.Set) ||
                (hePhongThu == He.Set && heTanCong == He.Gio))
            {
                return 0.9f;
            }

            return 1f;
        }

        private bool KiemTraTuongChetTheoVaiTroCombat(Tuong tuong, bool laBenTanCong)
        {
            if (tuong == null)
            {
                return false;
            }

            if (laBenTanCong)
            {
                return tuong.atk <= 0f;
            }

            return tuong.def <= 0f;
        }
    }

    [System.Serializable]
    public class SpeedPhaseKetQua
    {
        public Tuong benDiTruoc;
        public Tuong benDiSau;
        public Tuong benTanCong;
        public Tuong benPhongThu;
        public string log;
    }

    [System.Serializable]
    public class SkillPhaseKetQua
    {
        public bool skipExchangeDamage;
        public bool coKichHoatSkill;
        public Tuong tuongBiHa;
        public Tuong mucTieuBiChanSkill;
        public string log = string.Empty;
    }

    [System.Serializable]
    public class PassivePhaseKetQua
    {
        public bool benTanCongConSong;
        public bool benPhongThuConSong;
        public string log = string.Empty;
    }

    [System.Serializable]
    public class DamagePhaseKetQua
    {
        public bool boQuaDamagePhase;
        public float satThuongGayRaBoiBenDiTruoc;
        public float satThuongGayRaBoiBenDiSau;
        public float heSoHeBenDiTruoc;
        public float heSoHeBenDiSau;
        public bool benDiTruocChiMang;
        public bool benDiSauChiMang;
        public bool benDiTruocChet;
        public bool benDiSauChet;
        public string log = string.Empty;
    }
}
