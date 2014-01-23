using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;

namespace RelayDNPSecurity
{
    public partial class ucOSAsymKeyGen : ucDNPSAv5SuperClass
    {
        public ucOSAsymKeyGen()
        {
            InitializeComponent();
            this.initializeKeyValueControl();
        }

        private ucKeyValuesInputControl privateKeyBox;
        private ucKeyValuesInputControl publicKeyBox;
        private static string _privateKeyName = "Relay (Outstation) Private Key";
        private static string _publicKeyName = "Relay (Outstation) Private Key";

        #region Initialization

        void initializeKeyValueControl()
        {
            this.privateKeyBox = new ucKeyValuesInputControl(32, _privateKeyName);
            this.publicKeyBox = new ucKeyValuesInputControl(32, _publicKeyName);

            Point tempPoint = new Point(40, 15);
            this.privateKeyBox.Location = tempPoint;

            tempPoint = new Point(this.privateKeyBox.Location.X, this.privateKeyBox.Location.Y + this.privateKeyBox.Height + 5);
            this.publicKeyBox.Location = tempPoint;

            this.groupBoxMain.Controls.Add(this.privateKeyBox);
            this.groupBoxMain.Controls.Add(this.publicKeyBox);
        }

        #endregion

        private void buttonGetKeyPair_Click(object sender, EventArgs e)
        {            

        }

        private void buttonGenerateKey_Click(object sender, EventArgs e)
        {
            RsaKeyPairGenerator r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), 64));
            var keys = r.GenerateKeyPair();

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keys.Private);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keys.Public);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            
        }

        private void buttonSendKeyPair_Click(object sender, EventArgs e)
        {

        }
    }
}
