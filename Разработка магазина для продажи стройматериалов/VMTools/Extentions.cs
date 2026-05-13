using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.VMTools
{
    public static class Extentions
    {
        public static T ChangeAllProperties<T>(this T origObj, T newObj)
        {
            var props = typeof(T).GetProperties();


            foreach (var prop in props)
            {
                prop.SetValue(origObj, prop.GetValue(newObj));
            }

            return origObj;
        }

    }
}
