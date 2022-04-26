using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary.GA
{
    public class GA
    {
        public static List<T> Hybridize<T>(List<T> chainObjectsFather, List<T> chainObjectsMother)
        {
            if (chainObjectsFather == null || chainObjectsMother == null)
            {
                Exception e = new Exception("Chain parents are not null");
                throw e;
            }

            if (chainObjectsFather.Count != chainObjectsMother.Count)
            {
                Exception e = new Exception("Chain objects must have same size");
                throw e;
            }

            if (chainObjectsFather.Count == 0)
            {
                Exception e = new Exception("Chain objects have no thing");
                throw e;
            }

            List<T> child = new List<T>();
            int sizeChainObjects = chainObjectsFather.Count;
            int indexToCutGene = (new Random()).Next(sizeChainObjects);

            for (int i = indexToCutGene; i < sizeChainObjects; i++)
            {
                child.Add(chainObjectsFather[i]);
            }

            foreach (T motherItem in chainObjectsMother)
            {
                bool isNotInChild = true;

                foreach (T childItem in child)
                {
                    if (childItem.Equals(motherItem))
                    {
                        isNotInChild = false;
                        continue;
                    }
                }

                if (isNotInChild)
                {
                    child.Add(motherItem);
                }
            }
            return child;
        }
    }
}
