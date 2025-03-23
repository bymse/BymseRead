using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.Models;

public class RedirectProblemDetails : ProblemDetails
{
    public string? RedirectUrl
    {
        get => Extensions["redirectUrl"] as string;
        set => Extensions["redirectUrl"] = value;
    }
}