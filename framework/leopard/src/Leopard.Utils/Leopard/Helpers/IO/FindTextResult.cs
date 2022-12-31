using System.Collections.Generic;

namespace Leopard.Helpers.IO
{
    public class FindTextResult
    {
        /// <summary>
        /// 包含 路径 + 文件名 + 文件后缀
        /// </summary>
        public string FileFullName { get; set; }

        public List<FindTextItem> Matchs { get; set; } = new();

    }

    public class FindTextItem
    {
        public string LineText { get; set; }
        /// <summary>
        /// 行号从1开始
        /// </summary>
        public int LineNo { get; set; }

        public int LineIndex { get { return LineNo - 1; } }
    }
}
