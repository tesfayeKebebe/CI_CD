using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Web.API.Controllers;

[Route("api/PaymentProvider")]
[ApiController]
public class PaymentProviderController : ApiControllerBase
{
    [HttpPost]
    public async  Task<CreatePayInRequest> PostTelebirr([FromBody] PaymentRequest request)
    {
        try
        {
            const string publicKey = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgkgZgjyUqR0W3aNHuPsmZGjtM3d9VYyD0fQTJ4A4rCsXynptn4Ml3z8jFNZk24cGz8o06YzbSEwsaE7nEwNRm3rLV6Vyd+zvTETOjGWw/mYS0AXzDY+Ok1XA5q24fgU93ah3h4lDr/q/EBXpuTlCrJuTGGHbQdOXLGspjeNObywCK4ZxxRlOcDPUtN99RkbN0350gxSFm15g0g22yo6AdQal8onrGzg7vJLaF+dskty08m+DCby56mVgVgybpsfouel2ZEQJzTQiBbxEt18kTplvax1KewhWCwGS4qKeYuj3fVYSPFjPqwDPQTjD2lcdeQ1cRkP1A8zgWSE2I1+jWwIDAQAB";
            const string appId = "946ad8b43018449188eb3bd844030811";
            IDictionary<string, string> paymentProviderSettings = new Dictionary<string, string>
            {
                {SettingKeys.AppId, appId},
                {SettingKeys.AppKey, "de04edf91bf74fedbfd705890b085ad0"},
                {SettingKeys.ShortCode, "220124"},
                {SettingKeys.NotifyUrl, "https://webhooks.activbet.com/telebirr/payin"},
                {SettingKeys.MerchantName, "ACTIVE BET SPORT PLC"},
                {SettingKeys.ReturnUrl, "https://www.activbet.com"},
                {SettingKeys.TimeoutExpress, "30"}
            };
            var nonce = DateTime.UtcNow.Ticks.ToString();
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var outTradeNo = DateTime.UtcNow.Ticks.ToString();
            var jsonString = paymentProviderSettings.GenerateJsonString(request.UserName, timestamp, nonce, outTradeNo, request.Amount);
            var jsonStringForUssd = paymentProviderSettings.GenerateJsonStringUssd(request.UserName, timestamp, nonce, outTradeNo, request.Amount);
            var stringA = jsonString.GenerateStringA();
            var rsaParameter = publicKey.ConvertPublicKeyBase64StringToRsaParameter();
            var createPayIn = new CreatePayInRequest
            {
                AppId = appId,
                Sign = stringA.GetSign(),
                Ussd = jsonStringForUssd.Encrypt(rsaParameter)
            };
            return createPayIn;

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }
}

public class PaymentRequest
{
    public required string UserName { get; set; }
    public decimal Amount { get; set; }
}
public static class DictionaryExtensions
{
    public static string GenerateJsonString(this IDictionary<string, string> paymentProviderSettings, string? subject, long timestamp, 
        string nonce, string outTradeNo, decimal amount)
    {
        var appId = paymentProviderSettings.GetSettingValue(SettingKeys.AppId);
        var appKey = paymentProviderSettings.GetSettingValue(SettingKeys.AppKey);
        var shortCode = paymentProviderSettings.GetSettingValue(SettingKeys.ShortCode);
        var notifyUrl = paymentProviderSettings.GetSettingValue(SettingKeys.NotifyUrl);
        var merchantName = paymentProviderSettings.GetSettingValue(SettingKeys.MerchantName);
        var returnUrl = paymentProviderSettings.GetSettingValue(SettingKeys.ReturnUrl);
        var timeoutExpress = paymentProviderSettings.GetSettingValue(SettingKeys.TimeoutExpress);
            
        var builder = new StringBuilder();
        builder.Append('{');
        builder.Append($"\"appId\":\"{appId}\",");
        builder.Append($"\"appKey\":\"{appKey}\",");
        builder.Append($"\"nonce\":\"{nonce}\",");
        builder.Append($"\"notifyUrl\":\"{notifyUrl}\",");
        builder.Append($"\"outTradeNo\":\"{outTradeNo}\",");
        builder.Append($"\"receiveName\":\"{merchantName}\",");
        builder.Append($"\"returnUrl\":\"{returnUrl}\",");
        builder.Append($"\"shortCode\":\"{shortCode}\",");
        builder.Append($"\"subject\":\"{subject}\",");
        builder.Append($"\"timeoutExpress\":\"{timeoutExpress}\",");
        builder.Append($"\"timestamp\":\"{timestamp}\",");
        builder.Append($"\"totalAmount\":\"{amount.ToString(CultureInfo.InvariantCulture)}\"");
        builder.Append('}');

        return builder.ToString();
    }
    public static string GenerateJsonStringUssd(this IDictionary<string, string> paymentProviderSettings, string? subject, long timestamp, 
        string nonce, string outTradeNo, decimal amount)
    {
        var appId = paymentProviderSettings.GetSettingValue(SettingKeys.AppId);
        var shortCode = paymentProviderSettings.GetSettingValue(SettingKeys.ShortCode);
        var notifyUrl = paymentProviderSettings.GetSettingValue(SettingKeys.NotifyUrl);
        var merchantName = paymentProviderSettings.GetSettingValue(SettingKeys.MerchantName);
        var returnUrl = paymentProviderSettings.GetSettingValue(SettingKeys.ReturnUrl);
        var timeoutExpress = paymentProviderSettings.GetSettingValue(SettingKeys.TimeoutExpress);
            
        var builder = new StringBuilder();
        builder.Append('{');
        builder.Append($"\"appId\":\"{appId}\",");
        builder.Append($"\"nonce\":\"{nonce}\",");
        builder.Append($"\"notifyUrl\":\"{notifyUrl}\",");
        builder.Append($"\"outTradeNo\":\"{outTradeNo}\",");
        builder.Append($"\"receiveName\":\"{merchantName}\",");
        builder.Append($"\"returnUrl\":\"{returnUrl}\",");
        builder.Append($"\"shortCode\":\"{shortCode}\",");
        builder.Append($"\"subject\":\"{subject}\",");
        builder.Append($"\"timeoutExpress\":\"{timeoutExpress}\",");
        builder.Append($"\"timestamp\":\"{timestamp}\",");
        builder.Append($"\"totalAmount\":\"{amount.ToString(CultureInfo.InvariantCulture)}\"");
        builder.Append('}');

        return builder.ToString();
    }
    
