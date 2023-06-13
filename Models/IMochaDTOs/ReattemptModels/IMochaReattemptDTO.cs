namespace IPR_BE.Models;


public class ReattemptDTO{

    public int testInvitationId {get; set;}
    public string? testUrl {get; set;}

    public string? callbackUrlRegistered {get; set;}

    public string? redirectUrlRegistered {get; set;}
}

public class ReattemptRequest{

    public string? startDateTime {get; set;}
    public string? endDateTime {get; set;}
    public int timeZoneId{get; set;}
    public string? callbackUrl {get; set;}
    public string? redirectUrl {get; set;}
}