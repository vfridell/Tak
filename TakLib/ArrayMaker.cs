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

        public static List<string> Split(char numberChar)
        {
            List<string> returnList = new List<string>();
            int num = int.Parse(numberChar.ToString());
            int currentNum = num;
            returnList.Add($"{currentNum}");
            while(currentNum > 1)
            {
                currentNum--;
                returnList.Add($"{currentNum}{num - currentNum}");
            }
            return returnList;
        }


        public static List<string> SplitRecursive(int num, string baseString)
        {
            List<string> returnList = new List<string>();

            int currentNum = num;
            returnList.Add($"{currentNum}");
            while (currentNum > 1)
            {
                currentNum--;
                string newBase = $"{baseString}{currentNum}{num - currentNum}";
                returnList.Add(newBase);
                for(int i = currentNum-1; i > 1; i--)
                    if(currentNum-2 > 0) returnList.AddRange(SplitRecursive(currentNum, newBase));
            }
            return returnList;
        }
    }
}
