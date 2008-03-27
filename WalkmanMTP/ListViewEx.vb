'http://www.codeproject.com/KB/list/DragAndDropListView.aspx
'BY Matt Hawley
'
'march 2008
'adapted to visual basic by dr. zoidberg. added support for accepting dragitems
'other than listViewItems (for dragging from treeviews for example)
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.Windows.Forms
Imports System.ComponentModel

Namespace ListViewDnD
    Public Class ListViewEx
        Inherits ListView
#Region "Private Members"

        Private m_previousItem As ListViewItem
        Private m_allowReorder As Boolean
        Private m_lineColor As Color

#End Region

#Region "Public Properties"

        <Category("Behavior")> _
        Public Property AllowReorder() As Boolean
            Get
                Return m_allowReorder
            End Get
            Set(ByVal value As Boolean)
                m_allowReorder = value
            End Set
        End Property

        <Category("Appearance")> _
        Public Property LineColor() As Color
            Get
                Return m_lineColor
            End Get
            Set(ByVal value As Color)
                m_lineColor = value
            End Set
        End Property

#End Region

#Region "Protected and Public Methods"

        Public Sub New()
            MyBase.New()
            m_allowReorder = True
            m_lineColor = Color.Red
        End Sub

        Protected Overloads Overrides Sub OnDragDrop(ByVal drgevent As DragEventArgs)
            If Not m_allowReorder Then
                MyBase.OnDragDrop(drgevent)
                Return
            End If

            ' get the currently hovered row that the items will be dragged to
            Dim clientPoint As Point = MyBase.PointToClient(New Point(drgevent.X, drgevent.Y))
            Dim hoverItem As ListViewItem = MyBase.GetItemAt(clientPoint.X, clientPoint.Y)

            If Not drgevent.Data.GetDataPresent(GetType(DragItemData).ToString()) OrElse DirectCast(drgevent.Data.GetData(GetType(DragItemData).ToString()), DragItemData).ListView Is Nothing OrElse DirectCast(drgevent.Data.GetData(GetType(DragItemData).ToString()), DragItemData).DragItems.Count = 0 Then
                MyBase.OnDragDrop(drgevent) 'dr.zoidberg
                Return
            End If

            ' retrieve the drag item data
            Dim data As DragItemData = DirectCast(drgevent.Data.GetData(GetType(DragItemData).ToString()), DragItemData)

            If hoverItem Is Nothing Then
                For i As Integer = 0 To data.DragItems.Count - 1
                    ' the user does not wish to re-order the items, just append to the end
                    Dim newItem As ListViewItem = DirectCast(data.DragItems(i), ListViewItem)
                    MyBase.Items.Add(newItem)
                Next
            Else
                ' the user wishes to re-order the items

                ' get the index of the hover item
                Dim hoverIndex As Integer = hoverItem.Index

                ' determine if the items to be dropped are from
                ' this list view. If they are, perform a hack
                ' to increment the hover index so that the items
                ' get moved properly.
                If Me.Equals(data.ListView) Then
                    If hoverIndex > MyBase.SelectedItems(0).Index Then
                        hoverIndex += 1
                    End If
                End If
                For i As Integer = data.DragItems.Count - 1 To 0 Step -1

                    ' insert the new items into the list view
                    ' by inserting the items reversely from the array list
                    Dim newItem As ListViewItem = DirectCast(data.DragItems(i), ListViewItem)
                    MyBase.Items.Insert(hoverIndex, newItem)
                Next
            End If

            ' remove all the selected items from the previous list view
            ' if the list view was found
            If data.ListView IsNot Nothing Then
                For Each itemToRemove As ListViewItem In data.ListView.SelectedItems
                    data.ListView.Items.Remove(itemToRemove)
                Next
            End If

            ' set the back color of the previous item, then nullify it
            If m_previousItem IsNot Nothing Then
                m_previousItem = Nothing
            End If

            Me.Invalidate()

            ' call the base on drag drop to raise the event
            MyBase.OnDragDrop(drgevent)
        End Sub

        Protected Overloads Overrides Sub OnDragOver(ByVal drgevent As DragEventArgs)
            If Not m_allowReorder Then
                MyBase.OnDragOver(drgevent)
                Return
            End If

            If Not drgevent.Data.GetDataPresent(GetType(DragItemData).ToString()) Then
                ' the item(s) being dragged do not have any data associated
                'drgevent.Effect = DragDropEffects.None 'dr.zoidberg
                MyBase.OnDragOver(drgevent)             'dr.zoidberg
                Return
            End If

            If MyBase.Items.Count > 0 Then
                ' get the currently hovered row that the items will be dragged to
                Dim clientPoint As Point = MyBase.PointToClient(New Point(drgevent.X, drgevent.Y))
                Dim hoverItem As ListViewItem = MyBase.GetItemAt(clientPoint.X, clientPoint.Y)

                Dim g As Graphics = Me.CreateGraphics()

                If hoverItem Is Nothing Then
                    'MessageBox.Show(base.GetChildAtPoint(new Point(clientPoint.X, clientPoint.Y)).GetType().ToString());

                    ' no item was found, so no drop should take place
                    drgevent.Effect = DragDropEffects.Move

                    If m_previousItem IsNot Nothing Then
                        m_previousItem = Nothing
                        Invalidate()
                    End If

                    hoverItem = MyBase.Items(MyBase.Items.Count - 1)

                    If Me.View = View.Details OrElse Me.View = View.List Then
                        g.DrawLine(New Pen(m_lineColor, 2), New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height), New Point(hoverItem.Bounds.X + Me.Bounds.Width, hoverItem.Bounds.Y + hoverItem.Bounds.Height))
                        g.FillPolygon(New SolidBrush(m_lineColor), New Point() {New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5), New Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height + 5)})
                        g.FillPolygon(New SolidBrush(m_lineColor), New Point() {New Point(Me.Bounds.Width - 4, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5), New Point(Me.Bounds.Width - 9, hoverItem.Bounds.Y + hoverItem.Bounds.Height), New Point(Me.Bounds.Width - 4, hoverItem.Bounds.Y + hoverItem.Bounds.Height + 5)})
                    Else
                        g.DrawLine(New Pen(m_lineColor, 2), New Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y), New Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y + hoverItem.Bounds.Height))
                        g.FillPolygon(New SolidBrush(m_lineColor), New Point() {New Point(hoverItem.Bounds.X + hoverItem.Bounds.Width - 5, hoverItem.Bounds.Y), New Point(hoverItem.Bounds.X + hoverItem.Bounds.Width + 5, hoverItem.Bounds.Y), New Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y + 5)})
                        g.FillPolygon(New SolidBrush(m_lineColor), New Point() {New Point(hoverItem.Bounds.X + hoverItem.Bounds.Width - 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), New Point(hoverItem.Bounds.X + hoverItem.Bounds.Width + 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), New Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5)})
                    End If

                    ' call the base OnDragOver event
                    MyBase.OnDragOver(drgevent)

                    Return
                End If

                ' determine if the user is currently hovering over a new
                ' item. If so, set the previous item's back color back
                ' to the default color.
                If (m_previousItem IsNot Nothing AndAlso Not ReferenceEquals(m_previousItem, hoverItem)) OrElse m_previousItem Is Nothing Then
                    Me.Invalidate()
                End If

                ' set the background color of the item being hovered
                ' and assign the previous item to the item being hovered
                'hoverItem.BackColor = Color.Beige;
                m_previousItem = hoverItem

                If Me.View = View.Details OrElse Me.View = View.List Then
                    g.DrawLine(New Pen(m_lineColor, 2), New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y), New Point(hoverItem.Bounds.X + Me.Bounds.Width, hoverItem.Bounds.Y))
                    g.FillPolygon(New SolidBrush(m_lineColor), New Point() {New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y - 5), New Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y), New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + 5)})
                    g.FillPolygon(New SolidBrush(m_lineColor), New Point() {New Point(Me.Bounds.Width - 4, hoverItem.Bounds.Y - 5), New Point(Me.Bounds.Width - 9, hoverItem.Bounds.Y), New Point(Me.Bounds.Width - 4, hoverItem.Bounds.Y + 5)})
                Else
                    g.DrawLine(New Pen(m_lineColor, 2), New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y), New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height))
                    g.FillPolygon(New SolidBrush(m_lineColor), New Point() {New Point(hoverItem.Bounds.X - 5, hoverItem.Bounds.Y), New Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y), New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + 5)})
                    g.FillPolygon(New SolidBrush(m_lineColor), New Point() {New Point(hoverItem.Bounds.X - 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), New Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), New Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5)})
                End If

                ' go through each of the selected items, and if any of the
                ' selected items have the same index as the item being
                ' hovered, disable dropping.
                For Each itemToMove As ListViewItem In MyBase.SelectedItems
                    If itemToMove.Index = hoverItem.Index Then
                        drgevent.Effect = DragDropEffects.None
                        hoverItem.EnsureVisible()
                        Return
                    End If
                Next

                ' ensure that the hover item is visible
                hoverItem.EnsureVisible()
            End If

            ' everything is fine, allow the user to move the items
            drgevent.Effect = DragDropEffects.Move

            ' call the base OnDragOver event
            MyBase.OnDragOver(drgevent)
        End Sub

        Protected Overloads Overrides Sub OnDragEnter(ByVal drgevent As DragEventArgs)
            If Not m_allowReorder Then
                MyBase.OnDragEnter(drgevent)
                Return
            End If

            If Not drgevent.Data.GetDataPresent(GetType(DragItemData).ToString()) Then
                ' the item(s) being dragged do not have any data associated
                'drgevent.Effect = DragDropEffects.None 'dr.zoidberg
                MyBase.OnDragEnter(drgevent)            'dr.zoidberg
                Return
            End If

            ' everything is fine, allow the user to move the items
            drgevent.Effect = DragDropEffects.Move

            ' call the base OnDragEnter event
            MyBase.OnDragEnter(drgevent)
        End Sub

        Protected Overloads Overrides Sub OnItemDrag(ByVal e As ItemDragEventArgs)
            If Not m_allowReorder Then
                MyBase.OnItemDrag(e)
                Return
            End If

            ' call the DoDragDrop method
            MyBase.DoDragDrop(GetDataForDragDrop(), DragDropEffects.Move)

            ' call the base OnItemDrag event
            MyBase.OnItemDrag(e)
        End Sub

        Protected Overloads Overrides Sub OnLostFocus(ByVal e As EventArgs)
            ' reset the selected items background and remove the previous item
            ResetOutOfRange()

            Invalidate()

            ' call the OnLostFocus event
            MyBase.OnLostFocus(e)
        End Sub

        Protected Overloads Overrides Sub OnDragLeave(ByVal e As EventArgs)
            ' reset the selected items background and remove the previous item
            ResetOutOfRange()

            Invalidate()

            ' call the base OnDragLeave event
            MyBase.OnDragLeave(e)
        End Sub

