//created by Lukmon Agboola(Codeberg)
//Modifief by Aptoide

using System.Collections;
using UnityEngine;

namespace Aptoide.AppcoinsUnity{

    public class AppcoinsSKU
    {
        public string Name;
        public string SKUID;
        public double Price;

        public AppcoinsSKU(string skuid, double price)
        {
            //CheckSKUID(skuid);
            //CheckPrice(price);

            SKUID = skuid;
            Price = price;
        }

        public AppcoinsSKU(string name, string skuid, double price) : 
        this(skuid, price)
        {
            Name = name;
        }

        //private void CheckSKUID(string skuid)
        //{
        //    if (skuid == null || skuid.Equals(""))
        //    {
        //        throw new InvalidSKUIdException();
        //    }
        //}

        //private void CheckPrice(double price)
        //{
        //}
    }
} //namespace Aptoide.AppcoinsUnity
