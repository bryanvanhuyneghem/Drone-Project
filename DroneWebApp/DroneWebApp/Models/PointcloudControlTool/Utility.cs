using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class Utility
    {
        public static bool ContainsList(List<List<int>> list, List<int> item)
        {
            item.Sort();

            for (int i=0; i<list.Count; i++)
            {
                List<int> temp = list[i];
                if (temp.Count == item.Count)
                {
                    temp.Sort();
                    if (Equals(temp, item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool Equals(List<int> list1, List<int> list2)
        {
            bool equals = true;

            for (int i=0; i<list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    equals = false;
                }
            }
            return equals;
        }
    }
}