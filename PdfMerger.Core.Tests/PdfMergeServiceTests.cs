using PdfMerger.Core;
using PdfSharp.Pdf;
using Xunit;
using Xunit.Abstractions;

namespace PdfMerger.Core.Tests;

public class PdfMergeServiceTests
{
    private readonly ITestOutputHelper _output;
    private readonly PdfMergeService _service = new();

    public PdfMergeServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task WhenMergingTwoPdfsThenReturnsCombinedPageCount()
    {
        string first = CreateSamplePdf(2);
        string second = CreateSamplePdf(3);
        string output = CreateOutputPath();

        int pageCount = await _service.MergeAsync(new[] { first, second }, output);

        Assert.Equal(5, pageCount);
    }

    [Fact]
    public async Task WhenStartingOnNewSheetAndDocumentHasOddPagesThenBlankPageIsInserted()
    {
        string first = CreateSamplePdf(3);
        string second = CreateSamplePdf(2);
        string output = CreateOutputPath();

        int pageCount = await _service.MergeAsync(
            new[] { first, second },
            output,
            startEachDocumentOnNewSheet: true);

        Assert.Equal(6, pageCount);
    }

    [Fact]
    public async Task WhenStartingOnNewSheetAndDocumentHasEvenPagesThenNoBlankPageIsInserted()
    {
        string first = CreateSamplePdf(2);
        string second = CreateSamplePdf(2);
        string output = CreateOutputPath();

        int pageCount = await _service.MergeAsync(
            new[] { first, second },
            output,
            startEachDocumentOnNewSheet: true);

        Assert.Equal(4, pageCount);
    }

    [Fact]
    public async Task WhenStartingOnNewSheetThenNoBlankPageIsAddedAfterLastDocument()
    {
        string only = CreateSamplePdf(3);
        string output = CreateOutputPath();

        int pageCount = await _service.MergeAsync(
            new[] { only },
            output,
            startEachDocumentOnNewSheet: true);

        Assert.Equal(3, pageCount);
    }

    [Fact]
    public async Task WhenNotStartingOnNewSheetAndDocumentHasOddPagesThenNoBlankPageIsInserted()
    {
        string first = CreateSamplePdf(3);
        string second = CreateSamplePdf(2);
        string output = CreateOutputPath();

        int pageCount = await _service.MergeAsync(
            new[] { first, second },
            output,
            startEachDocumentOnNewSheet: false);

        Assert.Equal(5, pageCount);
    }

    [Fact]
    public async Task WhenStartingOnNewSheetWithMultipleOddDocumentsThenBlankPageInsertedAfterEachButLast()
    {
        string first = CreateSamplePdf(1);
        string second = CreateSamplePdf(1);
        string third = CreateSamplePdf(1);
        string output = CreateOutputPath();

        int pageCount = await _service.MergeAsync(
            new[] { first, second, third },
            output,
            startEachDocumentOnNewSheet: true);

        Assert.Equal(5, pageCount);
    }

    [Fact]
    public async Task WhenMergingThenOutputFileIsCreated()
    {
        string input = CreateSamplePdf(1);
        string output = CreateOutputPath();

        await _service.MergeAsync(new[] { input }, output);

        Assert.True(File.Exists(output));
    }

    [Fact]
    public async Task WhenMergingThenOutputDocumentHasExpectedPageCount()
    {
        string first = CreateSamplePdf(2);
        string second = CreateSamplePdf(4);
        string output = CreateOutputPath();

        await _service.MergeAsync(new[] { first, second }, output);

        using PdfDocument merged = PdfSharp.Pdf.IO.PdfReader.Open(output, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
        Assert.Equal(6, merged.PageCount);
    }

    [Fact]
    public async Task WhenInputPathsAreNullThenThrowsArgumentNullException()
    {
        string output = CreateOutputPath();

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _service.MergeAsync(null!, output));
    }

    [Fact]
    public async Task WhenInputPathsAreEmptyThenThrowsArgumentException()
    {
        string output = CreateOutputPath();

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.MergeAsync(Array.Empty<string>(), output));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task WhenOutputPathIsBlankThenThrowsArgumentException(string outputPath)
    {
        string input = CreateSamplePdf(1);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.MergeAsync(new[] { input }, outputPath));
    }

    [Fact]
    public async Task WhenInputFileDoesNotExistThenThrowsFileNotFoundException()
    {
        string missing = Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.pdf");
        string output = CreateOutputPath();

        await Assert.ThrowsAsync<FileNotFoundException>(
            () => _service.MergeAsync(new[] { missing }, output));
    }

    [Fact]
    public async Task WhenCancellationIsRequestedThenThrowsOperationCanceledException()
    {
        string input = CreateSamplePdf(1);
        string output = CreateOutputPath();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => _service.MergeAsync(new[] { input }, output, cancellationToken: cts.Token));
    }

    private string CreateSamplePdf(int pageCount)
    {
        using PdfDocument document = new();

        for (int i = 0; i < pageCount; i++)
        {
            document.AddPage();
        }

        string path = Path.Combine(Path.GetTempPath(), $"pdfmerge-test-{Guid.NewGuid():N}.pdf");
        document.Save(path);
        _output.WriteLine($"Created sample PDF ({pageCount} pages): {path}");

        return path;
    }

    private string CreateOutputPath()
    {
        string path = Path.Combine(Path.GetTempPath(), $"pdfmerge-output-{Guid.NewGuid():N}.pdf");
        _output.WriteLine($"Output path: {path}");

        return path;
    }
}
