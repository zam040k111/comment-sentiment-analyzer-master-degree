using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.WEB.Interfaces;

namespace GameStore.WEB.Services
{
    public static class PdfService
    {
        public static byte[] GenerateFile(IPdfObjectData pdfData)
        {
            if (pdfData == null)
            {
                throw new NotFoundException();
            }

            var document = new Document();
            var page = new Page();
            document.Pages.Add(page);

            var table = new Table2(0, 100, 600, 600);
            var column1 = table;
            pdfData.ColumnWidths.ForEach(cWidth => table.Columns.Add(cWidth));
            column1.CellDefault.Align = TextAlign.Center;

            var row1 = table.Rows.Add(40, Font.HelveticaBold, 16, Grayscale.Black,
               Grayscale.Gray);
            row1.CellDefault.Align = TextAlign.Center;
            row1.CellDefault.VAlign = VAlign.Center;

            pdfData.ColumnNames.ForEach(col => row1.Cells.Add(col));

            foreach (var item in pdfData.Rows)
            {
                var row2 = table.Rows.Add(30);
                item.ForEach(row => row2.Cells.Add(row));
            }

            table.CellDefault.Padding.Value = 5.0f;
            table.CellSpacing = 5.0f;
            table.Border.Top.Color = RgbColor.Blue;
            table.Border.Bottom.Color = RgbColor.Blue;
            table.Border.Top.Width = 2;
            table.Border.Bottom.Width = 2;
            table.Border.Left.LineStyle = LineStyle.None;
            table.Border.Right.LineStyle = LineStyle.None;


            var y = 0;
            var fontSize = 18;
            var font = Font.HelveticaBold;
            foreach (var head in pdfData.Headers)
            {
                page.Elements.Add(new Label(head,0, y, 500, 50, font, fontSize, TextAlign.Right));
                y += 50;
                fontSize = 14;
                font = Font.Helvetica;
            }

            page.Elements.Add(new Label(
                pdfData.Footer,
                0,
                (pdfData.Rows.Count * 50) + 200,
                500,
                50,
                Font.HelveticaBold,
                18,
                TextAlign.Right));

            page.Elements.Add(table);

            return document.Draw();
        }
    }
}
