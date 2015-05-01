﻿using System;
using System.IO;
using NUnit.Framework;
using Sdm.Core;
using Sdm.Core.Messages;
using Sdm.Core.Util;

namespace SdmCore.Tests
{
    [TestFixture]
    public class MessageTests
    {
        private readonly ProtocolId[] protocols = {ProtocolId.Binary, ProtocolId.Json};

        private void MultiprotocolSaveLoad(ISdmSerializable obj, Action cb)
        {
            var ms = new MemoryStream();
            var msw = ms.AsUnclosable();
            foreach (var p in protocols)
            {
                obj.Save(msw, p);
                msw.Position = 0;
                obj.Load(msw, p);
                msw.Position = 0;
                if (cb != null)
                    cb();
            }
        }

        [Test]
        public void SaveLoadMessageHeader()
        {
            // arrange
            const MessageId refId = MessageId.SvUserlistUpdate;
            const MessageFlags refFlags = MessageFlags.Secure;
            const int refSize = 42;
            var hdr = new MsgHeader {Id = refId, Flags = refFlags, Size = refSize};
            // act
            MultiprotocolSaveLoad(hdr, () =>
            {
                // assert
                Assert.AreEqual(hdr.Id, refId);
                Assert.AreEqual(hdr.Flags, refFlags);
                Assert.AreEqual(hdr.Size, refSize);
            });
        }

        [Test]
        public void SaveLoadSvPublicKeyChallenge()
        {
            // arrange
            const int refKeySize = 2048;
            var msg = new SvPublicKeyChallenge {KeySize = refKeySize};
            // act
            MultiprotocolSaveLoad(msg, () =>
            {
                // assert
                Assert.AreEqual(msg.KeySize, refKeySize);
            });
        }

        [Test]
        public void SaveLoadClPublicKeyRespond()
        {
            // arrange
            const string refKey = "rsa_public_key";
            var msg = new ClPublicKeyRespond {Key = refKey};
            // act
            MultiprotocolSaveLoad(msg, () =>
            {
                // assert
                Assert.AreEqual(msg.Key, refKey);
            });
        }

        [Test]
        public void SaveLoadSvAuthChallenge()
        {
            // arrange
            byte[] refSessionKey = {88, 10, 42, 98, 61, 210, 99, 65, 88, 1, 24, 89, 61, 201, 99, 56};
            var msg = new SvAuthChallenge {SessionKey = refSessionKey};
            // act
            MultiprotocolSaveLoad(msg, () =>
            {
                // assert
                Assert.AreEqual(msg.SessionKey, refSessionKey);
            });
        }

        [Test]
        public void SaveLoadClAuthRespond()
        {
            // arrange
            const string refLogin = "username";
            const string refPassword = "password";
            var msg = new ClAuthRespond {Login = refLogin, Password = refPassword};
            // act
            MultiprotocolSaveLoad(msg, () =>
            {
                // assert
                Assert.AreEqual(msg.Login, refLogin);
                Assert.AreEqual(msg.Password, refPassword);
            });
        }

        [Test]
        public void SaveLoadSvAuthResult()
        {
            // arrange
            const string refMessage = "You have been banned by serveradmin";
            const AuthResult refResult = AuthResult.Banned;
            var msg = new SvAuthResult {Message = refMessage, Result = refResult};
            // act
            MultiprotocolSaveLoad(msg, () =>
            {
                // assert
                Assert.AreEqual(msg.Message, refMessage);
                Assert.AreEqual(msg.Result, refResult);
            });
        }

        [Test]
        public void SaveLoadClDisconnect()
        {
            // arrange
            var msg = new ClDisconnect();
            // act, assert (no data, just save and load)
            MultiprotocolSaveLoad(msg, null);
        }

        [Test]
        public void SaveLoadSvDisconnect()
        {
            // arrange
            const string refMessage = "Server will be available again in 5 min";
            const DisconnectReason refReason = DisconnectReason.Shutdown;
            var msg = new SvDisconnect {Message = refMessage, Reason = refReason};
            // act
            MultiprotocolSaveLoad(msg, () =>
            {
                // assert
                Assert.AreEqual(msg.Message, refMessage);
                Assert.AreEqual(msg.Reason, refReason);
            });
        }

        [Test]
        public void SaveLoadClUserlistRequest()
        {
            // arrange
            var msg = new ClUserlistRequest();
            // act, assert (no data, just save and load)
            MultiprotocolSaveLoad(msg, null);
        }

        [Test]
        public void SaveLoadSvUserlistRespond()
        {
            // arrange
            string[] refUsernames = {"admin", "user", "godzilla", "king", "octopus"};
            var msg = new SvUserlistRespond {Usernames = refUsernames};
            // act
            MultiprotocolSaveLoad(msg, () =>
            {
                // assert
                Assert.AreEqual(msg.Usernames, refUsernames);
            });
        }

        [Test]
        public void SaveLoadSvUserlistUpdate()
        {
            // arrange
            string[] refConnected = {"godzilla", "octopus"};
            string[] refDisconnected = {"admin", "user", "king"};
            var msg = new SvUserlistUpdate {Connected = refConnected, Disconnected = refDisconnected};
            // act
            MultiprotocolSaveLoad(msg, () =>
            {
                // assert
                Assert.AreEqual(msg.Connected, refConnected);
                Assert.AreEqual(msg.Disconnected, refDisconnected);
            });
        }

        [Test]
        public void SaveLoadCsChatMessage()
        {
            // arrange
            const string refUsername = "king";
            const string refMessage = "I'm a king";
            var msg = new CsChatMessage {Username = refUsername, Message = refMessage};
            // act
            MultiprotocolSaveLoad(msg, () =>
            {
                // assert
                Assert.AreEqual(msg.Username, refUsername);
                Assert.AreEqual(msg.Message, refMessage);
            });
        }
    }
}
