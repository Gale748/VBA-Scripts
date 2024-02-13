Sub ExtractListItemsToExcelAfterMarker()
    Dim wdApp As Word.Application
    Dim wdDoc As Word.Document
    Dim xlApp As Object
    Dim xlBook As Object
    Dim xlSheet As Object
    Dim para As Paragraph
    Dim rowNumber As Long
    Dim currentHeading As String
    Dim capture As Boolean
    
    Set wdApp = Word.Application
    Set wdDoc = wdApp.ActiveDocument
    
    ' Attempt to create a new instance of Excel
    On Error Resume Next
    Set xlApp = CreateObject("Excel.Application")
    If xlApp Is Nothing Then
        MsgBox "Excel is not available."
        Exit Sub
    End If
    On Error GoTo 0
    
    xlApp.Visible = True
    Set xlBook = xlApp.Workbooks.Add
    Set xlSheet = xlBook.Sheets(1)
    
    xlSheet.Cells(1, 1).Value = "Heading Name"
    xlSheet.Cells(1, 2).Value = "List Item"
    
    rowNumber = 2
    currentHeading = ""
    capture = False
    
    For Each para In wdDoc.Paragraphs
        If para.Style Like "Heading*" Then
            currentHeading = Trim(para.Range.Text) ' This should capture the full heading text, including numbering
        ElseIf InStr(para.Range.Text, ": [R]") > 0 Then
            capture = True ' Start capturing after the marker
        ElseIf capture Then
            If para.Range.ListFormat.ListType <> WdListType.wdListNoNumbering Then
                xlSheet.Cells(rowNumber, 1).Value = currentHeading ' Use the captured heading
                xlSheet.Cells(rowNumber, 2).Value = Trim(para.Range.Text) ' List item text
                rowNumber = rowNumber + 1
            End If
        End If
    Next para
    
    xlSheet.Columns("A:B").AutoFit
    MsgBox "Extraction completed successfully!"
    
    ' Clean up
    Set xlSheet = Nothing
    Set xlBook = Nothing
    Set xlApp = Nothing
    Set wdDoc = Nothing
    Set wdApp = Nothing
End Sub
