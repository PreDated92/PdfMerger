using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PdfMerger.Core;

/// <summary>
/// Provides functionality to merge multiple PDF documents into a single output document.
/// This service contains no UI dependencies so it can be reused across different front ends.
/// </summary>
public sealed class PdfMergeService
{
    /// <summary>
    /// Merges the supplied PDF files, in the order provided, into a single PDF written to
    /// <paramref name="outputPath"/>.
    /// </summary>
    /// <param name="inputPaths">The full paths of the PDF files to merge, in the desired output order.</param>
    /// <param name="outputPath">The full path of the merged PDF file to create.</param>
    /// <param name="startEachDocumentOnNewSheet">
    /// When <see langword="true"/>, a blank page is inserted after any document that has an odd
    /// number of pages so that, during double-sided (duplex) printing, the next document always
    /// begins on the front of a new sheet of paper.
    /// </param>
    /// <param name="cancellationToken">A token used to cancel the merge operation.</param>
    /// <returns>The total number of pages written to the merged document.</returns>
    public async Task<int> MergeAsync(
        IReadOnlyList<string> inputPaths,
        string outputPath,
        bool startEachDocumentOnNewSheet = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inputPaths);

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new ArgumentException("An output path must be provided.", nameof(outputPath));
        }

        if (inputPaths.Count == 0)
        {
            throw new ArgumentException("At least one input PDF is required to merge.", nameof(inputPaths));
        }

        // PdfSharp is synchronous; run the work off the calling (UI) thread.
        return await Task.Run(
                () => Merge(inputPaths, outputPath, startEachDocumentOnNewSheet, cancellationToken),
                cancellationToken)
            .ConfigureAwait(false);
    }

    private static int Merge(
        IReadOnlyList<string> inputPaths,
        string outputPath,
        bool startEachDocumentOnNewSheet,
        CancellationToken cancellationToken)
    {
        using PdfDocument outputDocument = new();

        for (int documentIndex = 0; documentIndex < inputPaths.Count; documentIndex++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string inputPath = inputPaths[documentIndex];

            if (string.IsNullOrWhiteSpace(inputPath))
            {
                throw new ArgumentException("One of the input paths is empty.", nameof(inputPaths));
            }

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException($"The PDF file was not found: {inputPath}", inputPath);
            }

            using PdfDocument inputDocument = PdfReader.Open(inputPath, PdfDocumentOpenMode.Import);

            PdfPage? lastPage = null;
            for (int pageIndex = 0; pageIndex < inputDocument.PageCount; pageIndex++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                lastPage = outputDocument.AddPage(inputDocument.Pages[pageIndex]);
            }

            bool anotherDocumentFollows = documentIndex < inputPaths.Count - 1;

            // For duplex printing, a document with an odd page count would leave the next
            // document starting on the back of a sheet. Insert a blank page (matching the
            // previous page size) so the following document begins on a new sheet.
            if (startEachDocumentOnNewSheet
                && anotherDocumentFollows
                && lastPage is not null
                && inputDocument.PageCount % 2 != 0)
            {
                PdfPage blankPage = outputDocument.AddPage();
                blankPage.Width = lastPage.Width;
                blankPage.Height = lastPage.Height;
            }
        }

        // Capture the page count before saving; once saved, the in-memory
        // document is finalized and can no longer be accessed or modified.
        int mergedPageCount = outputDocument.PageCount;

        outputDocument.Save(outputPath);

        return mergedPageCount;
    }
}
