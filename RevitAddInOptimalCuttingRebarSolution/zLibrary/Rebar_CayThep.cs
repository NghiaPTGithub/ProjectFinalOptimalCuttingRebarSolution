using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class Rebar_CayThep//Thep
    {
        internal string ID;
        internal string Name;
        internal int Length;
        internal Steel Material;
        #region Thuộc tính xử lý thép
        //1. Thép nguyên liệu
        //id cua thanh thep nguyen lieu: nếu là thép nguyên cây thì id = "InputNew"
        //còn nếu tận dụng dc thì id là id của thanh thép sau khi dc cắt dư ra thanh nguyên liệu
        public string IdRebarSource;

        //chiều dai thanh thép nguyên liệu
        public int LengthRebarSource;

        //2. Thép dư sau cắt
        //chiều dài thép dư
        public int LengthResidual;

        //nếu thanh thép dư cắt ra sau khi gia công thanh thép này chưa được tận dụng,
        //biến này sẽ là true, thể hiện thép ở trạng thái thép vụn, khi được tận dụng, set nó sang false
        public bool IsRebarWastage;

        #endregion 
        //internal List <int> ToaDoVungNoi;
        internal Rebar_CayThep() { }
        internal Rebar_CayThep(string id, int length, Steel material)
        {
            ID = id;
            Name = id;
            Length = length;
            Material = material;

            IdRebarSource = "";
            LengthRebarSource = 0;
            LengthResidual = 0;
            IsRebarWastage = true;
        }

        //phương thức lấy về khối lượng thanh thép
        internal double GetMass()
        {
            return Length * Material.SpecificWeight;
        }
    }
}
