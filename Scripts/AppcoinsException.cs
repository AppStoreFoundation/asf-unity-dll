using System;

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

public class NoProductsException : AppcoinsException
{
    const string _message = "Appcoins Unity prefab has no products available.";

    public NoProductsException() : base(_message)
    {
    }

    public NoProductsException(string new_message)
        : base(new_message)
    {
    }

    public NoProductsException(Exception inner)
        : base(_message, inner)
    {
    }

    public NoProductsException(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}

public class NullProductException : AppcoinsException
{
    private const string _message = "AppcoinsUnity prefab has one or more " +
        "null products.";

    public NullProductException() : base(_message)
    {
    }

    public NullProductException(string new_message)
        : base(new_message)
    {
    }

    public NullProductException(Exception inner)
        : base(_message, inner)
    {
    }

    public NullProductException(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}

public class RepeatedProductException : AppcoinsException
{
    const string _message = "AppcoinsUnity prefab has two or more products " +
        "with the same SKU id";

    public RepeatedProductException() : base(_message)
    {
    }

    public RepeatedProductException(string new_message)
        : base(new_message)
    {
    }

    public RepeatedProductException(Exception inner) 
        : base(_message, inner)
    {
    }

    public RepeatedProductException(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}
