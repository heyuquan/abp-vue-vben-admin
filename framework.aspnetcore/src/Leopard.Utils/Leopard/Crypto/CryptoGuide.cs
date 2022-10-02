using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Crypto
{
    // 利用静态构造函数实现单例模式
    // 由CLR保证，在程序第一次使用该类之前被调用，而且只调用一次。同静态变量一样, 它会随着程序运行, 就被实例化, 同静态变量一个道理。

    // 单钥密码体制/对称密码体制              ：DES、AES
    // 双钥密码体制/非对称密码体制/公钥秘钥   ：RSA、DSA
    // 摘要算法/Hash算法/散列算法：是一种将任意长度的输入浓缩成固定长度的字符串的算法(不同算法散列值长度不一样)
    // 消息摘要算法分为三类：
    //   #、MD(Message Digest)：消息摘要 eg：md5
    //   #、SHA(Secure Hash Algorithm)：安全散列
    //   #、MAC(Message Authentication Code)：消息认证码

    /// <summary>
    /// 编码、加密相关类的导航
    /// </summary>
    public static class CryptoGuide
    {
        /// <summary>
        /// Base64 单例
        /// </summary>
        public static Base64 Base64 { get { return Base64.Instance; } }

        /// <summary>
        /// Md5 单例
        /// </summary>
        public static Md5Crypto Md5 { get { return Md5Crypto.Instance; } }

        /// <summary>
        /// RSA 单例
        /// </summary>
        public static RSACrypto RSA { get { return RSACrypto.Instance; } }

        /// <summary>
        /// AES 单例
        /// </summary>
        public static AESCrypto AES { get { return AESCrypto.Instance; } }

        /// <summary>
        /// Hex（十六进制） 单例
        /// </summary>
        public static HexEncoding Hex { get { return HexEncoding.Instance; } }

        /// <summary>
        /// 创建证书
        /// </summary>
        public static CerMaker CerMaker { get { return CerMaker.Instance; } }

        /// <summary>
        /// Sha1
        /// </summary>
        public static SHA1Crypto Sha1 { get { return SHA1Crypto.Instance; } }
    }
}
