using System.Text;
using System.Text.Json;
using YiKdWebClient.Tests.TestInfrastructure;
using YiKdWebClient.ToolsHelper;

namespace YiKdWebClient.Tests;

public class AttachmentHelperTests
{
    [Fact]
    public void ReadFileInChunksByAction_splits_file_and_marks_last_chunk()
    {
        var path = CreateTemporaryFile(new byte[] { 0, 1, 2, 3, 4 });
        try
        {
            var chunks = new List<FileChunk>();

            AttachmentHelper.ReadFileInChunksByAction(path, chunks.Add, 2);

            Assert.Equal(3, chunks.Count);
            Assert.Equal(new long[] { 0, 1, 2 }, chunks.Select(chunk => chunk.Chunkindex));
            Assert.Equal(new byte[] { 0, 1 }, chunks[0].Chunkbyte);
            Assert.Equal(new byte[] { 2, 3 }, chunks[1].Chunkbyte);
            Assert.Equal(new byte[] { 4 }, chunks[2].Chunkbyte);
            Assert.False(chunks[0].IsLast);
            Assert.False(chunks[1].IsLast);
            Assert.True(chunks[2].IsLast);
            Assert.All(chunks, chunk => Assert.Equal(Path.GetFileName(path), chunk.Filename));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ReadFileInChunksByAction_marks_exact_boundary_chunk_as_last()
    {
        var path = CreateTemporaryFile(new byte[] { 0, 1, 2, 3 });
        try
        {
            var chunks = new List<FileChunk>();

            AttachmentHelper.ReadFileInChunksByAction(path, chunks.Add, 2);

            Assert.Equal(2, chunks.Count);
            Assert.False(chunks[0].IsLast);
            Assert.True(chunks[1].IsLast);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ReadFileInChunksByAction_rejects_non_positive_chunk_size()
    {
        var path = CreateTemporaryFile(new byte[] { 1 });
        try
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                AttachmentHelper.ReadFileInChunksByAction(path, _ => { }, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                AttachmentHelper.ReadFileInChunksByAction(path, _ => { }, -1));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ReadBase64ChunksByAction_splits_decoded_bytes()
    {
        var chunks = new List<FileChunk>();
        var base64 = Convert.ToBase64String(new byte[] { 0, 1, 2, 3, 4 });

        AttachmentHelper.ReadBase64ChunksByAction(base64, "sample.bin", chunks.Add, 2);

        Assert.Equal(3, chunks.Count);
        Assert.Equal(new byte[] { 0, 1 }, chunks[0].Chunkbyte);
        Assert.Equal(new byte[] { 2, 3 }, chunks[1].Chunkbyte);
        Assert.Equal(new byte[] { 4 }, chunks[2].Chunkbyte);
        Assert.False(chunks[0].IsLast);
        Assert.False(chunks[1].IsLast);
        Assert.True(chunks[2].IsLast);
        Assert.All(chunks, chunk => Assert.Equal("sample.bin", chunk.Filename));
    }

    [Fact]
    public void ReadBase64ChunksByAction_rejects_non_positive_chunk_size()
    {
        const string base64 = "AQ==";

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AttachmentHelper.ReadBase64ChunksByAction(base64, "sample.bin", _ => { }, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AttachmentHelper.ReadBase64ChunksByAction(base64, "sample.bin", _ => { }, -1));
    }

    [Fact]
    public void FileChunk_setting_bytes_updates_base64()
    {
        var chunk = new FileChunk { Chunkbyte = new byte[] { 0, 1, 2, 255 } };

        Assert.Equal("AAEC/w==", chunk.ChunkBase64);
        Assert.Equal(new byte[] { 0, 1, 2, 255 }, chunk.Chunkbyte);
    }

    [Fact]
    public void CheckUploadModelData_accepts_valid_header_attachment()
    {
        var upload = CreateValidUploadModel();

        var exception = Record.Exception(() => AttachmentHelper.CheckUploadModelData(upload));

        Assert.Null(exception);
    }

    [Fact]
    public void CheckUploadModelData_rejects_each_required_blank_field()
    {
        AssertValidationFailure(model => model.data.FileName = string.Empty, "文件名");
        AssertValidationFailure(model => model.data.FormId = string.Empty, "表单ID");
        AssertValidationFailure(model => model.data.InterId = string.Empty, "单据内码");
        AssertValidationFailure(model => model.data.FileId = string.Empty, "文件ID");
        AssertValidationFailure(model => model.data.SendByte = string.Empty, "文件字节流");
    }

    [Fact]
    public void CheckUploadModelData_requires_entry_key_and_entry_id_together()
    {
        var missingId = CreateValidUploadModel();
        missingId.data.Entrykey = "FEntity";
        missingId.data.EntryinterId = string.Empty;

        var missingKey = CreateValidUploadModel();
        missingKey.data.Entrykey = string.Empty;
        missingKey.data.EntryinterId = "1001";

        Assert.Contains(
            "Entrykey",
            Assert.Throws<ArgumentException>(() => AttachmentHelper.CheckUploadModelData(missingId)).Message);
        Assert.Contains(
            "Entrykey",
            Assert.Throws<ArgumentException>(() => AttachmentHelper.CheckUploadModelData(missingKey)).Message);
    }

    [Fact]
    public void AttachmentUploadByBase64_uploads_every_chunk_and_updates_file_id()
    {
        var uploadCall = 0;
        using var server = new LoopbackHttpServer(request =>
        {
            if (request.PathAndQuery.Contains("AttachmentUpLoad", StringComparison.Ordinal))
            {
                uploadCall++;
                return SuccessfulUploadResponse("file-" + uploadCall);
            }

            return new TestHttpResponse();
        });
        using var client = TestClientFactory.CreateApiHeaderClient(server.K3CloudUrl);
        var upload = CreateUploadTemplateForTransfer();
        var progress = new List<FileChunk>();

        var response = AttachmentHelper.AttachmentUploadByBase64(
            Convert.ToBase64String(new byte[] { 0, 1, 2, 3, 4 }),
            "sample.bin",
            client,
            upload,
            2,
            (chunk, _) => progress.Add(chunk));

        Assert.Contains("\"FileId\":\"file-3\"", response);
        Assert.Equal("file-3", upload.data.FileId);
        Assert.Equal(3, progress.Count);
        Assert.True(progress[^1].IsLast);
        Assert.Equal(3, server.Requests.Count(request => request.PathAndQuery.Contains("AttachmentUpLoad", StringComparison.Ordinal)));
        Assert.Equal(3, server.Requests.Count(request => request.PathAndQuery.Contains("AuthService.Logout", StringComparison.Ordinal)));
    }

    [Fact]
    public void AttachmentUploadByFilePath_uploads_file_chunks()
    {
        var path = CreateTemporaryFile(new byte[] { 0, 1, 2 });
        try
        {
            var uploadCall = 0;
            using var server = new LoopbackHttpServer(request =>
            {
                if (request.PathAndQuery.Contains("AttachmentUpLoad", StringComparison.Ordinal))
                {
                    uploadCall++;
                    return SuccessfulUploadResponse("file-" + uploadCall);
                }

                return new TestHttpResponse();
            });
            using var client = TestClientFactory.CreateApiHeaderClient(server.K3CloudUrl);
            var upload = CreateUploadTemplateForTransfer();

            var response = AttachmentHelper.AttachmentUploadByFilePath(path, client, upload, 2);

            Assert.Contains("\"FileId\":\"file-2\"", response);
            Assert.Equal("file-2", upload.data.FileId);
            Assert.Equal(Path.GetFileName(path), upload.data.FileName);
            Assert.True(upload.data.IsLast);
            Assert.Equal(2, uploadCall);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void AttachmentUploadByBase64_returns_server_failure_response()
    {
        const string failed = "{\"Result\":{\"ResponseStatus\":{\"IsSuccess\":false},\"FileId\":\"\"}}";
        using var server = new LoopbackHttpServer(request =>
            request.PathAndQuery.Contains("AttachmentUpLoad", StringComparison.Ordinal)
                ? new TestHttpResponse(Body: failed)
                : new TestHttpResponse());
        using var client = TestClientFactory.CreateApiHeaderClient(server.K3CloudUrl);

        var response = AttachmentHelper.AttachmentUploadByBase64(
            "AQ==",
            "sample.bin",
            client,
            CreateUploadTemplateForTransfer(),
            2);

        Assert.Contains(failed, response);
    }

    private static void AssertValidationFailure(Action<UploadModel> invalidate, string expectedMessage)
    {
        var model = CreateValidUploadModel();
        invalidate(model);

        var exception = Assert.Throws<ArgumentException>(() => AttachmentHelper.CheckUploadModelData(model));

        Assert.Contains(expectedMessage, exception.Message);
    }

    private static UploadModel CreateValidUploadModel()
    {
        return new UploadModel
        {
            data = new UploadModelData
            {
                FileName = "sample.bin",
                FormId = "TEST_Form",
                InterId = "100",
                Entrykey = string.Empty,
                EntryinterId = "-1",
                FileId = "file-id",
                SendByte = "AQ=="
            }
        };
    }

    private static UploadModel CreateUploadTemplateForTransfer()
    {
        return new UploadModel
        {
            data = new UploadModelData
            {
                FormId = "TEST_Form",
                InterId = "100",
                Entrykey = string.Empty,
                EntryinterId = "-1"
            }
        };
    }

    private static TestHttpResponse SuccessfulUploadResponse(string fileId)
    {
        return new TestHttpResponse(
            Body: JsonSerializer.Serialize(new
            {
                Result = new
                {
                    ResponseStatus = new { IsSuccess = true },
                    FileId = fileId
                }
            }));
    }

    private static string CreateTemporaryFile(byte[] contents)
    {
        var path = Path.Combine(Path.GetTempPath(), $"YiKdWebClient-{Guid.NewGuid():N}.bin");
        File.WriteAllBytes(path, contents);
        return path;
    }
}
