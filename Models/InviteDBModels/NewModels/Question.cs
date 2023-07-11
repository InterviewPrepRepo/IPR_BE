namespace IPR_BE.Models;

public class DbQuestion {
    public long questionId {get; set;}
    public string prompt {get; set;} = "";
    public int questionTypeId {get; set;}
    public int possiblePoints {get; set;}

}