    public static string GetSettingValue(this IDictionary<string, string> settings, string key)
    {
        if (settings == null)
            throw new ArgumentNullException(nameof(settings));

        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        if (!settings.TryGetValue(key, out var retVal))
            throw new Exception($"Missing payment provider settings with name {key}.");

        return retVal;
    }
}
public static class SettingKeys
{
    public const string MerchantName = "merchant_name";
    public const string ShortCode = "short_code";
    public const string AppId = "app_id";
    public const string AppKey = "app_key";
    public const string ProviderPublicKey = "provider_public_key";
    public const string PublicKey = "public_key";
    public const string PrivateKey = "private_key";
    public const string Url = "url";
    public const string NotifyUrl = "notify_url";
    public const string ReturnUrl = "return_url";
    public const string TimeoutExpress = "timeout_express";
    public const string TransactionTimeoutHours = "transaction_timeout_hours";
    public const string HttpTimeoutMilliSeconds = "http_timeout_milliseconds";
}
public class CreatePayInRequest
{
    [DataMember(Name = "appid")]
    public string AppId { get; set; }
    
    [DataMember(Name = "sign")]
    public string Sign { get; set; }

    [DataMember(Name = "ussd")]
    public string Ussd { get; set; }
}
public static class StringExtensions
{
    public static string GenerateStringA(this string jsonString)
    {
        var dic = new Dictionary<string, string>();
        
        foreach (var item in jsonString.Trim().Split(','))
        {
            var key = item.Split(':')[0].Replace("{", "").Replace("\"", "");
            var value = item.Substring(item.IndexOf(':') + 1, item.Length - item.IndexOf(':') - 1).Replace("}", "").Replace("\"", "");
            dic.Add(key, value);
        }
            
        var vDic = from objDic in dic orderby objDic.Key ascending select objDic;
        var str = new StringBuilder();
            
        foreach (var (pKey, pValue) in vDic)
        {
            str.Append(pKey + "=" + pValue + "&");
        }

        return str.ToString()[..(str.ToString().Length - 1)];
    }

