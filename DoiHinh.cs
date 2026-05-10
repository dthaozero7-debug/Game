using System.Collections.Generic;
using UnityEngine;

namespace TheBattleCard
{
    [System.Serializable]
    public class DoiHinh
    {
        public List<Tuong> danhSachTuong = new List<Tuong>();
        public int chiSoTuongHienTai;

        public Tuong LayTuongHienTai()
        {
            if (danhSachTuong == null || danhSachTuong.Count == 0)
            {
                Debug.LogWarning("[DoiHinh] Danh sach tuong rong.");
                return null;
            }

            if (chiSoTuongHienTai < 0 || chiSoTuongHienTai >= danhSachTuong.Count)
            {
                Debug.LogWarning($"[DoiHinh] Chi so tuong hien tai khong hop le: {chiSoTuongHienTai}.");
                return null;
            }

            return danhSachTuong[chiSoTuongHienTai];
        }

        public bool ChuyenSangTuongTiepTheoNeuTuongHienTaiChet(bool laBenTanCong)
        {
            Tuong tuongHienTai = LayTuongHienTai();
            if (tuongHienTai == null)
            {
                return false;
            }

            bool tuongDaChet = laBenTanCong ? tuongHienTai.atk <= 0f : tuongHienTai.def <= 0f;
            if (!tuongDaChet)
            {
                return false;
            }

            chiSoTuongHienTai++;

            if (chiSoTuongHienTai < danhSachTuong.Count)
            {
                Debug.Log($"[DoiHinh] Chuyen sang tuong tiep theo: {danhSachTuong[chiSoTuongHienTai].ten}.");
                return true;
            }

            Debug.Log("[DoiHinh] Khong con tuong de chuyen.");
            return false;
        }

        public bool KiemTraDoiHinhDaThua(bool laBenTanCong)
        {
            if (danhSachTuong == null || danhSachTuong.Count == 0)
            {
                return true;
            }

            for (int i = chiSoTuongHienTai; i < danhSachTuong.Count; i++)
            {
                Tuong tuong = danhSachTuong[i];
                bool tuongConSong = laBenTanCong ? tuong.atk > 0f : tuong.def > 0f;
                if (tuongConSong)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
