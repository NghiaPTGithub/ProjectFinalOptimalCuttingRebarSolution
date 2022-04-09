using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoreLinq;
using RevitAddInOptimalCuttingRebarSolution.ViewModel;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class DataRebarInformation
    {
        #region Basic information
        //Level
        public List<Level> AllLevels { get; }
        public Level MinLevel { get; }
        public Level MaxLevel { get; }

        //Category
        public List<Category> AllCategoris { get; }

        //Family
        public List<Family> AllFamilies { get; }

        //Type
        public List<FamilySymbol> AllTypes { get; }

        //Diameter
        public List<int> AllDiameters { get; }
        public List<RebarInformation> ListRebarInfor { get; set; }
        #endregion

        #region Data Cutting Rebar

        #endregion 

        //constructor
        public DataRebarInformation(List<RebarInformation> listRebarInformation)
        {
            AllLevels = new List<Level>();
            AllCategoris = new List<Category>();
            AllDiameters = new List<int>();
            AllFamilies = new List<Family>();
            AllTypes = new List<FamilySymbol>();
            ListRebarInfor = new List<RebarInformation>();
            foreach (RebarInformation rebar in listRebarInformation)
            {
                ListRebarInfor.Add(rebar);
            }

            //set all level

            List<Level> _listlevel = new List<Level>();
            foreach (RebarInformation rif in ListRebarInfor)
            {
                _listlevel.Add(rif.ECLevel);
            }
            AllLevels = _listlevel.DistinctBy(x => x.Id).ToList();

            //set level min
            double minLevel = AllLevels.Min(x => x.Elevation);
            MinLevel = AllLevels.Where(x => x.Elevation == minLevel).ToList()[0];

            //set level max
            double maxLevel = AllLevels.Max(x => x.Elevation);
            MaxLevel = AllLevels.Where(x => x.Elevation == maxLevel).ToList()[0];

            //set all category
            List<Category> _categories = new List<Category>();
            foreach (RebarInformation rif in ListRebarInfor)
            {
                _categories.Add(rif.ECCategory);
            }
            AllCategoris = _categories.DistinctBy(x => x.Id).ToList();

            //set all family
            List<Family> _families = new List<Family>();
            foreach (RebarInformation rif in ListRebarInfor)
            {
                _families.Add(rif.ECFamily);
            }
            AllFamilies = _families.DistinctBy(x => x.Id).ToList();

            //set all type
            List<FamilySymbol> _types = new List<FamilySymbol>();
            foreach (RebarInformation rif in ListRebarInfor)
            {
                _types.Add(rif.ECFamilySymbol);
            }
            AllTypes = _types.DistinctBy(x => x.Id).ToList();

            //set all diameter
            List<int> _AllDiameters = new List<int>();
            foreach (RebarInformation rif in ListRebarInfor)
            {
                _AllDiameters.Add(rif.Diameter);
            }
            AllDiameters = _AllDiameters.Distinct().ToList();
        }
        #region Methode Find by key word
        public List<RebarInformation> FindByKeyWord(string keyword)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            result.AddRange(FindECCategoryByKeyWord(keyword));
            result.AddRange(FindECFamilyByKeyWord(keyword));
            result.AddRange(FindECLevelByKeyWord(keyword));
            result.AddRange(FindECTypeByKeyWord(keyword));
            return result.Distinct(new RebarInforComparer()).ToList();
        }
        /// <summary>
        /// Tim kiem RebarInformation theo level cua element contain
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<RebarInformation> FindECLevelByKeyWord(string keyword)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            List<string> listInfor = ListRebarInfor
                .Select(x => ConvertToUnSign(x.ECLevel.Name))
                .ToList();
            //loai khoang trang
            for (int i = 0; i < listInfor.Count; i++)
            {
                listInfor[i] = listInfor[i].Replace(" ", "");
            }
            keyword = keyword.Replace(" ", "");
            //tim kiem
            for (int i = 0; i < listInfor.Count; i++)
            {
                int index = listInfor[i].IndexOf(ConvertToUnSign(keyword));
                if (index > 0) result.Add(ListRebarInfor[i]);
            }
            return result;
        }
        /// <summary>
        /// Tim kiem RebarInformation theo Category cua element contain
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<RebarInformation> FindECCategoryByKeyWord(string keyword)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            List<string> listInfor = ListRebarInfor
                .Select(x => ConvertToUnSign(x.ECCategory.Name))
                .ToList();
            //loai khoang trang
            for (int i = 0; i < listInfor.Count; i++)
            {
                listInfor[i] = listInfor[i].Replace(" ", "");
            }
            keyword = keyword.Replace(" ", "");
            //tim kiem
            for (int i = 0; i < listInfor.Count; i++)
            {
                int index = listInfor[i].IndexOf(ConvertToUnSign(keyword));
                if (index > 0) result.Add(ListRebarInfor[i]);
            }
            return result;
        }
        /// <summary>
        /// Tim kiem RebarInformation theo Family cua element contain
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<RebarInformation> FindECFamilyByKeyWord(string keyword)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            List<string> listInfor = ListRebarInfor
                .Select(x => ConvertToUnSign(x.ECFamily.Name))
                .ToList();
            //loai khoang trang
            for (int i = 0; i < listInfor.Count; i++)
            {
                listInfor[i] = listInfor[i].Replace(" ", "");
            }
            keyword = keyword.Replace(" ", "");
            //tim kiem
            for (int i = 0; i < listInfor.Count; i++)
            {
                int index = listInfor[i].IndexOf(ConvertToUnSign(keyword));
                if (index > 0) result.Add(ListRebarInfor[i]);
            }
            return result;
        }
        /// <summary>
        /// Tim kiem RebarInformation theo type - familysymbol cua element contain
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<RebarInformation> FindECTypeByKeyWord(string keyword)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            List<string> listInfor = ListRebarInfor
                .Select(x => ConvertToUnSign(x.ECFamilySymbol.Name))
                .ToList();
            //loai khoang trang
            for (int i = 0; i < listInfor.Count; i++)
            {
                listInfor[i] = listInfor[i].Replace(" ", "");
            }
            keyword = keyword.Replace(" ", "");
            //tim kiem
            for (int i = 0; i < listInfor.Count; i++)
            {
                int index = listInfor[i].IndexOf(ConvertToUnSign(keyword));
                if (index >= 0) result.Add(ListRebarInfor[i]);
            }
            return result;
        }
        private string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }
        #endregion
        #region Filter by list id
        /// <summary>
        /// Filter category, family, type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<RebarInformation> FilterByIDElement(string id)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            foreach (RebarInformation rebar in ListRebarInfor)
            {
                //filter category
                if (rebar.ECCategory.Id.ToString() == id)
                    result.Add(rebar);

                //filter family
                if (rebar.ECFamily.Id.ToString() == id)
                    result.Add(rebar);

                //filter type
                if (rebar.ECFamilySymbol.Id.ToString() == id)
                    result.Add(rebar);
            }
            return result;
        }

        #endregion
    }

}
