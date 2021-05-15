using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using KRV.LawnPro.BL.Models;
using System;
using System.IO;

namespace KRV.LawnPro.Reporting
{
    public class PDF
    {
        public static void Export(string filename, Invoice  invoice)
        {
            try
            {
                System.IO.Directory.CreateDirectory(@"c:\temp\");

                PdfWriter writer = new PdfWriter(@"c:\temp\" + filename + ".pdf");
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Invocie logo
                ImageData imageData = ImageDataFactory.Create("LawnPro.png");
                Image pdfImg = new Image(imageData).ScaleAbsolute(200, 100).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(pdfImg);

                // Invoice Header
                Paragraph title = new Paragraph("Lawn Pro Services LLC")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold();
                document.Add(title);

                Paragraph address = new Paragraph("1825 N Bluemound Dr\nAppleton, WI 54912\n(920) 555-5555")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(15);
                document.Add(address);

                Paragraph customerLabel = new Paragraph("Customer:")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                    .SetFontSize(15)
                    .SetBold()
                    .SetFixedLeading(15);
                document.Add(customerLabel);

                Paragraph customerAddress = new Paragraph(invoice.CustomerFirstName + " " + invoice.CustomerLastName +"\n" + invoice.CustomerStreetAddress + "\n" + invoice.CustomerCity + ", " + invoice.CustomerState + "  " + invoice.CustomerZip)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                    .SetFontSize(15);
                document.Add(customerAddress);


                // Invoice Table
                int rows = 2;
                int cols = 4;

                string[,] data = new string[rows, cols];

                int row = 0;
                data[row, 0] = "Date";
                data[row, 1] = "Service Description";
                data[row, 2] = "Technician";
                data[row, 3] = "Total";
                row++;
                data[row, 0] = invoice.ServiceDate.ToShortDateString();
                data[row, 1] = invoice.ServiceType;
                data[row, 2] = invoice.EmployeeFullName;
                data[row, 3] = invoice.InvoiceTotal.ToString("c2");


                Table table = new Table(cols, false).SetWidth(UnitValue.CreatePercentValue(100));

                for (int iRow = 1; iRow <= rows; iRow++)
                {

                    for (int iCol = 1; iCol <= cols; iCol++)
                    {
                        Cell cell = new Cell(1, 1);
                        cell.Add(new Paragraph(data[iRow - 1, iCol - 1]));

                        if (iRow == 1)
                        {
                            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                            cell.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            cell.SetBold();
                        }
                        else
                        {
                            if (iRow % 2 == 0)
                            {
                                cell.SetBackgroundColor(ColorConstants.GREEN, .5f);
                            }
                        }

                        if (iCol < 4)
                        {
                            cell.SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);
                        }
                        else
                        {
                            cell.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                        }

                        table.AddCell(cell);
                    }
                }

                table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                document.Add(table);
                document.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
