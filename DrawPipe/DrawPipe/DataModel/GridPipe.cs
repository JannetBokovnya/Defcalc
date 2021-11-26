namespace DrawPipe.DataModel
{
  public  class GridPipe
    {
      
       public string KEYPIPE { get; set; } //ключ трубы
       public string NUMBERPIPE { get; set; } //номер трубы
       public string KM { get; set; }//километраж
       public double ANGLESHOV { get; set; }//угол шва
       public string LENGHT { get; set; }//длина
       public string DEPTHPIPE { get; set; }//толщина стенки
       public string NUMBERDEFECT { get; set; }//количество дефектов
       public string NUMBERAKT { get; set; }//номер акта
       public double TUBERADIUS { get; set; } //радиус трубы
       public bool ISSELECT { get; set; }
       public string MAXDEPTHDEF { get; set; } //МАКСИМАЛЬНАЯ ГЛУБИНА ДЕФЕКТА (для вывода в таблице)
       public string MAXSQRDEF { get; set; } //максимальная площпдь дефекта (для вывода в таблице)

       public GridPipe(string keypipe, string numberpipe, string km, 
                       double angleshov, string lenght, string depthpipe, 
                       string numberdefect, string numberakt, bool isSelect, double tubeRadius,
                       string maxdepthdef, string maxsqrdef)
       {
           KEYPIPE = keypipe;
           NUMBERPIPE = numberpipe;
           KM = km;
           ANGLESHOV = angleshov;
           LENGHT = lenght;
           DEPTHPIPE = depthpipe;
           NUMBERDEFECT = numberdefect;
           NUMBERAKT = numberakt;
           ISSELECT = isSelect;
           TUBERADIUS = tubeRadius;
           MAXDEPTHDEF = maxdepthdef;
           MAXSQRDEF = maxsqrdef;
          

       }
    }
}
