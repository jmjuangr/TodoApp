namespace Models;

public class TareaItem
{
    public int TareaId { get; set; }                
    public string Titulo { get; set; } = "";    
    public string Descripcion { get; set; } = "";
    public DateTime Fecha { get; set; }      
    public bool Estado { get; set; } =false; 
    public string Prioridad  { get; set; } ="Media";

   
}