    public static string GenerateUssdString(this string jsonString)
    {
        var dic = new Dictionary<string, string>();
        
        foreach (var item in jsonString.Trim().Split(','))
        {
            var key = item.Split(':')[0].Replace("{", "").Replace("\"", "");

            if (key.Equals("appKey"))
            {
                continue;
            }
                
            var value = item.Substring(item.IndexOf(':') + 1, item.Length - item.IndexOf(':') - 1).Replace("}", "").Replace("\"", "");
            dic.Add(key, value);
        }
            
        var vDic = from objDic in dic orderby objDic.Key ascending select objDic;
        var str = new StringBuilder();
            
        foreach (var (pKey, pValue) in vDic)
        {
            str.Append(pKey + "=" + pValue + "&");
        }

        return str.ToString()[..(str.ToString().Length - 1)];
    }

    public static AsymmetricKeyParameter ConvertPublicKeyBase64StringToRsaParameter(this string publicKeyBase64String)
    {
        try
        {
            var publicKeyBytes = Convert.FromBase64String(publicKeyBase64String);
            var asn1Object = Asn1Object.FromByteArray(publicKeyBytes);
            var publicKeyInfo = SubjectPublicKeyInfo.GetInstance(asn1Object);
        
            // byte[] publicKeyDerRestored = Convert.FromBase64String(publicKeyBase64String);
            // RsaKeyParameters publicKeyRestored = (RsaKeyParameters)PublicKeyFactory.CreateKey(publicKeyDerRestored);
            // return publicKeyRestored;
            return (RsaKeyParameters) PublicKeyFactory.CreateKey(publicKeyInfo);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }
        
    public static string Encrypt(this string text, ICipherParameters encryptionKey)
    {
        const int MAX_ENCRYPT_BLOCK = 245;
        
        var offset = 0;
        var i = 0;
        var outBytes = new List<byte>();
        var c = CipherUtilities.GetCipher("RSA/NONE/PKCS1PADDING");
        c.Init(true, encryptionKey);
        
        var data = Encoding.UTF8.GetBytes(text);
        var inputLength = data.Length;
        
        while (inputLength - offset > 0)
        {
            outBytes.AddRange(inputLength - offset > MAX_ENCRYPT_BLOCK
                ? c.DoFinal(data, offset, MAX_ENCRYPT_BLOCK)
                : c.DoFinal(data, offset, inputLength - offset));

            i++;
            offset = i * MAX_ENCRYPT_BLOCK;
        }
        
        return Convert.ToBase64String(outBytes.ToArray());
    }
    
    public static string GetSign(this string strData)
    {
        var bytValue = Encoding.UTF8.GetBytes(strData);
        var sha256 = new SHA256CryptoServiceProvider();
        
        try
        {
            var retVal = sha256.ComputeHash(bytValue);
            var sb = new StringBuilder();

            foreach (var t in retVal)
            {
                sb.Append(t.ToString("x2"));
            }

            return sb.ToString().ToUpper();
        }
        catch (Exception ex)
        {
            throw new Exception("GetSHA256HashFromString() fail,error:" + ex.Message);
        }
    }
    
    public static string Decrypt(this string text, ICipherParameters decryptKey)
    {
        const int MAX_DECRYPT_BLOCK = 256;
            
        var offset = 0;
        var i = 0;
        var aMS = new MemoryStream();
        var encryptionBytes = Convert.FromBase64String(text);
        var inputLength = encryptionBytes.Length;
        var c = CipherUtilities.GetCipher("RSA/NONE/PKCS1PADDING");
        c.Init(false, decryptKey);

        while (inputLength - offset > 0)
        {
            var cache = inputLength - offset > MAX_DECRYPT_BLOCK 
                ? c.DoFinal(encryptionBytes, offset, MAX_DECRYPT_BLOCK) 
                : c.DoFinal(encryptionBytes, offset, inputLength - offset);
                
            aMS.Write(cache, 0, cache.Length);
            i++;
            offset = i * MAX_DECRYPT_BLOCK;
        }
            
        var cipherBytes = aMS.ToArray();
        return Encoding.UTF8.GetString(cipherBytes);
    }
}