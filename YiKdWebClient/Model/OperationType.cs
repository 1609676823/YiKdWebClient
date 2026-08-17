using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.Model
{
#pragma warning disable CS1591
    public enum OperationType
    {
        /// <summary>
        ///查看表单
        /// </summary>
        [EnumMember(Value = "View")]
        [Description("View")]
        View,

        /// <summary>
        /// 保存
        /// </summary>
        [EnumMember(Value = "Save")]
        [Description("Save")]
        Save,

        /// <summary>
        /// 批量保存
        /// </summary>
        [EnumMember(Value = "BatchSave")]
        [Description("BatchSave")]
        BatchSave,

        /// <summary>
        ///提交表单
        /// </summary>
        [EnumMember(Value = "Submit")]
        [Description("Submit")]
        Submit,

        /// <summary>
        ///审核表单
        /// </summary>
        [EnumMember(Value = "Audit")]
        [Description("Audit")]
        Audit,

        /// <summary>
        ///反审核表单
        /// </summary>
        [EnumMember(Value = "UnAudit")]
        [Description("UnAudit")]
        UnAudit,

        /// <summary>
        ///删除表单
        /// </summary>
        [EnumMember(Value = "Delete")]
        [Description("Delete")]
        Delete,


        /// <summary>
        ///表单数据查询
        /// </summary>
        [EnumMember(Value = "ExecuteBillQuery")]
        [Description("ExecuteBillQuery")]
        ExecuteBillQuery,


        /// <summary>
        ///暂存表单
        /// </summary>
        [EnumMember(Value = "Draft")]
        [Description("Draft")]
        Draft,

        /// <summary>
        ///分配表单
        /// </summary>
        [EnumMember(Value = "Allocate")]
        [Description("Allocate")]
        Allocate,

        /// <summary>
        ///下推
        /// </summary>
        [EnumMember(Value = "Push")]
        [Description("Push")]
        Push,

        /// <summary>
        ///分组保存
        /// </summary>
        [EnumMember(Value = "GroupSave")]
        [Description("GroupSave")]
        GroupSave,


        /// <summary>
        ///弹性域保存
        /// </summary>
        [EnumMember(Value = "FlexSave")]
        [Description("FlexSave")]
        FlexSave,

        /// <summary>
        ///发送消息接口
        /// </summary>
        [EnumMember(Value = "SendMsg")]
        [Description("SendMsg")]
        SendMsg,

        /// <summary>
        ///登出
        /// </summary>
        [EnumMember(Value = "Logout")]
        [Description("Logout")]
        Logout,

        /// <summary>
        ///通用操作
        /// </summary>
        [EnumMember(Value = "ExecuteOperation")]
        [Description("ExecuteOperation")]
        ExecuteOperation,

        /// <summary>
        ///切换上下文默认组织
        /// </summary>
        [EnumMember(Value = "SwitchOrg")]
        [Description("SwitchOrg")]
        SwitchOrg,


        /// <summary>
        ///工作流审批
        /// </summary>
        [EnumMember(Value = "WorkflowAudit")]
        [Description("WorkflowAudit")]
        WorkflowAudit,


        /// <summary>
        ///简单账表查询
        /// </summary>
        [EnumMember(Value = "DynamicFormService")]
        [Description("DynamicFormService")]
        DynamicFormService,
    }
}
