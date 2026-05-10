using UnityEngine;

namespace TheBattleCard
{
    public class XuLyHieuUng
    {
        public string ThemHieuUng(
            Tuong mucTieu,
            TrangThaiHieuUng hieuUngMoi)
        {
            if (mucTieu == null)
            {
                return "[XuLyHieuUng] Khong the them hieu ung vi muc tieu null.\n";
            }

            if (hieuUngMoi == null)
            {
                return $"[XuLyHieuUng] {mucTieu.ten} nhan hieu ung null.\n";
            }

            if (mucTieu.danhSachHieuUng == null)
            {
                mucTieu.danhSachHieuUng =
                    new System.Collections.Generic.List<TrangThaiHieuUng>();
            }

            mucTieu.danhSachHieuUng.Add(hieuUngMoi);

            return
                $"[XuLyHieuUng] Them {hieuUngMoi.loaiHieuUng} vao {mucTieu.ten} " +
                $"({hieuUngMoi.soRoundConLai} round).\n";
        }

        public bool CoHieuUng(
            Tuong tuong,
            LoaiHieuUng loaiHieuUng)
        {
            if (tuong == null)
            {
                return false;
            }

            if (tuong.danhSachHieuUng == null)
            {
                return false;
            }

            for (int i = 0; i < tuong.danhSachHieuUng.Count; i++)
            {
                TrangThaiHieuUng hieuUng =
                    tuong.danhSachHieuUng[i];

                if (hieuUng == null)
                {
                    continue;
                }

                if (hieuUng.loaiHieuUng == loaiHieuUng &&
                    hieuUng.soRoundConLai > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public string XuLyHieuUngDauRound(
            Tuong tuong,
            bool laBenTanCong)
        {
            if (tuong == null)
            {
                return "[XuLyHieuUng] Tuong null.\n";
            }

            if (tuong.danhSachHieuUng == null ||
                tuong.danhSachHieuUng.Count == 0)
            {
                return $"[XuLyHieuUng] {tuong.ten} khong co hieu ung.\n";
            }

            string log = string.Empty;

            for (int i = tuong.danhSachHieuUng.Count - 1; i >= 0; i--)
            {
                TrangThaiHieuUng hieuUng =
                    tuong.danhSachHieuUng[i];

                if (hieuUng == null)
                {
                    tuong.danhSachHieuUng.RemoveAt(i);
                    continue;
                }

                switch (hieuUng.loaiHieuUng)
                {
                    case LoaiHieuUng.Burn:
                        log += XuLyBurn(
                            tuong,
                            hieuUng,
                            laBenTanCong);
                        break;

                    case LoaiHieuUng.Freeze:
                        log +=
                            $"[XuLyHieuUng] {tuong.ten} dang bi Freeze.\n";
                        break;

                    default:
                        log +=
                            $"[XuLyHieuUng] {tuong.ten} co hieu ung chua ho tro.\n";
                        break;
                }

                hieuUng.soRoundConLai--;

                log +=
                    $"[XuLyHieuUng] {hieuUng.loaiHieuUng} con lai " +
                    $"{hieuUng.soRoundConLai} round.\n";

                if (hieuUng.soRoundConLai <= 0)
                {
                    log +=
                        $"[XuLyHieuUng] Xoa {hieuUng.loaiHieuUng} khoi {tuong.ten}.\n";

                    tuong.danhSachHieuUng.RemoveAt(i);
                }
            }

            return log;
        }

        private string XuLyBurn(
            Tuong tuong,
            TrangThaiHieuUng hieuUng,
            bool laBenTanCong)
        {
            float giaTriDot =
                Mathf.Max(0f, hieuUng.giaTriHieuUng);

            if (laBenTanCong)
            {
                float atkTruoc = tuong.atk;

                tuong.atk =
                    Mathf.Max(0f, tuong.atk - giaTriDot);

                return
                    $"[XuLyHieuUng] Burn -> {tuong.ten} mat " +
                    $"{giaTriDot:0.00} ATK " +
                    $"({atkTruoc:0.00} -> {tuong.atk:0.00}).\n";
            }

            float defTruoc = tuong.def;

            tuong.def =
                Mathf.Max(0f, tuong.def - giaTriDot);

            return
                $"[XuLyHieuUng] Burn -> {tuong.ten} mat " +
                $"{giaTriDot:0.00} DEF " +
                $"({defTruoc:0.00} -> {tuong.def:0.00}).\n";
        }
    }
}
