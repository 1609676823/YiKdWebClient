using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace YiKdWebClient.Tests;

public class InvalidInputBehaviorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void View_should_not_throw_when_formId_invalid(string? formId)
    {
        var client = new YiK3CloudClient();

        string? res = null;
        var ex = Record.Exception(() => res = client.View(formId!, "{}")); // 这里的 ! 是“故意传 null”用的

        Assert.Null(ex);                 // ① 明确断言：不抛异常
        AssertInvalidResult(res);        // ② 断言：结果符合“非法参数”的约定
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void View_should_not_throw_when_payload_invalid(string? payload)
    {
        var client = new YiK3CloudClient();

        string? res = null;
        var ex = Record.Exception(() => res = client.View("SEC_User", payload!));

        Assert.Null(ex);
        AssertInvalidResult(res);
    }

    private static void AssertInvalidResult(string? res)
    {
        // ✅ 你必须在这里定义“非法参数时应该返回什么”
        // 下面给你三种常见契约，选一种与你库一致的：

        // 契约A：非法参数返回空字符串（最常见的“无异常”风格）
         Assert.True(!string.IsNullOrWhiteSpace(res));

        // 契约B：非法参数返回一个错误 JSON（推荐：调用方可读、可记录）
        // Assert.False(string.IsNullOrWhiteSpace(res));
        // Assert.Contains("error", res!, StringComparison.OrdinalIgnoreCase);

        // 契约C：非法参数返回固定错误码/固定前缀
        // Assert.StartsWith("INVALID_", res);

        // 临时兜底（不推荐长期用）：至少保证不为 null，避免“什么都没返回”
        Assert.NotNull(res);
    }
}
