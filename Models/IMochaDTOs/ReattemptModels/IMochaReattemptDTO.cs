using System.Text.RegularExpressions;

namespace IPR_BE.Models;


//sent back to FE
public class ReattemptDTO{

    public long testInvitationId {get; set;}
    public string? testUrl {get; set;}

    public string? callbackUrlRegistered {get; set;}

    public string? redirectUrlRegistered {get; set;}
}

//comes in from FE and to send to iMocha
public class ReattemptRequest{

    public ReattemptRequest() {}
    public ReattemptRequest(string host, string  redirectUrl) {
        setCallBackUrl(host);
        this.redirectUrl = redirectUrl;
    }
    public string setCallBackUrl(string host) {
        callbackUrl = "https://" + host + "/interviewbot/imocha";
        return callbackUrl;
    }

    public DateTime? startDateTime {get; set;}
    public DateTime? endDateTime {get; set;}
    public int timeZoneId{get; set;}
    public string? callbackUrl {get; set;}
    public string? redirectUrl {get; set;}
    public long testId {get; set;}
}