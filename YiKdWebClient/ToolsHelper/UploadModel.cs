using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace YiKdWebClient.ToolsHelper
{
    /// <summary>
    /// 文件上传模型
    /// </summary>
    public class UploadModel
    {
        /// <summary>
        /// 文件上传模型
        /// </summary>
        public UploadModel() { }
        /// <summary>
        /// 文件上传模型数据结构
        /// </summary>
        [JsonPropertyName("data")]
        //[JsonIgnore]
        public UploadModelData data { get; set; }=new UploadModelData();
    }

    /// <summary>
    /// 文件上传模型数据结构
    /// </summary>
    public class UploadModelData
    {
        /// <summary>
        /// 文件名 (字符, 必填)
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 表单ID (字符, 必填)
        /// </summary>
        public string FormId { get; set; } = string.Empty;

        /// <summary>
        /// 是否为最后一项 (布尔, 必填)
        /// </summary>
        public bool IsLast { get; set; } = false;

        /// <summary>
        /// 单据内码 (字符, 必填)
        /// </summary>
        public string InterId { get; set; } = string.Empty;

        /// <summary>
        /// 单据体标识，上船单据体附件时填写所述单据体的标识 (字符, 选填)
        /// </summary>
        public string Entrykey { get; set; } = string.Empty;

        /// <summary>
        /// 分录内码，如果是单据头附件，要么不填(null)，要么填-1 (字符, 选填)
        /// </summary>
        public string EntryinterId { get; set; } ="-1";

        /// <summary>
        /// 单据编号 (字符, 必填)
        /// </summary>
        public string BillNO { get; set; } = string.Empty;

        /// <summary>
        /// 附件别名 (字符, 选填)
        /// </summary>
        public string AliasFileName { get; set; } = string.Empty;

        /// <summary>
        /// 文件ID；如果分多次上传，首次上传必填 (字符, 必填)
        /// </summary>
        public string FileId { get; set; } = string.Empty;

        /// <summary>
        /// Base64后的文件字节流 (字符, 必填)
        /// </summary>
        public string SendByte { get; set; } = string.Empty;
    }



}
