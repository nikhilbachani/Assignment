using System.Security.Cryptography;

namespace PracticeApi.Infrastructure.Security;

/// <summary>
/// Service for encrypting and decrypting sensitive data
/// </summary>
public interface IEncryptionService
{
  /// <summary>
  /// Encrypts the given plain text
  /// </summary>
  /// <param name="plainText"></param>
  /// <returns></returns>
  string Encrypt(string plainText);

  /// <summary>
  /// Decrypts the given cipher text
  /// </summary>
  /// <param name="cipherText"></param>
  /// <returns></returns>
  string Decrypt(string cipherText);
}

/// <summary>
/// Service for encrypting and decrypting sensitive data
/// </summary>
public class EncryptionService : IEncryptionService
{
  private readonly byte[] _key;
  private readonly byte[] _iv;

  /// <summary>
  /// Initializes a new instance of the <see cref="EncryptionService"/> class
  /// </summary>
  /// <param name="key">32 character encryption key</param>
  /// <param name="iv">16 character initialization vector</param>
  /// <exception cref="ArgumentException"></exception>
  public EncryptionService(byte[] key, byte[] iv)
  {
    if (key.Length != 32 || iv.Length != 16)
    {
      throw new ArgumentException("Key must be 32 characters and IV must be 16 characters.");
    }

    _key = key;
    _iv = iv;
  }

  /// <inheritdoc cref="IEncryptionService.Encrypt"/>
  /// <exception cref="ArgumentNullException"></exception>
  public string Encrypt(string plainText)
  {
    if (string.IsNullOrEmpty(plainText))
    {
      throw new ArgumentNullException(nameof(plainText));
    }

    using var aes = Aes.Create();
    aes.Key = _key;
    aes.IV = _iv;

    using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
    using var ms = new MemoryStream();
    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
    using (var writer = new StreamWriter(cs))
    {
      writer.Write(plainText);
      writer.Flush(); // Ensure all data is flushed to the CryptoStream
      cs.FlushFinalBlock(); // Ensure the CryptoStream is finalized
    }

    return Convert.ToBase64String(ms.ToArray());
  }

  /// <inheritdoc cref="IEncryptionService.Decrypt"/>
  /// <exception cref="ArgumentNullException"></exception>
  public string Decrypt(string cipherText)
  {
    if (string.IsNullOrEmpty(cipherText))
    {
      throw new ArgumentNullException(nameof(cipherText));
    }

    using var aes = Aes.Create();
    aes.Key = _key;
    aes.IV = _iv;

    using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
    using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
    using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
    using var reader = new StreamReader(cs);

    return reader.ReadToEnd();
  }
}
