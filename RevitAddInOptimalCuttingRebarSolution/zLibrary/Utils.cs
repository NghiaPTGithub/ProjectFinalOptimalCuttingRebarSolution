using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public static class Utils
    {
        /// <summary>
        /// Copies the contents of an IEnumerable list to an ObservableCollection
        /// </summary>
        /// <typeparam name="T">The type of objects in the source list</typeparam>
        /// <param name="enumerableList">The source list to be converted</param>
        /// <returns>An ObservableCollection containing the objects from the source list</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableList)
        {
            if (enumerableList != null)
            {
                // Create an emtpy observable collection object
                var observableCollection = new ObservableCollection<T>();

                // Loop through all the records and add to observable collection object
                foreach (var item in enumerableList)
                {
                    observableCollection.Add(item);
                }

                // Return the populated observable collection
                return observableCollection;
            }
            return null;
        }

        #region Find by key word
        public static List<RebarInformation> FindRebarInforByKeyword(List<RebarInformation> ListRebarInfor, string keyword)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            if (keyword.Replace(" ", "") == "")
            {
                result = ListRebarInfor;
            }
            else
            {
                result.AddRange(FindECCategoryByKeyWord(ListRebarInfor, keyword));
                result.AddRange(FindECFamilyByKeyWord(ListRebarInfor, keyword));
                result.AddRange(FindECLevelByKeyWord(ListRebarInfor, keyword));
                result.AddRange(FindECTypeByKeyWord(ListRebarInfor, keyword));
                result.AddRange(FindIDRebarByKeyWord(ListRebarInfor, keyword));
                result.AddRange(FindRebarLenghtByKeyWord(ListRebarInfor, keyword));
                result.AddRange(FindRebarDiameterByKeyWord(ListRebarInfor, keyword));
                result = result.Distinct(new RebarInforComparer()).ToList();
            }
            return result;
        }

        /// <summary>
        /// Tim kiem RebarInformation theo diameter cua thanh thep
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<RebarInformation> FindRebarDiameterByKeyWord(List<RebarInformation> ListRebarInfor, string keyword)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            List<string> listInfor = ListRebarInfor
                .Select(x => ConvertToUnSign(x.Diameter.ToString()))
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
        /// Tim kiem RebarInformation theo lenght cua thanh thep
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<RebarInformation> FindRebarLenghtByKeyWord(List<RebarInformation> ListRebarInfor, string keyword)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            List<string> listInfor = ListRebarInfor
                .Select(x => ConvertToUnSign(x.Lenght.ToString()))
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
        /// Tim kiem RebarInformation theo id cua thanh thep
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<RebarInformation> FindIDRebarByKeyWord(List<RebarInformation> ListRebarInfor, string keyword)
        {
            List<RebarInformation> result = new List<RebarInformation>();
            List<string> listInfor = ListRebarInfor
                .Select(x => ConvertToUnSign(x.IDRebar))
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
        /// Tim kiem RebarInformation theo level cua element contain
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<RebarInformation> FindECLevelByKeyWord(List<RebarInformation> ListRebarInfor, string keyword)
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
        public static List<RebarInformation> FindECCategoryByKeyWord(List<RebarInformation> ListRebarInfor, string keyword)
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
        public static List<RebarInformation> FindECFamilyByKeyWord(List<RebarInformation> ListRebarInfor, string keyword)
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
        public static List<RebarInformation> FindECTypeByKeyWord(List<RebarInformation> ListRebarInfor, string keyword)
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
        private static string ConvertToUnSign(string input)
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
        public static List<RebarInformation> FilterByIDElement(List<RebarInformation> ListRebarInfor, string id)
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

        public static bool IsInteger(string pText, bool isPositive)
        {
            Regex regex;
            regex = isPositive ? new Regex(@"^[+]?[0-9]+$") : new Regex(@"^[-]?[0-9]+$");
            return regex.IsMatch(pText);
        }
        public static bool IsInteger(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]+$");
            return regex.IsMatch(pText);
        }
        public static bool IsDouble(string pText, bool isPositive)
        {
            Regex regex;
            regex = isPositive ? new Regex(@"^[+]?[0-9]*.?[0-9]+$") : new Regex(@"^[-]?[0-9]*.?[0-9]+$");
            return regex.IsMatch(pText);
        }
        public static bool IsDouble(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*.?[0-9]+$");
            return regex.IsMatch(pText);
        }

        public static IEnumerable<T> GetRandomSample<T>(this IList<T> list, int sampleSize)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (sampleSize > list.Count) throw new ArgumentException("sampleSize may not be greater than list count", "sampleSize");
            var indices = new Dictionary<int, int>(); int index;
            var rnd = new Random();

            for (int i = 0; i < sampleSize; i++)
            {
                int j = rnd.Next(i, list.Count);
                if (!indices.TryGetValue(j, out index)) index = j;

                yield return list[index];

                if (!indices.TryGetValue(i, out index)) index = i;
                indices[j] = index;
            }
        }

        public static List<T> GetRandomList<T>(this List<T> list, int sizeGet)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (sizeGet > list.Count) throw new ArgumentException("SizeGet may not be greater than list count", "SizeGet");

            List<T> result = new List<T>();
            List<T> copyList = new List<T>();
            foreach (T item in list)
            {
                copyList.Add(item);
            }
            //lap cho toi khi lay du size Get
            while (result.Count < sizeGet)
            {
                Random ran = new Random();
                int index = ran.Next(0, copyList.Count - 1);
                result.Add(copyList[index]);
                copyList.RemoveAt(index);
            }
            return result;

        }
    }
}