#End Region

#Region "Private Methods"

        Private Function GetDataForDragDrop() As DragItemData
            ' create a drag item data object that will be used to pass along with the drag and drop
            Dim data As New DragItemData(Me)

            ' go through each of the selected items and 
            ' add them to the drag items collection
            ' by creating a clone of the list item
            For Each item As ListViewItem In Me.SelectedItems
                data.DragItems.Add(item.Clone())
            Next

            Return data
        End Function

        Private Sub ResetOutOfRange()
            ' determine if the previous item exists,
            ' if it does, reset the background and release 
            ' the previous item
            If m_previousItem IsNot Nothing Then
                m_previousItem = Nothing
            End If

        End Sub

#End Region

#Region "DragItemData Class"

        Public Class DragItemData
#Region "Private Members"

            Private m_listView As ListViewEx
            Private m_dragItems As ArrayList

#End Region

#Region "Public Properties"

            Public ReadOnly Property ListView() As ListViewEx
                Get
                    Return m_listView
                End Get
            End Property

            Public ReadOnly Property DragItems() As ArrayList
                Get
                    Return m_dragItems
                End Get
            End Property

#End Region

#Region "Public Methods and Implementation"

            Public Sub New(ByVal listView As ListViewEx)
                m_listView = listView
                m_dragItems = New ArrayList()
            End Sub

#End Region
        End Class

#End Region
    End Class
End Namespace
