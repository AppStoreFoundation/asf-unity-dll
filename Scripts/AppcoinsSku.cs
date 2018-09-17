//created by Lukmon Agboola(Codeberg)
//Modifief by Aptoide

using System;
using System.Collections;
using UnityEngine;

namespace Aptoide.AppcoinsUnity{

    public class AppcoinsSKU
    {
        public string name;
        public string skuID;
        public double price;

        public AppcoinsSKU(string skuid, double p)
        {
            CheckSKUid(skuid);
            CheckPrice(p);

            name = "";
            skuID = skuid;
            price = p;
        }

        public AppcoinsSKU(string n, string skuid, double p) : 
        this(skuid, p)
        {
            name = n;
        }

        public string GetName()
        {
            return name;
        }

        public string GetSKUId()
        {
            return skuID;
        }

        public double GetPrice()
        {
            return price;
        }

        public void CheckSKU()
        {
            CheckSKUid(skuID);
            CheckPrice(price);
        }

        private void CheckSKUid(string id)
        {
            if (id == null || id.Equals(""))
            {
                throw new InvalidSKUIdException();
            }
        }

        private void CheckPrice(double p)
        {
            if (p < 0)
            {
                throw new InvalidSKUPriceException();
            }
        }

        public override bool Equals(System.Object obj)
        {
            if (!(obj is AppcoinsSKU otherSKU))
            {
                throw new NullSKUProductException();
            }

            return skuID.Equals(otherSKU.GetSKUId()) ? true : false;
        }
    }
} //namespace Aptoide.AppcoinsUnity
