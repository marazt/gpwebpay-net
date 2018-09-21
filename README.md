# GPWebpayNet

GPWebpayNet is a helper library for communication with [GP webpay](http://www.gpwebpay.com) payment gateway and providing payments
via HTTP. 
It follows documentation that can be found [here](http://www.gpwebpay.cz/en/Download).

GP webpay does not provide any SDK to be able to integrate their services in a simple way. They provide sample 
code in **.NET3.5** (really, in 2017), **Java** and **PHP**. But only for evaluation that your process of signing payment request generates valid
digest. Thats all. Another proble is that these examples are quite outdated - today, there is .NetCore2 that
has different assemblies for working with ceritficates. 


### Version
- **Version 1.3.0** - 2018-09-21


### Getting Started/Installing
```
Install-Package GPWebpayNet.Sdk -Version 1.3.0
```

### .NET Framework Support

This SDK supports:
- Full .NET Framework 4.6.1
- .netstandart 1.6
- .netstandart 2.0


### Use Cases

You can use is together with WebAPI and SPA of with ASP MVC. It is up to you.

**The base GP webpay process (simplified) is as follows:**
1. Generate payment request 
1. Send it via GET or POST go payment gateway
1. Process GET request from GP webpay


**The process with API and SPA is as follows:**
1. SPA send order request to API
1. API generates payment request for GET (url) and sends it back to SPA
1. SPA use url and redirects to it
1. GP webpay send request to API
1. API process request and redirect to SPA with corresponding result


**The process with MVC is as follows:**
1. Send request to MVC
1. MVC generates payment request for GET/POST and redirects to GP webpay
1. GP webpay send request to MVC
1. MVC process request and redirect to SPA with corresponding result



### Project Structure


#### GPWebpayNet.Example
It contains examples how to use this SDK. For more details you can check *GPWebpayNet.SDK.Spec* too.


#### GPWebpayNet.SDK
The main project.
The main project class is *GPWebpayNet.Sdk.Services.ClientService* that provides methods needed for payment process.
Other classes are used by this class. Whole project is designed to be used with some IoC framework so classes are decoupled.


#### GPWebpayNet.Example
It contains SDK unit tests.


### Usage Samples

#### Get Redirect URL for Payment Request
```csharp
public string GetRedirectUrl()
{
    var loggerFactory = new LoggerFactory()
        .AddConsole();

    const string url = Constants.GPWebpayUrlTest;
    const string privateCertificateFile = "certs/client.pfx";
    const string privateCertificateFilePassword = "test";
    const string publicCertificateFile = "certs/server_pub.pem";
    const string publicCertificateFilePassword = null;
    var doc = new XmlDocument();
    doc.AppendChild(doc.CreateElement("Info"));

    var request = new PaymentRequest
    {
        MerchantNumber = "235235",
        OrderNumber = 2412,
        Amount = new decimal(64.6546),
        Currency = CurrencyCodeEnum.Eur,
        DepositFlag = 1,
        MerOrderNumber = "MerOrderNumber",
        Url = "https://www.example.org",
        Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
        MD = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.",
        PaymentMethod = PaymentMethodEnum.Mps,
        DisabledPaymentMethod = PaymentMethodEnum.Crd,
        PaymentMethods = new[] {PaymentMethodEnum.Mcm, PaymentMethodEnum.NotSet},
        Email = "user@example.org",
        ReferenceNumber = "77987",
        AddInfo = doc.DocumentElement,
        Lang = "CZ"
    };

    var encodingLogger = loggerFactory.CreateLogger<EncodingService>();
    var clientServiceLogger = loggerFactory.CreateLogger<ClientService>();
    var clientService = new ClientService(new EncodingService(encodingLogger),
        new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLogger);

    return clientService.GenerateGPWebPayRedirectUrl(
        url,
        request,
        privateCertificateFile,
        privateCertificateFilePassword,
        publicCertificateFile, 
        publicCertificateFilePassword);
        
    // In case of `X509Certificate2` error on IIS or Azure while calling `GenerateGPWebPayRedirectUrl` or `PostRequestAsync`
    // call such method with different *keyStorageFlags* argument:
    // return clientService.GenerateGPWebPayRedirectUrl(
    //     url,
    //     request,
    //     privateCertificateFile,
    //     privateCertificateFilePassword,
    //     publicCertificateFile, 
    //     publicCertificateFilePassword,
    //     Encoding.DefaultEncoding,
    //     X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet);

}
```


#### Process Incomming GP webpay request
```csharp
public void ProcessIncommingGPWPRequest()
{
    var loggerFactory = new LoggerFactory()
        .AddConsole();
            
    // Args from Request object
    var queryArgs = new QueryCollection(new Dictionary<string, StringValues>()
    {
        {"OPERATION", new StringValues("Operation")},
        {"ORDERNUMBER", new StringValues("12332")},
        {"PRCODE", new StringValues("0")},
        {"SRCODE", new StringValues("0")},
        {"RESULTTEXT", new StringValues("ResultText")},
        {"USERPARAM1", new StringValues("UserParam1")},
        {"DIGEST", new StringValues("Digest")},
        {"DIGEST1", new StringValues("Digest1")},
    });
            
    const string merchantNumber = "25236236";
    const string publicCertificateFile = "certs/server_pub.pem";
    const string publicCertificateFilePassword = null;
        
            
    var encodingLogger = loggerFactory.CreateLogger<EncodingService>();
    var clientServiceLogger = loggerFactory.CreateLogger<ClientService>();
    var clientService = new ClientService(new EncodingService(encodingLogger),
        new PaymentRequestTransformer(), new PaymentResponseTransformer(), clientServiceLogger);
    
    // Service will creates PaymentResponse from incomming args and validate response digest with 
    // public certificate provided by GPWP and then check if "PRCODE" and "SRCODE" values have correct or error values
    // ReSharper disable once UnusedVariable
    var paymentResponse = clientService.ProcessGPWebPayResponse(queryArgs, merchantNumber, publicCertificateFile, password);
}
```


## Contributing

Pull requests, bug reports, and feature requests are welcome.


## Authors

* **Marek Polak** - *Initial work* - [marazt](https://github.com/marazt)


## License

© 2017-2018 Marek Polak. This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.


## Acknowledgments

* Enjoy it!
* If you want, you can support this project too.


## Donate 

TODO Paypal Donate
