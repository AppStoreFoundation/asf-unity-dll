//created by Lukmon Agboola(Codeberg)
//Modifief by Aptoide

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aptoide.AppcoinsUnity{

    /// <summary>
    /// Class that instatiated an AppCoins SKU.
    /// <list type="bullet">
    /// <item>
    /// <term>AppcoinsSkU</term>
    /// <description>AppcoinsSKU constructor.</description>
    /// </item>
    /// <item>
    /// <term>GetName</term>
    /// <description>Get the name of Appcoins SKU object.</description>
    /// </item>
    /// <item>
    /// <term>GetSKUId</term>
    /// <description>Get the ID of Appcoins SKU object.</description>
    /// </item>
    /// <item>
    /// <term>GetPrice</term>
    /// <description>Get the price of Appcoins SKU object.</description>
    /// </item>
    /// <item>
    /// <term>CheckSKU</term>
    /// <description>
    /// Check if Appcoins SKU object is well instatiated.
    /// </description>
    /// </item>
    /// <item>
    /// <term>CheckSKUId</term>
    /// <description>
    /// Check if the ID of Appcoins SKU object is valid.
    /// </description>
    /// </item>
    /// <item>
    /// <term>CheckPrice</term>
    /// <description>
    /// Check if the price of Appcoins SKU object is valid.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Equals</term>
    /// <description>
    /// Check if two Appcoins SKU objects are the same.
    /// </description>
    /// </item>
    /// <item>
    /// <term>GetHashCode</term>
    /// <description>Get the hash code of Appcoins SKU object.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>
    /// AppcoinsSKU can be instatiated without a name.
    /// </para>
    /// </remarks>
    public class AppcoinsSKU
    {
        // SKU name
        public string name;

        // SKU ID
        public string skuID;

        // SKU Price
        public double price;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Aptoide.AppcoinsUnity.AppcoinsSKU"/> class.
        /// </summary>
        /// <param name="skuid">SKU ID.</param>
        /// <param name="p">SKU price.</param>
        public AppcoinsSKU(string skuid, double p)
        {
            CheckSKUId(skuid);
            CheckPrice(p);

            name = "";
            skuID = skuid;
            price = p;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Aptoide.AppcoinsUnity.AppcoinsSKU"/> class.
        /// </summary>
        /// <param name="n">SKU name.</param>
        /// <param name="skuid">SKU ID.</param>
        /// <param name="p">SKU price.</param>
        public AppcoinsSKU(string n, string skuid, double p) : 
        this(skuid, p)
        {
            name = n;
        }

        /// <summary>
        /// Get the SKU name.
        /// </summary>
        /// <returns>The SKU name.</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Get the SKU ID.
        /// </summary>
        /// <returns>The SKU ID.</returns>
        public string GetSKUId()
        {
            return skuID;
        }

        /// <summary>
        /// Get the SKU price.
        /// </summary>
        /// <returns>The SKU price.</returns>
        public double GetPrice()
        {
            return price;
        }

        /// <summary>
        /// Check if the Appcoins SKU is well instatiated (if has a valid ID and
        /// a valid price).
        /// </summary>
        public void CheckSKU()
        {
            CheckSKUId(skuID);
            CheckPrice(price);
        }

        /// <summary>
        /// Check if SKU ID is valid (If is not null or an empty string).
        /// </summary>
        /// <exception cref="Aptoide.AppcoinsUnity.InvalidSKUIdException">
        /// Thrown when SKU ID is null or an empty string.
        /// </exception>
        /// <param name="id">SKU ID.</param>
        private void CheckSKUId(string id)
        {
            if (id == null || id.Equals(""))
            {
                throw new InvalidSKUIdException();
            }
        }

        /// <summary>
        /// Check if SKU price is valid (Greater or equal than 0). 
        /// </summary>
        /// <exception cref="Aptoide.AppcoinsUnity.InvalidSKUPriceException">
        /// Thrown when SKU price is less than 0.
        /// </exception>
        /// <param name="p">SKU price.</param>
        private void CheckPrice(double p)
        {
            if (p < 0)
            {
                throw new InvalidSKUPriceException();
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to 
        /// the current <see cref="T:Aptoide.AppcoinsUnity.AppcoinsSKU"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the 
        /// current <see cref="T:Aptoide.AppcoinsUnity.AppcoinsSKU"/>.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to the 
        /// current <see cref="T:Aptoide.AppcoinsUnity.AppcoinsSKU"/> 
        /// (if both have the same SKU ID); 
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="Aptoide.AppcoinsUnity.NullSKUProductException">
        /// Thrown when <see cref="object"/> is not an Object of
        /// <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/>.
        /// </exception>
        public override bool Equals(System.Object obj)
        {
            if (!(obj is AppcoinsSKU otherSKU))
            {
                throw new NullSKUProductException();
            }

            return skuID.Equals(otherSKU.GetSKUId()) ? true : false;
        }

        /// <summary>
        /// Serves as a hash function for a 
        /// <see cref="T:Aptoide.AppcoinsUnity.AppcoinsSKU"/> object.
        /// </summary>
        /// <returns>
        /// A hash code for this instance that is suitable for use in hashing 
        /// algorithms and data structures such as a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return 714161071 + 
                EqualityComparer<string>.Default.GetHashCode(skuID);
        }
    }
} //namespace Aptoide.AppcoinsUnity
