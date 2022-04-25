using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitAddInOptimalCuttingRebarSolution.Model.Rebar;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary.ProcessHandelData
{
    public class ProcessCutRebar
    {
        #region Property
        public List<RebarElement> ChainRebarToHandle;

        const double FreshRebarLength = 11700;
        const string IdNewFreshRebar = "new";
        #endregion

        #region Constructor
        public ProcessCutRebar(List<RebarElement> chainRebar)
        {
            ChainRebarToHandle = chainRebar;
        }
        #endregion

        #region Method
        public void Process()
        {
            for (int i = 0; i < ChainRebarToHandle.Count; i++)
            {
                if (i == 0)
                {
                    ChainRebarToHandle[i].InputRebarId = IdNewFreshRebar;
                    ChainRebarToHandle[i].InputRebarLength = FreshRebarLength;
                }
                else
                {
                    // Tìm thanh thép phía trước đó, có chỉ số thép dư khớp với chiều dài thanh thép hiện tại
                    int indexResult = 0;

                    for (int j = 0; j < i; j++)
                    {
                        double differentLength = double.MaxValue;

                        if (ChainRebarToHandle[j].ResidualRebarLength >= ChainRebarToHandle[i].Length &&
                            ChainRebarToHandle[j].ResidualRebarLength - ChainRebarToHandle[i].Length < differentLength)
                        {
                            indexResult = j;
                        }
                    }

                    ChainRebarToHandle[i].InputRebarId = ChainRebarToHandle[indexResult].Id;
                    ChainRebarToHandle[i].InputRebarLength = ChainRebarToHandle[indexResult].Length;
                }

                ChainRebarToHandle[i].ResidualRebarLength = ChainRebarToHandle[i].InputRebarLength - ChainRebarToHandle[i].Length;
                ChainRebarToHandle[i].IsWasteRebar = true;
            }
        }
        #endregion

    }
}
