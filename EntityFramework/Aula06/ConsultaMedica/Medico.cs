public class Medico
{ 
    public int MedicoId {get; set;}
    public string Nome { get; set; } = null!;
    public string Especialidade { get; set; } = null!;
    public int CRM { get; set; } 
    public List<Consulta> Consultas { get; set; } = new List<Consulta>();

} 