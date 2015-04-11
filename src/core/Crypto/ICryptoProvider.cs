﻿using System.Collections.Generic;
using System.IO;

namespace Sdm.Core
{
    public interface ICryptoProvider
    {
        /// <summary>
        /// Gets valid key sizes (in bits).
        /// </summary>
        IEnumerable<int> ValidKeySizes { get; }
        /// <summary>
        /// Initializes ICryptoProvider instance using given key size.
        /// </summary>
        /// <param name="keySize">Key size (in bits)</param>
        void Initialize(int keySize);
        /// <summary>
        /// Returns size of encrypted data.
        /// </summary>
        /// <param name="noncryptedSize">Size of reference (non-crypted) data.</param>
        int ComputeEncryptedSize(int noncryptedSize);
        /// <summary>
        /// Returns size of decrypted data.
        /// </summary>
        /// <param name="encryptedSize">Size of encrypted data.</param>
        int ComputeDecryptedSize(int encryptedSize);
        void Encrypt(Stream dst, Stream src, int srcByteCount);
        void Decrypt(Stream dst, Stream src, int srcByteCount);
    }

    public interface ISymmetricCryptoProvider : ICryptoProvider
    {
        byte[] Key { get; set; }
    }
    
    public interface IAsymmetricCryptoProvider : ICryptoProvider
    {
        string GetKey(bool includePrivateParams = false);
        void SetKey(string key);
    }
}
