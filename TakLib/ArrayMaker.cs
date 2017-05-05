using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class CarryStackArrayMaker //: IEnumerable<List<int>>
    {
        private int _maxLength, _maxValue;
        private List<int> _lastList;
        private int _currentLength, _currentValue;
        private bool _done = false;

        public CarryStackArrayMaker(int maxLength, int maxValue)
        {
            _maxLength = maxLength;
            _maxValue = maxValue;
            _currentLength = maxLength;
            _currentValue = maxValue;
        }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //public IEnumerator<List<int>> GetEnumerator()
        //{
        //    if(_maxLength <= 0) throw new Exception("maxLength must be greater than zero");
        //    if(_maxValue <= 0) throw new Exception("maxValue must be greater than zero");

        //    for (int i = 1; i < _maxValue; i++)
        //    {

        //    }

        //}

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

        /*
        public static List<string> GetAllDropLists(int maxPicked, int maxDistance)
        {
            List<string> returnList = new List<string>();
            for (int i = 1; i <= maxPicked; i++)
                returnList.AddRange(GetAllDropListsRecursive(i, maxDistance, ""));
            return returnList;
        }

        public static List<string> GetAllDropListsRecursive(int maxPicked, int maxDistance, string baseList)
        {

            List<string> returnList = new List<string>();
            if (baseList.Length > maxDistance) return returnList;

            int sum = baseList.ToCharArray().Aggregate<char, int, int>(0, (i, c) => i + int.Parse(c.ToString()), i => i);
            if (sum == maxPicked)
            {
                returnList.Add(baseList);
                return returnList;
            }

            int picked = 1;
            while (picked + sum <= maxPicked)
            {
                string newBase = $"{baseList}{picked}";

                returnList.AddRange(GetAllDropListsRecursive(maxPicked, maxDistance, newBase));
                picked++;
            }

            return returnList;
        }
        */
    }
}
