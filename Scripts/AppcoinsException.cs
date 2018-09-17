using System;

namespace Aptoide.AppcoinsUnity
{
    public class AppcoinsException : Exception
    {
        public readonly string message;
        public AppcoinsException()
        {
        }
        
        public AppcoinsException(string mes) : base(mes)
        {
            message = mes;
        }
        
        public AppcoinsException(string mes, Exception inner)
            : base(mes, inner)
        {
            message = mes;
        }
    }
    
    public class MoreThanOneAppcoinsPrefabException : AppcoinsException
    {
        const string _message = "It was encoutered more than one AppcoinsUnity" +
            "prefab in all open scenes. Please make sure that you only have one." +
            "(AppcoinsUnity prefab has the property: 'DontDestroyOnLoad', so you " +
            "have access to it at all scenes.";
        
        public MoreThanOneAppcoinsPrefabException() : base(_message)
        {
        }
        
        public MoreThanOneAppcoinsPrefabException(string new_message)
            : base(new_message)
        {
        }
        
        public MoreThanOneAppcoinsPrefabException(Exception inner)
            : base(_message, inner)
        {
        }
        
        public MoreThanOneAppcoinsPrefabException(string new_message,
                                                  Exception inner) : base(new_message, inner)
        {
        }
    }
    
    public class NoSKUProductsException : AppcoinsException
    {
        const string _message = "Appcoins Unity prefab has no products available.";
        
        public NoSKUProductsException() : base(_message)
        {
        }
        
        public NoSKUProductsException(string new_message)
            : base(new_message)
        {
        }
        
        public NoSKUProductsException(Exception inner)
            : base(_message, inner)
        {
        }
        
        public NoSKUProductsException(string new_message,
                                      Exception inner) : base(new_message, inner)
        {
        }
    }
    
    public class NullSKUProductException : AppcoinsException
    {
        private const string _message = "AppcoinsUnity prefab has one or more " +
            "null products.";
        
        public NullSKUProductException() : base(_message)
        {
        }
        
        public NullSKUProductException(string new_message)
            : base(new_message)
        {
        }
        
        public NullSKUProductException(Exception inner)
            : base(_message, inner)
        {
        }
        
        public NullSKUProductException(string new_message,
                                       Exception inner) : base(new_message, inner)
        {
        }
    }
    
    public class RepeatedSKUProductException : AppcoinsException
    {
        const string _message = "AppcoinsUnity prefab has two or more products " +
            "with the same SKU Id";
        
        public RepeatedSKUProductException() : base(_message)
        {
        }
        
        public RepeatedSKUProductException(string new_message)
            : base(new_message)
        {
        }
        
        public RepeatedSKUProductException(Exception inner) 
            : base(_message, inner)
        {
        }
        
        public RepeatedSKUProductException(string new_message,
                                           Exception inner) : base(new_message, inner)
        {
        }
    }
    
    public class InvalidSKUIdException : AppcoinsException
    {
        const string _message = "AppcoinsUnity prefab has one or more products " +
            "with an invalid SKU Id";
        
        public InvalidSKUIdException() : base(_message)
        {
        }
        
        public InvalidSKUIdException(string new_message)
            : base(new_message)
        {
        }
        
        public InvalidSKUIdException(Exception inner)
            : base(_message, inner)
        {
        }
        
        public InvalidSKUIdException(string new_message,
                                     Exception inner) : base(new_message, inner)
        {
        }
    }
    
    public class InvalidSKUPriceException : AppcoinsException
    {
        const string _message = "SKU price is negative";
        
        public InvalidSKUPriceException() : base(_message)
        {
        }
        
        public InvalidSKUPriceException(string new_message)
            : base(new_message)
        {
        }
        
        public InvalidSKUPriceException(Exception inner)
            : base(_message, inner)
        {
        }
        
        public InvalidSKUPriceException(string new_message,
                                        Exception inner) : base(new_message, inner)
        {
        }
    }
    
    public class PurchaserObjectIsNullException : AppcoinsException
    {
        const string _message = "ASFAppcoinsUnity purchaserObject is not defined";
        
        public PurchaserObjectIsNullException() : base(_message)
        {
        }
        
        public PurchaserObjectIsNullException(string new_message)
            : base(new_message)
        {
        }
        
        public PurchaserObjectIsNullException(Exception inner)
            : base(_message, inner)
        {
        }
        
        public PurchaserObjectIsNullException(string new_message,
                                              Exception inner) : base(new_message, inner)
        {
        }
    }
    
    public class IABIsTurnedOffException : AppcoinsException
    {
        const string _message = "Trying to make a purchase when is IAB turned off";
        
        public IABIsTurnedOffException() : base(_message)
        {
        }
        
        public IABIsTurnedOffException(string new_message)
            : base(new_message)
        {
        }
        
        public IABIsTurnedOffException(Exception inner)
            : base(_message, inner)
        {
        }
        
        public IABIsTurnedOffException(string new_message,
                                       Exception inner) : base(new_message, inner)
        {
        }
    }
    
    public class SkuIsNotRegistedException : AppcoinsException
    {
        const string _message = "Trying to purchase an unregistered SKU";
        
        public SkuIsNotRegistedException() : base(_message)
        {
        }
        
        public SkuIsNotRegistedException(string new_message)
            : base(new_message)
        {
        }
        
        public SkuIsNotRegistedException(Exception inner)
            : base(_message, inner)
        {
        }
        
        public SkuIsNotRegistedException(string new_message,
                                         Exception inner) : base(new_message, inner)
        {
        }
    }

    public class CannotRegisterSKUException : AppcoinsException
    {
        const string _message = "SKU register time experied (perhaps 'SetupIAB'" +
            " method has already been called";

        public CannotRegisterSKUException() : base(_message)
        {
        }

        public CannotRegisterSKUException(string new_message)
            : base(new_message)
        {
        }

        public CannotRegisterSKUException(Exception inner)
            : base(_message, inner)
        {
        }

        public CannotRegisterSKUException(string new_message,
                                         Exception inner) : base(new_message, inner)
        {
        }
    }
}