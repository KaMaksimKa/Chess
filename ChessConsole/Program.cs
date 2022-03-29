using System.Drawing;
using ChessConsole.Models;

/*var chessBoard = new ChessBoard();

chessBoard.Move(new Point(1, 1), new Point(1, 2));
chessBoard.Move(new Point(2, 0), new Point(0, 2));
for (int i = 0; i < 8; i++)
{
    for (int j = 0; j < 8; j++)
    {
        Console.Write(chessBoard.Board[i,j]+" ");
    }

    Console.WriteLine();
    Console.WriteLine();
}*/
Point a = new Point(1, 1);
var b = a;
b.X += 1;
Console.WriteLine(a);
Console.WriteLine(b);
