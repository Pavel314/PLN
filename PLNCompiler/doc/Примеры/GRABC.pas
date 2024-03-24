library GRABC;
uses GraphABC;
begin
MoveTo(0,0);
Circle(100,100,100);
Line(0,0,0,0);
SetPixel(0,0,clRed);
Rectangle(0,0,0,0);
SaveWindow('asd');
WindowWidth();
WindowHeight();
GraphABC.LoadWindow('asd');
GraphABC.SetBrushColor(Color.Red);
GraphABC.SetPenColor(Color.Red);
GraphABC.SetPenWidth(1);
GraphABC.LineRel(0,0);
LockDrawing();
Redraw();
end.