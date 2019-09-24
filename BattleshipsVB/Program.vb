Imports System.IO

Class Program
    Public Structure ShipType
        Public Name As String
        Public Size As Integer
    End Structure

    Const TrainingGame As String = "Training.txt"

    Private Shared Sub GetRowColumn(ByRef Row As Integer, ByRef Column As Integer)
        Console.WriteLine()
        Console.Write("Please enter column: ")
        Column = GetRowOrColumnNumber()
        Console.Write("Please enter row: ")
        Row = GetRowOrColumnNumber()
    End Sub

    Private Shared Function GetRowOrColumnNumber() As Integer
        While True
            Dim num = Convert.ToInt32(Console.ReadLine())

            If num > 9 OrElse num < 0 Then
                Console.Write("Invalid value entered")
            Else
                Return num
            End If
        End While
    End Function

    Private Shared Sub MakePlayerMove(ByVal Board As Char(,), ByVal Ships As ShipType())
        Dim Row As Integer = 0
        Dim Column As Integer = 0
        GetRowColumn(Row, Column)

        If Board(Row, Column) = "m"c OrElse Board(Row, Column) = "h"c Then
            Console.WriteLine("Sorry, you have already shot at the square (" & Column & "," & Row & "). Please try again.")
        ElseIf Board(Row, Column) = "-"c Then
            Console.WriteLine("Sorry, (" & Column & "," & Row & ") is a miss.")
            Board(Row, Column) = "m"c
        Else
            Console.WriteLine("Hit at (" & Column & "," & Row & ").")
            Board(Row, Column) = "h"c
        End If
    End Sub

    Private Shared Sub SetUpBoard(ByVal Board As Char(,))
        For Row As Integer = 0 To 10 - 1

            For Column As Integer = 0 To 10 - 1
                Board(Row, Column) = "-"c
            Next
        Next
    End Sub

    Private Shared Sub LoadGame(ByVal TrainingGame As String, ByVal Board As Char(,))
        Dim Line As String = ""
        Dim BoardFile = OpenFile(TrainingGame)

        For Row As Integer = 0 To 10 - 1
            Line = BoardFile.ReadLine()

            For Column As Integer = 0 To 10 - 1
                Board(Row, Column) = Line(Column)
            Next
        Next

        BoardFile.Close()
    End Sub

    Private Shared Sub PlaceRandomShips(ByVal Board As Char(,), ByVal Ships As ShipType())
        Dim RandomNumber As Random = New Random()
        Dim Valid As Boolean
        Dim Orientation As Char = " "c
        Dim Row As Integer = 0
        Dim Column As Integer = 0
        Dim HorV As Integer = 0

        For Each Ship In Ships
            Valid = False

            While Valid = False
                Row = RandomNumber.[Next](0, 10)
                Column = RandomNumber.[Next](0, 10)
                HorV = RandomNumber.[Next](0, 2)

                If HorV = 0 Then
                    Orientation = "v"c
                Else
                    Orientation = "h"c
                End If

                Valid = ValidateBoatPosition(Board, Ship, Row, Column, Orientation)
            End While

            Console.WriteLine("Computer placing the " & Ship.Name)
            PlaceShip(Board, Ship, Row, Column, Orientation)
        Next
    End Sub

    Private Shared Sub PlaceShip(ByVal Board As Char(,), ByVal Ship As ShipType, ByVal Row As Integer, ByVal Column As Integer, ByVal Orientation As Char)
        If Orientation = "v"c Then

            For Scan As Integer = 0 To Ship.Size - 1
                Board(Row + Scan, Column) = Ship.Name(0)
            Next
        ElseIf Orientation = "h"c Then

            For Scan As Integer = 0 To Ship.Size - 1
                Board(Row, Column + Scan) = Ship.Name(0)
            Next
        End If
    End Sub

    Private Shared Function ValidateBoatPosition(ByVal Board As Char(,), ByVal Ship As ShipType, ByVal Row As Integer, ByVal Column As Integer, ByVal Orientation As Char) As Boolean
        If Not WouldFitWithinBoard(Ship, Row, Column, Orientation) Then
            Return False
        Else

            If Orientation = "v"c Then

                For Scan As Integer = 0 To Ship.Size - 1

                    If Board(Row + Scan, Column) <> "-"c Then
                        Return False
                    End If
                Next
            ElseIf Orientation = "h"c Then

                For Scan As Integer = 0 To Ship.Size - 1

                    If Board(Row, Column + Scan) <> "-"c Then
                        Return False
                    End If
                Next
            End If
        End If

        Return True
    End Function

    Private Shared Function WouldFitWithinBoard(ByVal Ship As ShipType, ByVal Row As Integer, ByVal Column As Integer, ByVal Orientation As Char) As Boolean
        If Orientation = "v"c AndAlso Row + Ship.Size > 10 Then
            Return False
        ElseIf Orientation = "h"c AndAlso Column + Ship.Size > 10 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Shared Function CheckWin(ByVal Board As Char(,)) As Boolean
        For Row As Integer = 0 To 10 - 1

            For Column As Integer = 0 To 10 - 1

                If Board(Row, Column) = "A"c OrElse Board(Row, Column) = "B"c OrElse Board(Row, Column) = "S"c OrElse Board(Row, Column) = "D"c OrElse Board(Row, Column) = "P"c Then
                    Return False
                End If
            Next
        Next

        Return True
    End Function

    Private Shared Sub PrintBoard(ByVal Board As Char(,))
        Console.WriteLine()
        Console.WriteLine("The board looks like this: ")
        Console.WriteLine()
        Console.Write(" ")

        For Column As Integer = 0 To 10 - 1
            Console.Write(" " & Column & "  ")
        Next

        Console.WriteLine()

        For Row As Integer = 0 To 10 - 1
            Console.Write(Row & " ")

            For Column As Integer = 0 To 10 - 1

                If Board(Row, Column) = "-"c Then
                    Console.Write(" ")
                ElseIf Board(Row, Column) = "A"c OrElse Board(Row, Column) = "B"c OrElse Board(Row, Column) = "S"c OrElse Board(Row, Column) = "D"c OrElse Board(Row, Column) = "P"c Then
                    Console.Write(" ")
                Else
                    Console.Write(Board(Row, Column))
                End If

                If Column <> 9 Then
                    Console.Write(" | ")
                End If
            Next

            Console.WriteLine()
        Next
    End Sub

    Private Shared Sub DisplayMenu()
        Console.WriteLine("MAIN MENU")
        Console.WriteLine("")
        Console.WriteLine("1. Start new game")
        Console.WriteLine("2. Load training game")
        Console.WriteLine("9. Quit")
        Console.WriteLine()
    End Sub

    Private Shared Function GetMainMenuChoice() As Integer
        Dim Choice As Integer = 0
        Console.Write("Please enter your choice: ")
        Choice = Convert.ToInt32(Console.ReadLine())
        Console.WriteLine()
        Return Choice
    End Function

    Private Shared Sub PlayGame(ByVal Board As Char(,), ByVal Ships As ShipType())
        Dim GameWon As Boolean = False

        While GameWon = False
            PrintBoard(Board)
            MakePlayerMove(Board, Ships)
            GameWon = CheckWin(Board)

            If GameWon = True Then
                Console.WriteLine("All ships sunk!")
                Console.WriteLine()
            End If
        End While
    End Sub

    Private Shared Sub SetUpShips(ByVal Ships As ShipType())
        Ships(0).Name = "Aircraft Carrier"
        Ships(0).Size = 5
        Ships(1).Name = "Battleship"
        Ships(1).Size = 4
        Ships(2).Name = "Submarine"
        Ships(2).Size = 3
        Ships(3).Name = "Destroyer"
        Ships(3).Size = 3
        Ships(4).Name = "Patrol Boat"
        Ships(4).Size = 2
    End Sub

    Private Shared Function OpenFile(ByVal fileName As String, ByVal Optional path As String = "") As StreamReader
        Return New StreamReader(path & fileName)
    End Function

    Shared Sub Main()
        Dim Ships As ShipType() = New ShipType(4) {}
        Dim Board As Char(,) = New Char(9, 9) {}
        Dim MenuOption As Integer = 0

        While MenuOption <> 9
            SetUpBoard(Board)
            SetUpShips(Ships)
            DisplayMenu()
            MenuOption = GetMainMenuChoice()

            If MenuOption = 1 Then
                PlaceRandomShips(Board, Ships)
                PlayGame(Board, Ships)
            End If

            If MenuOption = 2 Then
                LoadGame(TrainingGame, Board)
                PlayGame(Board, Ships)
            End If
        End While
    End Sub
End Class
