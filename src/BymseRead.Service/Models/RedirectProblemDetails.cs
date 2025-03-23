using Microsoft.AspNetCore.Mvc;

namespace BymseRead.Service.Models;

public class RedirectProblemDetails : ProblemDetails
{
    public string RedirectUrl { get; set; } = string.Empty;
}