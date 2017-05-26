using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class CarryStackArrayMaker 
    {
        public static List<List<int>> GetAllDropLists(int maxPicked, int maxDistance)
        {
            List<List<int>> returnList = new List<List<int>>();
            for(int i = 1; i <= maxPicked; i++)
                returnList.AddRange(GetAllDropListsRecursive(i, maxDistance, new List<int>() ));
            return returnList;
        }


        public static List<List<int>> GetAllDropListsRecursive(int maxPicked, int maxDistance, List<int> baseList)
        {

            List<List<int>> returnList = new List<List<int>>();
            if (baseList.Count > maxDistance) return returnList;

            int sum = baseList.Sum();
            if (sum == maxPicked)
            {
                returnList.Add(baseList);
                return returnList;
            }

            int picked = 1;
            while (picked + sum <= maxPicked)
            {
                List<int> newBase = new List<int>(baseList) {picked};

                returnList.AddRange(GetAllDropListsRecursive(maxPicked, maxDistance, newBase));
                picked++;
            }

            return returnList;
        }

    }
}
