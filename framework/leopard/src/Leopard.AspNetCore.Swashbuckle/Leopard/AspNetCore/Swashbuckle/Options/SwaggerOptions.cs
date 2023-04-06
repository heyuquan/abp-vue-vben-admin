using System.ComponentModel.DataAnnotations;

namespace Leopard.AspNetCore.Swashbuckle.Options
{
    public class SwaggerOptions
    {
        public const string SectionName = "Application:Swagger";

        /// <summary>
        /// ClientId
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// ClientSecret
        /// </summary>
        public string ClientSecret { get; set; }

        // 重写了 token 机制，不需要再传入token
        ///// <summary>
        ///// ClientScopes
        ///// </summary>
        //public string[] ClientScopes { get; set; }
        /// <summary>
        /// 隐藏abp的节点
        /// </summary>
        public bool IsHideAbpEndpoints { get; set; } = true;
    }
}
