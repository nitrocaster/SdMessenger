﻿using System;
using System.IO;
using System.Text;
using Sdm.Core;
using Sdm.Core.Crypto;
using Sdm.Core.Util;
using Rfc2898DeriveBytes = System.Security.Cryptography.Rfc2898DeriveBytes;

namespace Sdm.ClientWPF
{
    internal sealed class ClientConfig
    {
        private static readonly ISymmetricCryptoProvider Aes; // XXX: dispose?
        private const string ConfigFileName = "sdm_client.ini";

        // [client]
        public ProtocolId Protocol = ProtocolId.Json;
        public string Host = "";
        public ushort Port = 5477;
        public string Login = "";
        public string Password = "";
        public bool Remember = false;
        // [security]
        public SdmSymmetricAlgorithm SymAlgorithm = SdmSymmetricAlgorithm.AES;
        public SdmAsymmetricAlgorithm AsymAlgorithm = SdmAsymmetricAlgorithm.RSA;
        // [socket]
        public bool UseIPv6 = false;
        public int SocketSendTimeout = 1000;
        public int SocketSendBufferSize = 0x8000;
        public int SocketReceiveBufferSize = 0x8000;
        // [misc]
        public int UpdateSleep = 50;

        static ClientConfig()
        {
            Aes = CryptoProviderFactory.Instance.CreateSymmetric(SdmSymmetricAlgorithm.AES);
            Aes.KeySize = 256;
            var salt = new byte[] {0x10, 0x77, 0x16, 0x18, 0xf7, 0x6a, 0x8d, 0x0f};
            var tsm = new Rfc2898DeriveBytes("octoc@t", salt);
            var keySize = Aes.KeySize/8;
            Aes.Key = tsm.GetBytes(keySize);
            var blockSize = Aes.IV.Length;
            Aes.IV = tsm.GetBytes(blockSize);
        }

        public ClientConfig() { Load(); }

        private static string Encrypt(string plainText)
        {
            var src = new MemoryStream(Encoding.UTF8.GetBytes(plainText));
            var dst = new MemoryStream();
            Aes.Encrypt(dst.AsUnclosable(), src, (int)src.Length);
            var cipherText = Convert.ToBase64String(dst.GetBuffer(), 0, (int)dst.Length);
            return cipherText;
        }

        private static string Decrypt(string cipherText)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);
            var src = new MemoryStream(cipherBytes);
            var dst = new MemoryStream();
            Aes.Decrypt(dst.AsUnclosable(), src, (int)src.Length);
            var plainText = Encoding.UTF8.GetString(dst.GetBuffer(), 0, (int)dst.Length);
            return plainText;
        }

        private void Load()
        {
            IniFile cfg;
            try
            { cfg = new IniFile(ConfigFileName, Encoding.UTF8); }
            catch (IOException)
            { return; }
            string tmp = null;
            if (cfg.ContainsSection("socket"))
            {
                cfg.TryGetBool("socket", "ipv6", ref UseIPv6);
                cfg.TryGetInt32("socket", "send_timeout", ref SocketSendTimeout);
                cfg.TryGetInt32("socket", "send_buffer_size", ref SocketSendBufferSize);
                cfg.TryGetInt32("socket", "recv_buffer_size", ref SocketReceiveBufferSize);
            }
            if (cfg.ContainsSection("client"))
            {
                if (cfg.TryGetString("client", "protocol", ref tmp))
                {
                    ProtocolId p;
                    if (Enum.TryParse(tmp, true, out p))
                        Protocol = p;
                }
                if (cfg.TryGetString("client", "host", ref tmp))
                    Host = tmp;
                int port = 0;
                if (cfg.TryGetInt32("client", "port", ref port))
                {
                    if (0 <= port && port <= ushort.MaxValue)
                        Port = (ushort)port;
                }
                if (cfg.TryGetString("client", "login", ref tmp))
                    Login = tmp;
                if (cfg.TryGetString("client", "password", ref tmp) && tmp != "")
                    Password = Decrypt(tmp);
                cfg.TryGetBool("client", "remember", ref Remember);
            }
            if (cfg.ContainsSection("security"))
            {
                if (cfg.TryGetString("security", "sym_algorithm", ref tmp))
                {
                    SdmSymmetricAlgorithm sa;
                    if (Enum.TryParse(tmp, true, out sa))
                        SymAlgorithm = sa;
                }
                if (cfg.TryGetString("security", "asym_algorithm", ref tmp))
                {
                    SdmAsymmetricAlgorithm aa;
                    if (Enum.TryParse(tmp, true, out aa))
                        AsymAlgorithm = aa;
                }
            }
            if (cfg.ContainsSection("misc"))
                cfg.TryGetInt32("misc", "update_sleep", ref UpdateSleep);
        }

        public void Save()
        {
            using (var w = new IniWriter(ConfigFileName, false, Encoding.UTF8))
            {
                w.WriteSection("client");
                w.Write("protocol", Protocol.ToString().ToLower());
                w.Write("host", Host);
                w.Write("port", Port);
                w.Write("login", Login);
                w.Write("password", Password == "" ? "" : Encrypt(Password));
                w.Write("remember", Remember);
                w.WriteSection("security");
                w.Write("sym_algorithm", SymAlgorithm.ToString().ToLower());
                w.Write("asym_algorithm", AsymAlgorithm.ToString().ToLower());
                w.WriteSection("socket");
                w.Write("ipv6", UseIPv6);
                w.Write("send_timeout", SocketSendTimeout);
                w.Write("send_buffer_size", SocketSendBufferSize);
                w.Write("recv_buffer_size", SocketReceiveBufferSize);
                w.WriteSection("misc");
                w.Write("update_sleep", UpdateSleep);
            }
        }
    }
}
