using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace HSB.Core.Encryption;

/// <summary>
/// IHashPassword interface, provides a way to hash passwords with a salt.
/// </summary>
public interface IHashPassword
{
    /// <summary>
    /// get - Specifies the PRF which should be used for the key derivation algorithm.
    /// </summary>
    KeyDerivationPrf Derivation { get; }

    /// <summary>
    /// get - The number of iterations of the pseudo-random function to apply during the key derivation process.
    /// </summary>
    int IterationCount { get; }

    /// <summary>
    /// get - The desired length (in bytes) of the derived key.
    /// </summary>
    int NumBytesRequested { get; }

    /// <summary>
    /// Generate a hash for the specified password and salt.
    /// The salt is prepended to the hash, which means you must know how many bytes each salt is to compare.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    string Hash(string password, byte[] salt);

    /// <summary>
    /// Generate a hash for the specified password and salt.
    /// The salt is prepended to the hash, which means you must keep a copy of the salt elsewhere to compare.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    string Hash(string password, string salt);

    /// <summary>
    /// Generate a hash for the specified password and salt.
    /// The salt is randomly generated based on the specified size.
    /// The salt is prepended to the hash, which means you must keep a copy of the length elsewhere to compare.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="saltLength"></param>
    /// <returns></returns>
    string Hash(string password, int saltLength = 50);
}
