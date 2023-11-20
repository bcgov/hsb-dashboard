using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace HSB.Core.Encryption;

/// <summary>
/// HashPassword class, provides a way to hash passwords with a salt.
/// </summary>
public class HashPassword : IHashPassword
{
    #region Properties
    /// <summary>
    /// get - Specifies the PRF which should be used for the key derivation algorithm.
    /// </summary>
    public KeyDerivationPrf Derivation { get; }

    /// <summary>
    /// get - The number of iterations of the pseudo-random function to apply during the key derivation process.
    /// </summary>
    public int IterationCount { get; }

    /// <summary>
    /// get - The desired length (in bytes) of the derived key.
    /// </summary>
    public int NumBytesRequested { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a HashPassword object.
    /// </summary>
    public HashPassword()
    {
        this.Derivation = KeyDerivationPrf.HMACSHA1;
        this.IterationCount = 1000;
        this.NumBytesRequested = 256 / 8;
    }

    /// <summary>
    /// Creates a new instance of a HashPassword object, initializes with specified parameters.
    /// </summary>
    /// <param name="keyDerivation"></param>
    /// <param name="iterationCount"></param>
    /// <param name="numBytesRequested"></param>
    public HashPassword(KeyDerivationPrf keyDerivation, int iterationCount, int numBytesRequested)
    {
        this.Derivation = keyDerivation;
        this.IterationCount = iterationCount;
        this.NumBytesRequested = numBytesRequested;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Generate a hash for the specified password and salt.
    /// The salt is prepended to the hash, which means you must know how many bytes each salt is to compare.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public string Hash(string password, byte[] salt)
    {
        if (String.IsNullOrWhiteSpace(password)) throw new ArgumentException("Parameter cannot be null, empty, or whitespace.", nameof(password));
        if (salt == null) throw new ArgumentNullException(nameof(salt));
        if (salt.Length == 0) throw new ArgumentException("Parameter must not be zero length array.", nameof(salt));

        return Encoding.UTF8.GetString(salt) + Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: this.Derivation,
            iterationCount: this.IterationCount,
            numBytesRequested: this.NumBytesRequested
            ));
    }

    /// <summary>
    /// Generate a hash for the specified password and salt.
    /// The salt is prepended to the hash, which means you must keep a copy of the salt elsewhere to compare.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public string Hash(string password, string salt)
    {
        if (String.IsNullOrWhiteSpace(salt)) throw new ArgumentException("Parameter cannot be null, empty, or whitespace.", nameof(salt));

        return Hash(password, Encoding.UTF8.GetBytes(salt));
    }

    /// <summary>
    /// Generate a hash for the specified password and salt.
    /// The salt is randomly generated based on the specified size.
    /// The salt is prepended to the hash, which means you must keep a copy of the length elsewhere to compare.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="saltLength">The number of characters for the randomly generated salt.</param>
    /// <returns></returns>
    public string Hash(string password, int saltLength = 50)
    {
        if (saltLength <= 0) throw new ArgumentOutOfRangeException(nameof(saltLength), "Parameter must be greater than zero.");

        var rnd = new Random();
        var data = new char[saltLength];
        for (var i = 0; i < saltLength; i++)
        {
            data[i] = Convert.ToChar(rnd.Next('0', 'z'));
        }

        return Hash(password, Encoding.UTF8.GetBytes(data));
    }
    #endregion
}
