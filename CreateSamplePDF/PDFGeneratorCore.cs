using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace CreateSamplePDF
{
    public class PDFGeneratorCore : IPDFGeneratorCore
    {
        private readonly ILogger<PDFGeneratorCore> _logger;
        private readonly List<TransactionDetails> _transactionDetails = new()
        {
            new TransactionDetails() { TransactionDate = "14/12/2021", Remarks = "payment from Dave", MoneyIn = "20.00", MoneyOut = "", Balance = "2,020.00" },
            new TransactionDetails() { TransactionDate = "16/12/2021", Remarks = "Purchase Groceries from Aldi", MoneyIn = "", MoneyOut = "30.00", Balance = "1,090.00" },
            new TransactionDetails() { TransactionDate = "18/12/2021", Remarks = "Purchase Groceries from Pasteries shop", MoneyIn = "", MoneyOut = "20.00", Balance = "2,010.00" },
            new TransactionDetails() { TransactionDate = "20/12/2021", Remarks = "Purchase Groceries from TK Mx", MoneyIn = "", MoneyOut = "30.00", Balance = "1,980.00" },
            new TransactionDetails() { TransactionDate = "24/12/2021", Remarks = "Payment for Water Corp", MoneyIn = "", MoneyOut = "40.00", Balance = "1,940.00" },
            new TransactionDetails() { TransactionDate = "26/12/2021", Remarks = "Salary from Meta", MoneyIn = "1,000.00", MoneyOut = "", Balance = "2,940.00" },
            new TransactionDetails() { TransactionDate = "01/01/2022", Remarks = "Gym Subscription", MoneyIn = "", MoneyOut = "100.00", Balance = "2,840.00" },
            new TransactionDetails() { TransactionDate = "05/01/2022", Remarks = "JD purchases", MoneyIn = "", MoneyOut = "500.00", Balance = "2,340.00" },
            new TransactionDetails() { TransactionDate = "10/01/2022", Remarks = "Payment from Bukayo", MoneyIn = "1,660.00", MoneyOut = "", Balance = "4,000.00" },
        };
        public PDFGeneratorCore(ILogger<PDFGeneratorCore> logger)
        {
            _logger = logger;

        }

        public string CreateSamplePDf()
        {
            try
            {
                //===============================Declare file Path, Name of file, font and colours=========================================================
                string pdfPathFolder = "C:/PdfFolder";
                if (!Directory.Exists(pdfPathFolder))
                {
                    Directory.CreateDirectory(pdfPathFolder);
                }
                string fileName = "MyStatement" + DateTime.Now.ToString("ddMMMMyyyyHHmmssfffff") + ".pdf";
                string fullFilePath = System.IO.Path.Combine(pdfPathFolder, fileName);
                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.CP1252, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                DeviceRgb purpleColour = new(128, 0, 128);
                DeviceRgb offPurpleColour = new(230, 230, 250);

                PdfDocument pdfDocument = new(new PdfWriter(new FileStream(fullFilePath, FileMode.Create, FileAccess.Write)));
                Document document = new(pdfDocument, PageSize.A4, false); //find a way to include the margins
                document.SetMargins(20, 20, 20, 40);

                //width is 595 height is 842

                //===============================Set headers footers using PDFEventHandlers=========================================================
                pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, new PDFHeaderEventHandler());
                PDFFooterEventHandler currentEvent = new();
                pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, currentEvent);

                //=========================================================== Address and Account Summary =========================================================

                Table addressTable = new Table(new float[] { 345F }).SetFontSize(10F).SetFontColor(ColorConstants.BLACK).SetFont(font).SetBorder(Border.NO_BORDER)
                    .SetMarginTop(150F).SetMarginLeft(10f);
                addressTable.AddCell(new Cell().Add(new Paragraph("MR JOHN DOE"))
                    .Add(new Paragraph("1004, BORIS ROAD"))
                    .Add(new Paragraph("LONDON"))
                    .Add(new Paragraph("XY01 3WP")).SetBorder(Border.NO_BORDER));

                Table summaryTable = new Table(new float[] { 130F }).SetFontSize(8F).SetFontColor(ColorConstants.BLACK).SetFont(font).SetBorder(Border.NO_BORDER);
                summaryTable.AddCell(new Cell().Add(new Paragraph("Goodman Bank Account")).SetBorder(Border.NO_BORDER).SetFontColor(purpleColour).SetFontSize(14F));
                summaryTable.AddCell(new Cell().Add(new Paragraph("14 Dec 1994 - 13 Jan 1995")).SetBorder(Border.NO_BORDER));
                summaryTable.AddCell(new Cell().Add(new Paragraph("Mr John Doe")).SetBorder(Border.NO_BORDER));

                List summaryBullets = new();
                summaryBullets.Add(new ListItem("Sort Code 00-01-02"));
                summaryBullets.Add(new ListItem("Account No. 00011122"));
                summaryBullets.Add(new ListItem("SWIFTBIC CNGDVV11"));
                summaryBullets.Add(new ListItem("IBAN CFDSA 111 2344 2233"));
                summaryTable.AddCell(new Cell().Add(summaryBullets).SetBorder(Border.NO_BORDER).SetPaddingBottom(10f));

                summaryTable.AddCell(new Cell().Add(new Paragraph("At a glance")).SetFontSize(12F).SetBorder(Border.NO_BORDER).SetBackgroundColor(purpleColour).SetFontColor(ColorConstants.WHITE)
                    .SetBorderTopLeftRadius(new BorderRadius(10F)).SetBorderTopRightRadius(new BorderRadius(10F)));

                summaryTable.AddCell(new Cell().Add(new Table(new float[] { 65f, 65f }).AddCell(new Cell().Add(new Paragraph("Start balance")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).
                    AddCell(new Cell().Add(new Paragraph("£2000.00")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER))).SetBorder(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(purpleColour, 1F)));
                summaryTable.AddCell(new Cell().Add(new Table(new float[] { 65f, 65f }).AddCell(new Cell().Add(new Paragraph("Money In")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).
                    AddCell(new Cell().Add(new Paragraph("£2680.00")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER))).SetBorder(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(purpleColour, 1F)));
                summaryTable.AddCell(new Cell().Add(new Table(new float[] { 65f, 65f }).AddCell(new Cell().Add(new Paragraph("Money out")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).
                    AddCell(new Cell().Add(new Paragraph("£720.00")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER))).SetBorder(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(purpleColour, 1F)));
                summaryTable.AddCell(new Cell().Add(new Table(new float[] { 65f, 65f }).AddCell(new Cell().Add(new Paragraph("End Balance")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).
                    AddCell(new Cell().Add(new Paragraph("£4000.00")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER))).SetBorder(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(purpleColour, 1F)));

                //=========================================================== This merges Address table and Summary Table ========================================
                Table addressSummaryMergeTable = new Table(new float[] { 385F, 30F, 130F }).SetFont(font);
                addressSummaryMergeTable.AddCell(new Cell().Add(addressTable).SetBorder(Border.NO_BORDER).SetHorizontalAlignment(HorizontalAlignment.LEFT));
                addressSummaryMergeTable.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER));
                addressSummaryMergeTable.AddCell(new Cell().Add(summaryTable).SetBorder(Border.NO_BORDER).SetHorizontalAlignment(HorizontalAlignment.RIGHT));

                document.Add(addressSummaryMergeTable);

                //=========================================================== Header and Noticeboard =========================================================

                Table headerTable = new Table(new float[] { 300F }).SetFontColor(purpleColour).SetFont(font).SetBorder(Border.NO_BORDER).SetHorizontalAlignment(HorizontalAlignment.LEFT);
                headerTable.AddCell(new Cell().Add(new Paragraph("Your Goodman Bank Account Statement")).SetBorder(Border.NO_BORDER).SetFontSize(15F));
                headerTable.AddCell(new Cell().Add(new Paragraph("Current account statement")).SetBorder(Border.NO_BORDER).SetFontSize(11F).SetPaddingTop(7F));

                Table noticeBoard = new Table(new float[] { 130F }).SetFont(font).SetBorder(Border.NO_BORDER).SetFontSize(8F).SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                noticeBoard.AddCell(new Cell().Add(new Paragraph("NoticeBoard")).SetBorder(Border.NO_BORDER).SetFontSize(11F).SetBackgroundColor(purpleColour).SetFontColor(ColorConstants.WHITE)
                    .SetBorderTopLeftRadius(new BorderRadius(10F)).SetBorderTopRightRadius(new BorderRadius(10F)));
                noticeBoard.AddCell(new Cell().Add(new Paragraph("Your deposit is eligible for protection by the Financial Services Compensation Scheme")).SetBorder(Border.NO_BORDER));

                //=========================================================== Merges Header and NoticeBoard =========================================================
                Table summNoticeBoardMerge = new Table(new float[] { 385F, 30F, 130F }).SetFont(font).SetMarginTop(10F);
                summNoticeBoardMerge.AddCell(new Cell().Add(headerTable).SetBorder(Border.NO_BORDER).SetHorizontalAlignment(HorizontalAlignment.LEFT));
                summNoticeBoardMerge.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER));
                summNoticeBoardMerge.AddCell(new Cell().Add(noticeBoard).SetBorder(Border.NO_BORDER).SetHorizontalAlignment(HorizontalAlignment.RIGHT));

                document.Add(summNoticeBoardMerge);

                //=========================================================== transactionsTable =========================================================
                Table transactionsTable = new Table(new float[] { 80, 110, 70, 70, 70 }).SetFont(font).SetFontSize(10F).SetFontColor(ColorConstants.BLACK).SetBorder(Border.NO_BORDER).SetMarginTop(10);
                transactionsTable.AddCell(new Cell(1, 2).Add(new Paragraph("Your transactions").SetPadding(3)).SetBorder(Border.NO_BORDER).SetBackgroundColor(purpleColour)
                        .SetFontColor(ColorConstants.WHITE).SetPaddingBottom(3F).SetFontSize(11F).SetBorderTopRightRadius(new BorderRadius(10F)).SetBorderTopLeftRadius(new BorderRadius(10F)).SetBorderBottom(new SolidBorder(purpleColour, 1F)));
                transactionsTable.AddCell(new Cell(1, 3).Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetFontSize(11F).SetBorderBottom(new SolidBorder(purpleColour, 1F)));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("Date")).SetBorder(Border.NO_BORDER).SetFontSize(11F).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("Description")).SetBorder(Border.NO_BORDER).SetFontSize(11F).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("Money out")).SetBorder(Border.NO_BORDER).SetFontSize(11F).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetTextAlignment(TextAlignment.RIGHT));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("Money In")).SetBorder(Border.NO_BORDER).SetFontSize(11F).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetTextAlignment(TextAlignment.RIGHT));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("Balance")).SetBorder(Border.NO_BORDER).SetFontSize(11F).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetTextAlignment(TextAlignment.RIGHT));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("14 Dec")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("Start balance")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("2,000.00")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour).SetTextAlignment(TextAlignment.RIGHT));

                int backgroundCounter = 0;
                for (int i = 0; i < _transactionDetails.Count; i++)
                {
                    //=============================================== fill in data under the headers =====================================================
                    transactionsTable.AddCell(new Cell().Add(new Paragraph(_transactionDetails[i].TransactionDate)).SetBackgroundColor((backgroundCounter % 2 == 0) ? ColorConstants.WHITE : offPurpleColour)
                        .SetBorder(new SolidBorder(ColorConstants.WHITE, 1F)).SetFontColor(ColorConstants.BLACK));
                    transactionsTable.AddCell(new Cell().Add(new Paragraph(_transactionDetails[i].Remarks)).SetBackgroundColor((backgroundCounter % 2 == 0) ? ColorConstants.WHITE : offPurpleColour)
                        .SetBorder(new SolidBorder(ColorConstants.WHITE, 1F)).SetFontColor(ColorConstants.BLACK).SetKeepTogether(true));
                    transactionsTable.AddCell(new Cell().Add(new Paragraph(_transactionDetails[i].MoneyOut)).SetBackgroundColor((backgroundCounter % 2 == 0) ? ColorConstants.WHITE : offPurpleColour)
                        .SetBorder(new SolidBorder(ColorConstants.WHITE, 1F)).SetFontColor(ColorConstants.BLACK).SetTextAlignment(TextAlignment.RIGHT));
                    transactionsTable.AddCell(new Cell().Add(new Paragraph(_transactionDetails[i].MoneyIn)).SetBackgroundColor((backgroundCounter % 2 == 0) ? ColorConstants.WHITE : offPurpleColour)
                        .SetBorder(new SolidBorder(ColorConstants.WHITE, 1F)).SetFontColor(ColorConstants.BLACK).SetTextAlignment(TextAlignment.RIGHT));
                    transactionsTable.AddCell(new Cell().Add(new Paragraph(_transactionDetails[i].Balance)).SetBackgroundColor((backgroundCounter % 2 == 0) ? ColorConstants.WHITE : offPurpleColour)
                        .SetBorder(new SolidBorder(ColorConstants.WHITE, 1F)).SetFontColor(ColorConstants.BLACK).SetTextAlignment(TextAlignment.RIGHT));
                    backgroundCounter++;
                }
                transactionsTable.AddCell(new Cell().Add(new Paragraph("13 Jan")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("End balance")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour));
                transactionsTable.AddCell(new Cell().Add(new Paragraph("4,000.00")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(purpleColour, 0.5F)).SetBorderRight(new SolidBorder(ColorConstants.WHITE, 0.5F)).SetFontColor(purpleColour).SetBackgroundColor(offPurpleColour).SetTextAlignment(TextAlignment.RIGHT));

                document.Add(transactionsTable);
                //===============================================Miscelleanous or Question table =====================================================

                Table miscTable = new Table(new float[] { 95F, 305F }).SetFont(font).SetFontSize(10F).SetFontColor(ColorConstants.BLACK).SetBorder(Border.NO_BORDER).SetMarginTop(20).SetKeepTogether(true);
                miscTable.AddCell(new Cell().Add(new Paragraph("Anything Wrong?").SetFontColor(purpleColour)).SetBorder(Border.NO_BORDER));
                miscTable.AddCell(new Cell().Add(new Paragraph("If you've spotted any incorrect or unusual transactions, kindly contact us immediately")).SetBorder(Border.NO_BORDER));

                document.Add(miscTable);

                //Write the page number
                currentEvent.WritePageNumbers(pdfDocument, document);
                //close the document
                document.Close();
                return "Sample PDF successfully generated";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    /// <summary>
    /// Call this class to place the logo
    /// </summary>
    public class PDFHeaderEventHandler : IEventHandler
    {
        public void HandleEvent(Event currentEvent)
        {
            try
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
                string logoPath = "C:/Assets/Logo.png";
                var logo = ImageDataFactory.Create(logoPath);
                PdfPage page = docEvent.GetPage();
                PdfDocument pdf = docEvent.GetDocument();
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new(page.GetLastContentStream(), page.GetResources(), pdf);
                if (pdf.GetPageNumber(page) == 1)
                {
                    //i want the logo just on page 1
                    pdfCanvas.AddImageAt(logo, pageSize.GetWidth() - logo.GetWidth() - 480, pageSize.GetHeight() - logo.GetHeight() - 15, true);
                    _ = new Canvas(pdfCanvas, pageSize);
                }
                else
                {
                    _ = new Canvas(pdfCanvas, pageSize);
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    /// <summary>
    /// PDF Footer EventHandler to handle the footers on all pages
    /// </summary>
    public class PDFFooterEventHandler : IEventHandler
    {
        public void HandleEvent(Event currentEvent)
        {
            try
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;

                PdfPage page = docEvent.GetPage();
                PdfDocument pdf = docEvent.GetDocument();
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new(page.GetLastContentStream(), page.GetResources(), pdf);
                int pageNumber = pdf.GetPageNumber(page);
                int numberOfPages = pdf.GetNumberOfPages();

                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.CP1252, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                DeviceRgb offPurpleColour = new(230, 230, 250);

                float[] tableWidth = { 445, 50F };
                Table footerTable = new Table(tableWidth).SetFixedPosition(0F, 15F, pageSize.GetWidth()).SetBorder(Border.NO_BORDER);

                var botom = pageSize.GetBottom() + 15F;
                var getwidth = pageSize.GetWidth();

                footerTable.AddCell(new Cell().Add(new Paragraph("Goodman Bank PLC is authorised by the Prudential Regulation Authority and regulated by the Financial Conduct Authority and the Prudential Regulation Authority(FRN: 54113411) Registered in Brussels and Vienna (Company Number: 23336654) Registered Office: 15 Downing Street, London XY11 6TF"))
                                    .SetFont(font).SetFontSize(7F).SetBackgroundColor(offPurpleColour).SetBorder(Border.NO_BORDER).SetPaddingLeft(25F).SetPaddingRight(10F));



                Canvas canvas = new(pdfCanvas, pageSize);
                canvas.Add(footerTable).SetBorder(Border.NO_BORDER);

            }
            catch (Exception)
            {
                //_logger.LogError(ex, "An error occurred while in HandleEvent method in PDFFooterEventHandler class : {RequestId}");

                throw;
            }

        }
        /// <summary>
        /// Call this method Write the page numbers
        /// </summary>
        /// <param name="pdf"> pdfDocument</param>
        /// <param name="document">Document</param>
        public void WritePageNumbers(PdfDocument pdf, Document document)
        {
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.CP1252, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            DeviceRgb offPurpleColour = new(230, 230, 250);
            int numberOfPages = pdf.GetNumberOfPages();

            for (int i = 1; i <= numberOfPages; i++)
            {
                // Write aligned text to the specified by parameters point
                document.ShowTextAligned(new Paragraph("Page " + i + " of " + numberOfPages).SetFont(font).SetFontSize(7F).SetBackgroundColor(offPurpleColour).SetBorder(Border.NO_BORDER).SetWidth(50F).SetPaddings(8F, 28F, 9F, 7F).SetTextAlignment(TextAlignment.RIGHT),
                        555, 15.5f, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);
            }
        }
    }
}
