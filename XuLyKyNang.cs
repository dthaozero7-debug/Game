using UnityEngine;

namespace TheBattleCard
{
    public class XuLyKyNang
    {
        private readonly XuLyHieuUng xuLyHieuUng =
            new XuLyHieuUng();

        public string DungSkillTheoUuTien(
            SpeedPhaseKetQua speedPhase,
            Tuong nguoiDung,
            Tuong mucTieu,
            SkillPhaseKetQua ketQua,
            string nhanLuot)
        {
            if (nguoiDung == null || mucTieu == null)
            {
                return $"[SkillPhase] {nhanLuot}: Du lieu Tuong null.\n";
            }

            if (xuLyHieuUng.CoHieuUng(
                nguoiDung,
                LoaiHieuUng.Freeze))
            {
                return
                    $"[SkillPhase] {nguoiDung.ten} bi Freeze -> khong the dung skill.\n";
            }

            if (ketQua.mucTieuBiChanSkill == nguoiDung)
            {
                ketQua.mucTieuBiChanSkill = null;

                return
                    $"[SkillPhase] {nhanLuot}: bi chan skill, bo qua luot dung skill.\n";
            }

            bool skill1ThanhCong;

            string log = ThuKichHoatKyNang(
                speedPhase,
                nguoiDung,
                mucTieu,
                nguoiDung.skill1,
                ketQua,
                nhanLuot,
                "skill1",
                out skill1ThanhCong);

            if (!skill1ThanhCong)
            {
                bool skill2ThanhCong;

                log += ThuKichHoatKyNang(
                    speedPhase,
                    nguoiDung,
                    mucTieu,
                    nguoiDung.skill2,
                    ketQua,
                    nhanLuot,
                    "skill2",
                    out skill2ThanhCong);
            }

            return log;
        }

        private string ThuKichHoatKyNang(
            SpeedPhaseKetQua speedPhase,
            Tuong nguoiDung,
            Tuong mucTieu,
            KyNang kyNang,
            SkillPhaseKetQua ketQua,
            string nhanLuot,
            string tenO,
            out bool kichHoatThanhCong)
        {
            if (kyNang == null)
            {
                kichHoatThanhCong = false;

                return
                    $"[SkillPhase] {nhanLuot}: {tenO} null, bo qua.\n";
            }

            float roll = Random.value;

            if (roll > kyNang.tiLeKichHoat)
            {
                kichHoatThanhCong = false;

                return
                    $"[SkillPhase] {nhanLuot}: {kyNang.ten} that bai " +
                    $"(roll {roll:0.00} > tiLe {kyNang.tiLeKichHoat:0.00}).\n";
            }

            kichHoatThanhCong = true;

            string log =
                $"[SkillPhase] {nhanLuot}: {kyNang.ten} kich hoat thanh cong. ";

            switch (kyNang.loaiKyNang)
            {
                case LoaiKyNang.BuffAtk:
                    nguoiDung.atk += kyNang.effectValue;

                    log +=
                        $"Tang ATK nguoi dung +{kyNang.effectValue}.";
                    break;

                case LoaiKyNang.BuffDef:
                    nguoiDung.def += kyNang.effectValue;

                    log +=
                        $"Tang DEF nguoi dung +{kyNang.effectValue}.";
                    break;

                case LoaiKyNang.GiamAtk:
                    mucTieu.atk = Mathf.Max(
                        0f,
                        mucTieu.atk - kyNang.effectValue);

                    log +=
                        $"Giam ATK muc tieu -{kyNang.effectValue}.";
                    break;

                case LoaiKyNang.GiamDef:
                    mucTieu.def = Mathf.Max(
                        0f,
                        mucTieu.def - kyNang.effectValue);

                    log +=
                        $"Giam DEF muc tieu -{kyNang.effectValue}.";
                    break;

                case LoaiKyNang.ChanSkill:
                    ketQua.mucTieuBiChanSkill = mucTieu;

                    log +=
                        "Muc tieu bi chan skill o luot tiep theo.";
                    break;

                default:
                    log +=
                        "Loai ky nang chua ho tro o SkillPhase.";
                    break;
            }

            bool mucTieuLaBenTanCong =
                speedPhase != null &&
                speedPhase.benTanCong == mucTieu;

            bool mucTieuDaChet =
                KiemTraTuongChetTheoVaiTroCombat(
                    mucTieu,
                    mucTieuLaBenTanCong);

            if (mucTieuDaChet)
            {
                ketQua.skipExchangeDamage = true;
                ketQua.tuongBiHa = mucTieu;

                log +=
                    " Muc tieu da chet theo role combat -> skip exchange damage.";
            }

            return log + "\n";
        }

        private bool KiemTraTuongChetTheoVaiTroCombat(
            Tuong tuong,
            bool laBenTanCong)
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
}
