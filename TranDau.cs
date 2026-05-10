using UnityEngine;

namespace TheBattleCard
{
    [System.Serializable]
    public class TranDau
    {
        public DoiHinh doiHinhA;
        public DoiHinh doiHinhB;
        public int soVongToiDa = 50;

        private readonly HeThongChienDau heThongChienDau = new HeThongChienDau();

        public KetQuaTranDau ChayTranDau()
        {
            KetQuaTranDau ketQua = new KetQuaTranDau();

            if (doiHinhA == null || doiHinhB == null)
            {
                ketQua.logTong += "[TranDau] Loi: Doi hinh khong hop le.\n";
                Debug.LogWarning(ketQua.logTong);
                return ketQua;
            }

            int vong = 1;
            while (!doiHinhA.KiemTraDoiHinhDaThua(true) && !doiHinhB.KiemTraDoiHinhDaThua(false) && vong <= soVongToiDa)
            {
                Tuong tuongA = doiHinhA.LayTuongHienTai();
                Tuong tuongB = doiHinhB.LayTuongHienTai();

                if (tuongA == null || tuongB == null)
                {
                    ketQua.logTong += "[TranDau] Dung tran vi khong lay duoc tuong hien tai.\n";
                    break;
                }

                ketQua.logTong += $"\n===== VONG {vong} =====\n";
                ketQua.logTong += $"[TruocVong] A:{tuongA.ten} (ATK:{tuongA.atk:0.00}, DEF:{tuongA.def:0.00}) | B:{tuongB.ten} (ATK:{tuongB.atk:0.00}, DEF:{tuongB.def:0.00})\n";

                SpeedPhaseKetQua speedPhase = heThongChienDau.XuLySpeedPhase(tuongA, tuongB);
                SkillPhaseKetQua skillPhase = heThongChienDau.XuLySkillPhase(speedPhase);

                ketQua.logTong += speedPhase.log + "\n";
                ketQua.logTong += skillPhase.log + "\n";

                if (skillPhase.skipExchangeDamage)
                {
                    ketQua.logTong += "[TranDau] SkillPhase da ha muc tieu -> cap nhat doi hinh ngay, bo qua DamagePhase/PassivePhase.\n";
                    doiHinhA.ChuyenSangTuongTiepTheoNeuTuongHienTaiChet(true);
                    doiHinhB.ChuyenSangTuongTiepTheoNeuTuongHienTaiChet(false);
                    vong++;
                    continue;
                }

                DamagePhaseKetQua damagePhase = heThongChienDau.XuLyDamagePhase(speedPhase, skillPhase);
                PassivePhaseKetQua passivePhase = heThongChienDau.XuLyPassivePhase(speedPhase, damagePhase);

                ketQua.logTong += damagePhase.log + "\n";
                ketQua.logTong += passivePhase.log;

                doiHinhA.ChuyenSangTuongTiepTheoNeuTuongHienTaiChet(true);
                doiHinhB.ChuyenSangTuongTiepTheoNeuTuongHienTaiChet(false);

                vong++;
            }

            bool doiAThua = doiHinhA.KiemTraDoiHinhDaThua(true);
            bool doiBThua = doiHinhB.KiemTraDoiHinhDaThua(false);

            if (doiAThua && doiBThua)
            {
                ketQua.ketQua = "Hoa";
            }
            else if (doiAThua)
            {
                ketQua.ketQua = "DoiBThang";
            }
            else if (doiBThua)
            {
                ketQua.ketQua = "DoiAThang";
            }
            else
            {
                ketQua.ketQua = "ChuaKetThuc";
            }

            ketQua.logTong += $"\n[KetQuaTranDau] {ketQua.ketQua}\n";
            Debug.Log(ketQua.logTong);
            return ketQua;
        }
    }

    [System.Serializable]
    public class KetQuaTranDau
    {
        public string ketQua = string.Empty;
        public string logTong = string.Empty;
    }
}
