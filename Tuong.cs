using System.Collections.Generic;
using UnityEngine;

namespace TheBattleCard
{
    [System.Serializable]
    public class Tuong
    {
        public string ten;

        public float atk;
        public float def;

        public float tiLeChiMang;

        public float khangSkill;
        public float tangHieuQuaSkill;

        public int speed;

        public He he;

        public List<TrangThaiHieuUng> danhSachHieuUng =
            new List<TrangThaiHieuUng>();

        public KyNang skill1;
        public KyNang skill2;
        public KyNang noiTai;
    }

    public enum He
    {
        Lua = 0,
        Dat = 1,
        Nuoc = 2,
        Gio = 3,
        Bang = 4,
        Set = 5
    }
}
