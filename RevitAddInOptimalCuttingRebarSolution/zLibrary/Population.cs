using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class Population //QuanThe
    {
        //bien so kiem soat id
        protected int CountSteel;
        protected int CountElementGA;//CountElement
        protected int CountRebar;//CountRebar
        protected int CountIndividual;//CountTrinhTuElement
        //chua phuong thuc thong ke cac steel trong phan tu gen
        internal int Populationsize; //SLCaTheMax
        internal int MaxGenerationQuantity;//SLTheHeMax
        internal int GenerationQuantity; //SLTheHe
        internal int IndividualSize;//SizeCaThe
        internal double RatioHybridizeElement;//TyLeLaiGhepTrenQuanThe
        internal double RatioMutateElement;//TyLeDotBienTrenQuanThe
        internal double RatioMutateLocal;//TyLeDotBienCucBoTrenQuanThe
        internal int FactorMutateElement; //HeSoDotBienTrenQuanThe
        internal int FactorMutateLocal;//HeSoDotBienCucBo
        internal double RatioMutateElementLocal;//TyLeDotBienCucBoTrenElement
        internal List<ElementGA> ListElementGene;//list chua cac phan tu element tao nen gen - ListPhanTuGenElement
        internal List<IndividualChainElement> ListIndividual;//list chua cac trinh tu la cac ca the - ListTrinhTuElement can xu ly trc khi dua vao
        internal List<Steel> ListSteelUsed;//danh sach cach vat lieu trong chuoi thep
        internal IndividualChainElement BestIndividual;
        public Population(int slCaTheMax, int slTheHeMax, List<ElementGA> listPhanTuGen, List<Steel> listSteel)
        {
            Populationsize = slCaTheMax;
            MaxGenerationQuantity = slTheHeMax;
            GenerationQuantity = 0;
            ListElementGene = listPhanTuGen;
            ListIndividual = new List<IndividualChainElement>();
            ListSteelUsed = listSteel;
            IndividualSize = ListElementGene.Count;
            RatioHybridizeElement = 0.5;
            RatioMutateElement = 0.5;
            RatioMutateLocal = 0.9;
            FactorMutateElement = 2;
            FactorMutateLocal = 2;
            RatioMutateElementLocal = 0.8;

            //Setup id
            CountSteel = 0;
            CountRebar = 0;
            CountElementGA = 0;
            CountIndividual = 0;
            foreach (Steel steel in ListSteelUsed)
            {
                steel.ID = CreateNewID("Steel");
            }
            foreach (ElementGA elem in ListElementGene)
            {
                elem.ID = CreateNewID("Element");
                foreach (Rebar_CayThep t in elem.ListRebar)
                {
                    t.ID = CreateNewID("Rebar");
                }
            }
        }
        public Population()
        {

        }
        protected void Create() //KhoiTaoQuanThe
        {
            //sinh ngau nhien trinh tu gen tu ListPhanTuGen, cho toi khi so ca the dat SLCaTheBanDau
            //1. Xu li ngoai le
            if (IndividualSize == 0)
            {
                //khoi tao ngoại le
                Exception KhongCoPhanTu = new Exception("Khong co phan tu Gen");
                throw KhongCoPhanTu;
            }
            //2. Sinh ngau nhien 1 chuoi trinh tu element cho toi khi so ca the dat SLCaTheMax
            IndividualChainElement t0 = new IndividualChainElement(CreateNewID("Sequence"), ListElementGene);
            t0.TinhChiPhiTyLeHaoHutKL(ListSteelUsed);
            ListIndividual.Add(t0);
            while (ListIndividual.Count < Populationsize)
            {
                //Sinh chuoi trinh tu cac element
                IndividualChainElement t = new IndividualChainElement();
                t = MutateElement(t0, (int)(0.5 * IndividualSize), CreateNewID("Sequence"));
                t.TinhChiPhiTyLeHaoHutKL(ListSteelUsed);
                ListIndividual.Add(t);
            }
            SortListIndividual();
            //ListIndividual.
            //    Sort((t1, t2) =>
            //    {
            //        if (t1.RatioWasteMass > t2.RatioWasteMass) return 1;
            //        else if (t1.RatioWasteMass == t2.RatioWasteMass) return 0;
            //        return -1;
            //    });
        }
        protected IndividualChainElement HybridizeElement(IndividualChainElement t1, IndividualChainElement t2, string idchild) //LaiGhepElement
        {

            //xu li ngoai le khi 2 doan gen dai khong bang nhau
            if (t1.ChainElement.Count != t2.ChainElement.Count)
            {
                Exception e1 = new Exception("2 doan gen khong bang nhau");
                throw e1;
            }
            if (t1.ChainElement.Count * t2.ChainElement.Count == 0)
            {
                Exception e2 = new Exception("Trinh tu rong ");
                throw e2;
            }
            int length = t1.ChainElement.Count;
            //Lai ghep 1 diem cat
            List<ElementGA> listElementChild = new List<ElementGA>();
            //1. Chon ngau nhien vi tri diem cat
            Random position = new Random();
            int p = position.Next(length - 1);
            //2. Trao doi doan gen duoc chon
            int i = 0, j = 0;
            for (i = p; i < length; i++)
            {
                listElementChild.Add(t2.ChainElement[i]);
            }
            for (i = 0; i < length; i++)
            {
                for (j = 0; j < length - p; j++)
                {
                    if (t1.ChainElement[i].Name == listElementChild[j].Name) break;
                    else
                    {
                        if (j == length - p - 1)
                        {
                            listElementChild.Add(t1.ChainElement[i]);
                        }
                    }
                }
            }
            return new IndividualChainElement(idchild, listElementChild);
        }
        protected IndividualChainElement MutateElement(IndividualChainElement t, int hesoDotBien, string idNew) //DotBienElement
        {
            List<ElementGA> listElementChild = new List<ElementGA>();
            int n = t.ListInputRebar.Count;
            Random r = new Random();
            int count = r.Next(hesoDotBien) + 1;
            int p1, p2;
            foreach (ElementGA elementGA in t.ChainElement)
            {
                listElementChild.Add(elementGA);
            }

            while (count > 0)
            {
                p1 = r.Next(n);
                p2 = r.Next(n);
                var temp = listElementChild[p1];
                listElementChild[p1] = listElementChild[p2];
                listElementChild[p2] = temp;
                count--;
            }
            return new IndividualChainElement(idNew, listElementChild);
        }
        protected IndividualChainElement MutateElementLocal(IndividualChainElement t, double tyleDotBien, int hesoDotBienCucBo, string idNew) //DotBienCucBo
        {
            if (0 > tyleDotBien || tyleDotBien > 1)
            {
                Exception e1 = new Exception("Gia tri ty le dot bien khong phu hop");
                throw e1;
            }
            //so luong cac element duoc lay ra trong list element de dot bien cuc bo 
            int slRandom = (int)(tyleDotBien * t.ChainElement.Count);
            //tao trinh tu moi
            IndividualChainElement tg = new IndividualChainElement(idNew, t.ChainElement);

            //chon ngau nhien cac phan tu de tien hanh dot bien
            List<int> listIndexElementDotBien = new List<int>();//list index cua cac phan tu dc chon ngau nhien de dot bien
            listIndexElementDotBien = GetRandomIndex(t.ChainElement.Count, tyleDotBien);
            foreach (int j in listIndexElementDotBien)
            {
                //chuyen list thep ve arraylist
                ElementGA e = tg.ChainElement[j];
                tg.ChainElement.RemoveAt(j);
                tg.ChainElement.Insert(j, DotBienTaiElement(e, hesoDotBienCucBo, CreateNewID("Element")));
            }
            return tg;
        }
        protected void NaturalSelectionVersion1()
        {
            //1.Sap xep cac trinh tu element trong List TrinhTuElement theo chieu ty le hao hut giam dan
            ListIndividual.Sort((t1, t2) => { if (t1.RatioWasteMass > t2.RatioWasteMass) return 1; else if (t1.RatioWasteMass == t2.RatioWasteMass) return 0; return -1; });
            if (ListIndividual[0].RatioWasteMass > 0)
            {
                while (ListIndividual.Count > Populationsize)
                {
                    //2.Chon ngau nhien 1 chi so a (0 < a < 1)
                    Random r = new Random();
                    int index = r.Next(1, Populationsize);
                    //3. Loai ca the thu a * [Kich thuoc quan the toi da] cho toi khi kich thuoc quan the bang kich thuoc quan the toi da
                    ListIndividual.RemoveAt(index);
                }
            }
        }
        protected void NaturalSelectionWithRulet()
        {
            //thiết lập trọng số 
            //Giữ lại k phần tử đầu tiên trong list các phần tử đã được sắp xếp theo trình tự hao hụt tăng dần
            int k = ListIndividual.Count < 100 ? 1 : 10;
            //1. Sap xep cac trinh tu element theo thu tu tang dan cua hao hut
            SortListIndividual();
            //ListIndividual.Sort((t1, t2) => { if (t1.RatioWasteMass > t2.RatioWasteMass) return 1; else if (t1.RatioWasteMass == t2.RatioWasteMass) return 0; return -1; });
            if (ListIndividual[0].RatioWasteMass > 0)
            {
                List<IndividualChainElement> tg = new List<IndividualChainElement>();
                foreach (IndividualChainElement individualChainElement in ListIndividual)
                {
                    tg.Add(individualChainElement);
                }
                ListIndividual = new List<IndividualChainElement>();
                for (int i = 0; i < k; i++)
                {
                    ListIndividual.Add(tg[i]);
                }
                //2. Tinh ham uoc luong cho tung ca the fitness=1/Ty le hao hut;
                //3. Quay banh xe Rulet tim ca the de dua vao the he moi
                while (ListIndividual.Count < Populationsize)
                {
                    List<double> fitness = new List<double>();
                    double sumFitness = 0;
                    foreach (IndividualChainElement trinhTu in tg)
                    {
                        //hàm mục tiêu fitness = 1/ tỷ lệ hao hụt  
                        sumFitness = sumFitness + 1 / trinhTu.RatioWasteMass;
                        fitness.Add(1 / trinhTu.RatioWasteMass);
                    }
                    double sumEval = 0;
                    List<double> listEval = new List<double>();
                    for (int i = 0; i < fitness.Count; i++)
                    {
                        listEval.Add(fitness[i] / sumFitness);
                        sumEval = sumEval + listEval[i];
                    }
                    List<double> listXacSuat = new List<double>();
                    for (int i = 0; i < listEval.Count; i++)
                    {
                        listXacSuat.Add(listEval[i] / sumEval);
                    }
                    List<double> listSumXacSuat = new List<double>();
                    double sumXacSuat = 0;
                    for (int i = 0; i < listXacSuat.Count; i++)
                    {
                        sumXacSuat = sumXacSuat + listXacSuat[i];
                        listSumXacSuat.Add(sumXacSuat);
                    }
                    int indexRulet = 0;
                    Random r = new Random();
                    double a = (double)r.NextDouble();
                    for (int i = 0; i < listSumXacSuat.Count; i++)
                    {
                        if (listSumXacSuat[i] >= a)
                        {
                            indexRulet = i; break;
                        }
                    }
                    ListIndividual.Add(tg[indexRulet]);
                    //tg.RemoveAt(indexRulet);
                }
            }
            SortListIndividual();

        }
        public void Evolution()
        {
            //1.Khoi tao quan the
            Create();
            //2.Thuc hien lap cho toi khi: so luong the he dat toi da hoac sau n the he, trinh tu ca the khong thay doi
            int sizeLaiGhep = (int)(Populationsize * RatioHybridizeElement);
            int sizeDotBien = (int)(Populationsize * RatioMutateElement);
            int sizeDotBienCucBo = (int)(Populationsize * RatioMutateLocal);
            while (GenerationQuantity < MaxGenerationQuantity && (ListIndividual[0].RatioWasteMass > 0))
            {
                List<IndividualChainElement> listMutate = new List<IndividualChainElement>();
                listMutate = ListIndividual.GetRandomList(sizeDotBien);

                List<IndividualChainElement> listMutateLocal = new List<IndividualChainElement>();
                listMutateLocal = ListIndividual.GetRandomList(sizeDotBienCucBo);

                //2.1 Lai ghep: chon ngau nhien a%*Populationsize cap trong Populationsize ca the dau tien, lai ghep chung voi nhau tao 2 con, ket qua them vao list cac ca the
                int slCapLaiGhepMax = (int)(RatioHybridizeElement * Populationsize);
                int countParents = 0;
                while (countParents < sizeLaiGhep)
                {
                    IndividualChainElement child1 = new IndividualChainElement();
                    IndividualChainElement child2 = new IndividualChainElement();
                    //lua chon bo me
                    List<int> listParents = new List<int>();//index cua bo me
                    listParents = GetRandomIndex(Populationsize, 2);
                    child1 = HybridizeElement(ListIndividual[listParents[0]], ListIndividual[listParents[1]], CreateNewID("Sequence"));
                    child2 = HybridizeElement(ListIndividual[listParents[0]], ListIndividual[listParents[1]], CreateNewID("Sequence"));
                    child1.TinhChiPhiTyLeHaoHutKL(ListSteelUsed);
                    child2.TinhChiPhiTyLeHaoHutKL(ListSteelUsed);
                    ListIndividual.Add(child1);
                    ListIndividual.Add(child2);
                    countParents++;
                }
                //2.2 Chon ngau nhien RatioMutateElement%*Populationsize ca the trong quan the de tien hanh dot bien level element, ket qua them vao list cac ca the

                foreach (IndividualChainElement individualChainElement in listMutate)
                {
                    IndividualChainElement trinhTuElementDotBien = new IndividualChainElement();
                    trinhTuElementDotBien = MutateElement(individualChainElement, FactorMutateElement, CreateNewID("Sequence"));
                    trinhTuElementDotBien.TinhChiPhiTyLeHaoHutKL(ListSteelUsed);
                    ListIndividual.Add(trinhTuElementDotBien);
                }

                ////2.2 Dot bien: gay dot bien cac ca the con lai, ket qua them vao list cac ca the
                //for (int i = sizeLaiGhep; i < Populationsize; i++)
                //{
                //    IndividualChainElement trinhTuElementDotBien = new IndividualChainElement();
                //    trinhTuElementDotBien = MutateElement(ListIndividual[i], FactorMutateElement, CreateNewID("Sequence"));
                //    trinhTuElementDotBien.TinhChiPhiTyLeHaoHutKL(ListSteelUsed);
                //    ListIndividual.Add(trinhTuElementDotBien);
                //}

                //2.3 Dot bien cuc bo tai element: gay dot bien c% so luong ca the cuoi cung, ket qua them vao list cac ca the

                foreach (IndividualChainElement individualChainElement in listMutateLocal)
                {
                    IndividualChainElement trinhTuElementDotBienCucBo = new IndividualChainElement();
                    trinhTuElementDotBienCucBo = MutateElementLocal(individualChainElement, RatioMutateElementLocal, FactorMutateLocal, CreateNewID("Sequence"));
                    trinhTuElementDotBienCucBo.TinhChiPhiTyLeHaoHutKL(ListSteelUsed);
                    ListIndividual.Add(trinhTuElementDotBienCucBo);
                }
                //2.4 Chon loc tu nhien
                NaturalSelectionWithRulet();
                GenerationQuantity++;
            }
            //3.Lay ca the dung dau danh sach. Neu co m ca the co hao hut giong nhau thi lay ca the co so vet cat it nhat
            BestIndividual = ListIndividual.FirstOrDefault();
        }
        protected ElementGA DotBienTaiElement(ElementGA e, int hesoDotBien, string idNew)
        {
            //chuyen list thep ve arraylist
            ArrayList arr = new ArrayList();
            arr.AddRange(e.ListRebar);
            //dot bien array list
            DotBienArrayList db = new DotBienArrayList(arr);
            ArrayList kq = db.GetNewArrayList(hesoDotBien);
            //chuyen array list ve list thep
            List<Rebar_CayThep> result = new List<Rebar_CayThep>();
            foreach (var item in kq)
            {
                result.Add(item as Rebar_CayThep);
            }
            return new ElementGA(idNew, e.Name, result);
        }//sub methode
        protected string CreateNewID(string typeObject)
        {
            string result = "";
            switch (typeObject)
            {
                case "Steel": { CountSteel++; result = "S." + CountSteel; break; }
                case "Rebar": { CountRebar++; result = "R." + CountRebar; break; }
                case "Element": { CountElementGA++; result = "E." + CountElementGA; break; }
                case "Sequence": { CountIndividual++; result = "SQ." + CountIndividual; break; }
                default: { result = ""; break; }
            }
            return result;
        }
        private List<Rebar_CayThep> ConvertToListThep(List<ElementGA> listPhanTuGen)//phuong thuc phan ra chuoi element thanh thanh chuoi cac thanh thep
        {
            List<Rebar_CayThep> result = new List<Rebar_CayThep>();
            foreach (ElementGA e in listPhanTuGen)
            {
                result.AddRange(e.ListRebar);
            }
            return result;
        }
        private ArrayList ChangeArrayIndex(ArrayList arr, List<int> listIndex)//thay doi trinh tu arrlist theo index input
        {
            ArrayList result = new ArrayList();
            //xu li ngoai le
            if (listIndex.Count != arr.Count)
            {
                Exception e1 = new Exception("Do dai 2 list khac nhau");
                throw e1;
            }
            //
            foreach (int i in listIndex)
            {
                result.Add(arr[i]);
            }
            return result;
        }
        private List<Rebar_CayThep> ArrToListThep(ArrayList arr)
        {
            if (!(arr[0] is Rebar_CayThep))
            {
                Exception e1 = new Exception("Array khong phai Thep");
                throw e1;
            }
            List<Rebar_CayThep> result = new List<Rebar_CayThep>();
            foreach (var item in arr)
            {
                result.Add(item as Rebar_CayThep);
            }
            return result;
        }
        private List<ElementGA> ArrToListElement(ArrayList arr)
        {
            if (!(arr[0] is ElementGA))
            {
                Exception e1 = new Exception("Array khong phai Thep");
                throw e1;
            }
            List<ElementGA> result = new List<ElementGA>();
            foreach (var item in arr)
            {
                result.Add(item as ElementGA);
            }
            return result;
        }
        private ArrayList LaiGhepArrayList(ArrayList arr1, ArrayList arr2)
        {
            ArrayList child = new ArrayList();
            //xu li ngoai le khi 2 doan gen dai khong bang nhau
            if (arr1.Count != arr2.Count)
            {
                Exception e1 = new Exception("2 doan gen khong bang nhau");
                throw e1;
            }
            if (arr1.Count * arr2.Count == 0)
            {
                Exception e2 = new Exception("Trinh tu rong ");
                throw e2;
            }
            int length = arr1.Count;
            //Lai ghep 1 diem cat
            //1. Chon ngau nhien vi tri diem cat
            Random position = new Random();
            int p = position.Next(length - 1);
            //2. Trao doi doan gen duocj chon
            int i = 0, j = 0;
            for (i = p; i < length; i++)
            {
                child.Add(arr2[i]);
            }

            for (i = 0; i < length; i++)
            {
                for (j = 0; j < length - p; j++)
                {
                    if (arr1[i].GetHashCode() == child[j].GetHashCode()) break;
                    else
                    {
                        if (j == length - p - 1)
                        {
                            child.Add(arr1[i]);
                        }
                    }
                }
            }
            return child;
        }
        private ArrayList DotBienArrayList(ArrayList arr, int hesoDotBien)
        {
            ArrayList child = new ArrayList();
            int n = arr.Count;
            Random r = new Random();
            int count = r.Next(hesoDotBien) + 1;
            int p1, p2;
            child = arr;
            while (count > 0)
            {
                p1 = r.Next(n);
                p2 = r.Next(n);
                var temp = child[p1];
                child[p1] = child[p2];
                child[p2] = temp;
                count--;
            }
            return child;
        }
        private List<Rebar_CayThep> ListElementToListRebar(List<ElementGA> listElement)
        {
            return listElement.Select(s => s.ListRebar) as List<Rebar_CayThep>;
        }
        private List<int> GetRandomIndex(int sizeList, double tyLeRandom)
        {
            if (0 > tyLeRandom || tyLeRandom > 1)
            {
                Exception e1 = new Exception("Gia tri ty le random khong phu hop");
                throw e1;
            }
            List<int> result = new List<int>();
            List<int> listIndex = new List<int>();
            for (int i = 0; i < sizeList; i++) { listIndex.Add(i); }
            int size = (int)(tyLeRandom * sizeList);
            while (result.Count < size)
            {
                Random r = new Random();
                int index = r.Next(0, listIndex.Count - 1);
                result.Add(listIndex[index]);
                listIndex.RemoveAt(index);
                r = null;
            }
            return result;
        }
        private List<int> GetRandomIndex(int sizeList, int slRandom)
        {
            if (slRandom > sizeList)
            {
                Exception e1 = new Exception("Gia tri so luong random khong phu hop");
                throw e1;
            }
            List<int> result = new List<int>();
            List<int> listIndex = new List<int>();
            for (int i = 0; i < sizeList; i++) { listIndex.Add(i); }
            while (result.Count < slRandom)
            {
                Random r = new Random();
                int index = r.Next(0, listIndex.Count - 1);
                result.Add(listIndex[index]);
                listIndex.RemoveAt(index);
                r = null;
            }
            return result;
        }
        private void SortListIndividual()
        {
            var kq = ListIndividual.OrderBy(x => x.RatioWasteMass);
            ListIndividual = kq.ToList();
        }      
    }
}


