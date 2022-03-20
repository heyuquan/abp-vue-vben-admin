using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Crypto
{
    // 利用静态构造函数实现单例模式
    // 由CLR保证，在程序第一次使用该类之前被调用，而且只调用一次。同静态变量一样, 它会随着程序运行, 就被实例化, 同静态变量一个道理。

    /// <summary>
    /// 编码、加密相关类的导航
    /// </summary>
    public class CryptoGuide
    {
        private CryptoGuide()
        {

        }

        private static readonly Base64 _base64 = new Base64();
        public static Base64 Base64 { get { return _base64; } }


        private static readonly EasyMd5 _easyMd5 = new EasyMd5();
        public static EasyMd5 EasyMd5 { get { return _easyMd5; } }


        private static readonly RSACrypto _rsa = new RSACrypto();
        public static RSACrypto RSA { get { return _rsa; } }


        private static readonly AESCrypto _aes = new AESCrypto();
        public static AESCrypto AES { get { return _aes; } }

    }
}
